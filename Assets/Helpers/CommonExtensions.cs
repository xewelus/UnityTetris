using UnityEditor;
using UnityEngine;

namespace Assets
{
	public static class CommonExtensions
	{
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
