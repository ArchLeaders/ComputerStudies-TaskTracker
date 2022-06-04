from nextcord import Embed
from nextcord.ext import commands
from bot import INVALID_COLOR, NOTICE, NOTICE_COLOR
from utils import get_image, get_settings_data, set_settings_data


class TaskEdit(commands.Cog):
    """Tracking editing commands"""

    def __init__(self, bot: commands.Bot):
        self.bot = bot

    @commands.command()
    async def skip(self, ctx, task_number: int = None, sequence: str = None):

        # for intellisence and function formatting
        ctx: commands.Context = ctx

        if not task_number:
            await ctx.channel.send(
                embed=Embed(
                    title="Error",
                    description="You must specify a task number and sequence name to use the __skip__ command.\n**-skip** __`task number`__ `sequence name`",
                    colour=INVALID_COLOR,
                ),
                reference=ctx.message,
                ephemeral=True,
            )
            return

        settings = await get_settings_data(ctx.guild.id)
        tasks = settings["tasks"]
        if len(tasks) < task_number:
            await ctx.send(
                embed=Embed(
                    title="Error",
                    description=f"Could not find task {task_number}.",
                    colour=INVALID_COLOR,
                ),
                reference=ctx.message,
                ephemeral=True,
            )
            return

        task = tasks[task_number - 1]

        if not sequence:
            await ctx.channel.send(
                embed=Embed(
                    title="Error",
                    description="You must specify a sequence to skip.\n**-skip** `task number` __`sequence name`__\n\n"
                    + f"Available sequence names in task {task_number}: `{', '.join(task['Sequence'].keys())}`",
                    colour=INVALID_COLOR,
                ),
                reference=ctx.message,
                ephemeral=True,
            )
            return

        if sequence not in task["Session"]:
            await ctx.send(
                embed=Embed(
                    title="Error",
                    description=f"Could not find sequence __{sequence}__ in the task {task_number}."
                    + f"\nAvailable sequence names in task {task_number}: `{', '.join(task['Sequence'].keys())}`",
                    colour=INVALID_COLOR,
                ),
                reference=ctx.message,
                ephemeral=True,
            )
            return

        # advance session
        task["Session"][sequence] = (
            task["Session"][sequence] + 1
            if len(task["Sequence"][sequence]) > (task["Session"][sequence] + 1)
            else 0
        )

        await set_settings_data(ctx.guild.id, settings)

        # create help embed
        embed: Embed = Embed(
            color=NOTICE_COLOR,
            title="Success!",
            description=f"The sequence __{sequence}__ was moved to the next user.",
        )
        embed.add_field(
            name=f"Active user in: {sequence}",
            value=f"Full sequence: `{', '.join(task['Sequence'][sequence])}`\n"
            + f"Current: `{task['Sequence'][sequence][task['Session'][sequence]]} ({task['Session'][sequence]})`",
        )
        embed.set_thumbnail(url=NOTICE)
        embed.set_image(url=get_image())
        embed.set_footer(text=f"Task number: {task_number} | Sequence name: {sequence}")

        await ctx.channel.send(embed=embed, reference=ctx.message, ephemeral=True)

    @commands.command()
    async def replace(self, ctx: commands.Context):
        await ctx.channel.send(
            "I ain't ready yet. Give me a month or two :man_shrugging:",
            reference=ctx.message,
            ephemeral=True,
        )


def setup(bot: commands.Bot):
    bot.add_cog(TaskEdit(bot))
