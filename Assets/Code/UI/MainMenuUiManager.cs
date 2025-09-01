using UnityEngine;
using UnityEngine.UI;

public class MainMenuUiManager : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private Slider _bombSlider;
    [SerializeField] private Text _bombCounter;

    public Button PlayButton => _playButton; 
    public Button OptionsButton => _optionsButton; 
    public Button ExitButton => _exitButton;
    public Button BackButton => _backButton;
    public Slider BombSlider => _bombSlider;
    public Text BombCounter => _bombCounter;
}
