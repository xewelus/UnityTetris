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

			this.UpdatePos();
		}

		private Vector2 GetPostion()
		{
			return this.pos + this.rotationInfo.Bounds.center + new Vector2(0.5f, 0.5f);
		}

		private void UpdatePos()
		{
			if (this.rotateAction != null)
			{
				this.rotateAction.NeedMove = false;
			}
			this.Figure.transform.localPosition = this.GetPostion();
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

			Vector2 position = this.GetPostion();

			this.rotateAction = new RotateAction(
				figure: this.Figure, 
				multiplier: left ? 1f : -1f,
				position: position, 
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
			private readonly Transform transform;
			private readonly float multiplier;
			private readonly float startAngle;
			private readonly Vector2 position0;
			private readonly Vector2 position;
			public event Action Complete;
			public bool NeedMove = true;

			public RotateAction(Figure figure, float multiplier, Vector2 position, GameParameters.FigureRotationParameters parameters)
			{
				this.multiplier = multiplier;
				this.transform = figure.transform;
				this.position0 = figure.transform.localPosition;
				this.position = position;
				this.startAngle = figure.transform.rotation.eulerAngles.z;

				this.tween = figure.gameObject.Tween(
					key: "Rotate", 
					start: 0f,
					end: 1f, 
					duration: parameters.Time, 
					scaleFunc: parameters.Tween.ToFunction(),
					progress: this.Progress, 
					completion: this.OnComplete);
			}

			private void Progress(ITween<float> t)
			{
				float angle = this.startAngle + 90f * this.multiplier * t.CurrentValue;
				this.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);

				if (this.NeedMove)
				{
					Vector2 p = this.position0 + (this.position - this.position0) * t.CurrentValue;
					this.transform.localPosition = p;
				}
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