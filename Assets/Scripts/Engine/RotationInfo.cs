using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Engine
{
	public class RotationInfo
	{
		private readonly Collection collection;
		public readonly List<Vector2> Points;
		public readonly List<Vector2Int> Points0 = new List<Vector2Int>();
		private readonly int index;

		private RotationInfo(List<Vector2> points, Collection collection, int index)
		{
			this.Points = points;
			this.collection = collection;
			this.index = index;

			foreach (Vector2 point in points)
			{
				Vector2Int p = new Vector2Int(Mathf.CeilToInt(point.x), Mathf.FloorToInt(point.y));
				this.Points0.Add(p);
			}
		}

		public static RotationInfo Create(List<Vector2> points)
		{
			Collection collection = new Collection(points);
			RotationInfo info = collection.Get(0);
			return info;
		}

		public RotationInfo GetRotated(bool left)
		{
			return this.collection.GetRotated(left, this.index);
		}

		private RotationInfo CreateRotated()
		{
			List<Vector2> list = Rotate(this.Points);
			RotationInfo info = new RotationInfo(list, this.collection, this.index + 1);
			return info;
		}

		private static List<Vector2> Rotate(List<Vector2> list)
		{
			List<Vector2> result = new List<Vector2>();
			foreach (Vector2 point in list)
			{
				Vector2 p = new Vector2(point.y, -point.x);
				result.Add(p);
			}
			return result;
		}

		private class Collection
		{
			private readonly List<RotationInfo> infos = new List<RotationInfo>(4);

			public Collection(List<Vector2> list)
			{
				RotationInfo info = new RotationInfo(list, this, 0);
				this.infos.Add(info);

				for (int i = 0; i < 3; i++)
				{
					info = info.CreateRotated();
					this.infos.Add(info);
				}
			}

			public RotationInfo Get(int index)
			{
				return this.infos[index];
			}

			protected internal RotationInfo GetRotated(bool left, int index)
			{
				int i;
				if (left)
				{
					if (index > 0)
					{
						i = index - 1;
					}
					else
					{
						i = this.infos.Count - 1;
					}
				}
				else
				{
					if (index < this.infos.Count - 1)
					{
						i = index + 1;
					}
					else
					{
						i = 0;
					}
				}

				return this.infos[i];
			}
		}
	}
}
