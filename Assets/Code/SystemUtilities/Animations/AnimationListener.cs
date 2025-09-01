using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator), typeof(AudioSource))]
public class AnimationListener : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _audioSource;
    private Fader _fader;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _fader = GameObject.Find("Fader").GetComponent<Fader>();

        Configure(_fader);
        FadeIn();
    }

    private void FadeIn()
    {
        _fader.SetFade(true, 1f);
    }

    public void FadeOut()
    {
        _fader.SetFade(false, 1f);
    }

    private void Configure(IFader fader)
    {
        fader.OnFadeEnd += Updated;
    }

    private void Updated(bool isFadeIn, bool fadeState)
    {
        if (isFadeIn)
        {
            if (fadeState)
            {
                RunTrigger("PlayLogo");
                _audioSource.Play();
            }
        }
        else
        {
            if (fadeState)
                GoMenu();
        }
    }

    private void RunTrigger(string trigger)
    {
        _animator.SetTrigger(trigger);
    }

    private void GoMenu()
    {
        SceneManager.LoadScene(1);
    }
}
