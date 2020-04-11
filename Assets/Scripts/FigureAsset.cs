using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using Assets.Interfaces;
using Assets.Scripts.Engine;
using UnityEngine;

namespace Assets.Scripts
{
	[Serializable]
	public class FigureAsset : MonoBehaviour, IOnValidate
	{
		private const int DEFAULT_SIZE = 4;

		[DataMember]
		public int Width = DEFAULT_SIZE;

		[DataMember]
		public int Height = DEFAULT_SIZE;

		[DataMember]
		public List<Item> Rotations = new List<Item>();

		private Size? prevSize;

		public void OnValidate()
		{
			if (Util.IsPlayingOrWillChangePlaymode())
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

		private Item GetFigureAssetItem(int index)
		{
			int rotationIndex;
			Math.DivRem(index, this.Rotations.Count, out rotationIndex);
			FigureAsset.Item item = this.Rotations[rotationIndex];
			return item;
		}

		public List<Vector2> GetCubesPositions(int index)
		{
			FigureAsset.Item item = this.GetFigureAssetItem(index);
			List<Vector2> list = item.GetCubesPositions();
			return list;
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

			private IEnumerable<Vector2Int> GetCells()
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

			private Vector2 GetCenter()
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

			public List<Vector2> GetCubesPositions()
			{
				Vector2 center = this.GetCenter();

				List<Vector2> list = new List<Vector2>();
				foreach (Vector2Int point in this.GetCells())
				{
					Vector2 p = point - center + Vector2.one * 0.5f;
					list.Add(p);
				}
				return list;
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
