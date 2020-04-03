using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
	private GameSystem m_Instance;
	public GameSystem Instance { get { return this.m_Instance; } }

	public GameObject AtomCube;

	// ReSharper disable once UnusedMember.Local
	void Awake()
	{
		this.m_Instance = this;

		
		for (int y = 0; y < 20; y++)
		{
			for (int x = 0; x < 10; x++)
			{
				Instantiate(this.AtomCube, new Vector3(x, y, 0), Quaternion.identity);
			}
		}
	}

	// ReSharper disable once UnusedMember.Local
	void OnDestroy()
	{
		this.m_Instance = null;
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
