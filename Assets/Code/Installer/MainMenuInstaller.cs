using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MainMenuInstaller : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private GameObject[] _menus;
    private MainMenuUiManager _uiManager;
    private SceneController _sceneController;
    private int _newBombValue;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _uiManager = GetComponent<MainMenuUiManager>();
        
        _uiManager.PlayButton.onClick.AddListener(Play);
        _uiManager.OptionsButton.onClick.AddListener(Options);
        _uiManager.BackButton.onClick.AddListener(MainMenu);
        _uiManager.BombSlider.onValueChanged.AddListener(SliderBombValue);

#if UNITY_WEBGL
        _uiManager.ExitButton.gameObject.SetActive(false);
#else
        _uiManager.ExitButton.onClick.AddListener(Exit);
#endif
        _sceneController = new SceneController(this);

        MainMenu();
    }

    private void Play()
    {
        _gameData.SceneIndex = 2;
        _sceneController.LoadScene(0);
    }

    private void Options()
    {
        InicializeBombSlider();
        GoMenu(1);
    }

    private void InicializeBombSlider() 
    {
        _newBombValue = _gameData.Bombs;
        _uiManager.BombSlider.value = _newBombValue;
        RefreshBombValue(_newBombValue);
    }

    private void MainMenu() 
    {
        GoMenu(0);
    }

    private void GoMenu(int index) 
    {
        foreach(GameObject menu in _menus) 
        { 
            menu.SetActive(false);
        }

        _menus[index].SetActive(true);
    }

    private void SliderBombValue(float value) 
    {
        _newBombValue = (int)value;

        _gameData.Bombs = _newBombValue;

        RefreshBombValue(_newBombValue);
    }

    private void RefreshBombValue(int value) 
    {
        _uiManager.BombCounter.text = string.Empty;
        _uiManager.BombCounter.text = value.ToString();
    }

    private void Exit()
    {
        Application.Quit();
    }
}
