using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Assets.Scripts.Engine;
using UnityEngine;

namespace Assets.Scripts
{
	[Serializable]
	public class Figure : MonoBehaviour
	{
		[DataMember]
		public AtomCube AtomCube;

		[NonSerialized]
		public readonly List<AtomCube> Cubes = new List<AtomCube>();

		[DataMember]
		public FigureAsset FigureAsset;

		[DataMember]
		public int RotationIndex;

		[DataMember]
		public Color Color = Color.white;

		public void CreateCubes()
		{
			this.Cubes.Clear();

			if (this.FigureAsset == null) return;

			List<Vector2> posList = this.FigureAsset.GetCubesPositions(this.RotationIndex);
			foreach (Vector2 p in posList)
			{
				this.CreateAtom(p);
			}
		}

		private void CreateAtom(Vector2 p, Color? color = null)
		{
			AtomCube cube = Util.CreateLocal(this.AtomCube, this.transform, p);
			this.Cubes.Add(cube);

			Renderer rndr = cube.GetComponent<Renderer>();
			this.SetMaterial(rndr, color);
		}

		protected virtual void SetMaterial(Renderer rndr, Color? color = null)
		{
			color = color ?? this.Color;
			rndr.sharedMaterial = MaterialsScope.Cache.Default.GetOrCreate(color.Value, rndr.sharedMaterial);
		}
	}
}
