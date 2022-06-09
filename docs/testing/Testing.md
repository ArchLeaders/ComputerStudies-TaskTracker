# Task Tracker Testing Process

To complete the test process, check that you have the requirements and proceed to follow the instructions below.<br>
For each step, note the results in [this word document](https://github.com/ArchLeaders/TaskTracker/raw/master/docs/res/TaskTrackerTestingProcess.docx).

When documenting the results of each step, compare your results with the expected results noted in the provided [word document](https://github.com/ArchLeaders/TaskTracker/raw/master/docs/res/TaskTrackerTestingProcess.docx). Then assign a criteria for each step.
Add any additional information in the notes section.

**Requirments<br>**

- <a target="_blank" href="https://discord.com">Discord</a> with a verified account.
- PC running Windows 10 or 11.

## Criteria

| Perfect | Good | OK | Poor | Doesn't Work |
| ------- | ---- | -- | ---- | ------------ |
| The product works exactly as intended and expresses good usability and productivity. | The product works as intended and expresses good usability. | The product works close to what was originally intended but has room for improvement. | The product works but is unusable. | The product does not work at all. |

<br>

# Test Instructions

### 1. Setup/Registration

- Create a <a target="_blank" href="https://user-images.githubusercontent.com/80713508/171991196-67e71963-f645-4047-b02d-29bc1bbd84d5.png">new server</a> within Discord

- Use <a target="_blank" href="https://discord.com/api/oauth2/authorize?client_id=935398186258939944&permissions=8&scope=bot">this link</a> to <a target="_blank" href="https://user-images.githubusercontent.com/80713508/171992223-3ebb7e3e-e63d-4727-bde7-67a9336dc62d.png">add Task Tracker</a> to your server.

- In your newly created Discord server, send <a target="_blank" href="https://user-images.githubusercontent.com/80713508/171992200-f8ed0b08-1734-4540-a091-da3763209dc9.png">`-register`<a> in any text channel to register the bot.

- Follow the instructions in the help message.

<br>

### 2. Creating a Task

- Create a new task and set every property (exluding the `Sequence` and `Time`) to anything.

- In that task, edit the `Time` to be the next minute and save.

- Wait for the task to trigger and document the results.

<br>

### 3. Modify a Task in Discord

- After the task executes, go back into the **Setup Wizard** and set the [`Sequence`](https://github.com/ArchLeaders/TaskTracker/blob/master/docs/Sequence.md) property of any existing task.

- After **saving** the changes, type `-tasks` in your Discord server to show a list of tasks on this server.

- Using that information, type `-skip task_number sequence_name`, `task_number` being the task you wish to edit, and `sequence_name` as the name of the sequence you are skipping.

- Wait for the **command** to complete and document the results.
