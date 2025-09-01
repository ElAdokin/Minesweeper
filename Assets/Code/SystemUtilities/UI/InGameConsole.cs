using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InGameConsole : MonoBehaviour
{
    [SerializeField] private Text _debugText;
    [SerializeField] private uint _qsize = 15;  // number of messages to keep
    
    private Queue _myLogQueue = new Queue();

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        _myLogQueue.Enqueue("[" + type + "] : " + logString);
        
        if (type == LogType.Exception)
            _myLogQueue.Enqueue(stackTrace);
        while (_myLogQueue.Count > _qsize)
            _myLogQueue.Dequeue();

        PrintLogs();
    }

    private void PrintLogs() 
    {
        _debugText.text = "\n" + string.Join("\n", _myLogQueue.ToArray());
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }
}
