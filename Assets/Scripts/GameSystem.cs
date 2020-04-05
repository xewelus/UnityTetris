using Assets.Interfaces;
using UnityEngine;

public class GameSystem : MonoBehaviour, IAwake
{
	private static GameSystem instance;
	public static GameSystem Instance { get { return instance; } }

	public GameObject AtomCube;

	public void Awake()
	{
		instance = this;  
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
