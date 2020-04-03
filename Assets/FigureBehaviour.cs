using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureBehaviour : MonoBehaviour
{
	public GameObject AtomCube;

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
			    Vector3 localPoint = new Vector3(x, 0, 0);
				Vector3 point = this.transform.TransformPoint(localPoint);
				GameObject atom = Instantiate(this.AtomCube, point, Quaternion.identity, this.transform);
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

    // Update is called once per frame
    void Update()
    {
	    this.transform.Rotate(0.1f, 0f, 0f);
    }
}
