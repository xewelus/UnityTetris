using System;
using Assets.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

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
			//this.FillCubes();
		}

		// ReSharper disable once UnusedMember.Local
		void OnDestroy()
		{
			instance = null; 
		}

		// ReSharper disable once UnusedMember.Local
		void Update()
		{
			// global game update logic goes here
		}

		// ReSharper disable once UnusedMember.Local
		void OnGui()
		{
			// common GUI code goes here
		}
	}
}
