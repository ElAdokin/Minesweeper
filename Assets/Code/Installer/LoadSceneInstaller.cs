using UnityEngine;

public class LoadSceneInstaller : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    private LoadSceneUiManager _uiManager;
    private SceneController _sceneController;
    private float _loadingProgress;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _uiManager = GetComponent<LoadSceneUiManager>();
        _uiManager.ContinueButton.onClick.AddListener(Continue);
        _uiManager.ContinueButton.gameObject.SetActive(false);
        _uiManager.SetLoadingText("Loading...");
        _uiManager.SetLoadingBarValue(_loadingProgress);

        _sceneController = new SceneController(this);
        _sceneController.NotifyLoadStatus += LoadStatus;
    }

    private void Start()
    {
        LoadNextScene();
    }

    private void LoadNextScene() 
    {
        _sceneController.LoadAsyncScene(_gameData.SceneIndex == 2 ? 2 : 1);
        _gameData.SceneIndex = 0;
    }

    private void LoadStatus(float status)
    {
        _loadingProgress = status;
        _uiManager.SetLoadingBarValue(_loadingProgress);

        if (_loadingProgress == 1)
        {
            _uiManager.SetLoadingText("Scene Loaded");
            _uiManager.ContinueButton.gameObject.SetActive(true);
        }
    }

    private void Continue()
    {
        _sceneController.ActivateScene();
    }
}
