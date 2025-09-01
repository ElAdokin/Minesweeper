using UnityEngine;

public struct DirectionData
{
    private Direction _direction;
    private Vector3 _directionVector;

    public DirectionData(Direction direction, Vector3 directionVector)
    {
        _direction = direction;
        _directionVector = directionVector;
    }

    public Vector3 DirectionVector { get => _directionVector;}
    public Direction Direction { get => _direction;}
}