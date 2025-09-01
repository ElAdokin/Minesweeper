using System;

public interface IAnimationEvent
{
	event Action<int> OnAnimationEvent;
}
