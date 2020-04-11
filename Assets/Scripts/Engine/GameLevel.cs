using System;
using DigitalRuby.Tween;
using UnityEngine;

namespace Assets.Scripts.Engine
{
	public partial class GameLevel
	{
		private readonly GameDesk gameDesk;
		private FigureInfo figureInfo;
		private float? lastTime;
		private readonly KeyboardController keyboard;
		private readonly Timing timing = new Timing();
		private readonly CubesArray cubesArray;
		private readonly AtomCubePool atomCubePool;

		public GameLevel(GameDesk gameDesk)
		{
			TweenFactory.DefaultTimeFunc = () => this.timing.deltaTime;

			this.gameDesk = gameDesk;

			this.cubesArray = new CubesArray(gameDesk.Width, gameDesk.Height);
			this.cubesArray.CellChanged += this.CubesArray_OnCellChanged;

			if (gameDesk.Parameters == null) throw new Exception("gameDesk.Parameters");
			if (gameDesk.Parameters.Keyboard == null) throw new Exception("gameDesk.Parameters.Keyboard");

			this.keyboard = new KeyboardController(gameDesk.Parameters.Keyboard, this.timing);
			KeyboardEvents.Init(this, this.keyboard);

			this.atomCubePool = new AtomCubePool(this.gameDesk.AtomCube, MaterialsScope.Cache.Default);
		}

		public void Update()
		{
			if (this.gameDesk.FigureAssetScope == null) throw new NullReferenceException("FigureAssetScope");
			if (this.gameDesk.Figure == null) throw new NullReferenceException("Figure");

			this.timing.Update();
			this.UpdateFigure();
			this.keyboard.Update();
		}

		private void UpdateFigure()
		{
			float now = this.timing.time;
			if (this.figureInfo == null)
			{
				this.figureInfo = this.CreateFigure();
				this.lastTime = now;
			}
			else if (this.lastTime == null || this.lastTime.Value + this.gameDesk.TimeInterval <= now)
			{
				this.MoveFigureDown();
			}

			if (this.figureInfo != null)
			{
				this.figureInfo.Update(this.timing);
			}
		}

		private void MoveFigureDown()
		{
			if (this.figureInfo == null) return;

			bool ok = this.figureInfo.MoveDown(true);
			this.lastTime = this.timing.time;
			if (!ok)
			{
				this.figureInfo = null;
			}
		}

		private FigureInfo CreateFigure()
		{
			Vector2Int localPoint = new Vector2Int(this.gameDesk.Width / 2, this.gameDesk.Height);
			Color color = this.gameDesk.MaterialsScope.GetRandomColor();
			FigureInfo info = new FigureInfo(
				sample: this.gameDesk.Figure,
				localPoint: localPoint,
				transform: this.gameDesk.CupLayer.transform,
				color: color,
				figureAssetScope: this.gameDesk.FigureAssetScope,
				parameters: this.gameDesk.Parameters,
				cubesArray: this.cubesArray,
				atomCubePool: this.atomCubePool);
			return info;
		}

		private void CubesArray_OnCellChanged(CubesArray.CellChangedEventArgs e)
		{
			if (e.PrevCellType == CubesArray.CellType.Fixed && e.NewCellType == CubesArray.CellType.None)
			{
				if (e.PrevAtomCubeItem != null)
				{
					e.PrevAtomCubeItem.AtomCube.transform.parent = null;
					this.atomCubePool.Release(e.PrevAtomCubeItem);
				}
			}

			if (e.PrevCellType != CubesArray.CellType.Fixed && e.NewCellType == CubesArray.CellType.Fixed)
			{
				AtomCubePool.Item poolItem = this.atomCubePool.Get();
				poolItem.AtomCube.transform.parent = this.gameDesk.CupLayer.transform;
				poolItem.AtomCube.transform.localPosition = new Vector3(e.Point.x + 0.5f, e.Point.y + 0.5f);
				poolItem.SetColor(Color.grey);
				e.NewAtomCubeItem = poolItem;
			}
		}
	}
}
