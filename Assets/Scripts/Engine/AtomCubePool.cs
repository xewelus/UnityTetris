using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Engine
{
	public class AtomCubePool : Pool<AtomCubePool.Item>
	{
		private readonly AtomCube atomCubeSample;
		private readonly Transform parent;
	
		public AtomCubePool(
			AtomCube sample,
			MaterialsScope.Cache materialsCache) : base(() => CreateFunc(sample, materialsCache))
		{
		}

		private static Item CreateFunc(AtomCube sample, MaterialsScope.Cache materialsCache)
		{
			AtomCube cube = Object.Instantiate(sample);

			Renderer renderer = sample.GetComponent<Renderer>();
			PoolInfo poolInfo = new PoolInfo(materialsCache, renderer.sharedMaterial);

			Item item = new Item(cube, poolInfo);
			return item;
		}

		public class PoolInfo
		{
			public readonly MaterialsScope.Cache MaterialsCache;
			public readonly Material MaterialSample;

			public PoolInfo(MaterialsScope.Cache materialsCache, Material materialSample)
			{
				this.MaterialsCache = materialsCache ?? throw new Exception("materialsCache");
				this.MaterialSample = materialSample ?? throw new Exception("materialSample");
			}
		}

		public class Item : IPoolable
		{
			public readonly AtomCube AtomCube;
			private readonly Renderer renderer;
			private readonly PoolInfo poolInfo;

			public Item(AtomCube atomCube, PoolInfo poolInfo)
			{
				this.AtomCube = atomCube;
				this.renderer = atomCube.GetComponent<Renderer>();
				this.poolInfo = poolInfo;
			}

			public void PrepareForUse()
			{
				this.AtomCube.transform.localPosition = Vector3.zero;
				this.SetColor(Color.black);
			}

			public void SetColor(Color color)
			{
				this.renderer.sharedMaterial = this.poolInfo.MaterialsCache.GetOrCreate(color, this.poolInfo.MaterialSample);
			}
		}
	}
}
