using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
	private GameSystem m_Instance;
	public GameSystem Instance { get { return this.m_Instance; } }

	public static Random Rnd = new Random();

	public GameObject AtomCube;

	// ReSharper disable once UnusedMember.Local
	void Awake()
	{
		this.m_Instance = this;

		
		for (int y = 0; y < 20; y++)
		{
			for (int x = 0; x < 10; x++)
			{
				GameObject atom = Instantiate(this.AtomCube, new Vector3(x, y, 0), Quaternion.identity);
				Renderer rndr = atom.GetComponent<Renderer>();

				int color = Random.Range(1, 4);
				if (color == 1)
				{
					rndr.material.color = Color.blue;
				}
				else if (color == 2)
				{
					rndr.material.color = Color.red;
				}
				else if (color == 3)
				{
					rndr.material.color = Color.green;
				}
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
