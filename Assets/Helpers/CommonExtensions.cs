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
		}
	}
}
