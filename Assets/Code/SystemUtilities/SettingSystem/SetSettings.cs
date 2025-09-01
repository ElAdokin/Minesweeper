using UnityEngine;

public class SetSettings
{
    private const int _fps = 60;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void OnBeforeSplashScreen()
    {
        //Debug.Log("Before SplashScreen is shown and before the first scene is loaded.");
        SetSettingSystem();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoad()
    {
        //Debug.Log("First scene loading: Before Awake is called.");
        
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void OnAfterSceneLoad()
    {
        //Debug.Log("First scene loaded: After Awake is called.");
    }

    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeInitialized()
    {
        //Debug.Log("Runtime initialized: First scene loaded: After Awake is called.");
        
    }

    static void SetSettingSystem()
    {
        Application.targetFrameRate = _fps;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
