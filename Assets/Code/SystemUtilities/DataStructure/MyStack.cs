using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MyStack<T>
{
	[SerializeField] private List<T> _items;
	private T _popItem;
	private int _maxDimension;
	private int _maxindex;
	private int _popIndex;

	public MyStack(int capacity)
	{
		_maxDimension = capacity;
		_maxindex = _maxDimension - 1;
		_items = new List<T>();
		_items.Clear();
	}

	public void Push(T item)
	{
		if (_items.Count < _maxDimension)
		{
			_items.Add(item);
			return;
		}

		_items.RemoveAt(_maxindex);
		_items.Add(item);
	}

	public T Pop()
	{
		try
		{
			_popIndex = _items.Count - 1;
			_popItem = _items[_popIndex];
			_items.RemoveAt(_popIndex);

			return _popItem;
		}
		catch (Exception e)
		{
			Debug.LogError("Error on Pop demand = " + e);
			return default;
		}
	}

	public int GetDimension()
	{
		return _items.Count;
	}

	public void ClearStack()
	{
		_items.Clear();
	}
}