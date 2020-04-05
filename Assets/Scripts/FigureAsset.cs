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
		public RowList Rows = new RowList();

		private Size? prevSize;

		private void Fill()
		{
			int width = this.Width;
			int height = this.Height;

			for (int y = 0; y < height; y++)
			{
				Row row;
				if (y < this.Rows.List.Count)
				{
					row = this.Rows.List[y];
				}
				else
				{
					row = new Row();
					this.Rows.List.Add(row);
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

			if (this.Rows.List.Count > height)
			{
				this.Rows.List.RemoveRange(height, this.Rows.List.Count - height);
			}

			this.prevSize = new Size(width, height);
		}

		[PublicAPI]
		void OnValidate()
		{
			if (EditorApplication.isPlayingOrWillChangePlaymode)
			{
				return;
			}

			if (this.prevSize == null || this.prevSize.Value.Width != this.Width || this.prevSize.Value.Height != this.Height)
			{
				this.Fill();
			}
		}

		public IEnumerable<Point> GetCells()
		{
			for (int y = 0; y < this.Rows.List.Count; y++)
			{
				Row row = this.Rows.List[this.Rows.List.Count - y - 1];
				for (int x = 0; x < row.Values.Count; x++)
				{
					bool value = row.Values[x];
					if (value)
					{
						yield return new Point(x, y);
					}
				}
			}
		}

		[Serializable]
		public class RowList
		{
			[DataMember]
			public List<Row> List = new List<Row>();
		}

		[Serializable]
		public class Row
		{
			[DataMember]
			public List<bool> Values = new List<bool>();
		}
	}
}
