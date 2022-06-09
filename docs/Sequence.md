# Task Tracker - Sequences

### What are _sequences_ and how do they work?

Sequences are something like rotational schedules. Suppose you have a task that says you need to clean the dishes at `2:00 PM`.<br>
In real life, three people do this job on rotation. (`user 1` > `user 2` > `user 3` > _`loop back`_).<br>
Sequences keep track of who needs to do the task, and when the task is executed, it's moved to the next person.

<br>

### How do I add a sequence?

In the `Sequence` field, sequences can be typed using this syntax: `sequence_name(item_1, item_2);`.<br>
An example of this would be: `dishes(Mark, John, Paul);`. When the task first runs, `Mark` will be used, and the next time `John` will be used, etc.

After defining a new sequence, you can call it in your message just like variables.<br>
For example `$dishes needs to do the dishes!` will be translated to `Mark needs to do the dishes!` when Task Tracker executes it.

<br>

### Rules

1. Any amount of items can be added to a single sequence.
2. Duplicate sequence names can be used, but the last typed one (left to right) will always overwrite the previous one.
3. Sequences must be seperated with a semicolin. Example: `sequence(a, b, c); sequence2(d, e, f);`.
4. Sequence names are case sensitive and must <ins>not</ins> contain any spaces, use a hyphen or underscore instead.
5. You can use variables as a sequence item. Note that the usual syntax used in the mesaages (prefixing the variable name with `$`) is not required and will not work.

<br>

### Current Issues/Bugs

- Spaces in sequence items will be removed. To fix this temporarily, create a variable with spaces and use that variable in the sequence.
