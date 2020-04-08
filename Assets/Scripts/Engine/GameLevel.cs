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

		public GameLevel(GameDesk gameDesk)
		{
			TweenFactory.DefaultTimeFunc = () => this.timing.deltaTime;

			this.gameDesk = gameDesk;

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
					Vector3Int localPoint = new Vector3Int(this.gameDesk.Width / 2, this.gameDesk.Height, 0);
					Color color = this.gameDesk.MaterialsScope.GetRandomColor();
					this.figureInfo = new FigureInfo(
						sample: this.gameDesk.Figure,
						localPoint: localPoint,
						transform: this.gameDesk.CupLayer.transform,
						color: color,
						figureAssetScope: this.gameDesk.FigureAssetScope,
						parameters: this.gameDesk.Parameters);
				}
				else
				{
					bool ok = this.figureInfo.MoveDown();
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
	}
}
