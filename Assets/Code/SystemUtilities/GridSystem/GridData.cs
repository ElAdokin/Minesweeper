using UnityEngine;

[CreateAssetMenu(fileName = "GridData", menuName = "Scriptable Objects/Grid/GridData")]
public class GridData : ScriptableObject
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private float _cellSize;
    [SerializeField] private Vector3 _originPosition;
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private bool _debugDraw;

    public int Width => _width;
    public int Height => _height;
    public float CellSize => _cellSize;
    public Vector3 OriginPosition => _originPosition;
    public GameObject CellPrefab => _cellPrefab;
    public bool DebugDraw => _debugDraw;
}
