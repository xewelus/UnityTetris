using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
	private static GameSystem instance;
	public static GameSystem Instance { get { return instance; } }

	public GameObject AtomCube;
	public Material Material1;
	public Material Material2;
	public Material Material3;

	// ReSharper disable once UnusedMember.Local
	void Awake()
	{
		instance = this;  
		UnityEditor.EditorUtility.DisplayDialog("Hello World!", "Hello", "OK");
		//this.FillCubes();
	}

	private void FillCubes()
	{
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
