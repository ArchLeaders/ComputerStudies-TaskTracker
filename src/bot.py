import datetime
import json
import nextcord
import os
import pytz

from nextcord import Color
from nextcord.ext import commands
from pathlib import Path

#
# bot client and config constants
#

SRC: str = os.environ["SRC"]  # local and remote source directory
# m-dash added for mobile support
BOT = commands.Bot(command_prefix=('--', '—'))
BOT_NAME = 'Task Tracker'

# these will be client based when testing is complete
SERVER_ID: int = 929941418657595482
LOCAL_TIME = pytz.timezone('America/Vancouver')

# prompt images
NOTICE = 'https://cdn.discordapp.com/attachments/954237048275996693/954817191998546000/notice.png'
INVALID = 'https://cdn.discordapp.com/attachments/954237048275996693/954817192678010911/invalid.png'
WARNING = 'https://cdn.discordapp.com/attachments/954237048275996693/954817192199847936/warning.png'
HELP = 'https://cdn.discordapp.com/attachments/954237048275996693/954817192426360952/help.png'
ACESS_DENIED = 'https://cdn.discordapp.com/attachments/954237048275996693/954893350878724118/access_denied.png'

# prompt colors
NOTICE_COLOR = Color(0x33DD85)
INVALID_COLOR = Color(0xE03F5A)
WARNING_COLOR = Color(0xEDC740)
HELP_COLOR = Color(0x57A3ED)


#
# helper functions
#

def update_database(database: str, obj):
    Path(f'{SRC}/database/{database}.json').write_text(json.dumps(obj, indent=4))l


def load_database(database: str):
    return json.loads(Path(f'{SRC}/database/{database}.json').read_text())


def role(ctx: commands.Context, id: int) -> nextcord.Role:
    return ctx.guild().get_role(id)


def is_local_host() -> bool:
    try:
        # import local file to register the discord token
        # this will fail if it's on a remote host
        import env

        # add TOKEN and SRC
        env.reg()

    except:
        return False


#
# on ready events
#

for file in Path(f'{SRC}/src/cogs').glob('**/*.py'):
    """Load cogs in the cog directory"""

    if file.is_file() and '_view.py' not in file.name:
        # format name
        if not is_local_host():
            name = file.as_posix().replace('.py', '').replace(
                '/app/src/cogs/', '').replace('/', '.')
        else:
            name = file.as_posix().replace('.py', '').replace(
                '/', '.').replace('src.cogs.', '')

        # load cog
        BOT.load_extension(f'cogs.{name}')

        # log success
        print(f'Loaded: cogs.{name}')


@BOT.event
async def on_ready():
    # set status
    await BOT.change_presence(activity=nextcord.Activity(type=nextcord.ActivityType.listening, name='tasks...'))

    # log online status to the console
    print(f'{BOT.user} has started.')

    # get running server id
    # SERVER_ID should be a get function not a constant
    #
    # SERVER_ID = ctx.guild()

# run bot client
BOT.run(os.environ['TOKEN'])
