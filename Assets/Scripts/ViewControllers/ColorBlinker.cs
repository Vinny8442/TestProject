using System;
using System.Collections;
using UnityEngine;

namespace test.project
{
	[RequireComponent(typeof(MeshRenderer))]
	public class ColorBlinker : MonoBehaviour
	{
		private MaterialPropertyBlock _propertyBlock;
		private Color _originalColor;
		private MeshRenderer _meshRenderer;
		private Coroutine _blinkingCoroutine;

		private void Start()
		{
			_propertyBlock = new MaterialPropertyBlock();
			_meshRenderer = GetComponent<MeshRenderer>();
			_originalColor = _meshRenderer.sharedMaterial.color;
		}

		public void Blink(Color color)
		{
			_blinkingCoroutine = StartCoroutine(BlinkingCoroutine(color));
		}

		private IEnumerator BlinkingCoroutine(Color color)
		{
			for (int i = 0; i < 3; i++)
			{
				SetColor(color);
				yield return new WaitForSeconds(0.07f);
				SetColor(_originalColor);
				yield return new WaitForSeconds(0.07f);
			}
			SetColor(color);
			_blinkingCoroutine = null;
		}

		public void Restore()
		{
			if (_blinkingCoroutine != null)
			{
				StopCoroutine(_blinkingCoroutine);
			}
			SetColor(_originalColor);
		}

		private void SetColor(Color color)
		{
			_propertyBlock.SetColor("_BaseColor", color);
			_meshRenderer.SetPropertyBlock(_propertyBlock);
		}
	}
}