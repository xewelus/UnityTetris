using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FigureAsset.RowList))]
public class FigureAssetDrawer : PropertyDrawer
{
	private const float ROW_HEIGHT = 20f;

	[PublicAPI]
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{  
		EditorGUI.BeginProperty(position, label, property);

		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		int indent = EditorGUI.indentLevel;
		try
		{
			EditorGUI.indentLevel = 0;

			SerializedProperty listProperty = property.FindPropertyRelative(nameof(FigureAsset.RowList.List));

			int y = 0;
			while (true)
			{
				SerializedProperty rowProperty = listProperty.GetArrayElementAtIndex(y);
				if (rowProperty == null) break;

				SerializedProperty valuesProperty = rowProperty.FindPropertyRelative(nameof(FigureAsset.Row.Values));

				int x = 0;
				while (true)
				{
					SerializedProperty valueProperty = valuesProperty.GetArrayElementAtIndex(x);
					if (valueProperty == null) break;

					Rect rect = new Rect(position.x + x * 20f, position.y + y * ROW_HEIGHT, 20f, ROW_HEIGHT);
					EditorGUI.PropertyField(rect, valueProperty, GUIContent.none);

					x++;
				}

				y++;
			}
		}
		finally
		{
			EditorGUI.indentLevel = indent;
		}

		EditorGUI.EndProperty();
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		SerializedProperty listProperty = property.FindPropertyRelative(nameof(FigureAsset.RowList.List));

		int y = 0;
		while (true)
		{
			SerializedProperty rowProperty = listProperty.GetArrayElementAtIndex(y);
			if (rowProperty == null) break;
			y++;
		}
		return y * ROW_HEIGHT;
	}
}
