using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using Point = System.Drawing.Point;

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

			if (this.FigureAsset == null)
			{
				return;
			}

			int rotationIndex;
			Math.DivRem(this.RotationIndex, this.FigureAsset.Rotations.Count, out rotationIndex);
			FigureAsset.Item item = this.FigureAsset.Rotations[rotationIndex];

			foreach (Point p in item.GetCells()) 
			{
				this.CreateAtom(p);
			}
		}

		private void CreateAtom(Point p)
		{
			Vector3 localPoint = new Vector3(p.X + 0.5f, p.Y + 0.5f, 0);
			Vector3 point = this.transform.TransformPoint(localPoint);

			AtomCube cube = Instantiate(this.AtomCube, point, Quaternion.identity, this.transform);
			this.Cubes.Add(cube);

			Renderer rndr = cube.GetComponent<Renderer>();
			this.SetMaterial(rndr);
		}

		protected virtual void SetMaterial(Renderer rndr)
		{
			rndr.sharedMaterial = Instantiate(rndr.sharedMaterial);
			rndr.sharedMaterial.color = this.Color;
		}

		public class Info
		{
			private readonly Figure Figure;
			private Vector3Int pos;
			public Info(Figure sample, Vector3Int localPoint, Transform transform, Color color, FigureAssetScope figureAssetScope)
			{
				this.pos = localPoint;

				FigureAsset figureAsset = figureAssetScope.GetRandom();
				this.pos.y -= figureAsset.Height;

				Vector3 point = transform.TransformPoint(this.pos);
				Figure figure = Instantiate(sample, point, Quaternion.identity, transform);
				figure.Color = color;

				figure.FigureAsset = figureAsset;
				figure.CreateCubes();
				this.Figure = figure;
			}

			public void MoveDown()
			{
				this.pos.y -= 1;
				this.Figure.transform.localPosition = this.pos;
			}
		}
	}
}
