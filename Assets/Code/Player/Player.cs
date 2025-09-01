using UnityEngine;

public class Player
{
    private GameInstaller _gameInstaller;
    private GameManager _gameManager;
    private InputHandler _inputHandler;

    private Vector3 _mousePosition;
    private GridObject _lastGridObject;

    public Player(GameInstaller gameInstaller, GameManager gameManager, InputHandler inputHandler)
    {
        _gameInstaller = gameInstaller;
        _gameManager = gameManager;
        _inputHandler = inputHandler;

        AssingReference();
    }

    private void AssingReference()
    {
        _inputHandler.MousePosition += MousePosition;
        _inputHandler.LeftMouseClickDown += LeftMouseClickDown;
    }

    private void MousePosition(Vector3 position) 
    { 
        _mousePosition = position;

        UpdateGridPosition();
    }
    private void UpdateGridPosition()
    {
        if (_gameManager.StopGame) return;

        if (_lastGridObject != null)
        {
            _lastGridObject.Default();
        }

        _lastGridObject = _gameManager.Grid.GetCellObject(_mousePosition);

        if (_lastGridObject != null)
        {
            if (_gameManager.CanPutBomb)
                _lastGridObject.HoverBomb();
            else
                _lastGridObject.Hover();
        }
    }
    
    private void LeftMouseClickDown()
    {
        if (_gameManager.StopGame) return;
        
        if (_lastGridObject == null) return;

        if (_lastGridObject.IsSelected) return;

        if (_gameManager.CanPutBomb)
        {
            if (_lastGridObject.CellType == CellType.Bomb)
            {
                _lastGridObject.MarkAsBomb();
                _gameManager.CheckForWin();
            }
            else 
            {
                _lastGridObject.Explode();
                _gameManager.GameOver();
            }
        }
        else 
        {
            if (_lastGridObject.CellType == CellType.Bomb)
            {
                _lastGridObject.Explode();
                _gameManager.GameOver();
            }
            else
            {
                _lastGridObject.IsSelected = true;
            }
        }
    }
}
