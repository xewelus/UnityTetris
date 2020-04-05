using System;
using System.Runtime.Serialization;
using Assets.Interfaces;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using Point = System.Drawing.Point;

namespace Assets.Scripts
{
	[Serializable]
	public class Figure : MonoBehaviour, IOnValidate, IOnGUI, IStart, IUpdate
	{
		[DataMember]
		public GameObject AtomCube;

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
		public Color Color = Color.white;

		private bool wasOnValidate;

		public void OnGUI()
		{
			// common GUI code goes here
			Debug.Log("OnGui");

			if (GUI.Button(new Rect(Random.Range(10, 600), 10, 150, 100), "I am a button " + Random.Range(0, 1)))
			{
				print("You clicked the button!");
			}
		}

		public void Start()
		{
		}

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

			if (this.FigureAsset == null)
			{
				return;
			}

			foreach (Point p in this.FigureAsset.GetCells())
			{
				this.CreateAtom(p);
			}

			this.prevFigureAsset = this.FigureAsset;
		}

		private void CreateAtom(Point p)
		{
			Vector3 localPoint = new Vector3(p.X, p.Y, 0);
			Vector3 point = this.transform.TransformPoint(localPoint);

			GameObject atom = Instantiate(this.AtomCube, point, Quaternion.identity, this.transform);

			Renderer rndr = atom.GetComponent<Renderer>();

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

		public void Update()
		{
			this.transform.Rotate(0.1f, 0f, 0f);
		}
	}
}
