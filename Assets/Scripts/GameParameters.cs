using System;
using System.Runtime.Serialization;
using Assets.Scripts.Engine;
using UnityEngine;

namespace Assets.Scripts
{
	[Serializable]
	public class GameParameters : MonoBehaviour
	{
		[DataMember]
		public KeyboardParameters Keyboard;

		[DataMember]
		public FigureRotationParameters FigureRotation;

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

			[DataMember]
			public KeyCode Pause = KeyCode.Pause;
		}

		[Serializable]
		public class FigureRotationParameters
		{
			[DataMember]
			public float Time = 0.1f;

			[DataMember]
			public TweenScaleFunctionsEnum Tween = TweenScaleFunctionsEnum.SineEaseInOut;
		}
	}
}
