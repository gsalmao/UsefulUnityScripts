using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

/// <summary>
/// Base Task class
/// </summary>
[System.Serializable]
public abstract class Task
{
    public Task()
    {
        rewards = new List<Modify>();
    }

    public event Action OnCompleteTask = delegate { };

    public abstract void Init();
    public virtual void Finish()
    {
        //Some tasks don't require a finish method, so I made it as virtual
    }

    [TextArea(3,10)]
    public string taskDescription;
    [GUIColor(0f,1f,0f)]
    [SerializeReference] public List<Modify> rewards;

    public void TaskCompleted()
    {
        FMODUnity.RuntimeManager.PlayOneShot(Consts.Sound.QuestTaskDone);
        foreach (Modify reward in rewards)
            reward.Execute();
        QuestsMenu.DrawTaskStatic();
        OnCompleteTask();
    }
}

public class TakeSelfiesTask : Task
{
    [LabelWidth(105)]
    public int selfiesAmount;

    private int counter;

    public override void Init()
    {
        counter = 0;
        InteractFan.OnTakePhoto += SelfieCounter;
    }

    private void SelfieCounter()
    {
        counter++;
        if (counter >= selfiesAmount)
            TaskCompleted();
    }
}

public class UseItemTask : Task
{
    public ItemInfo item;
    public override void Init()
    {
        Inventory.OnUseItem += CheckItem;
    }

    private void CheckItem(ItemInfo itemUsed)
    {
        if (itemUsed == item)
        {
            Inventory.OnUseItem -= CheckItem;
            TaskCompleted();
        }
    }
}

public class TalkWithTask : Task
{
    public InteractDialogue interactDialogue;

    public override void Init()
    {
        interactDialogue.OnThisDialogueFinished += TaskCompleted;
        //subject.GetComponentInChildren<CharIcon>()?.ShowIcon(CharEnum.Icon.Exclamation, questRef.color);
    }

    public override void Finish()
    {
        base.Finish();
        interactDialogue.OnThisDialogueFinished -= TaskCompleted;
    }
}

public class UseSkillTask : Task
{
    public override void Init()
    {
        SkinHead.OnUseSkill += TaskCompleted;
    }

    public override void Finish()
    {
        base.Finish();
        SkinHead.OnUseSkill -= TaskCompleted;
    }
}

public class ThrowItemsAwayTask : Task
{
    public List<ItemInfo> items;
    private List<ItemInfo> remainingItems;

    public override void Init()
    {
        remainingItems = new List<ItemInfo>();
        remainingItems = items;
        InteractGiveItem.OnThrowAwayItem += CheckItemsThrownAway;
    }

    private void CheckItemsThrownAway(Item item)
    {
        if (remainingItems.Contains(item.itemInfo))
            remainingItems.Remove(item.itemInfo);
        if (remainingItems.Count == 0)
        {
            InteractGiveItem.OnThrowAwayItem -= CheckItemsThrownAway;
            TaskCompleted();
        }
    }
}

public class ScareTask : Task
{
    public Character character;
    public override void Init()
    {
        character.OnScare += TaskCompleted;
        //character.gameObject.GetComponentInChildren<CharIcon>()?.ShowIcon(CharEnum.Icon.Exclamation, questRef.color);
    }

    public override void Finish()
    {
        base.Finish();
        character.OnScare -= TaskCompleted;
        //character.gameObject.GetComponentInChildren<CharIcon>()?.HideIcon();
    }
}

public class GiveItemTask : Task
{
    public InteractGiveItem interactGiveItem;
    public InteractDialogue interactDialogue;
    public ItemInfo itemInfo;
    public string nextTag;

    public override void Init()
    {
        interactGiveItem.givableItems.Add(itemInfo);
        interactGiveItem.OnReceiveItem += SetNextDialogue;
        //interactGiveItem.gameObject.GetComponentInChildren<CharIcon>()?.ShowIcon(CharEnum.Icon.Exclamation, questRef.color);
    }

    private void SetNextDialogue(Item receivedItem)
    {
        if (receivedItem.itemInfo == itemInfo)
        {
            interactDialogue.ChangeDialogue(nextTag);
            InventoryController.OnCloseInventory += interactDialogue.Interaction;
            DialogueManager.OnDialogueFinished += TaskCompleted;
        }
    }

    public override void Finish()
    {
        base.Finish();
    }
}

public class TakeItemTask : Task
{
    public InteractableItem interactableItem;
    public override void Init()
    {
        interactableItem.OnPickUpItem += TaskCompleted;
    }

    public override void Finish()
    {
        base.Finish();
        interactableItem.OnPickUpItem -= TaskCompleted;
    }
}

public class DashTask : Task
{
    public InteractableObject interactableObject;

    public override void Init()
    {
        interactableObject.OnReceiveDash += TaskCompleted;
    }

    public override void Finish()
    {
        base.Finish();
        interactableObject.OnReceiveDash -= TaskCompleted;
    }
}

public class MoveToTask : Task
{
    public QuestLocal questLocal;
    public GameObject gameObject;

    public override void Init()
    {
        questLocal.objectToMove = gameObject;
        questLocal.OnMovedToLocal += TaskCompleted;
    }

    public override void Finish()
    {
        base.Finish();
        questLocal.objectToMove = null;
        questLocal.OnMovedToLocal -= TaskCompleted;
    }
}