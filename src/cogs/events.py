import asyncio
import pytz
import json

from bot import BOT, DB_CHANNEL, HELP, HELP_COLOR, SRC
from datetime import datetime
from nextcord import Embed, File, TextChannel, PermissionOverwrite, CategoryChannel
from nextcord.ext import commands, tasks
from oead import yaz0
from pathlib import Path

SERVER_CONFIG = None
SERVER_CONFIG_ID: int = 0

class Events(commands.Cog):
    """Handles events"""

    def __init__(self, bot: commands.Bot):
        self.bot = bot
        self.read_tasks.start()

    def cog_unload(self):
        self.read_tasks.cancel()

    @tasks.loop(minutes=1)
    async def read_tasks(self):
        """Reads through the staged tasks"""

        # get the server list
        servers = BOT.get_channel(DB_CHANNEL).last_message.attachments[0].read()
        servers = json.loads(servers)

        # create list of python tasks
        py_tasks = []

        # iterate the servers and handle each server accordingly
        for server, _ in servers:
            py_tasks.append(asyncio.create_task(self.handle_server(server)))

        # await all tasks
        asyncio.gather(*py_tasks)

    async def handle_server(self, id: int):

        # get local settings
        settings = BOT.get_channel(id).last_message.attachments[0].read()

        # decompress if compressed
        if settings[0-4] == b'Yaz0':
            settings = json.loads(yaz0.decompress(settings))
        else:
            settings = json.loads(settings)

        for _, task in settings['tasks']:

            # skip tasks that are not set for the current time
            if task['time'] != self.get_time(settings['timezone'], '%I:%m%p'):
                continue

            # check the day of the week
            if self.get_time(settings['timezone'], '%I:%m%p') not in task['days'] and len(task['days']) != 0:
                continue

            # get the task channel and message
            channel = BOT.get_channel(task['channel'])
            message = task['message']

            # iterate vars replacing every instance found in the message
            for var in settings['vars']:

                # needs to be improved to support proposed features
                str(message).replace(f'${var}', str(var)
                    ).replace('@role', f'<@&{task["role"]}>'
                    ).replace('@user', f'<@{task["user"]}>')

            # send the message in channel
            channel.send(message)

    def get_hour(tz: str) -> int:
        return int(datetime.now(pytz.timezone(tz)).strftime('%I'))

    def get_time(self, tz: str, strftime) -> str:
        return datetime.now(pytz.timezone(tz)).strftime(strftime.replace('%I', self.get_hour(tz)))

    @commands.command()
    async def register(self, ctx: commands.Context, id: int = -1):
        """Register the server"""

        # send a loading message
        reg = await ctx.send('Registering TaskTracker...')
        
        # check the passed args for a channel id
        channel = BOT.get_channel(id)
        
        # if the check was unsuccessful, get the channel in context
        if not channel:
            channel = BOT.get_channel(ctx.channel.id)

        # add settings config
        create: bool = True

        # check for an existing task-tracker-metadata category
        for guild_category in channel.guild.channels:
            if guild_category.name == 'task-tracker-metadata':
                create = False
                category = guild_category

        # create the task-tracker-metadata category
        if create == True:
            category = await channel.guild.create_category(
                'task-tracker-metadata',
                overwrites = {
                    channel.guild.default_role: PermissionOverwrite(**{'view_channel': False})
                }
            )

        # reset create
        create = True

        # check for an existing server channel in the task-tracker-metadata category
        for _channel in category.channels:
            if _channel.name == 'server':
                create = False
                settings = _channel

        # create a server channel in the task-tracker-metadata category
        if create == True:
            settings: TextChannel = await channel.guild.create_text_channel('server', category=category)

        # create the default config file to the settings channel
        defaults = {
            'vars': [],
            'tasks': []
        }

        # upload the default config file to the settings channel
        file = Path(f'{SRC}\\new.json'); file.write_text(json.dumps(defaults, indent=4))
        await settings.send('**DO NOT POST ANY MESSAGES IN THIS CHANNEL**', file=File(fp=file, filename=f'{settings.id}'))
        file.unlink()

        # update server list
        server_list = BOT.get_channel(DB_CHANNEL).last_message

        if not server_list:
            server_list = {}
        else:
            server_list = await json.loads(server_list.attachments[0].read())

        server_list[settings.id] = 0

        server_list_json = Path(f'{SRC}\\tmp.json')
        server_list_json.write_text(json.dumps(server_list, indent=4))
        await BOT.get_channel(DB_CHANNEL).send(file=File(fp=server_list_json, filename='DATA.json'))
        server_list_json.unlink()

        # create help embed
        embed: Embed = Embed(
            color = HELP_COLOR,
            title = 'Server Setup',
            description = 'Follow these instructions to setup TaskTracker for your server. *(Windows Only)*'
        )
        embed.add_field(
            name='Step One',
            value='Download the [SetupWizard](https://github.com/ArchLeaders/ComputerStudies-TaskTracker/releases/latest) from [GitHub]() to configure TaskTracker.',
            inline=False
        )
        embed.add_field(
            name='Step Two',
            value='Run the downloaded executable and insert the __ID below__ to authorize your server.',
            inline=False
        )
        embed.set_thumbnail(url=HELP)
        embed.set_footer(text=f'ID: {settings.id}')

        # delete the loading message
        try: await reg.delete()
        except: {}

        # send the embed with a comfirmation message
        await ctx.send(f'**Registered TaskTracker in <#{channel.id}>.**', embed=embed)

def setup(bot: commands.Bot):
    bot.add_cog(Events(bot))