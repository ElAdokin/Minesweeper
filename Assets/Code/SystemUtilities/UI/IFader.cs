using System;

public interface IFader
{
    event Action<bool, bool> OnFadeEnd;
}