from nextcord.ext import commands, tasks

from bot import BOT, load_database

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

        meta = load_database('example', 'jsonc')['test']

        for num in meta:
            await BOT.get_channel(935399743557881896).send(num)


def setup(bot: commands.Bot):
    bot.add_cog(Events(bot))