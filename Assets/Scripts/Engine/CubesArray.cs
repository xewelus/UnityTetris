using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Engine
{
	public class CubesArray
	{
		private readonly List<Row> Rows = new List<Row>();
		private readonly List<Cell> figureCells = new List<Cell>();
		public readonly RectInt Bounds;

		public delegate void CellChangedDelegate(CellChangedEventArgs e);
		public event CellChangedDelegate CellChanged;

		public CubesArray(int width, int height)
		{
			this.Bounds = new RectInt(0, 0, width, height);

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

		public void FixFigure()
		{
			foreach (Cell cell in this.figureCells)
			{
				this.SetCell(cell.Point, CellType.Fixed);
			}
			this.figureCells.Clear();

			this.CheckFullRows();
		}

		private void CheckFullRows()
		{
			for (int y = this.Rows.Count - 1; y >= 0; y--)
			{
				Row row = this.Rows[y];
				if (row.IsFull())
				{
					this.RemoveRow(y);
				}
			}
		}

		private void RemoveRow(int rowIndex)
		{
			for (int y = rowIndex; y < this.Bounds.height; y++)
			{
				for (int x = 0; x < this.Bounds.width; x++)
				{
					Vector2Int destPoint = new Vector2Int(x, y);
					if (y == this.Rows.Count - 1)
					{
						this.SetCell(destPoint, CellType.None);
					}
					else
					{
						Cell srcCell = this.GetCell(new Vector2Int(x, y + 1));
						CellType cellType = srcCell.Type == CellType.None ? CellType.None : CellType.Fixed;
						this.SetCell(destPoint, cellType);
					}
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
				this.SetCell(cell.Point, CellType.None);
			}
			this.figureCells.Clear();
		}

		public bool CheckFigure(RotationInfo rotationInfo, Vector2Int newPos, bool checkCells)
		{
			RectInt rect = rotationInfo.Bounds;
			rect = rect.Offset(newPos);
			if (!this.Bounds.Contains(rect))
			{
				return false;
			}

			if (checkCells)
			{
				foreach (Vector2Int point in rotationInfo.Points0)
				{
					Vector2Int p = point + newPos;
					Cell cell = this.GetCell(p);
					if (cell == null) continue;

					if (cell.Type == CellType.Fixed)
					{
						return false;
					}
				}
			}

			return true;
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
				CellChangedEventArgs args = new CellChangedEventArgs(cell.Point, prevCellType, cell.Type, cell.AtomCubeItem);
				this.CellChanged.Invoke(args);
				cell.AtomCubeItem = args.NewAtomCubeItem;
			}
		}

		public void SetTestLine()
		{
			for (int i = 0; i < this.Bounds.xMax - 1; i++)
			{
				this.SetCell(new Vector2Int(i, 0), CellType.Fixed);
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
			public AtomCubePool.Item AtomCubeItem;

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
			public readonly List<Cell> Cells = new List<Cell>();

			public bool IsFull()
			{
				foreach (Cell cell in this.Cells)
				{
					if (cell.Type != CellType.Fixed) return false;
				}
				return true;
			}
		}

		public class CellChangedEventArgs : EventArgs
		{
			public readonly Vector2Int Point;
			public readonly CellType PrevCellType;
			public readonly CellType NewCellType;
			public readonly AtomCubePool.Item PrevAtomCubeItem;
			public AtomCubePool.Item NewAtomCubeItem;

			public CellChangedEventArgs(Vector2Int point, CellType prevCellType, CellType newCellType, AtomCubePool.Item prevAtomCubeItem)
			{
				this.Point = point;
				this.PrevCellType = prevCellType;
				this.NewCellType = newCellType;
				this.PrevAtomCubeItem = prevAtomCubeItem;
				this.NewAtomCubeItem = prevAtomCubeItem;
			}
		}
	}
}