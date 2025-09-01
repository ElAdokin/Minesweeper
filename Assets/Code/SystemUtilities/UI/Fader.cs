using System;
using System.Collections;
using UnityEngine;

public class Fader : MonoBehaviour, IFader
{
    private Optional<IEnumerator> _coroutine = new Optional<IEnumerator>();
    private CoroutineController _coroutineController;

    [SerializeField] private CanvasGroup _fader;

    private float _fadeTime = 0;
    private const float FrameTime = .017f;
    private float _alphaAmount;

    private Yielders _yielders = new Yielders();

    public event Action<bool, bool> OnFadeEnd;

    private bool _fadeEnd;
    private bool _isFadeIn;

    public void SetAlpha(float alpha) 
    { 
        _fader.alpha = alpha;
    }

    public void SetFade(bool isFadeIn, float duration)
    {
        if (_coroutine.IfPresent()) StopBehaviour();

        if(_fader == null)
            _fader = FindAnyObjectByType<CanvasGroup>();

		_fadeTime = 0;
        _fadeEnd = false;

        _isFadeIn = isFadeIn;

        if (_isFadeIn)
            _coroutine = new Optional<IEnumerator>(FadeIn(duration));
        else
            _coroutine = new Optional<IEnumerator>(FadeOut(duration));

        _coroutineController = new CoroutineController(_coroutine, this);
        _coroutineController.StartCurrentCoroutine();
    }

    public IEnumerator FadeIn(float duration)
    {
        Debug.Log("Fade In");

        _alphaAmount = 0;
        
        if(_fader.alpha != _alphaAmount)
            _fader.alpha = _alphaAmount;

        while (_fadeTime < duration)
        {
            _fadeTime += FrameTime;
            _alphaAmount += FrameTime / duration;
            _fader.alpha = _alphaAmount;
            yield return null;
        }

        _alphaAmount = 1;

        if (_fader.alpha != _alphaAmount)
            _fader.alpha = _alphaAmount;
        
        _fadeEnd = true;

        StopBehaviour();

        yield break;
    }

    private IEnumerator FadeOut(float duration)
    {
        Debug.Log("Fade Out");

        _alphaAmount = 1;
        
        if (_fader.alpha != _alphaAmount)
            _fader.alpha = _alphaAmount;

        while (_fadeTime < duration)
        {
            _fadeTime += FrameTime;
            _alphaAmount -= FrameTime / duration;
            _fader.alpha = _alphaAmount;
            yield return null;
        }

        _alphaAmount = 0;

        if (_fader.alpha != _alphaAmount)
            _fader.alpha = _alphaAmount;
        
        _fadeEnd = true;
        
        StopBehaviour();
        
        yield break;
    }

    private void StopBehaviour()
    {
        if (_coroutine.IfPresent())
        {
            _coroutineController?.StopCurrentCoroutine();
            _coroutine = new Optional<IEnumerator>();
        }

        Notify();
    }

    private void Notify()
    {
        OnFadeEnd?.Invoke(_isFadeIn, _fadeEnd);
    }
}
