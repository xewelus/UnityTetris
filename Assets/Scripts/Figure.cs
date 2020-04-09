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

			List<Vector2> posList = this.GetCubesPositions();
			foreach (Vector2 p in posList)
			{
				this.CreateAtom(p);
			}
		}

		public List<Vector2> GetCubesPositions()
		{
			FigureAsset.Item item = this.GetFigureAssetItem();
			Vector2 center = item.GetCenter();

			List<Vector2> list = new List<Vector2>();
			foreach (Vector2Int point in item.GetCells())
			{
				Vector2 p = point - center + Vector2.one * 0.5f;
				list.Add(p);
			}
			return list;
		}

		public FigureAsset.Item GetFigureAssetItem()
		{
			int rotationIndex;
			Math.DivRem(this.RotationIndex, this.FigureAsset.Rotations.Count, out rotationIndex);
			FigureAsset.Item item = this.FigureAsset.Rotations[rotationIndex];
			return item;
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
