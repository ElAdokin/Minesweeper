using System.Collections.Generic;
using UnityEngine;

public class Randomizer
{
    private int _intResult;
    private float _floatResult;
    private int _newIndex;

    public int GetRandomIndexFromList(int countListElements)
    {
        _newIndex = Random.Range(0, countListElements);

        return _newIndex;
    }

    public int GetRamdonInt(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1);
    }

    public bool IsCritic(int criticalRate) 
    {
        return GetRamdonInt(0, 100) <= criticalRate;
    }

    public int GetRamdonIntWithException(int min, int max, int exception)
    {
        _intResult = UnityEngine.Random.Range(min, max + 1);

        while (_intResult == exception)
        {
            _intResult = UnityEngine.Random.Range(min, max + 1);
        }

        return _intResult;
    }

    public float GetRamdonFloat(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public float GetRamdonFloatWithException(float min, float max, float exception)
    {
        _floatResult = UnityEngine.Random.Range(min, max);

        while (_floatResult == exception)
        {
            _floatResult = UnityEngine.Random.Range(min, max);
        }

        return _floatResult;
    }

    public List<T> ShuffleList<T>(List<T> inputList)
    {
        int i = 0;
        int t = inputList.Count;
        int r = 0;
        T p;

        List<T> tempList = new List<T>();
        tempList.AddRange(inputList);

        while (i < t)
        {
            r = Random.Range(i, tempList.Count);
            p = tempList[i];
            tempList[i] = tempList[r];
            tempList[r] = p;
            i++;
        }

        return tempList;
    }

    public int GetRandomValueWithPercentage(List<int> values, List<int> percentages)
    {
        if (values.Count != percentages.Count)
        {
            Debug.Log("Values amount is different for percentage amount");
            return 0;
        }
        
        int randomPercentage = GetRamdonInt(1, 99);
        int acummulatedPercentage = 0;

        for (int i = 0; i < percentages.Count; i++) 
        {
            acummulatedPercentage += percentages[i];

            if (acummulatedPercentage >= randomPercentage)
            {
                return values[i];
            }
        }

        return 0;
    }
}
