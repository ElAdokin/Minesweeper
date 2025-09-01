using System;

public class UndefinedMethod
{
    private Action _method;

    public UndefinedMethod(Action method)
    {
        _method = method;
    }

    public void CallMethod()
    {
        _method?.Invoke();
    }
}
