using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Engine
{
	public class CubesArray
	{
		private readonly List<Row> Rows = new List<Row>();
		private readonly List<Cell> figureCells = new List<Cell>();

		public delegate void CellChangedDelegate(CellChangedEventArgs e);
		public event CellChangedDelegate CellChanged;

		public CubesArray(int width, int height)
		{
			for (int y = 0; y < height; y++)
			{
				Row row = new Row();
				this.Rows.Add(row);

				for (int x = 0; x < width; x++)
				{
					Cell cell = new Cell(new Vector2Int(x, y));
					row.Cells.Add(cell);
				}
			}
		}

		public void SetFigure(RotationInfo figureAsset, Vector2Int point)
		{
			this.RemoveFigure();

			foreach (Vector2Int p in figureAsset.Points0)
			{
				Vector2Int pp = p + point;
				Cell cell = this.SetCell(pp, CellType.Figure);
				if (cell != null)
				{
					this.figureCells.Add(cell);
				}
			}
		}

		private void RemoveFigure()
		{
			foreach (Cell cell in this.figureCells)
			{
				Debug.Log("RemoveFigure " + cell);
				this.SetCell(cell.Point, CellType.None);
			}
			this.figureCells.Clear();
		}

		private Cell GetCell(Vector2Int point)
		{
			if (point.y < 0) return null;
			if (point.y >= this.Rows.Count) return null;

			Row row = this.Rows[point.y];
			if (point.x < 0) return null;
			if (point.x >= row.Cells.Count) return null;

			Cell cell = row.Cells[point.x];
			return cell;
		}

		private Cell SetCell(Vector2Int point, CellType cellType)
		{
			Cell cell = this.GetCell(point);
			if (cell != null)
			{
				CellType prevCellType = cell.Type;
				cell.Type = cellType;
				this.InvokeCellChanged(cell, prevCellType);
			}
			return cell;
		}

		private void InvokeCellChanged(Cell cell, CellType prevCellType)
		{
			if (this.CellChanged != null)
			{
				CellChangedEventArgs args = new CellChangedEventArgs(cell.Point, prevCellType, cell.Type, cell.AtomCube);
				this.CellChanged.Invoke(args);
				cell.AtomCube = args.NewAtomCube;
			}
		}

		public enum CellType
		{
			None,
			Fixed,
			Figure
		}

		private class Cell
		{
			public readonly Vector2Int Point;
			public CellType Type = CellType.None;
			public AtomCube AtomCube;

			public Cell(Vector2Int point)
			{
				this.Point = point;
			}

			public override string ToString()
			{
				return this.Point.ToString();
			}
		}

		private class Row
		{
			public List<Cell> Cells = new List<Cell>();
		}

		public class CellChangedEventArgs : EventArgs
		{
			public readonly Vector2Int Point;
			public readonly CellType PrevCellType;
			public readonly CellType NewCellType;
			public readonly AtomCube PrevAtomCube;
			public AtomCube NewAtomCube;

			public CellChangedEventArgs(Vector2Int point, CellType prevCellType, CellType newCellType, AtomCube prevAtomCube)
			{
				this.Point = point;
				this.PrevCellType = prevCellType;
				this.NewCellType = newCellType;
				this.PrevAtomCube = prevAtomCube;
				this.NewAtomCube = prevAtomCube;
			}
		}
	}
}