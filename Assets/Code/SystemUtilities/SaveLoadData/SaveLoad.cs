using UnityEngine;
using System;
using System.IO;
using System.Text;

public static class SaveLoad<T>
{
    public static void Save(T data, string folder, string file)
    {
        string dataPath = GetFilePath(folder, file);

        string jsonData = JsonUtility.ToJson(data, true);
        
        byte[] byteData;

        byteData = Encoding.ASCII.GetBytes(jsonData);

        if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dataPath));
        }

        try
        {
            File.WriteAllBytes(dataPath, byteData);
            Debug.Log("Save data to: " + dataPath);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save data to: " + dataPath);
            Debug.LogError("Error " + e.Message);
        }
    }

    public static T Load(string folder, string file)
    {
        string dataPath = GetFilePath(folder, file);

        if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
        {
            Debug.LogWarning("File or path does not exist! " + dataPath);
            return default(T);
        }

        byte[] jsonDataAsBytes = null;

        try
        {
            jsonDataAsBytes = File.ReadAllBytes(dataPath);
            Debug.Log("<color=green>Loaded all data from: </color>" + dataPath);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed to load data from: " + dataPath);
            Debug.LogWarning("Error: " + e.Message);
            return default(T);
        }

        if (jsonDataAsBytes == null)
            return default(T);

        
        string jsonData;

        jsonData = Encoding.ASCII.GetString(jsonDataAsBytes);

        T returnedData = JsonUtility.FromJson<T>(jsonData);

        return (T)Convert.ChangeType(returnedData, typeof(T));
    }

    public static void DeleteSaveData(string folder, string file) 
    {
        string dataPath = GetFilePath(folder, file);

        if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
        {
            Debug.LogWarning("File or path does not exist! " + dataPath);
            return;
        }

        if (File.Exists(dataPath))
        {
            File.Delete(dataPath);
            Debug.Log("File delete at path: " + dataPath);
        }
        else 
        {
            Debug.LogWarning("File or path does not exist! " + dataPath);
        }
    }

    private static string GetFilePath(string FolderName, string FileName = "")
    {
        string filePath = string.Empty;

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        // mac
        filePath = Path.Combine(Application.streamingAssetsPath, ("data/" + FolderName));

        if (FileName != "")
            filePath = Path.Combine(filePath, (FileName + ".txt"));
#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        // windows
        filePath = Path.Combine(Application.persistentDataPath, ("data/" + FolderName));

        if (FileName != "")
            filePath = Path.Combine(filePath, (FileName + ".txt"));
#elif UNITY_ANDROID
        // android
        filePath = Path.Combine(Application.persistentDataPath, ("data/" + FolderName));

        if(FileName != "")
            filePath = Path.Combine(filePath, (FileName + ".txt"));
#elif UNITY_IOS
        // ios
        filePath = Path.Combine(Application.persistentDataPath, ("data/" + FolderName));

        if(FileName != "")
            filePath = Path.Combine(filePath, (FileName + ".txt"));
#endif
        return filePath;
    }
}
