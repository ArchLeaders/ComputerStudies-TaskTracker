import json
import os

from nextcord import Color
from nextcord.ext import commands
from pathlib import Path

#
# bot client and config constants
#

BOT = commands.Bot(command_prefix = ('--', '—')) # m-dash added for mobile support
BOT_NAME = 'Task Tracker'
SERVER_ID: int = 929941418657595482 # this will be client based when testing is complete
SRC: str = os.environ["SRC"] # local and remote source directory

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
    Path(f'{SRC}/database/{database}.json').write_text(json.dumps(obj, indent=4))


def load_database(database: str):
    return json.loads(Path(f'{SRC}/database/{database}.json').read_text())


def is_local_host() -> bool:
    try:
        # import local file to register the discord token
        # this will fail if it's on a remote host
        import env

        # add TOKEN and SRC
        env.reg()

    except: return False


# run bot client
BOT.run(os.environ['TOKEN'])