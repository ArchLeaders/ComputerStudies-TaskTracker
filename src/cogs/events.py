import json
from pathlib import Path

from nextcord import Embed, File, TextChannel, ChannelType, PermissionOverwrite
from nextcord.ext import commands, tasks
from bot import BOT, DB_CHANNEL, HELP, HELP_COLOR, SRC, load_database, role

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

        # iterate servers


        # download the last uploaded file


        # iterate the tasks

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
            'meta': {
                'system': channel.id,
                'settings': settings.id
            },
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

        server_list[channel.guild.id] = 0

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
        embed.set_footer(text=f'ID: {channel.guild.id}')

        # delete the loading message
        try: await reg.delete()
        except: {}

        # send the embed with a comfirmation message
        await ctx.send(f'**Registered TaskTracker in <#{channel.id}>.**', embed=embed)

def setup(bot: commands.Bot):
    bot.add_cog(Events(bot))