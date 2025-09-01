using System;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid<TGridObject> : CustomGrid<TGridObject>
{
    private const float HEX_VERTICAL_OFFSET_MULTIPLIER = 0.75f;

    private int _roughX;
    private int _roughZ;

    private Vector3Int _roughXZ;
    private Vector3Int _closestXZ;

    private bool _oddRow;

    private Vector2Int _padding;

    public HexGrid(GridData gridData, Func<CustomGrid<TGridObject>, int, int, TGridObject> createGridObject) : base(gridData, createGridObject)
    {
    
    }

    public override Vector3 GetCellWorldPosition(int x, int z) 
    {
        return
            new Vector3(x, 0, 0) * _cellSize +
            new Vector3(0, 0, z) * _cellSize * HEX_VERTICAL_OFFSET_MULTIPLIER + 
            ((Mathf.Abs(z) % 2) == 1 ? new Vector3(1, 0, 0) * _cellSize * .5f : Vector3.zero) +
            _originPosition;
    }

    protected override void GetXZFromWorldPosition(Vector3 worldPosition, out int x, out int z) 
    {
        _roughX = Mathf.RoundToInt((worldPosition - _originPosition).x / _cellSize);
        _roughZ = Mathf.RoundToInt((worldPosition - _originPosition).z / _cellSize / HEX_VERTICAL_OFFSET_MULTIPLIER);

        _roughXZ = new Vector3Int(_roughX, 0, _roughZ);

        _oddRow = _roughZ % 2 == 1;

        List<Vector3Int> neighbourXZList = new List<Vector3Int> 
        {
             _roughXZ + new Vector3Int(-1, 0, 0),
             _roughXZ + new Vector3Int(+1, 0, 0),

             _roughXZ + new Vector3Int(_oddRow ? +1 : -1, 0, +1),
             _roughXZ + new Vector3Int(+0, 0, +1),

             _roughXZ + new Vector3Int(_oddRow ? +1 : -1, 0, -1),
             _roughXZ + new Vector3Int(+0, 0, -1),
        };

        _closestXZ = _roughXZ;

        foreach (Vector3Int neighbourXZ in neighbourXZList) 
        {
            if (Vector3.Distance(worldPosition, GetCellWorldPosition(neighbourXZ.x, neighbourXZ.z)) <
                Vector3.Distance(worldPosition, GetCellWorldPosition(_closestXZ.x, _closestXZ.z))) 
            {
                // Closer than closest
                _closestXZ = neighbourXZ;
            }

        }

        x = _closestXZ.x;
        z = _closestXZ.z;
    }

    protected override void SetCellObject(int x, int z, TGridObject value) 
    {
        base.SetCellObject(x, z, value);
        
        TriggerCellObjectChanged(x, z);
    }

    public void TriggerCellObjectChanged(int x, int z) 
    {
        InvokeValueChanged(x, z);
    }

    public Vector2Int ValidateCellPosition(Vector2Int gridPosition) 
    {
        return new Vector2Int(
            Mathf.Clamp(gridPosition.x, 0, _width - 1),
            Mathf.Clamp(gridPosition.y, 0, _height - 1)
        );
    }

    public bool IsValidCellPosition(Vector2Int gridPosition) 
    {
        _referenceX = gridPosition.x;
        _referenceZ = gridPosition.y;

        if (_referenceX < 0) return false;

        if (_referenceZ < 0) return false;

        if (_referenceX >= _width) return false;

        if (_referenceZ >= _height) return false;

        return true;
    }

    public bool IsValidCellPositionWithPadding(Vector2Int gridPosition) 
    {
        _padding = new Vector2Int(2, 2);
        
        _referenceX = gridPosition.x;
        _referenceZ = gridPosition.y;

        if (_referenceX >= _padding.x && _referenceZ >= _padding.y && _referenceX < _width - _padding.x && _referenceZ < _height - _padding.y)
            return true;
        
        return false;
    }

    public override void AssingNeighborsPositions(int x, int z, List<Vector2Int>gridPositions)
    {
        //Check Up-Right Neighbor
        if (z + 1 < _height)
        {
            if (x == _width - 1)
            {
                if (z % 2 == 0)
                    gridPositions.Add(new Vector2Int(x, z + 1));
            }
            else 
            {
                if (z % 2 == 0)
                    gridPositions.Add(new Vector2Int(x, z + 1));
                else
                    gridPositions.Add(new Vector2Int(x + 1, z + 1));
            }
        }

        //Check Right Neighbor
        if (x + 1 < _width)
            gridPositions.Add(new Vector2Int(x + 1, z));

        //Check Right-Down Neighbor
        if (z - 1 >= 0) 
        {
            if (x == _width - 1)
            {
                if (z % 2 == 0)
                    gridPositions.Add(new Vector2Int(x, z - 1));
            }
            else 
            {
                if (z % 2 == 0)
                {
                    gridPositions.Add(new Vector2Int(x, z - 1));
                }
                else
                {
                    gridPositions.Add(new Vector2Int(x + 1, z - 1));
                }
            }
        }
        
        //Check Down-left Neighbor
        if (z - 1 >= 0)
        {
            if (x == 0)
            {
                if (z % 2 > 0)
                    gridPositions.Add(new Vector2Int(x, z - 1));
            }
            else
            {
                if (z % 2 == 0)
                {
                    gridPositions.Add(new Vector2Int(x - 1, z - 1));
                }
                else
                {
                    gridPositions.Add(new Vector2Int(x, z - 1));
                }
            }
        }

        //Check Left Neighbor
        if (x - 1 >= 0)
            gridPositions.Add(new Vector2Int(x - 1, z));

        //Check Left-Up Neighbor
        if (z + 1 < _height)
        {
            if (x == 0)
            {
                if (z % 2 != 0)
                    gridPositions.Add(new Vector2Int(x, z + 1));
            }
            else
            {
                if (z % 2 == 0)
                    gridPositions.Add(new Vector2Int(x - 1, z + 1));
                else
                    gridPositions.Add(new Vector2Int(x, z + 1));
            }
        }
    }

    protected override void DrawGrid()
    {
        TextMesh[,] debugTextArray = new TextMesh[_width, _height];

        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < _gridArray.GetLength(1); z++)
            {
                debugTextArray[x, z] = UiUtilities.CreateWorldText(_gridArray[x, z]?.ToString(), null, null, GetCellWorldPosition(x, z) + new Vector3(_cellSize, 0, _cellSize) * .5f, 40, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                debugTextArray[x, z].transform.localScale = Vector3.one * .13f;
                debugTextArray[x, z].transform.eulerAngles = new Vector3(90, 0, 0);

                Debug.DrawLine(GetCellWorldPosition(x, z), GetCellWorldPosition(x, z + 1), Color.white, 100f);
                Debug.DrawLine(GetCellWorldPosition(x, z), GetCellWorldPosition(x + 1, z), Color.white, 100f);
            }
        }

        Debug.DrawLine(GetCellWorldPosition(0, _height), GetCellWorldPosition(_width, _height), Color.white, 100f);
        Debug.DrawLine(GetCellWorldPosition(_width, 0), GetCellWorldPosition(_width, _height), Color.white, 100f);

        OnGridValueChanged += (object sender, OnGridValueChangeEventArgs eventArgs) =>
        {
            debugTextArray[eventArgs.X, eventArgs.Z].text = _gridArray[eventArgs.X, eventArgs.Z]?.ToString();
        };
    }
}
