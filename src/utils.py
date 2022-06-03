import json
import random
import pytz

from bot import BOT, SRC
from datetime import datetime
from nextcord import TextChannel, PermissionOverwrite, File, Colour, Embed
from pathlib import Path


def get_hour(tz: str) -> int:
    return int(datetime.now(pytz.timezone(tz)).strftime("%I"))


def get_time(tz: str, strftime) -> str:
    return datetime.now(pytz.timezone(tz)).strftime(
        strftime.replace("%I", str(get_hour(tz)))
    )


async def get_settings_channel(server: int) -> TextChannel:

    # get guild
    guild = BOT.get_guild(server)

    # check for an existing task-tracker-metadata category
    create = True
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

    create = True

    # check for an existing server channel in the task-tracker-metadata category
    for channel in category.channels:
        if channel.name == "server":
            create = False
            settings_channel = channel

    # create a server channel in the task-tracker-metadata category
    if create == True:
        settings_channel: TextChannel = await guild.create_text_channel(
            "server", category=category
        )

        # create the default config file to the settings channel
        defaults = {"timezone": "", "vars": {}, "tasks": []}

        # upload the default config file to the settings channel
        file = Path(f"{SRC}\\tmp.io")
        file.write_text(json.dumps(defaults, indent=4))
        await settings_channel.send(file=File(fp=file, filename=f"server.io.json"))
        file.unlink()

    return settings_channel


async def get_settings_data(server: int):
    server_channel = (
        await (await get_settings_channel(server)).history(limit=1).flatten()
    )
    settings_data = await server_channel[0].attachments[0].read()
    return json.loads(settings_data)


async def set_settings_data(server: int, settings):
    file = Path(f"{SRC}\\tmp.io")
    file.write_text(json.dumps(settings, indent=4))
    await (await get_settings_channel(server)).send(
        file=File(fp=file, filename=f"server.io.json")
    )
    file.unlink()


def get_image(index: int = None) -> str:

    DEF: str = "https://cdn.discordapp.com/attachments/954237048275996693/982051674153685022/Embed.png"

    # made to be expandable
    images = [
        # "https://cdn.discordapp.com/attachments/954237048275996693/982051674153685022/Embed.png",
        # "https://cdn.discordapp.com/attachments/954237048275996693/982051674493419550/Embed0.png",
        # "https://cdn.discordapp.com/attachments/954237048275996693/982051674816409600/Embed1.png",
        # "https://cdn.discordapp.com/attachments/954237048275996693/982051675118383124/Embed2.png",
        # "https://cdn.discordapp.com/attachments/954237048275996693/982051675344867368/Embed3.png",
        "https://cdn.discordapp.com/attachments/954237048275996693/982060736148828190/Embed4.png",
    ]

    if index:
        if len(images) >= index:
            return images[index]
        else:
            return DEF

    image_index: int = random.randint(0, (len(images) - 1))
    return images[image_index]


def get_embed_message(msg: str) -> str:

    if msg:
        if len(msg) > 200:
            return f"{msg[0:197]}..."
        else:
            return msg
