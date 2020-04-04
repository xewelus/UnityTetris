using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class FigureBehaviour : MonoBehaviour
{
	[DataMember]
	public GameObject AtomCube;

	public List<string> Places = new List<string>();

	[DataMember]
	public readonly string Guid = System.Guid.NewGuid().ToString();

	[DataMember]
	public bool EnableGeneration;

	[DataMember]
	public MaterialsScope MaterialsScope;

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

		if (!this.EnableGeneration)
		{
			return;
		}

		if (!this.gameObject.activeInHierarchy)
		{
			return;
		}

		if (this.EnableGeneration)
		{
			GameObject o = this.CreateAtom(Random.Range(-5, 5));
		}
	}

	private void DestroyChildren()
	{
		foreach (Transform t in this.transform)
		{
			EditorApplication.delayCall += () =>
			                               {
				                               Debug.Log("DestroyImmediate = " + t.gameObject); 
											   DestroyImmediate(t.gameObject);
			                               };
		}
	}

	private GameObject CreateAtom(int x)
	{
		Vector3 localPoint = new Vector3(x, 0, 0);
		Vector3 point = this.transform.TransformPoint(localPoint);

		GameObject atom = Instantiate(this.AtomCube, point, Quaternion.identity, this.transform);

		Renderer rndr = atom.GetComponent<Renderer>();

		int color = Random.Range(1, 4);

		Material material = null;
		if (color == 1)
		{
			if (this.MaterialsScope?.Material1 != null)
			{
				material = this.MaterialsScope.Material1;
			}
		}
		else if (color == 2)
		{
			if (this.MaterialsScope?.Material2 != null)
			{
				material = this.MaterialsScope.Material2;
			}
		}
		else if (color == 3)
		{
			if (this.MaterialsScope?.Material3 != null)
			{
				material = this.MaterialsScope.Material3;
			}
		}

		if (material != null)
		{
			rndr.sharedMaterial = material; 
			//rndr.sharedMaterial = Instantiate(material);
		}

		return atom;
	}

    // Update is called once per frame
    void Update()
    {
	    this.transform.Rotate(0.1f, 0f, 0f);
    }
}
