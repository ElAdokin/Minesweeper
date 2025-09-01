using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private List<GameObject> _poolObjects = new List<GameObject>();
    private Randomizer _randomizer = new Randomizer();

    public void AddObjectToPool(GameObject newObject)
    {
        _poolObjects.Add(newObject);
        //Debug.Log(newObject.name + " added to pool");
    }

    public void RemoveObjectFromPool(string name)
    {
        for (int i = 0; i < _poolObjects.Count; i++)
        {
            if (_poolObjects[i].name == name)
                _poolObjects.RemoveAt(i);
        }
    }

    public void RemoveObjectFromPoolByIndex(int index)
    {
        _poolObjects.RemoveAt(index);
    }

    public GameObject GetInactiveObject()
    {
        for (int i = 0; i < _poolObjects.Count; i++)
        {
            if (!_poolObjects[i].activeInHierarchy)
                return _poolObjects[i];
        }

        return null;
    }

    public GameObject GetInactiveObjectByTag(string tag)
    {
        for (int i = 0; i < _poolObjects.Count; i++)
        {
            if (!_poolObjects[i].activeInHierarchy && _poolObjects[i].tag == tag)
                return _poolObjects[i];
        }

        return null;
    }

    public GameObject GetInactiveObjectByName(string name)
    {
        for (int i = 0; i < _poolObjects.Count; i++)
        {
            if (!_poolObjects[i].activeInHierarchy && _poolObjects[i].name == name)
                return _poolObjects[i];
        }

        return null;
    }

    public void ClearPool()
    {
        _poolObjects.Clear();
    }

    public GameObject GetObjectByIndex(int index)
    {
        return _poolObjects[index];
    }

    public void DeactivateAllObjects()
    {
        for (int i = 0; i < _poolObjects.Count; i++)
        {
            _poolObjects[i].SetActive(false);
        }
    }

    public void ActivateAllObjects()
    {
        for (int i = 0; i < _poolObjects.Count; i++)
        {
            _poolObjects[i].SetActive(true);
        }
    }

    public int GetAmountOfElements()
    {
        return _poolObjects.Count;
    }

    public void SufflePool() 
    {
        _poolObjects = _randomizer.ShuffleList<GameObject>(_poolObjects);
    }

    public int GetObjectIndex(GameObject poolObject)
    {
        for (int i = 0; i < _poolObjects.Count; i++)
        {
            if (_poolObjects[i] == poolObject) return i;
        }

        return 0;
    }

    public int GetObjectIndexByName(string name)
    {
        for (int i = 0; i < _poolObjects.Count; i++)
        {
            if (_poolObjects[i].name == name) return i;
        }

        return 0;
    }
}
