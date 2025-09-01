using System;
using UnityEngine;

public class AnimationEventListener : MonoBehaviour, IAnimationEvent
{
	public event Action<int> OnAnimationEvent;
    public event Action OnExecuteEvent;
    public event Action OnMovementEvent;

    public void CallAnimationEvent(int index)
	{
		OnAnimationEvent?.Invoke(index);
	}

    public void CallExecuteEvent()
    {
        OnExecuteEvent?.Invoke();
    }

    public void CallMovementEvent()
    {
        OnMovementEvent?.Invoke();
    }
}
