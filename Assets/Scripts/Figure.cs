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

		[DataMember]
		public FigureAsset FigureAsset;

		[DataMember]
		public int RotationIndex;

		[DataMember]
		public Color Color = Color.white;

		public List<AtomCubePool.Item> CreateCubes(AtomCubePool pool)
		{
			if (this.FigureAsset == null) return null;

			List<AtomCubePool.Item> list = new List<AtomCubePool.Item>();
			List<Vector2> posList = this.FigureAsset.GetCubesPositions(this.RotationIndex);
			foreach (Vector2 p in posList)
			{
				AtomCubePool.Item item = this.CreateAtom(pool, p);
				list.Add(item);
			}
			return list;
		}

		private AtomCubePool.Item CreateAtom(AtomCubePool pool, Vector2 p, Color? color = null)
		{
			AtomCubePool.Item item = pool.Get();
			AtomCube cube = item.AtomCube;
			cube.transform.parent = this.transform;
			cube.transform.localPosition = p;

			Renderer rndr = cube.GetComponent<Renderer>();
			this.SetMaterial(item, rndr, color);
			return item;
		}

		protected virtual void SetMaterial(AtomCubePool.Item item, Renderer rndr, Color? color)
		{
			color = color ?? this.Color;
			item.SetColor(color.Value);
		}
	}
}
