using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class Quest
{
    public Quest()
    {
        questName   = "Nome da Quest";
        description = "Breve descrição da Quest, possivelmente entrará no jogo.";
        currentTask = 0;
        tasks       = new List<Task>();
        beginActive = true;
        finished    = false;
    }

    [ColoredFoldoutGroup("Quest Status", 0f, 0f, 1f)]
    public string questName;
    [ColoredFoldoutGroup("Quest Status", 0f, 0f, 1f)]
    public Color questColor;
    public bool beginActive;
    [ColoredFoldoutGroup("Quest Status", 0f, 0f, 1f)]
    [ReadOnlyAttribute] public bool finished;
    [ColoredFoldoutGroup("Quest Status", 0f, 0f, 1f)]
    [ReadOnlyAttribute] public int currentTask;


    [TextArea(5, 10)]
    [ColoredFoldoutGroup("Quest Status", 0f, 0f, 1f)]
    public string description;
    [GUIColor(0f,1f,1f)]
    [ListDrawerSettings(NumberOfItemsPerPage = 1)]
    [SerializeReference] public List<Task> tasks;

    public void Init()
    {
        InitTask();
    }

    public void NextTask()
    {
        FinishTask();

        if (currentTask < tasks.Count - 1)
        {
            currentTask++;
            InitTask();
        }
        else
            FinishQuest();
    }
    private void FinishQuest()
    {
        finished = true;
    }

    private void InitTask()
    {
        tasks[currentTask].Init();
        tasks[currentTask].OnCompleteTask += NextTask;
    }

    private void FinishTask()
    {
        tasks[currentTask].OnCompleteTask -= NextTask;
        tasks[currentTask].Finish();
    }

}
