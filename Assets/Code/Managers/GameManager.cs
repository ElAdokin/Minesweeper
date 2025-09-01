using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private GameInstaller _gameInstaller;
    
    private GameData _gameData;
    private Transform _gridParent;

    private HexGrid<GridObject> _grid;
    private GameObject _cellView;

    private GridObject _gridObject;

    private List<Vector2Int> _neighborsPositions = new List<Vector2Int>();

    private TextMesh _textObject;

    private List<GridObject> _bombsCells = new List<GridObject>();

    private Randomizer _randomizer = new Randomizer();
    private int _randomX;
    private int _randomZ;

    private Queue<GridObject> _clearEmptyCells = new Queue<GridObject>();
    
    private Optional<IEnumerator> _clearAction = new Optional<IEnumerator>();
    private CoroutineController _coroutineController;
    private Yielders _yielders = new Yielders();

    private bool _canPutBomb;

    private bool _stopGame;

    public GameData GameData => _gameData;
    
    public HexGrid<GridObject> Grid => _grid;

    public bool CanPutBomb => _canPutBomb;

    public bool StopGame => _stopGame;

    public GameManager(GameInstaller installer, GameData gameData, Transform gridParent)
    {
        _gameInstaller = installer;
        _gameData = gameData;
        _gridParent = gridParent;
        _stopGame = false;

        GetReferences();
    }

    private void GetReferences()
    {
        _gameInstaller.UiManager.NotifyBombToggle += BombToggle;
        _gameInstaller.UiManager.NotifyPauseGame += PauseGame;
        _gameInstaller.UiManager.NotifyRetry += Retry;
        _gameInstaller.UiManager.NotifyExit += Exit;
    }

    private void PauseGame(bool state) 
    { 
        _stopGame = state;
    }

    public void GenerateGrid()
    {
        _grid = new HexGrid<GridObject>(
           _gameData.GridData,
           (CustomGrid<GridObject> g, int x, int y) => new GridObject());

        for (int x = 0; x < _gameData.GridData.Width; x++)
        {
            for (int z = 0; z < _gameData.GridData.Height; z++)
            {
                _cellView = MonoBehaviour.Instantiate(_gameData.GridData.CellPrefab, _grid.GetCellWorldPosition(x, z), Quaternion.identity, _gridParent);
                _cellView.name = "Cell(" + x + "," + z + ")";

                _gridObject = _grid.GetCellObject(x, z);
                _gridObject.NotifySelected += NotifyCellSelected;

                _neighborsPositions.Clear();

                _grid.AssingNeighborsPositions(x, z, _neighborsPositions);

                foreach (Vector2Int position in _neighborsPositions)
                {
                    _gridObject.Neighbors.Add(_grid.GetCellObject(position.x, position.y));
                }

                _gridObject.SetVisualTransform(_cellView.transform, this);
                _gridObject.Default();
            }
        }

        CreateBombs(_gameData.Bombs);
        PutCameraOnGridCenter();
    }

    private void PutCameraOnGridCenter() 
    {
        float correctionX = _gameData.GridData.Width % 2 == 0 ? 0 : _gameData.GridData.CellSize / 2;
        float correctionZ = _gameData.GridData.Height % 2 == 0 ? 0 : _gameData.GridData.CellSize / 2; 
            
        _randomX = _gameData.GridData.Width / 2 - 1;
        _randomZ = _gameData.GridData.Height / 2 - 1;

        Vector3 referenceCellPosition = _grid.GetCellWorldPosition(_randomX, _randomZ);
        
        Camera.main.transform.position = new Vector3(
            referenceCellPosition.x + correctionX,
            Camera.main.transform.position.y,
            referenceCellPosition.z + correctionZ);
    }

    private void CreateBombs(int bombsNumber) 
    {
        _bombsCells.Clear();
        
        for (int i = 0; i < bombsNumber; i++)
        {
            GenerateRandomCoordinates();

            _gridObject = _grid.GetCellObject(_randomX, _randomZ);

            while (_gridObject.CellType != CellType.NoBombNear)
            {
                GenerateRandomCoordinates();
                _gridObject = _grid.GetCellObject(_randomX, _randomZ);
            }

            _gridObject.CellType = CellType.Bomb;

            _bombsCells.Add(_gridObject);
        }

        AssingNewTypes();
    }

    private void GenerateRandomCoordinates()
    {
        _randomX = _randomizer.GetRamdonInt(0, _gameData.GridData.Width - 1);
        _randomZ = _randomizer.GetRamdonInt(0, _gameData.GridData.Height - 1);
    }

    private void AssingNewTypes()
    {
        foreach (GridObject cell in _bombsCells) 
        {
            foreach (GridObject neighbor in cell.Neighbors)
            {
                if(neighbor.CellType != CellType.Bomb)
                    neighbor.CellType = (CellType)(((int)neighbor.CellType) + 1);
            }
        }
    }

    private void NotifyCellSelected(GridObject cell) 
    {
        AssingCellTypeToView(cell);

        if (cell.CellType == CellType.NoBombNear)
        {
            _clearEmptyCells.Enqueue(cell);

            if (_clearAction.IfPresent()) return;

            _clearAction = new Optional<IEnumerator>(ClearNeighbors());
            _coroutineController = new CoroutineController(_clearAction, _gameInstaller);
            _coroutineController.StartCurrentCoroutine();
        }
    }

    private IEnumerator ClearNeighbors()
    {
        while (_clearEmptyCells.Count > 0) 
        {
            yield return _yielders.MyWaitUntil(() => !_stopGame);
            
            _gridObject = _clearEmptyCells.Dequeue();

            if (!_gridObject.IsSelected)
            {
                _gridObject.Clear();
                AssingCellTypeToView(_gridObject);
            }

            if (_gridObject.CellType == CellType.NoBombNear)
                EnqueueNeighhbor(_gridObject);
            
            yield return _yielders.GetWaitForSecondsRealTime(_gameData.CheckBombFrequency);
        }

        StopClearAction();

        yield break;
    }

    private void EnqueueNeighhbor(GridObject gridObject) 
    {
        foreach (GridObject neighbor in _gridObject.Neighbors)
            if (!neighbor.IsSelected && !_clearEmptyCells.Contains(neighbor))
                _clearEmptyCells.Enqueue(neighbor);
    }

    private void StopClearAction()
    {
        if (_clearAction.IfPresent()) 
        {
            _coroutineController.StopCurrentCoroutine();
            _clearAction = new Optional<IEnumerator>();
        }
    }

    public void AssingCellTypeToView(GridObject cell)
    {
        if (cell.CellType == CellType.NoBombNear) return;
        
        _textObject = UiUtilities.CreateWorldText(GetCelltypeText(cell.CellType), _gameData.Font, cell.VisualTransform, Vector3.zero, 40, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
        _textObject.transform.localScale = Vector3.one * .13f;
        _textObject.transform.eulerAngles = new Vector3(90, 0, 0);
    }

    private string GetCelltypeText(CellType cellType) => cellType switch
    {
        CellType.NoBombNear => String.Empty,
        CellType.OneBombNear => "1",
        CellType.TwoBombNear => "2",
        CellType.ThreeBombNear => "3",
        CellType.FourBombNear => "4",
        CellType.FiveBombNear => "5",
        CellType.SixBombNear => "6",
        CellType.Bomb => "BOMB",
        _ => String.Empty
    };

    private void BombToggle(bool state) 
    {
        //Debug.Log("Bomb Toggle state = " + state);
        _canPutBomb = state;
    }

    public void CheckForWin()
    {
        foreach (GridObject bomb in _bombsCells) 
        { 
            if (!bomb.IsSelected) return;
        }

        Win();
    }

    private void Win()
    {
        _stopGame = true;
        _gameInstaller.UiManager.Win();
    }

    public void GameOver()
    {
        _stopGame = true;
        _gameInstaller.UiManager.GameOver();
    }

    private void Retry()
    {
        _gameInstaller.SceneController.LoadScene(2);
    }

    private void Exit()
    {
        _gameData.SceneIndex = 1;
        _gameInstaller.SceneController.LoadScene(0);
    }
}

public enum CellType 
{ 
    NoBombNear,
    OneBombNear, 
    TwoBombNear,
    ThreeBombNear,
    FourBombNear,
    FiveBombNear,
    SixBombNear,
    Bomb
}