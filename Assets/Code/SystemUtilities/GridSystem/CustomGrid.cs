using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid<TGridObject>
{
    protected GridData _gridData;

    protected int _width;
    protected int _height;
    protected float _cellSize;
    protected Vector3 _originPosition;

    protected TGridObject[,] _gridArray;

    protected int _referenceX;
    protected int _referenceZ;

    public int Width => _width; 
    public int Height => _height; 
    public float CellSize => _cellSize;

    public event EventHandler<OnGridValueChangeEventArgs> OnGridValueChanged;

    public CustomGrid(GridData gridData, Func<CustomGrid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        _gridData = gridData;

        _width = _gridData.Width;
        _height = _gridData.Height;
        _cellSize = _gridData.CellSize;
        _originPosition = _gridData.OriginPosition;

        _gridArray = new TGridObject[_width, _height];

        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < _gridArray.GetLength(1); z++)
            {
                _gridArray[x, z] = createGridObject(this, x, z);
            }
        }

        if(_gridData.DebugDraw)
            DrawGrid();
    }

    protected void InvokeValueChanged(int x, int z) 
    {
        OnGridValueChanged?.Invoke(this, new OnGridValueChangeEventArgs(x, z));
    }

    public virtual Vector3 GetCellWorldPosition(int x, int z)
    {
        return Vector3.zero;
    }

    protected virtual void GetXZFromWorldPosition(Vector3 worldPosition, out int x, out int z)
    {
        x = 0;
        z = 0;
    }

    public TGridObject GetCellObject(int x, int z)
    {
        if (x < 0) return default;

        if (z < 0) return default;

        if (x >= _width) return default;

        if (z >= _height) return default;

        return _gridArray[x, z];
    }

    public TGridObject GetCellObject(Vector3 worldPosition)
    {
        GetXZFromWorldPosition(worldPosition, out _referenceX, out _referenceZ);

        return GetCellObject(_referenceX, _referenceZ);
    }

    protected virtual void SetCellObject(int x, int z, TGridObject value)
    {
        if (x < 0) return;

        if (z < 0) return;

        if (x >= _width) return;

        if (z >= _height) return;

        _gridArray[x, z] = value;
    }

    protected void SetCellObject(Vector3 worldPosition, TGridObject value)
    {
        GetXZFromWorldPosition(worldPosition, out _referenceX, out _referenceZ);

        SetCellObject(_referenceX, _referenceZ, value);
    }

    public virtual void AssingNeighborsPositions(int x, int z, List<Vector2Int> gridPositions)
    { 
    
    }

    protected virtual void DrawGrid()
    {
        
    }
}
