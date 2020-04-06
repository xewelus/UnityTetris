﻿using System.Runtime.Serialization;
using Assets.Interfaces;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
	public class GameDesk : MonoBehaviour, IOnValidate
	{
		[DataMember]
		public int Width = 10;

		[DataMember]
		public int Height = 20;

		[DataMember]
		public GameObject AtomCube;

		[DataMember]
		public MaterialsScope MaterialsScope;

		[DataMember]
		public GameObject CupLayer;

		[DataMember]
		public GameObject BackWall;

		[DataMember]
		public bool ShowTestCubes;

		private bool isValidated;

		public void OnValidate()
		{
			if (EditorApplication.isPlayingOrWillChangePlaymode)
			{
				return;
			}

			if (this.BackWall != null)
			{
				this.BackWall.transform.localScale = new Vector3(this.Width, this.Height, this.BackWall.transform.localScale.z);
				this.BackWall.transform.localPosition = new Vector3(this.Width / 2f, this.Height / 2f, this.BackWall.transform.localPosition.z);
			}

			if (this.CupLayer != null)
			{
				this.CupLayer.transform.DestroyChildrenOnDelayCall();

				if (this.ShowTestCubes && this.AtomCube != null)
				{
					this.FillTestCubes();
				}
			}
		}

		private void FillTestCubes()
		{
			for (int y = 0; y < this.Height; y++)
			{
				for (int x = 0; x < this.Width; x++)
				{
					CreateRandomAtom(x, y, this.AtomCube, this.CupLayer, this.MaterialsScope);
				}
			}
		}

		private static void CreateRandomAtom(int x, int y, GameObject atomCube, GameObject cupLayer, MaterialsScope materialsScope)
		{
			Vector3 localPoint = new Vector3(x + 0.5f, y + 0.5f, 0);
			Vector3 point = cupLayer.transform.TransformPoint(localPoint);

			GameObject atom = Instantiate(atomCube, point, Quaternion.identity, cupLayer.transform);

			Renderer rndr = atom.GetComponent<Renderer>();

			if (materialsScope?.Count > 0)
			{
				int color = Random.Range(0, materialsScope.Count);
				rndr.sharedMaterial = materialsScope.GetMaterial(color);
			}
		}
	}
}
