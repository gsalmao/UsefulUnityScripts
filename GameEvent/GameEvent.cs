using System;
using UnityEngine;

/// <summary>
/// Game Events: using events as scriptable objects. Also, you can use the '*' and
/// the '/' operators to subscribe methods that will automatically unsubscribe,
/// once they are executed. Much freedom, such power!
/// </summary>
[CreateAssetMenu(fileName = "GameEvent", menuName = "Scriptable Objects/Game Event")]
public class GameEvent : ScriptableObject
{
    private Action listeners;
    private Action listenersExecuteAndUnsubscribe;

    public virtual void Invoke()
    {
        listeners?.Invoke();
        listenersExecuteAndUnsubscribe?.Invoke();
        listenersExecuteAndUnsubscribe = delegate { };
    }

    /// <summary>
    /// Subscribe a method to a GameEvent.
    /// </summary>
    public static GameEvent operator +(GameEvent gameEvent, Action action)
    {
        gameEvent.listeners += action;
        return gameEvent;
    }

    /// <summary>
    /// Unsubscribe a method from a GameEvent.
    /// </summary>
    public static GameEvent operator -(GameEvent gameEvent, Action action)
    {
        gameEvent.listeners -= action;
        return gameEvent;
    }

    /// <summary>
    /// Subscribe a method to a GameEvent, to be played once.
    /// </summary>
    public static GameEvent operator *(GameEvent gameEvent, Action action)
    {
        gameEvent.listenersExecuteAndUnsubscribe += action;
        return gameEvent;
    }

    /// <summary>
    /// Unsubscribe a method from a GameEvent, that was to be executed once.
    /// </summary>
    public static GameEvent operator /(GameEvent gameEvent, Action action)
    {
        gameEvent.listenersExecuteAndUnsubscribe -= action;
        return gameEvent;
    }
}