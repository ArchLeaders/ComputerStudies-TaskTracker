import bot
import json
import nextcord
import os
import pytz

from nextcord import Color, Intents
from nextcord.ext import commands
from pathlib import Path


def is_local_host() -> bool:
    try:
        # import local file to register the discord token
        # this will fail if it's on a remote host
        import env

        # add TOKEN and SRC
        env.reg()

        return True

    except:
        return False


#
# bot client and config constants
#

# get host service
is_local_host()

SRC: str = os.environ["SRC"]  # local and remote source directory
BOT = commands.Bot(
    command_prefix=("-"), intents=Intents.all()
)  # m-dash added for mobile support
BOT_NAME = "Task Tracker"

# these will be client based when testing is complete
CHANNEL_DB: int = 961123912505258024
CHANNEL_NEW_REG: int = 975930357545316393
LOCAL_TIME = pytz.timezone("America/Vancouver")

# prompt images
NOTICE = "https://cdn.discordapp.com/attachments/954237048275996693/954817191998546000/notice.png"
INVALID = "https://cdn.discordapp.com/attachments/954237048275996693/954817192678010911/invalid.png"
WARNING = "https://cdn.discordapp.com/attachments/954237048275996693/954817192199847936/warning.png"
HELP = "https://cdn.discordapp.com/attachments/954237048275996693/954817192426360952/help.png"
ACESS_DENIED = "https://cdn.discordapp.com/attachments/954237048275996693/954893350878724118/access_denied.png"

# prompt colors
NOTICE_COLOR = Color(0x33DD85)
INVALID_COLOR = Color(0xE03F5A)
WARNING_COLOR = Color(0xEDC740)
HELP_COLOR = Color(0x57A3ED)


#
# helper functions
#


def update_database(database: str, obj, ext: str = None):
    _ext = "json" if ext else ext
    Path(f"{SRC}/database/{database}.{ext}").write_text(json.dumps(obj, indent=4))


def load_database(database: str, ext: str = None):
    _ext = ext if ext else "json"
    return json.loads(Path(f"{SRC}/database/{database}.{_ext}").read_text())


def role(ctx: commands.Context, id: int) -> nextcord.Role:
    return ctx.channel.guild.get_role(id)


#
# on ready events
#


def load_cogs():
    for file in Path(f"{SRC}/src/cogs").glob("**/*.py"):
        """Load cogs in the cog directory"""

        try:
            name = "-"
            if file.is_file() and "_view.py" not in file.name:
                # format name
                name = (
                    file.as_posix()
                    .replace(".py", "")
                    .replace("/", ".")
                    .replace("src.cogs.", "")
                )

                # load cog
                BOT.load_extension(f"cogs.{name}")

                # log success
                print(f"Loaded: cogs.{name}")
        except Exception as ex:
            print(f"Failed to load {file} with name {name} as cog.\n{ex}")
            pass


@BOT.event
async def on_ready():
    # set status
    await BOT.change_presence(
        activity=nextcord.Activity(type=nextcord.ActivityType.listening, name="tasks")
    )

    # log online status to the console
    print(f"{BOT.user} has started.")
    load_cogs()


# run bot client
BOT.run(os.environ["TOKEN"])
