using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using Point = System.Drawing.Point;

public class FigureBehaviour : MonoBehaviour
{
	[DataMember]
	public GameObject AtomCube;

	[DataMember]
	public readonly string Guid = System.Guid.NewGuid().ToString();

	[DataMember]
	public MaterialsScope MaterialsScope;

	[DataMember]
	public FigureAsset FigureAsset;

	[DataMember]
	public Color Color = Color.white;

	[PublicAPI]
	void Start()
	{
    }

	[PublicAPI]
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

		if (this.FigureAsset == null)
		{
			return;
		}

		foreach (Point p in this.FigureAsset.GetCells())
		{
			this.CreateAtom(p);
		}
	}

	private void DestroyChildren()
	{
		foreach (Transform t in this.transform)
		{
			EditorApplication.delayCall += () =>
			                               {
				                               if (t != null)
				                               {
					                               DestroyImmediate(t.gameObject);
				                               }
			                               };
		}
	}

	private void CreateAtom(Point p)
	{
		Vector3 localPoint = new Vector3(p.X, p.Y, 0);
		Vector3 point = this.transform.TransformPoint(localPoint);

		GameObject atom = Instantiate(this.AtomCube, point, Quaternion.identity, this.transform);

		Renderer rndr = atom.GetComponent<Renderer>();

		if (this.MaterialsScope != null)
		{
			if (this.MaterialsScope.Count > 0)
			{
				int color = Random.Range(0, this.MaterialsScope.Count);
				rndr.sharedMaterial = this.MaterialsScope.GetMaterial(color);
			}
		}
		else
		{
			rndr.sharedMaterial = Instantiate(rndr.sharedMaterial);
			rndr.sharedMaterial.color = this.Color;
		}
	}

	[PublicAPI]
    void Update()
    {
	    this.transform.Rotate(0.1f, 0f, 0f);
    }
}
