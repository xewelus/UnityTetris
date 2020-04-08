using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
	[Serializable]
	public class FigureAsset : MonoBehaviour
	{
		private const int DEFAULT_SIZE = 4;

		[DataMember]
		public int Width = DEFAULT_SIZE;

		[DataMember]
		public int Height = DEFAULT_SIZE;

		[DataMember]
		public List<Item> Rotations = new List<Item>();

		private Size? prevSize;

		[PublicAPI]
		void OnValidate()
		{
			if (EditorApplication.isPlayingOrWillChangePlaymode)
			{
				return;
			}

			if (this.prevSize == null || this.prevSize.Value.Width != this.Width || this.prevSize.Value.Height != this.Height)
			{
				foreach (Item rowList in this.Rotations)
				{
					rowList.Fill(this.Width, this.Height);
				}
				this.prevSize = new Size(this.Width, this.Height);
			}
		}

		[Serializable]
		public class Item
		{
			[DataMember]
			public List<Row> List = new List<Row>();

			public void Fill(int width, int height)
			{
				for (int y = 0; y < height; y++)
				{
					Row row;
					if (y < this.List.Count)
					{
						row = this.List[y];
					}
					else
					{
						row = new Row();
						this.List.Add(row);
					}

					for (int x = 0; x < width; x++)
					{
						if (x >= row.Values.Count)
						{
							row.Values.Add(false);
						}
					}

					if (row.Values.Count > width)
					{
						row.Values.RemoveRange(width, row.Values.Count - width);
					}
				}

				if (this.List.Count > height)
				{
					this.List.RemoveRange(height, this.List.Count - height);
				}
			}

			public IEnumerable<Vector2Int> GetCells()
			{
				for (int y = 0; y < this.List.Count; y++)
				{
					Row row = this.List[this.List.Count - y - 1];
					for (int x = 0; x < row.Values.Count; x++)
					{
						bool value = row.Values[x];
						if (value)
						{
							yield return new Vector2Int(x, y);
						}
					}
				}
			}

			public Vector2 GetCenter()
			{
				Vector2Int? min = null;
				Vector2Int? max = null;
				foreach (Vector2Int point in this.GetCells())
				{
					if (min == null) min = point;
					if (max == null) max = point;
					min = new Vector2Int(Math.Min(min.Value.x, point.x), Math.Min(min.Value.y, point.y));
					max = new Vector2Int(Math.Max(max.Value.x, point.x), Math.Max(max.Value.y, point.y));
				}
				if (min == null || max == null) throw new Exception("min == null || max == null");
				Vector2 p = (Vector2.one + max.Value - min.Value) / 2f + min.Value;
				return p;
			}
		}

		[Serializable]
		public class Row
		{
			[DataMember]
			public List<bool> Values = new List<bool>();
		}
	}
}
