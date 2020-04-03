using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
	private GameSystem m_Instance;
	public GameSystem Instance { get { return m_Instance; } }

	void Awake()
	{
		m_Instance = this;
	}

	void OnDestroy()
	{
		m_Instance = null;
	}

	void Update()
	{
		// global game update logic goes here
	}

	void OnGui()
	{
		// common GUI code goes here
	}
}
