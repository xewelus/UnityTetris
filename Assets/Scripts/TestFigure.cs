using System;
using System.Runtime.Serialization;
using Assets.Interfaces;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
	[Serializable]
	public class TestFigure : Figure, IOnValidate
	{
		[DataMember]
		public MaterialsScope MaterialsScope;

		[DataMember]
		public bool EnableGeneration;

		[DataMember]
		public readonly string Guid = System.Guid.NewGuid().ToString();

		private FigureAsset prevFigureAsset;
		private bool wasOnValidate;
		public void OnValidate()
		{
			if (!this.wasOnValidate)
			{
				this.wasOnValidate = true;
				this.prevFigureAsset = this.FigureAsset;
			}

			if (EditorApplication.isPlayingOrWillChangePlaymode)
			{
				return;
			}

			if (!this.EnableGeneration)
			{
				return;
			}

			if (this.AtomCube == null)
			{
				return;
			}

			if (this.prevFigureAsset == this.FigureAsset)
			{
				return;
			}

			Debug.Log("Need regenerate " + this.name);

			this.transform.DestroyChildrenOnDelayCall();
			this.CreateCubes();

			this.prevFigureAsset = this.FigureAsset;
		}

		protected override void SetMaterial(Renderer rndr, Color? color = null)
		{
			if (this.MaterialsScope == null)
			{
				base.SetMaterial(rndr);
				return;
			}

			if (this.MaterialsScope.Count > 0)
			{
				int colorInt = Random.Range(0, this.MaterialsScope.Count);
				rndr.sharedMaterial = this.MaterialsScope.GetMaterial(colorInt);
			}
		}
	}
}