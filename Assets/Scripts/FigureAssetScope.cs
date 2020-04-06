using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Assets.Scripts
{
	[Serializable]
	public class FigureAssetScope : MonoBehaviour
	{
		[DataMember]
		public List<FigureAsset> FigureAssets = new List<FigureAsset>();

		public FigureAsset GetRandom()
		{
			if (this.FigureAssets.Count == 0)
			{
				throw new Exception("this.FigureAssets.Count == 0");
			}

			int i = UnityEngine.Random.Range(0, this.FigureAssets.Count);
			return this.FigureAssets[i];
		}
	}
}