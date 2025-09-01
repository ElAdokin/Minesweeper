using UnityEngine;
using UnityEngine.UI;

public class LoadSceneUiManager: MonoBehaviour 
{
    [SerializeField] private Text _loadingText;
    [SerializeField] private Slider _loadingBar;
    [SerializeField] private Button _continueButton;

    public Button ContinueButton => _continueButton;

    public void SetLoadingText(string text) 
    { 
        _loadingText.text = text;
    }

    public void SetLoadingBarValue(float value)
    {
        _loadingBar.value = value;
    }
}
