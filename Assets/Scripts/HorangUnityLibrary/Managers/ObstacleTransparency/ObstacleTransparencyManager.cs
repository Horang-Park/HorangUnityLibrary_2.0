using System;
using System.Collections.Generic;
using Horang.HorangUnityLibrary.Foundation.Manager;
using Horang.HorangUnityLibrary.Utilities;
using UniRx;
using UnityEngine;

namespace Horang.HorangUnityLibrary.Managers.ObstacleTransparency
{
	internal struct ObstacleRenderer
	{
		internal MeshRenderer meshRenderer;
		internal Material originalMaterial;
	}
	
	public sealed class ObstacleTransparencyManager : MonoBaseManager
	{
		[Header("Default Settings")]
		[SerializeField] private Material transparentMaterial;
		[SerializeField] private Transform fromTarget;
		[SerializeField] private Transform toTarget;
		[SerializeField] private LayerMask toTransparentLayerMask;

		[Header("Material Settings")]
		[SerializeField] private Color transparentShaderColor = Color.white;
		[SerializeField] private Color originalShaderColor = Color.white;
		[SerializeField] private string shaderColorPropertyName = "_Color";

		private readonly Dictionary<int, ObstacleRenderer> savedObstacleRenderers = new();
		private readonly List<ObstacleRenderer> transparentRenderers = new();

		private RaycastHit[] obstacleHits;
		private int shaderColorNameId;

		protected override void Awake()
		{
			base.Awake();

			shaderColorNameId = Shader.PropertyToID(shaderColorPropertyName);
		}

		private void Start()
		{
			Observable.EveryUpdate()
				.Subscribe(_ => { ObjectTransparentRevert(); })
				.AddTo(gameObject);
		}

		private void ObjectTransparentRevert()
		{
			if (fromTarget is null || toTarget is null)
			{
				Log.Print("Set \"fromTarget\" and \"toTarget\" in inspector.", LogPriority.Error);

				return;
			}

			if (transparentRenderers.Count > 0)
			{
				for (var index = 0; index < transparentRenderers.Count; index++)
				{
					transparentRenderers[index].meshRenderer.material = transparentRenderers[index].originalMaterial;
					transparentRenderers[index].meshRenderer.material.SetColor(shaderColorNameId, originalShaderColor);
				}
				
				transparentRenderers.Clear();
			}
			
			var v = VectorUpdate();
			
			ObstacleCheck(v.Item1, -toTarget.forward, v.Item3);
			ObstacleCheck(v.Item1, v.Item2, v.Item3);
		}

		private (Vector3, Vector3, float) VectorUpdate()
		{
			var characterPosition = toTarget.position - toTarget.TransformDirection(0.0f, 1.5f, 0.0f);
			var distance = (fromTarget.position - characterPosition).magnitude;
			// ReSharper disable once Unity.InefficientPropertyAccess
			var directionFromTarget = (fromTarget.position - characterPosition).normalized;

			return new ValueTuple<Vector3, Vector3, float>(characterPosition, directionFromTarget ,distance);
		}

		private void ObstacleCheck(Vector3 tT, Vector3 d, float dist)
		{
			Physics.RaycastNonAlloc(tT, d, obstacleHits, dist, toTransparentLayerMask);

			foreach (var hit in obstacleHits)
			{
				var instanceId = hit.colliderInstanceID;

				if (savedObstacleRenderers.ContainsKey(instanceId) is false)
				{
					var rendererComponent = hit.collider.gameObject.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
					var info = new ObstacleRenderer
					{
						meshRenderer = rendererComponent,
						originalMaterial = rendererComponent!.material,
					};
					
					savedObstacleRenderers[instanceId] = info;
				}

				savedObstacleRenderers[instanceId].meshRenderer.material = transparentMaterial;
				savedObstacleRenderers[instanceId].meshRenderer.material.SetColor(shaderColorNameId, transparentShaderColor);
				
				transparentRenderers.Add(savedObstacleRenderers[instanceId]);
			}
		}
	}
}