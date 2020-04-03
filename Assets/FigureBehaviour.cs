using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FigureBehaviour : MonoBehaviour
{
	public GameObject AtomCube;
	public int Test2;
	public List<string> Places = new List<string>();
	public readonly string Guid = System.Guid.NewGuid().ToString();

	private static readonly HashSet<FigureBehaviour> notCleared = new HashSet<FigureBehaviour>();
	private static readonly HashSet<GameObject> allAtoms = new HashSet<GameObject>();

	private int test;

	[SerializeField]
	public int Test
	{
		get
		{
			return this.test;
		}
		set
		{
			this.test = value;

			//var v = new Vector3(Random.Range(-5, 5), 0, 0);
			//GameObject atom = Instantiate(this.AtomCube, this.transform.TransformPoint(new Vector3(Random.Range(-5, 5), 0, 0)), Quaternion.identity, this.transform);
			//Renderer rndr = atom.GetComponent<Renderer>();

			//int color = Random.Range(1, 4);
			//if (color == 1)
			//{
			//	rndr.material.color = Color.blue;
			//}
			//else if (color == 2)
			//{
			//	rndr.material.color = Color.red;
			//}
			//else if (color == 3)
			//{
			//	rndr.material.color = Color.green;
			//}
		}
	}

    // Start is called before the first frame update
	void Start()
    {
	    UnityEditor.EditorUtility.DisplayDialog("FigureBehaviour Start", "Hello", "OK");
		if (this.AtomCube != null)
	    {
		    for (int x = 0; x < 4; x++)
		    {
			    this.CreateAtom(x);
		    }
		}
    }

	private readonly List<GameObject> objs = new List<GameObject>();
	void OnValidate()
	{
		lock (notCleared)
		{
			notCleared.Add(this);
		}

		this.ClearObjs();
		GameObject o = this.CreateAtom(Random.Range(-5, 5));
		this.objs.Add(o);

		lock (notCleared)
		{
			UnityEditor.EditorUtility.DisplayDialog("Test", notCleared.Count.ToString() + " / " + allAtoms.Count, "OK");
			foreach (FigureBehaviour b in notCleared)
			{
				if (!b.gameObject.scene.isLoaded)
				{
					b.ClearObjs();
				}
			}
		}
	}

	private void ClearObjs()
	{
		List<GameObject> cloned = new List<GameObject>(this.objs);

		//foreach (GameObject obj in cloned)
		//{
		//	EditorApplication.delayCall += () =>
		//	                               {
		//		                               DestroyImmediate(obj);
		//		                               allAtoms.Remove(obj);
		//	                               };
		//}


		EditorApplication.delayCall += () =>
												   {
													   foreach (GameObject obj in cloned)
													   {
														   DestroyImmediate(obj);
														   allAtoms.Remove(obj);
													   }

													   lock (notCleared)
													   {
														   notCleared.Remove(this);
													   }
												   };

		this.objs.Clear();
	}

	void DestroyFromEditor(GameObject obj)
	{
	    UnityEditor.EditorUtility.DisplayDialog("DestroyFromEditor", "Test", "OK");
		this.ClearObjs();
		EditorApplication.delayCall += () => DestroyImmediate(obj);
	}

	private GameObject CreateAtom(int x)
	{
		Vector3 localPoint = new Vector3(x, 0, 0);
		Vector3 point = this.transform.TransformPoint(localPoint);
		GameObject atom = Instantiate(this.AtomCube, point, Quaternion.identity, this.transform);
		allAtoms.Add(atom);
		Renderer rndr = atom.GetComponent<Renderer>();

		int color = Random.Range(1, 4);

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

	void OnDestroy()
	{
	    UnityEditor.EditorUtility.DisplayDialog("Test", "OnDestroy", "OK");
	}

	private int counter = 6;
	// ReSharper disable once UnusedMember.Local
	void OnGui()
	{
		if (this.counter-- <= 0) return;
		this.CreateAtom(Random.Range(-5, 5));
	}
}
