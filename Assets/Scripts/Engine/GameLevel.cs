using System;
using UnityEngine;

namespace Assets.Scripts.Engine
{
	public class GameLevel
	{
		private readonly GameDesk gameDesk;
		private FigureInfo figureInfo;
		private float? lastTime;
		private readonly KeyboardController keyboard;

		public GameLevel(GameDesk gameDesk)
		{
			this.gameDesk = gameDesk;

			if (gameDesk.Parameters == null) throw new Exception("gameDesk.Parameters");
			if (gameDesk.Parameters.Keyboard == null) throw new Exception("gameDesk.Parameters.Keyboard");

			this.keyboard = new KeyboardController(gameDesk.Parameters.Keyboard);
			this.keyboard.NeedLeft += this.Keyboard_NeedLeft;
			this.keyboard.NeedRight += this.Keyboard_NeedRight;
		}

		public void Update()
		{
			if (this.gameDesk.FigureAssetScope == null) throw new NullReferenceException("FigureAssetScope");
			if (this.gameDesk.Figure == null) throw new NullReferenceException("Figure");

			float now = Time.time;
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
						figureAssetScope: this.gameDesk.FigureAssetScope);
				}
				else
				{
					this.figureInfo.MoveDown();
				}

				this.lastTime = time;
			}

			this.keyboard.Update();
		}

		private void Keyboard_NeedLeft()
		{
			if (this.figureInfo != null)
			{
				this.figureInfo.MoveSide(true);
			}
		}

		private void Keyboard_NeedRight()
		{
			if (this.figureInfo != null)
			{
				this.figureInfo.MoveSide(false);
			}
		}
	}
}
