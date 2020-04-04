using System.Collections.Generic;
using System.Runtime.Serialization;
using JetBrains.Annotations;
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

	[DataMember]
	public FigureAsset FigureAsset;

	[PublicAPI]
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

	[PublicAPI]
	void OnValidate()
	{
		return;

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
			this.CreateAtom(Random.Range(-5, 5));
		}
	}

	private void DestroyChildren()
	{
		foreach (Transform t in this.transform)
		{
			EditorApplication.delayCall += () =>
			                               {
											   DestroyImmediate(t.gameObject);
			                               };
		}
	}

	private void CreateAtom(int x)
	{
		Vector3 localPoint = new Vector3(x, 0, 0);
		Vector3 point = this.transform.TransformPoint(localPoint);

		GameObject atom = Instantiate(this.AtomCube, point, Quaternion.identity, this.transform);

		Renderer rndr = atom.GetComponent<Renderer>();

		if (this.MaterialsScope.Count > 0)
		{
			int color = Random.Range(0, this.MaterialsScope.Count);
			rndr.sharedMaterial = this.MaterialsScope.GetMaterial(color);
		}
	}

	[PublicAPI]
    void Update()
    {
	    this.transform.Rotate(0.1f, 0f, 0f);
    }
}
