import asyncio
import pytz
import json

from bot import BOT, CHANNEL_DB, CHANNEL_NEW_REG, HELP, HELP_COLOR, SRC
from datetime import datetime
from nextcord import Embed, File, TextChannel, PermissionOverwrite, CategoryChannel
from nextcord.ext import commands, tasks
from oead import yaz0
from pathlib import Path

SERVER_CONFIG = None
SERVER_CONFIG_ID: int = 0


class Events(commands.Cog):
    """Handles events"""

    sequence_pos: dict = {}

    def __init__(self, bot: commands.Bot):
        self.bot = bot
        self.read_tasks.start()

    def cog_unload(self):
        self.read_tasks.cancel()

    @tasks.loop(minutes=1)
    async def read_tasks(self):
        """Reads through the staged tasks"""

        # get the server list
        try:
            server_channel = (
                await BOT.get_channel(CHANNEL_DB).history(limit=1).flatten()
            )
            servers = await server_channel[0].attachments[0].read()
            servers = json.loads(servers)
        except:
            print("ReadTasks returned 'No Servers'")
            return

        # create list of python tasks
        py_tasks = []

        # iterate the servers and handle each server accordingly
        for server, _ in servers.items():
            print(
                f"Execute '{server}' at {self.get_time('America/Vancouver', '%I:%M %p')}"
            )
            py_tasks.append(asyncio.create_task(self.handle_server(server)))

        # await all tasks
        asyncio.gather(*py_tasks)

    async def handle_server(self, id: int):

        # get local settings
        server_channel = await BOT.get_channel(int(id)).history(limit=1).flatten()
        settings = await server_channel[0].attachments[0].read()

        # decompress if compressed
        if settings[0 - 4] == b"Yaz0":
            settings = json.loads(yaz0.decompress(settings))
        else:
            settings = json.loads(settings)

        for task in settings["tasks"]:

            # skip tasks that are not set for the current time
            if task["Time"] != self.get_time(settings["timezone"], "%I:%M %p"):
                continue

            # check the day of the week
            if task["Days"][self.get_time(settings["timezone"], "%a")] == False:
                continue

            # get the task channel and message
            channel = BOT.get_channel(task["Channel"])

            # set message and replace basic vars
            message: str = (
                str(task["Message"])
                .replace("@role", f'<@&{task["Role"]}>')
                .replace("@user", f'<@{task["User"]}>')
                .replace(
                    "$time_now",
                    f'{self.get_time(settings["timezone"], "%I:%M%p")}',
                )
                .replace(
                    "$date_now",
                    f'{self.get_time(settings["timezone"], "%I:%M%p")}',
                )
            )

            # iterate sequence vars
            for key, value in task["Sequence"].items():

                # set sequence values
                message = message.replace(f"${key}", value[0])

            # iterate vars replacing every instance found in the message
            for key, value in settings["vars"].items():

                # replace vars
                message = message.replace(key, value)

            # send the message in channel
            await channel.send(message)

    def get_hour(self, tz: str) -> int:
        return int(datetime.now(pytz.timezone(tz)).strftime("%I"))

    def get_time(self, tz: str, strftime) -> str:
        return datetime.now(pytz.timezone(tz)).strftime(
            strftime.replace("%I", str(self.get_hour(tz)))
        )

    @commands.command()
    async def register(self, ctx: commands.Context):
        """Register the server"""

        # send a loading message
        reg = await ctx.send("Registering TaskTracker...")

        # get the channel in context
        guild = ctx.guild

        # add settings config
        create: bool = True

        # check for an existing task-tracker-metadata category
        for guild_category in guild.channels:
            if guild_category.name == "task-tracker-metadata":
                create = False
                category = guild_category

        # create the task-tracker-metadata category
        if create == True:
            category = await guild.create_category(
                "task-tracker-metadata",
                overwrites={
                    guild.default_role: PermissionOverwrite(**{"view_channel": False})
                },
            )

        # reset create
        create = True

        # check for an existing server channel in the task-tracker-metadata category
        for channel in category.channels:
            if channel.name == "server":
                create = False
                settings = channel

        # create a server channel in the task-tracker-metadata category
        if create == True:
            settings: TextChannel = await guild.create_text_channel(
                "server", category=category
            )

        # create the default config file to the settings channel
        defaults = {"timezone": "", "vars": {}, "tasks": []}

        # upload the default config file to the settings channel
        file = Path(f"{SRC}\\tmp.io")
        file.write_text(json.dumps(defaults, indent=4))
        await settings.send(file=File(fp=file, filename=f"server.io"))
        file.unlink()

        # update server list
        server_list = BOT.get_channel(CHANNEL_DB).last_message

        if not server_list:
            server_list = {}
        else:
            server_list = await json.loads(server_list.attachments[0].read())

        server_list[settings.id] = 0

        server_list_json = Path(f"{SRC}\\tmp.json")
        server_list_json.write_text(json.dumps(server_list, indent=4))
        db_channel = BOT.get_channel(CHANNEL_DB)
        await db_channel.purge(limit=100)
        await db_channel.send(file=File(fp=server_list_json, filename="DATA.json"))
        server_list_json.unlink()

        token_json = Path(f"{SRC}\\server.token")
        token_json.write_text(str(settings.id))
        token_msg = await BOT.get_channel(CHANNEL_NEW_REG).send(
            file=File(fp=token_json, filename="server.token")
        )
        token_json.unlink()

        # create help embed
        embed: Embed = Embed(
            color=HELP_COLOR,
            title="Server Setup",
            description="Follow these instructions to setup TaskTracker for your server. *(Windows Only)*",
        )
        embed.add_field(
            name="Step One:",
            value=f"Download the [SetupWizard](https://github.com/ArchLeaders/ComputerStudies-TaskTracker/releases/latest) from GitHub and this [token file]({token_msg.attachments[0].url})"
            + " to configure TaskTracker with your server.",
            inline=False,
        )
        embed.add_field(
            name="Step Two:",
            value="Run the downloaded executable and start adding tasks.",
            inline=False,
        )
        embed.set_thumbnail(url=HELP)
        embed.set_footer(text=f"ID: {settings.id}")

        # delete the loading message
        try:
            await reg.delete()
        except:
            {}

        # send the embed with a comfirmation message
        await ctx.send(embed=embed)


def setup(bot: commands.Bot):
    bot.add_cog(Events(bot))
