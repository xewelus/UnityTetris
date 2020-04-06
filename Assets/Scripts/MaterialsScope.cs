using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Assets.Interfaces;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
	[Serializable]
	public class MaterialsScope : MonoBehaviour, IOnValidate
	{
		[DataMember]
		public Material Material;

		[DataMember]
		public List<Color> Colors = new List<Color>();

		[SerializeField, HideInInspector]
		private List<Material> materials = new List<Material>();

		public void OnValidate()
		{
			if (EditorApplication.isPlayingOrWillChangePlaymode)
			{
				return;
			}

			if (this.Material == null)
			{
				if (this.materials.Count > 0)
				{
					this.materials.Clear();
				}
				return;
			}

			for (int i = 0; i < this.Colors.Count; i++)
			{
				Color color = this.Colors[i];

				Material material;
				if (i < this.materials.Count)
				{
					material = this.materials[i];
					if (material == null)
					{
						material = Instantiate(this.Material);
						this.materials[i] = material;
					}
				}
				else
				{
					material = Instantiate(this.Material);
					this.materials.Add(material);
				}

				if (material.color != color)
				{
					material.color = color;
					material.name = ColorUtility.ToHtmlStringRGB(color);
				}
			}

			if (this.materials.Count > this.Colors.Count)
			{
				this.materials.RemoveRange(this.Colors.Count, this.materials.Count - this.Colors.Count);
			}
		}

		public int Count
		{
			get
			{
				return this.materials.Count;
			}
		}

		public Material GetMaterial(int index)
		{
			return this.materials[index];
		}

		public Color GetRandomColor()
		{
			return this.Colors[UnityEngine.Random.Range(0, this.Colors.Count)];
		}
	}
}
