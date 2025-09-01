using System.IO;
using UnityEngine;

public class PerlinNoiseGenerator : MonoBehaviour
{
	[SerializeField] private int _numberOfLayers = 16;

	[SerializeField] private int _width = 256;
	[SerializeField] private int _height = 256;

	private int[,] _lastNoise;
	private int[,] _currentNoise;
	private int[,] _textureNoise;

	private void Start()
	{
		_lastNoise = new int[_width, _height];
		_currentNoise = new int[_width, _height];
		_textureNoise = new int[_width, _height];
	}

	public void CreateLayerNoises()
	{
		for (int i = 0; i < _numberOfLayers; i++)
		{
			if (i == 0)
			{
				GenerateNewArrayTexture();
				_textureNoise = (int[,])_currentNoise.Clone();
			}
			else
			{
				GenerateNewArrayTexture();
				MultiplyIntArrays();
			}

			 GenerateTextureFromArray(i);
			_lastNoise = (int[,])_textureNoise.Clone();
		}
	}

	private void GenerateNewArrayTexture()
	{
		for (int x = 0; x < _width; x++)
		{
			for (int y = 0; y < _height; y++)
			{
				_currentNoise[x, y] = UnityEngine.Random.Range(0, 2);
			}
		}
	}

	private void MultiplyIntArrays()
	{
		for (int x = 0; x < _textureNoise.GetLength(0); x++)
		{
			for (int y = 0; y < _textureNoise.GetLength(1); y++)
			{
				_textureNoise[x, y] = _lastNoise[x, y] * _currentNoise[x, y];
			}
		}
	}

	private void GenerateTextureFromArray(int index)
	{
		Texture2D texture = new Texture2D(_textureNoise.GetLength(0), _textureNoise.GetLength(1));

		for (int x = 0; x < _textureNoise.GetLength(0); x++)
		{
			for (int y = 0; y < _textureNoise.GetLength(1); y++)
			{
				texture.SetPixel(x, y, GetPixelColor(_textureNoise[x, y]));
			}
		}

		texture.Apply();

		byte[] bytes = texture.EncodeToPNG();

		File.WriteAllBytes(Application.dataPath + "/PerlinNoise/PerlinNoiseTexture_" + index + ".png", bytes);
	}

	private Color GetPixelColor(int value)
	{
		if (value == 1)
			return Color.white;

		return Color.black;
	}
}
