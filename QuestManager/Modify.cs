using Sirenix.OdinInspector;
using UnityEngine;

/*
 * 
 *          THIS SCRIPT USES ODIN INSPECTOR
 * 
 */

/// <summary>
/// Modify something within the game, when called. Can be anything. This class started as a questTask modifier
/// (Something to happen when you complete a task, such as change a dialogue, for example). Turned out to be
/// a much more global utility. Very useful for reward lists, editable through the inspector.
/// </summary>
[System.Serializable]
public abstract class Modify
{    
    public abstract void Execute();
}

public class GameObjectMod : Modify
{
    [LabelWidth(70)]
    public bool isActive;
    [LabelWidth(95)]
    public GameObject gameObject;
    public override void Execute()
    {
        gameObject.SetActive(isActive);
    }
}

public class RigidbodyMod : Modify
{
    [LabelWidth(80)]
    public Rigidbody rigidbody;
    [LabelWidth(50)]
    public RigidbodyModType type;

    
    public override void Execute()
    {
        switch (type)
        {
            case RigidbodyModType.FreezePosition:
                rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                break;
            case RigidbodyModType.UnfreezePosition:
                rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                break;
        }
    }

    public enum RigidbodyModType
    {
        FreezePosition,
        UnfreezePosition,

    }
}

public class AnimatorMod : Modify
{
    [LabelWidth(80)]
    public Animator animator;
    [LabelWidth(85)]
    public string animName;

    public override void Execute()
    {
        animator.Play(animName);
    }
}

public class PlayerReceiveMod : Modify
{
    [LabelWidth(80)]
    public int influence;
    [LabelWidth(80)]
    public int stamina;
    [LabelWidth(80)]
    public int money;
    [LabelWidth(80)]
    public ItemInfo item;
    public override void Execute()
    {
        if (influence != 0)
            PlayerInfluence.Change(influence);
        if (stamina != 0)
            PlayerStamina.Change(stamina);
        if (money != 0)
            PlayerWallet.Change(money);
        if (item != null)
            Inventory.AddItem(item);
    }
}

public class DialogueMod : Modify
{
    [LabelWidth(120)]
    public InteractDialogue interactDialogue;
    [LabelWidth(120)]
    public bool startDialogue;
    [LabelWidth(40)]
    public string tag;

    public override void Execute()
    {
        interactDialogue.ChangeDialogue(tag);
        if(startDialogue)
            interactDialogue.Interaction();
    }
}

public class SetNpcFsmStateMod : Modify
{
    [InfoBox("Target is not required for Idle State")]
    [LabelWidth(50)]
    public BaseFSM fsm;
    [LabelWidth(50)]
    public FSMEnums.BaseFSMState state;
    
    public Transform target;
    public override void Execute()
    {
        fsm.EnterBaseFSMState(state, target);
    }
}