using UnityEngine;

public class GameInstaller : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private Transform _gridParent;

    private GameManager _gameManager;
    private GameUiManager _uiManager;
    private InputHandler _inputHandler;
    private Player _player;
    private SceneController _sceneController;

    public GameUiManager UiManager => _uiManager;
    public SceneController SceneController => _sceneController;

    private void Awake()
    {
        _uiManager = gameObject.AddComponent<GameUiManager>();
        _uiManager.Initialize();

        _sceneController = new SceneController(this);

        _inputHandler = gameObject.AddComponent<InputHandler>();

        _gameManager = new GameManager(this, _gameData, _gridParent);
        _gameManager.GenerateGrid();

        _player = new Player(this, _gameManager, _inputHandler);
    }
}
