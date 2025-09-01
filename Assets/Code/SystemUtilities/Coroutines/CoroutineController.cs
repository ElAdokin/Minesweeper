using System.Collections;
using UnityEngine;

public class CoroutineController
{
    private Optional<IEnumerator> _currentCoroutine;
    private MonoBehaviour _currentMonobehaviour;
    private bool _runnigCoroutine;

    public CoroutineController(Optional<IEnumerator> currentCoroutine, MonoBehaviour currentMonoBehaviour)
    {
        _currentCoroutine = currentCoroutine;
        _currentMonobehaviour = currentMonoBehaviour;
    }

    public IEnumerator GetCurrentCoroutine()
    {
        return _currentCoroutine.GetTypeFromOptional();
    }

    public void StartCurrentCoroutine()
    {
        _currentMonobehaviour.StartCoroutine(_currentCoroutine.GetTypeFromOptional());
        _runnigCoroutine = true;
    }

    public void StopCurrentCoroutine()
    {
        if (_currentCoroutine.IfPresent())
        {
            _currentMonobehaviour.StopCoroutine(_currentCoroutine.GetTypeFromOptional());
            _currentCoroutine = new Optional<IEnumerator>();
            _runnigCoroutine = false;
        }
    }

    public bool IsRunnigCoroutine()
    {
        return _runnigCoroutine;
    }
}
