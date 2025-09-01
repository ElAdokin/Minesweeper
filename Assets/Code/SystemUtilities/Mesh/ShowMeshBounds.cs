using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShowMeshBounds : MonoBehaviour
{
	private GameObject _boundsObject;
	private Bounds _bounds;
	private Vector3 _center;
	private Vector3 _extents;
	private Material _material;
	private Color _color;
	private float _width;

	private Vector3 _frontTopLeft;
	private Vector3 _frontTopRight;
	private Vector3 _frontBottomLeft;
	private Vector3 _frontBottomRight;
	private Vector3 _backTopLeft;
	private Vector3 _backTopRight;
	private Vector3 _backBottomLeft;
	private Vector3 _backBottomRight;

	private GameObject _lineRendererObject;
	private LineRenderer _lineRenderer;

	private LineRenderer[] _lines;

	public void CreateBounds(Bounds bound, Material material, Color color, float width)
	{
		_bounds = bound;
		_material = material;
		_color = color;
		_width = width;

		_boundsObject = new GameObject
		{
			name = "Bounds"
		};

		if (_bounds.size.z > 0)
			_lines = new LineRenderer[12];
		else
			_lines = new LineRenderer[4];

		CalculateVertexPositons();
		DrawBox();
	}

	public void RefreshBounds(Bounds bounds)
	{
		_bounds = bounds;
		CalculateVertexPositons();
		DrawBox();
	}

	private void CalculateVertexPositons()
	{
		_center = _bounds.center;
		_extents = _bounds.extents;

		_frontTopLeft = new Vector3(_center.x - _extents.x, _center.y + _extents.y, _center.z - _extents.z);  
		_frontTopRight = new Vector3(_center.x + _extents.x, _center.y + _extents.y, _center.z - _extents.z);  
		_frontBottomLeft = new Vector3(_center.x - _extents.x, _center.y - _extents.y, _center.z - _extents.z);  
		_frontBottomRight = new Vector3(_center.x + _extents.x, _center.y - _extents.y, _center.z - _extents.z);  
		_backTopLeft = new Vector3(_center.x - _extents.x, _center.y + _extents.y, _center.z + _extents.z);  
		_backTopRight = new Vector3(_center.x + _extents.x, _center.y + _extents.y, _center.z + _extents.z);  
		_backBottomLeft = new Vector3(_center.x - _extents.x, _center.y - _extents.y, _center.z + _extents.z);  
		_backBottomRight = new Vector3(_center.x + _extents.x, _center.y - _extents.y, _center.z + _extents.z);  
	}

	private void DrawBox()
	{
		RefreshLine(0, _frontTopLeft, _frontTopRight);
		RefreshLine(1, _frontTopRight, _frontBottomRight);
		RefreshLine(2, _frontBottomRight, _frontBottomLeft);
		RefreshLine(3, _frontBottomLeft, _frontTopLeft);

		if (_lines.Length > 4)
		{
			RefreshLine(4, _backTopLeft, _backTopRight);
			RefreshLine(5, _backTopRight, _backBottomRight);
			RefreshLine(6, _backBottomRight, _backBottomLeft);
			RefreshLine(7, _backBottomLeft, _backTopLeft);

			RefreshLine(8, _frontTopLeft, _backTopLeft);
			RefreshLine(9, _frontTopRight, _backTopRight);
			RefreshLine(10, _frontBottomRight, _backBottomRight);
			RefreshLine(11, _frontBottomLeft, _backBottomLeft);
		}
	}

	private void RefreshLine(int index, Vector3 start, Vector3 end)
	{
		if (!_lines[index])
		{
			_lineRendererObject = new GameObject();

			_lineRendererObject.name = "Line_" + index.ToString();
			_lineRendererObject.transform.parent = _boundsObject.transform;

			_lineRenderer = _lineRendererObject.AddComponent<LineRenderer>();
			

			_lineRenderer.sharedMaterial = _material;
			_lineRenderer.startColor = _color;
			_lineRenderer.endColor = _color;
			_lineRenderer.startWidth = _width;
			_lineRenderer.endWidth = _width;

			_lines[index] = _lineRenderer;
		}
		else
			_lineRenderer = _lines[index];

		_lineRenderer.SetPosition(0, start);
		_lineRenderer.SetPosition(1, end);
	}
}
