import asyncio
import json

from bot import BOT, CHANNEL_DB, CHANNEL_NEW_REG, HELP, HELP_COLOR, SRC
from nextcord import File, Embed
from nextcord.ext import commands, tasks
from pathlib import Path
from utils import get_settings_channel, get_settings_data, get_time, set_settings_data


class Tasks(commands.Cog):
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
            print(f"Execute '{server}' at {get_time('America/Vancouver', '%I:%M %p')}")
            py_tasks.append(asyncio.create_task(self.handle_server(int(server))))

        # await all tasks
        asyncio.gather(*py_tasks)

    async def handle_server(self, id: int):

        update_setting: bool = False

        # get local settings
        settings = await get_settings_data(BOT.get_channel(id).guild.id)

        for task in settings["tasks"]:

            # skip tasks that are not set for the current time
            if task["Time"] != get_time(settings["timezone"], "%I:%M %p"):
                continue

            # check the day of the week
            if task["Days"][get_time(settings["timezone"], "%a")] == False:
                continue

            # get the task channel and message
            update_setting = True
            channel = BOT.get_channel(int(task["Channel"]))

            # set message and replace basic vars
            message: str = (
                str(task["Message"])
                .replace("@role", f'<@&{task["Role"]}>')
                .replace("@user", f'<@{task["User"]}>')
                .replace(
                    "$time_now",
                    f'{get_time(settings["timezone"], "%I:%M%p")}',
                )
                .replace(
                    "$date_now",
                    f'{get_time(settings["timezone"], "%I:%M%p")}',
                )
            )

            # iterate sequence vars
            for key, value in task["Sequence"].items():

                # set sequence values
                message = message.replace(
                    f"${key}", task["Sequence"][key][task["Session"][key]]
                )

                # advance session
                task["Session"][key] = (
                    task["Session"][key] + 1
                    if len(task["Sequence"][key]) > (task["Session"][key] + 1)
                    else 0
                )

            # iterate vars replacing every instance found in the message
            for key, value in settings["vars"].items():

                # replace vars
                message = message.replace(key, value)

            # send the message in channel
            await channel.send(message)

        # update the server settings
        if update_setting == True:
            set_settings_data(id, settings)

    @commands.command()
    async def register(self, ctx: commands.Context):
        """Register the server"""

        reg = await ctx.send("Registering TaskTracker...")

        # upload the default config file to the settings channel
        settings = await get_settings_channel(ctx.guild.id)

        file = Path(f"{SRC}\\tmp.io")
        file.write_text(json.dumps({"timezone": "", "vars": {}, "tasks": []}, indent=4))
        await settings.send(file=File(fp=file, filename=f"server.io.json"))
        file.unlink()

        # update server list
        server_list = BOT.get_channel(CHANNEL_DB).history(limit=1).flatten()
        server_list = (
            {}
            if not server_list[0]
            else await json.loads(server_list[0].attachments[0].read())
        )

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
            value=f"Download the [SetupWizard.exe](https://github.com/ArchLeaders/ComputerStudies-TaskTracker/releases/latest) from GitHub and this [token file]({token_msg.attachments[0].url})"
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
            pass

        # send the embed with a comfirmation message
        await ctx.send(embed=embed)


def setup(bot: commands.Bot):
    bot.add_cog(Tasks(bot))
