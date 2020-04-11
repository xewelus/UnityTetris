using UnityEngine;

namespace Assets.Scripts.Engine
{
	public static class Util
	{
		public static T CreateLocal<T>(T gameObject, Transform parent, Vector3? localPosition = null) where T : Component
		{
			T result = Object.Instantiate(gameObject, parent);
			if (localPosition == null)
			{
				localPosition = Vector3.zero;
			}
			result.transform.localPosition = localPosition.Value;
			return result;
		}

		public static bool IsPlayingOrWillChangePlaymode()
		{
#if UNITY_EDITOR
			return UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode;
#else
			return true;
#endif
		}
	}
}
