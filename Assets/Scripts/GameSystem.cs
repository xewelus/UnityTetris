using System;
using Assets.Interfaces;
using UnityEngine;

namespace Assets.Scripts
{
	[Serializable]
	public class GameSystem : MonoBehaviour, IAwake
	{
		private static GameSystem instance;
		public static GameSystem Instance { get { return instance; } }

		public void Awake()
		{
			instance = this;  
		}
	}
}
