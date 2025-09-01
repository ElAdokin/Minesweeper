using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GpuInstancer : MonoBehaviour
{
	[SerializeField] private Mesh _mesh;

	[SerializeField] private Material[] _materials;

	private List<List<UnityEngine.Matrix4x4>> _batches = new List<List<Matrix4x4>>();

	private int _addedMatricies = 0;

	private bool _canRenderInstances;

	private ShadowCastingMode _shadowCastingMode;

	private MaterialPropertyBlock _materialPropertyBlock;

	public void InitializeIntancesByPosition(List<Vector3> meshPositions, List<Vector3> _scales, ShadowCastingMode shadowCastingMode)
	{
		_shadowCastingMode = shadowCastingMode;
		_materialPropertyBlock = new MaterialPropertyBlock();

		_addedMatricies = 0;

		_batches.Add(new List<Matrix4x4>());

		for (int i = 0; i < meshPositions.Count; i++)
		{
			if (_addedMatricies < 1000 && _batches.Count != 0)
			{
				_batches[_batches.Count - 1].Add(Matrix4x4.TRS(meshPositions[i], Quaternion.identity, _scales[i]));
				_addedMatricies += 1;
			}
			else
			{
				_batches.Add(new List<Matrix4x4>());
				_addedMatricies = 0;
			}
		}

		_canRenderInstances = true;
	}

	public void ReleaseInstances()
	{
		_batches.Clear();
		_canRenderInstances = false;
	}

	private void RenderBatches()
	{
		foreach (var Batch in _batches)
		{
			for (int i = 0; i < _mesh.subMeshCount; i++)
			{
				Graphics.DrawMeshInstanced(_mesh, i, _materials[i], Batch, _materialPropertyBlock, _shadowCastingMode, true);
			}
		}
	}

	private void Update()
	{
		if(_canRenderInstances)
			RenderBatches();
	}
}
