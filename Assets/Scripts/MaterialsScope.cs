using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Assets.Interfaces;
using Assets.Scripts.Engine;
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
			if (Util.IsPlayingOrWillChangePlaymode())
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
						material = Cache.Default.GetOrCreate(color, this.Material);
						this.materials[i] = material;
					}
				}
				else
				{
					material = Cache.Default.GetOrCreate(color, this.Material);
					this.materials.Add(material);
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

		public class Cache
		{
			public static readonly Cache Default = new Cache();
			private readonly Dictionary<Key, Material> materials = new Dictionary<Key, Material>();

			public Material GetOrCreate(Color color, Material sample)
			{
				Key key = new Key(color, sample);
				lock (this.materials)
				{
					Material material;
					if (!this.materials.TryGetValue(key, out material))
					{
						material = Instantiate(sample);
						material.color = color;
						material.name = ColorUtility.ToHtmlStringRGB(color);
						this.materials.Add(key, material);
					}
					return material;
				}
			}

			private struct Key
			{
				[DataMember]
				public Color Color;
				[DataMember]
				public Material Sample;

				public Key(Color color, Material sample)
				{
					this.Color = color;
					this.Sample = sample;
				}
			}
		}
	}
}
