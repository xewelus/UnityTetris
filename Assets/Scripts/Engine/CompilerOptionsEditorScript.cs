using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Engine
{
	[InitializeOnLoad]
	public class CompilerOptionsEditorScript
	{
		static CompilerOptionsEditorScript()
		{
			Debug.Log("Compiled");
		}
	}
}
