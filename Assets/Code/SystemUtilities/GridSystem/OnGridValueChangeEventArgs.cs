using System;

public class OnGridValueChangeEventArgs : EventArgs
{
    private int _x;
    private int _z;

    public OnGridValueChangeEventArgs(int x,int z)
    {
        _x = x;
        _z = z;
    }

    public int X => _x; 
    public int Z => _z; 
}
