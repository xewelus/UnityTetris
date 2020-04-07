using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Assets.Interfaces;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using Point = System.Drawing.Point;

namespace Assets.Scripts
{
	[Serializable]
	public class Figure : MonoBehaviour, IOnValidate
	{
		[DataMember]
		public AtomCube AtomCube;

		[NonSerialized]
		public readonly List<AtomCube> Cubes = new List<AtomCube>();

		[DataMember]
		public readonly string Guid = System.Guid.NewGuid().ToString();

		[DataMember]
		public bool EnableGeneration;

		[DataMember]
		public MaterialsScope MaterialsScope;

		[DataMember]
		public FigureAsset FigureAsset;
		private FigureAsset prevFigureAsset;

		[DataMember]
		public int RotationIndex;

		[DataMember]
		public Color Color = Color.white;

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

			if (this.MaterialsScope != null)
			{
				if (this.MaterialsScope.Count > 0)
				{
					int color = Random.Range(0, this.MaterialsScope.Count);
					rndr.sharedMaterial = this.MaterialsScope.GetMaterial(color);
				}
			}
			else
			{
				rndr.sharedMaterial = Instantiate(rndr.sharedMaterial);
				rndr.sharedMaterial.color = this.Color;
			}
		}
	}
}
