using UnityEngine;

namespace Assets.Scripts.Engine
{
	public class FigureInfo
	{
		private readonly Figure Figure;
		private Vector3Int pos;
		public FigureInfo(Figure sample, Vector3Int localPoint, Transform transform, Color color, FigureAssetScope figureAssetScope)
		{
			this.pos = localPoint;

			FigureAsset figureAsset = figureAssetScope.GetRandom();
			this.pos.y -= figureAsset.Height;

			Vector3 point = transform.TransformPoint(this.pos);
			Figure figure = GameObject.Instantiate(sample, point, Quaternion.identity, transform);
			figure.Color = color;

			figure.FigureAsset = figureAsset;
			figure.CreateCubes();
			this.Figure = figure;
		}

		public void MoveDown()
		{
			this.pos.y -= 1;
			this.Figure.transform.localPosition = this.pos;
		}

		public void MoveSide(bool left)
		{
			this.pos.x = left ? this.pos.x - 1 : this.pos.x + 1;
			this.Figure.transform.localPosition = this.pos;
		}
	}
}