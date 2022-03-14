using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : SerializedMonoBehaviour
{
    [ListDrawerSettings(NumberOfItemsPerPage = 1)]
    public List<Quest> quests;


    private void Awake()
    {
        InitAllQuests();
    }

    public void InitAllQuests()
    {
        foreach (Quest quest in quests)
            if (quest.beginActive)
                quest.Init();
    }

    public void InitQuestOnIndex(int index)
    {
        quests[index].Init();
    }

    public int GetIndexOfQuest(Quest selectedQuest)
    {
        return quests.IndexOf(selectedQuest);
    }
}
