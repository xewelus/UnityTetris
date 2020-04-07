using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
	[CustomPropertyDrawer(typeof(FigureAsset.Item))]
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

				SerializedProperty listProperty = property.FindPropertyRelative(nameof(FigureAsset.Item.List));

				for (int y = 0; y < listProperty.arraySize; y++)
				{
					SerializedProperty rowProperty = listProperty.GetArrayElementAtIndex(y);
					SerializedProperty valuesProperty = rowProperty.FindPropertyRelative(nameof(FigureAsset.Row.Values));

					for (int x = 0; x < valuesProperty.arraySize; x++)
					{
						SerializedProperty valueProperty = valuesProperty.GetArrayElementAtIndex(x);

						Rect rect = new Rect(position.x + x * 20f, position.y + y * ROW_HEIGHT, 20f, ROW_HEIGHT);
						EditorGUI.PropertyField(rect, valueProperty, GUIContent.none);
					}
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
			SerializedProperty listProperty = property.FindPropertyRelative(nameof(FigureAsset.Item.List));
			return Math.Max(1, listProperty.arraySize) * ROW_HEIGHT;
		}
	}
}
