using System;

public class Optional<T>
{
    private readonly T value;

    public Optional(T value)
    {
        this.value = value;
    }

    public Optional()
    {

    }

    public T GetTypeFromOptional()
    {
        return value;
    }

    public void IfPresentDo(Action<T> consumer)
    {
        if (value != null)
        {
            consumer(value);
        }
    }

    public bool IfPresent()
    {
        if (this.value == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public T OrElseThrow(Exception exeption)
    {
        if (value == null)
        {
            throw exeption;
        }

        return value;
    }
}
