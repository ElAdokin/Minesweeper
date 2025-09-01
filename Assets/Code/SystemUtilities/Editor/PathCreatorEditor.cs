using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathCreator)), CanEditMultipleObjects]
public class PathCreatorEditor : Editor
{
	private PathCreator _pathCreator;

	private Vector3 _oldTangentDirection;
	private float _oldTangentDistanceMagnitude;

	private Vector3 _trackTangetHandle;
	private Vector3 _newTangentDir ;
	private float _newTangetMagnitude;

	private Vector3 _trackInverseTangetHandle;
	private Vector3 _newInverseTangentDir;
	private float _newInverseTangetMagnitude;

	void OnSceneGUI()
	{
		_pathCreator = target as PathCreator;

		if (_pathCreator.Nodes.Length == 0) return;

		for (int i = 0; i < _pathCreator.Nodes.Length; i++)
		{
			DrawNode(_pathCreator.Nodes[i]);

			if (_pathCreator.Nodes.Length == 1)
				return;

			if (i < _pathCreator.Nodes.Length - 1)
				DrawPath(_pathCreator.Nodes[i], _pathCreator.Nodes[i + 1]);
			else
				if(_pathCreator.Loop)
					DrawPath(_pathCreator.Nodes[_pathCreator.Nodes.Length - 1], _pathCreator.Nodes[0]);
		}
	}

	private void DrawNode(Node node)
	{
		CheckNodePosition(node);

		CheckNodeTangent(node);

		CheckNodeInverseTangent(node);

		Handles.DrawDottedLine(node.Tangent, node.InverseTangent, 5);
	}

	private void CheckNodePosition(Node node)
	{
		EditorGUI.BeginChangeCheck();

		_oldTangentDirection = (node.Tangent - node.Position).normalized;
		_oldTangentDistanceMagnitude = Vector3.Distance(node.Position, node.Tangent);

		node.Position = Handles.PositionHandle(node.Position, Quaternion.identity);

		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(target, "Changed Node position");
			node.Tangent = node.Position + _oldTangentDirection * _oldTangentDistanceMagnitude;
			node.InverseTangent = node.Position - _oldTangentDirection * _oldTangentDistanceMagnitude;
		}
	}

	private void CheckNodeTangent(Node node)
	{
		EditorGUI.BeginChangeCheck();

		_trackTangetHandle = Handles.PositionHandle(node.Tangent, Quaternion.identity);

		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(target, "Changed Node Tangent");
			_newTangentDir = (_trackTangetHandle - node.Position).normalized;
			_newTangetMagnitude = Vector3.Distance(node.Position, _trackTangetHandle);

			node.Tangent = node.Position + _newTangentDir * _newTangetMagnitude;
			node.InverseTangent = node.Position + _newTangentDir * -_newTangetMagnitude;
		}
	}

	private void CheckNodeInverseTangent(Node node)
	{
		EditorGUI.BeginChangeCheck();

		_trackInverseTangetHandle = Handles.PositionHandle(node.InverseTangent, Quaternion.identity);

		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(target, "Changed Node InverseTangent");
			_newInverseTangentDir = (_trackInverseTangetHandle - node.Position).normalized;
			_newInverseTangetMagnitude = Vector3.Distance(node.Position, _trackInverseTangetHandle);

			node.InverseTangent = node.Position + _newInverseTangentDir * _newInverseTangetMagnitude;
			node.Tangent = node.Position + _newInverseTangentDir * -_newInverseTangetMagnitude;
		}
	}

	private void DrawPath(Node origin, Node target)
	{
		Handles.DrawBezier(origin.Position, target.Position, origin.Tangent, target.InverseTangent, Color.red, null, 5f);
	}
}

