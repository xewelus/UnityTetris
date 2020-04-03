using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
	private GameSystem m_Instance;
	public GameSystem Instance { get { return this.m_Instance; } }

	// ReSharper disable once UnusedMember.Local
	void Awake()
	{
		this.m_Instance = this;
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
