using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance { get; private set; }
    [SerializeField] private GameObject _audioListenerObject;

    private void Awake()
    {
        _audioListenerObject.SetActive(false);
        
        SettingSingleton();
    }

    private void SettingSingleton() 
    {   
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(Instance);
        _audioListenerObject.SetActive(true);
    }
}
