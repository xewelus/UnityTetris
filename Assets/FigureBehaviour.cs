using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class FigureBehaviour : MonoBehaviour
{
	public GameObject AtomCube;
	public int Test2;
	public List<string> Places = new List<string>();
	public readonly string Guid = System.Guid.NewGuid().ToString();

	private static readonly HashSet<GameObject> allAtoms = new HashSet<GameObject>();

	void Start()
    {
		if (this.AtomCube != null)
	    {
		    for (int x = 0; x < 4; x++)
		    {
			    this.CreateAtom(x);
		    }
		}
    }

	void OnValidate()
	{
		if (EditorApplication.isPlayingOrWillChangePlaymode)
		{
			return;  
		}

		if (this.AtomCube == null)
		{
			return;
		}

		this.DestroyChildren();

		if (!this.gameObject.activeInHierarchy)
		{
			return;
		}

		GameObject o = this.CreateAtom(Random.Range(-5, 5));
	}

	private void DestroyChildren()
	{
		foreach (Transform t in this.transform)
		{
			EditorApplication.delayCall += () => DestroyImmediate(t.gameObject);
		}
	}

	private GameObject CreateAtom(int x)
	{
		Vector3 localPoint = new Vector3(x, 0, 0);
		Vector3 point = this.transform.TransformPoint(localPoint);

		GameObject atom = Instantiate(this.AtomCube, point, Quaternion.identity, this.transform);

		Renderer rndr = atom.GetComponent<Renderer>();

		int color = Random.Range(1, 4);

		Debug.Log("GameSystem.Instance = ", GameSystem.Instance);
		if (color == 1)
		{
			if (GameSystem.Instance?.Material1 != null)
			{
				rndr.material = GameSystem.Instance.Material1;
			}
		}
		else if (color == 2)
		{
			if (GameSystem.Instance?.Material2 != null)
			{
				rndr.material = GameSystem.Instance.Material2;
			}
		}
		else if (color == 3)
		{
			if (GameSystem.Instance?.Material3 != null)
			{
				rndr.material = GameSystem.Instance.Material3;
			}
		}
		return atom;
	}

    // Update is called once per frame
    void Update()
    {
	    this.transform.Rotate(0.1f, 0f, 0f);
    }
}
