using System;
using System.Collections.Generic;
using UnityEngine;

public class SquareGrid<TGridObject> : CustomGrid<TGridObject>
{
    private Vector3 _referenceWorldPosition;

    public SquareGrid(GridData gridData, Func<CustomGrid<TGridObject>, int, int, TGridObject> createGridObject) : base(gridData, createGridObject)
    {
    
    }

    public override Vector3 GetCellWorldPosition(int x, int z) 
    {
        return new Vector3(x, 0, z) * _cellSize + _originPosition;
    }

    protected override void GetXZFromWorldPosition(Vector3 worldPosition, out int x, out int z)
    {
        _referenceWorldPosition = worldPosition - _originPosition;
        x = Mathf.RoundToInt(_referenceWorldPosition.x / _cellSize);
        z = Mathf.RoundToInt(_referenceWorldPosition.z / _cellSize);
    }

    public override void AssingNeighborsPositions(int x, int z, List<Vector2Int> gridPositions)
    {
        //Check Up Neighbor
        if (z + 1 < _height) 
            gridPositions.Add(new Vector2Int(x, z + 1));

        //Check Up-Right Neighbor
        if (z + 1 < _height && x + 1 < _width)
            gridPositions.Add(new Vector2Int(x + 1, z + 1));

        //Check Right Neighbor
        if (x + 1 < _width)
            gridPositions.Add(new Vector2Int(x + 1, z));

        //Check Right-Down Neighbor
        if (z - 1 >= 0 && x + 1 < _width)
            gridPositions.Add(new Vector2Int(x + 1, z - 1));

        //Check Down Neighbor
        if (z - 1 >= 0)
            gridPositions.Add(new Vector2Int(x, z - 1));

        //Check Down-left Neighbor
        if (z - 1 >= 0 && x - 1 >= 0)
            gridPositions.Add(new Vector2Int(x - 1, z - 1));

        //Check Left Neighbor
        if (x - 1 >= 0)
            gridPositions.Add(new Vector2Int(x - 1, z));

        //Check Left-Up Neighbor
        if (z + 1 < _height && x - 1 >= 0)
            gridPositions.Add(new Vector2Int(x - 1, z + 1));
    }

    protected override void DrawGrid()
    {
        TextMesh[,] debugTextArray = new TextMesh[_width, _height];

        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < _gridArray.GetLength(1); z++)
            {
                debugTextArray[x, z] = UiUtilities.CreateWorldText(_gridArray[x, z].ToString(), null, null, GetCellWorldPosition(x, z) + new Vector3(_cellSize, _cellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter);

                Debug.DrawLine(GetCellWorldPosition(x, z), GetCellWorldPosition(x, z + 1), Color.white, 100f);
                Debug.DrawLine(GetCellWorldPosition(x, z), GetCellWorldPosition(x + 1, z), Color.white, 100f);
            }
        }

        Debug.DrawLine(GetCellWorldPosition(0, _height), GetCellWorldPosition(_width, _height), Color.white, 100f);
        Debug.DrawLine(GetCellWorldPosition(_width, 0), GetCellWorldPosition(_width, _height), Color.white, 100f);
    }
}
