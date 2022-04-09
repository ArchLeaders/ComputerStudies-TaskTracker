# TaskTracker – Prototype Outline
> [Source Code](https://github.com/ArchLeaders/ComputerStudies-TaskTracker)

This prototype is made up of three pieces, each one a key part in the product.
- Data Storage
- Task Handling
-	Task Configurator (SetupWizard)

<br>

## Data Storage (DS) - Task per server
> [Commits](https://github.com/ArchLeaders/ComputerStudies-TaskTracker/commit/503666f9f23b7d3519bb647075f7a3c95276a597)

For task tracker to function on a mass scale, it needs to have definable tasks
than run on a per-server basis.
This means the information for each server’s tasks need to be stored somewhere.

This section goes through each attempt of solving this issue.

<br>

### DS: First Iteration

The first iteration used a global database to store server data.
I worked on this concept for some time before realizing I obviously
could not store a database of possibly private information that I could access.

<br>

### DS: Second Iteration

The second and last iteration uses a mix of local data and a global list to define the server data (tasks).
As an example, to register the bot, you would first invite it via a special link. Once in the server run
the register command (as will be documented) to register your server.
This command will write the default settings to a private (excluding server admins/owner) channel and
add the ID to the global list, after that completes, a help message to get started with the SetupWizard is displayed to the user.

![register help - scrs](https://user-images.githubusercontent.com/80713508/162562397-5645d2a6-6704-49c0-8234-0ad6fe7ceb8f.png)

---

**Global List**

The global list is a JSON file hosted on a private discord server.
The file contains no personal information, only a dictionary of channel IDs with a value of 0.
It is created like this so no duplicates can be made.

```json
{
  "961119872505487360": 0,
  "961334714633965569": 0
}
```

Each channel ID references a channel in which a local settings file exists.<br>
When the code to execute tasks runs, it iterates through this dictionary and adds a new async function for each channel.
The async function downloads the local settings and parses it,
if any tasks are set to run at `DateTime.Now` they are executed accordingly.

<br>

**Local Settings**

The local settings file is also a JSON file, but it is hosted in the server (local to that server).
This file contains two primary indexes, "vars" and "tasks" and one property "timezone".
Vars contains user defined variables that can be used in the task message.
Tasks define what happens at any point in time. Only a basic notification system is
implemented as of now, but over time it could expand to support a broader range of things.

```jsonc
{
  "timezone": "America/Vancouver",
  "vars": {
    "user_m": "Marcus",
    "user_a": "Alex",
    "user_m_full": "Marcus J. Smith",
    "user_a_full": "Alex E. Smith"
  },
  "tasks": {
    "1:20PM": {
      
      // Days on which the task executes
      "days": [ "sun", "mon", "fri", "sun" ],
      
      // Channel in which to send the message
      "channel": 935399672988712973,
      
      // Role that will be pinged with @role
      "role": 935400272463810620,
      
      // User that will be pinged with @user
      "user": 783100922967621653,
      
      // this sequence defines which user is mentioned with $user in the message
      // in this example, user_m will be notified on Sunday and Friday and user_a
      // is notified on Monday and Saturday (assuming the bot started on 0)
      "sequence": [ "user_m", "user_a" ],
      
      // time_now is a default variable, these are used to define dynamic objects
      // such as the current time or date
      "message": "@role $user needs to do something at $time_now!"
    }
  }
}
```

All this data will be formatted by the SetupWizard where you can add/remove tasks and variables.

<br>

## Task Tracking (TT) – Sending reports per task
> [Commits](https://github.com/ArchLeaders/ComputerStudies-TaskTracker/commit/17b74d4ffece761a7d9ba06aeb191c93b60472bd)

Efficiently tracking tasks is essential for this program.
Because it is done so often (every minute), it needs to execute quickly to be finished before the minute is up.
This will be done with asyncio, a python package for thread management.

The following section goes through each attempt of solving this issue.

### TT: First Iteration

The first and only iteration in this section proposes a system where each server
is handled on its own thread, so they are all done at the same time.

```py
@tasks.loop(minutes=1)
async def read_tasks(self):
    """Reads through the staged tasks"""
    
    # get the server list
    servers = BOT.get_channel(DB_CHANNEL).last_message.attachments[0].read()
    servers = json.loads(servers)
    
    # create list of python tasks
    py_tasks = []
    
    # iterate the servers and handle each server accordingly
    for server, _ in servers:
        py_tasks.append(asyncio.create_task(self.handle_server(server)))
        
    # await all tasks
    asyncio.gather(*py_tasks)
```

The handle_server function parses the local settings and iterates the tasks with a similar method as defined above.

```py
async def handle_server(self, id: int):

    # get local settings
    settings = BOT.get_channel(id).last_message.attachments[0].read()
    settings = json.loads(settings)

    for time, task in settings['tasks']:

        # skip tasks that are not set for the current time
        if time != self.get_time(settings['timezone'], '%I:%m%p'):
            continue

        # check the day of the week
        if self.get_time(settings['timezone'], '%I:%m%p') not in task['days']:
            continue

        # get the task channel and message
        channel = BOT.get_channel(task['channel'])
        message = task['message']

        # iterate vars replacing every instance found in the message
        for var in settings['vars']:

            # needs to be improved to support proposed features
            str(message).replace(f'${var}', str(var)
                ).replace('@role', f'<@&{task["role"]}>'
                ).replace('@user', f'<@{task["user"]}>')

        # send the message in channel
        channel.send(message)
```



















