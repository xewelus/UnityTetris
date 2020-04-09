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
			float? time = this.lastTime;
			while (true)
			{
				if (time == null)
				{
					time = now;
				}
				else
				{
					time += this.gameDesk.TimeInterval;
					if (time > now)
					{
						break;
					}
				}

				if (this.figureInfo == null)
				{
					Vector2Int localPoint = new Vector2Int(this.gameDesk.Width / 2, this.gameDesk.Height);
					Color color = this.gameDesk.MaterialsScope.GetRandomColor();
					this.figureInfo = new FigureInfo(
						sample: this.gameDesk.Figure,
						localPoint: localPoint,
						transform: this.gameDesk.CupLayer.transform,
						color: color,
						figureAssetScope: this.gameDesk.FigureAssetScope,
						parameters: this.gameDesk.Parameters,
						cubesArray: this.cubesArray);
				}
				else
				{
					bool ok = this.figureInfo.MoveDown(true);
					if (!ok)
					{
						this.figureInfo = null;
					}
				}

				this.lastTime = time;
			}

			if (this.figureInfo != null)
			{
				this.figureInfo.Update(this.timing);
			}
		}

		private void CubesArray_OnCellChanged(CubesArray.CellChangedEventArgs e)
		{
			if (e.PrevAtomCube != null)
			{
				e.PrevAtomCube.DestroyObject();
				e.NewAtomCube = null;
			}

			if (e.NewCellType != CubesArray.CellType.None)
			{
				Vector3 pos = new Vector3(e.Point.x + 0.5f, e.Point.y + 0.5f);
				AtomCube cube = Util.CreateLocal(this.gameDesk.AtomCube, this.gameDesk.CupLayer.transform, pos);
				cube.GetComponent<Renderer>().material.color = Color.grey;
				e.NewAtomCube = cube;
			}
		}
	}
}
