using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController
{
    private MonoBehaviour _monoBehaviour;
    
    private Optional<IEnumerator> _loadAction = new Optional<IEnumerator>();
    private CoroutineController _coroutineController;
    private Yielders _yielders = new Yielders();

    private AsyncOperation _asyncLoad;

    public Action<float> NotifyLoadStatus;

    public SceneController(MonoBehaviour monoBehaviour)
    {
        _monoBehaviour = monoBehaviour;
    }

    public void LoadScene(int sceneIndex) 
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadAsyncScene(int sceneIndex)
    {
        if (_loadAction.IfPresent()) return;

        _loadAction = new Optional<IEnumerator>(LoadingAsyncScene(sceneIndex));
        _coroutineController = new CoroutineController(_loadAction, _monoBehaviour);
        _coroutineController.StartCurrentCoroutine();
    }

    private IEnumerator LoadingAsyncScene(int sceneIndex)
    {
        yield return _yielders.GetWaitForSecondsRealTime(2f);
        
        _asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        _asyncLoad.allowSceneActivation = false;
        
        NotifyLoadStatus?.Invoke(0);

        while (!_asyncLoad.isDone)
        {
            NotifyLoadStatus?.Invoke(_asyncLoad.progress);

            if (_asyncLoad.progress >= 0.9f)
                NotifyLoadStatus?.Invoke(1);

            yield return null;
        }
    }

    public void ActivateScene() 
    {
        _asyncLoad.allowSceneActivation = true;
        StopLoadAsync();
    }

    private void StopLoadAsync()
    {
        if (_loadAction.IfPresent())
        {
            _coroutineController.StopCurrentCoroutine();
            _loadAction = new Optional<IEnumerator>();
        }
    }
}
