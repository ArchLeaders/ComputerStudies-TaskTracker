import random

from bot import INVALID_COLOR, NOTICE, NOTICE_COLOR
from nextcord import Embed
from nextcord.ext import commands
from utils import get_image, get_settings_data, get_embed_message

SERVER_CONFIG = None
SERVER_CONFIG_ID: int = 0


class Info(commands.Cog):
    """Tracking commands"""

    def __init__(self, bot: commands.Bot):
        self.bot = bot

    @commands.command()
    async def tasks(self, ctx: commands.Context):
        """Get a list of active tasks running in the server."""

        settings = await get_settings_data(ctx.guild.id)
        embeds: list = []
        i: int = 0

        for task in settings["tasks"]:

            i += 1
            msg: str = task["Message"]
            sequence_str: str = ""

            for sequence_name, sequence_data in task["Sequence"].items():
                sequence_str += f"**[{sequence_name}]():\u2002**"
                sequence_str += f"{', '.join(sequence_data)} ({sequence_data[task['Session'][sequence_name]]} : {task['Session'][sequence_name]+1})\n"

            if len(msg) > 200:
                msg = f"{msg[0:197]}..."

            # create help embed
            embed: Embed = Embed(
                color=NOTICE_COLOR,
                title=task["Time"],
                description=msg,
            )
            embed.set_thumbnail(url=NOTICE)
            embed.set_image(url=get_image())
            embed.set_footer(text=f"Task number {i}.")
            embed.add_field(name="Sequences:", value=sequence_str)
            embeds.append(embed)

        await ctx.channel.send(embeds=embeds)

    @commands.command()
    async def task(self, ctx: commands.Context, task_number: int = None):
        """Get a info on a specific tasks in the server."""

        if not task_number:
            await ctx.send(
                embed=Embed(
                    title="Error",
                    description=f"You must specify a task number to get info on that task.",
                    colour=INVALID_COLOR,
                ),
                reference=ctx.message,
            )
            return

        settings = await get_settings_data(ctx.guild.id)
        tasks = settings["tasks"]

        if len(tasks) < task_number:
            await ctx.send(
                embed=Embed(
                    title="Error",
                    description=f"Could not find task number {task_number}.",
                    colour=INVALID_COLOR,
                ),
                reference=ctx.message,
            )
            return
        else:
            task = tasks[task_number - 1]

        # create embed
        embed: Embed = Embed(
            color=NOTICE_COLOR,
            title=task["Time"],
            description=get_embed_message(task["Message"]),
        )
        embed.set_thumbnail(url=NOTICE)
        embed.set_image(url=get_image())
        embed.set_footer(text=f"Task number {task_number}.")

        await ctx.channel.send(embed=embed)


def setup(bot: commands.Bot):
    bot.add_cog(Info(bot))
