using System;
using System.Collections.Generic;
using DigitalRuby.Tween;
using UnityEngine;

namespace Assets.Scripts.Engine
{
	public class FigureInfo
	{
		private readonly Figure Figure;
		private readonly GameParameters parameters;
		private readonly CubesArray cubesArray;
		private readonly Vector2Int size;
		private Vector2Int pos;
		private RotateAction rotateAction;
		private RotationInfo rotationInfo;
		public FigureInfo(
			Figure sample,
			Vector2Int size,
			Vector2Int localPoint,
			Transform transform, 
			Color color, 
			FigureAssetScope figureAssetScope,
			GameParameters parameters,
			CubesArray cubesArray)
		{
			this.parameters = parameters;
			this.cubesArray = cubesArray;

			this.size = size;
			this.pos = localPoint;

			FigureAsset figureAsset = figureAssetScope.GetRandom();
			this.pos.y -= figureAsset.Height;

			Figure figure = Util.CreateLocal(sample, transform, this.pos.ToVector3Int());
			figure.Color = color;

			figure.FigureAsset = figureAsset;
			figure.CreateCubes();
			this.Figure = figure;

			List<Vector2> points = figureAsset.GetCubesPositions(0);
			this.rotationInfo = RotationInfo.Create(points);
		}

		private void UpdatePos()
		{
			this.Figure.transform.localPosition = this.pos.ToVector3Int();
			this.UpdateCubeArray();
		}

		private void UpdateCubeArray()
		{
			this.cubesArray.SetFigure(this.rotationInfo, this.pos);
		}

		public bool MoveDown()
		{
			this.pos.y -= 1;
			this.UpdatePos();
			return this.pos.y >= 0;
		}

		public void MoveSide(bool left)
		{
			int newX = left ? this.pos.x - 1 : this.pos.x + 1;
			if (newX + this.rotationInfo.Bounds.x >= 0 && newX + this.rotationInfo.Bounds.xMax < this.size.x)
			{
				this.pos.x = left ? this.pos.x - 1 : this.pos.x + 1;
				this.UpdatePos();
			}
		}

		public void Rotate(bool left)
		{
			if (this.rotateAction != null) return;

			this.rotationInfo = this.rotationInfo.GetRotated(left);
			this.UpdateCubeArray();

			this.rotateAction = new RotateAction(
				figure: this.Figure, 
				multiplier: left ? -1f : 1f,
				parameters: this.parameters.FigureRotation);

			this.rotateAction.Complete += () => this.rotateAction = null;
		}

		public void Update(Timing timing)
		{
			if (this.rotateAction != null)
			{
				this.rotateAction.Update(timing);
			}
		}

		private class RotateAction
		{
			private readonly FloatTween tween;
			public event Action Complete;

			public RotateAction(Figure figure, float multiplier, GameParameters.FigureRotationParameters parameters)
			{
				Transform transform = figure.transform;

				float startAngle = transform.rotation.eulerAngles.z;
				float endAngle = startAngle + 90f * multiplier;

				this.tween = figure.gameObject.Tween(
					key: "Rotate", 
					start: startAngle,
					end: endAngle, 
					duration: parameters.Time, 
					scaleFunc: parameters.Tween.ToFunction(),
					progress: t => transform.localRotation = Quaternion.AngleAxis(t.CurrentValue, Vector3.forward), 
					completion: this.OnComplete);
			}

			public void Update(Timing timing)
			{
				this.tween.Update(timing.deltaTime);
			}

			private void OnComplete(ITween<float> obj)
			{
				this.Complete?.Invoke();
			}
		}
	}
}