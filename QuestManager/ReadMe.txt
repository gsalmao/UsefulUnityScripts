Important: some of the derived classes use Actions from outside classes. Make sure to
remove them to put it into your project.

A QuestManager is made of Quests.

A quest is made from tasks.

Every task, once it is completed, execute rewards, through the Modify class.

This design was meant to be implemented to create a nice Inspector list with
serialized abstract classes.