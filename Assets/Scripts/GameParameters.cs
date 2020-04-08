using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace Assets.Scripts
{
	[Serializable]
	public class GameParameters : MonoBehaviour
	{
		[DataMember]
		public KeyboardParameters Keyboard = new KeyboardParameters();

		[Serializable]
		public class KeyboardParameters
		{
			[DataMember]
			public float MoveSideDelay = 0.2f;

			[DataMember]
			public KeyCode Left = KeyCode.LeftArrow;

			[DataMember]
			public KeyCode Right = KeyCode.RightArrow;

			[DataMember]
			public KeyCode RotateLeft = KeyCode.UpArrow;

			[DataMember]
			public KeyCode RotateRight = KeyCode.Space;

			[DataMember]
			public KeyCode Down = KeyCode.DownArrow;
		}
	}
}
