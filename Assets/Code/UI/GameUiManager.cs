using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUiManager : MonoBehaviour
{
    [SerializeField] private GameObject _stopGameObject;
    private Text _stopText;
    [SerializeField] private Toggle _bombToggle;
    public Action<bool> NotifyBombToggle;
    public Action<bool> NotifyPauseGame;
    public Action NotifyRetry;
    public Action NotifyExit;

    private Button _pauseButton;
    private Button _retryButton;
    private Button _exitButton;

    public void Initialize()
    {
        _stopGameObject = GameObject.Find("StopGamePanel");
        _stopText = _stopGameObject.GetComponentInChildren<Text>();
        
        _bombToggle = GameObject.Find("BombToggle").GetComponent<Toggle>();
        
        _pauseButton = GameObject.Find("PauseButton").GetComponent<Button>();
        _retryButton = GameObject.Find("RetryButton").GetComponent<Button>();
        _exitButton = GameObject.Find("ExitButton").GetComponent<Button>();

        _stopText.text = "Pause";
        _retryButton.GetComponentInChildren<Text>().text = "Resume";
        
        _stopGameObject.SetActive(false);

        _bombToggle.onValueChanged.AddListener(BombToggle);

        _pauseButton.onClick.AddListener(Pause);
        _retryButton.onClick.AddListener(Resume);
        _exitButton.onClick.AddListener(Exit);
    }

    private void BombToggle(bool state) 
    { 
        NotifyBombToggle?.Invoke(state);
    }

    public void Pause()
    {
        NotifyPauseGame?.Invoke(true);
        _stopGameObject.SetActive(true);
    }

    public void Resume()
    {
        _stopGameObject.SetActive(false);
        NotifyPauseGame?.Invoke(false);
    }

    public void Win()
    {
        _stopText.text = "You Win !!!";
        _retryButton.GetComponentInChildren<Text>().text = "Retry";
        _retryButton.onClick.RemoveAllListeners();
        _retryButton.onClick.AddListener(Retry);
        _stopGameObject.SetActive(true);
    }

    public void GameOver()
    {
        _stopText.text = "Game Over";
        _retryButton.GetComponentInChildren<Text>().text = "Retry";
        _retryButton.onClick.RemoveAllListeners();
        _retryButton.onClick.AddListener(Retry);
        _stopGameObject.SetActive(true);
    }

    private void Retry() 
    {
        NotifyRetry?.Invoke();
    }

    private void Exit()
    {
        NotifyExit?.Invoke();
    }
}
