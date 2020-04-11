using UnityEditor;
using UnityEngine;

namespace Assets
{
	public static class CommonExtensions
	{
		/// <summary>
		/// Удаляет все чайлды в режиме редактора.
		/// </summary>
		public static void DestroyChildrenOnDelayCall(this Transform transform)
		{
#if UNITY_EDITOR
			foreach (Transform t in transform)
			{
				EditorApplication.delayCall += () =>
				                               {
					                               if (t != null)
					                               {
						                               Object.DestroyImmediate(t.gameObject);
					                               }
				                               };
			}
#endif
		}

		public static void DestroyObject<T>(this T obj) where T : MonoBehaviour
		{
			Object.Destroy(obj.gameObject);
		}

		public static Vector3Int ToVector3Int(this Vector2Int v)
		{
			return new Vector3Int(v.x, v.y, 0);
		}

		public static bool Contains(this RectInt rect, RectInt r)
		{
			return rect.Contains(r.min) && rect.Contains(r.max);
		}

		public static RectInt Offset(this RectInt rect, Vector2Int p)
		{
			rect.x += p.x;
			rect.y += p.y;
			return rect;
		}
	}
}
