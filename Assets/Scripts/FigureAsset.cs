using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

[Serializable]
public class FigureAsset : MonoBehaviour
{
	private const int DEFAULT_SIZE = 4;

	[DataMember]
	public int Width = DEFAULT_SIZE;

	[DataMember]
	public int Height = DEFAULT_SIZE;

	[DataMember]
	public List<Row> Rows = new List<Row>();

	[DataMember]
	public bool[,] Array = new bool[DEFAULT_SIZE, DEFAULT_SIZE];

	private Size? prevSize;


	private void Fill()
	{
		this.Array = new bool[this.Width, this.Height];

		for (int y = 0; y < this.Height; y++)
		{
			Row row;
			if (y < this.Rows.Count)
			{
				row = this.Rows[y];
			}
			else
			{
				row = new Row();
				this.Rows.Add(row);
			}

			for (int x = 0; x < this.Width; x++)
			{
				if (x >= row.Values.Count)
				{
					row.Values.Add(false);
				}

				this.Array[x, y] = false;
			}

			if (row.Values.Count > this.Width)
			{
				row.Values.RemoveRange(this.Width, row.Values.Count - this.Width);
			}
		}

		if (this.Rows.Count > this.Height)
		{
			this.Rows.RemoveRange(this.Height, this.Rows.Count - this.Height);
		}

		this.prevSize = new Size(this.Width, this.Height);
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

	[Serializable]
	public class Row
	{
		[DataMember]
		public List<bool> Values = new List<bool>();
	}
}
