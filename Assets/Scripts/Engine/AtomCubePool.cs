using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Engine
{
	public class AtomCubePool : Pool<AtomCubePool.Item>
	{
		private readonly AtomCube atomCubeSample;
		private readonly Transform parent;
		private readonly MaterialsScope.Cache materialsCache;
		private readonly Material materialSample;

		public AtomCubePool(AtomCube sample, MaterialsScope.Cache materialsCache) 
			: base(pool => CreateFunc((AtomCubePool)pool, sample))
		{
			this.materialsCache = materialsCache ?? throw new Exception("materialsCache");

			Renderer renderer = sample.GetComponent<Renderer>();
			this.materialSample = renderer.sharedMaterial;
		}

		private static Item CreateFunc(AtomCubePool pool, AtomCube sample)
		{
			AtomCube cube = Object.Instantiate(sample);

			Item item = new Item(pool, cube);
			return item;
		}

		public class Item : IPoolable
		{
			public readonly AtomCube AtomCube;
			private readonly Renderer renderer;
			private readonly AtomCubePool pool;

			public Item(AtomCubePool pool, AtomCube atomCube)
			{
				this.pool = pool;
				this.AtomCube = atomCube;
				this.renderer = atomCube.GetComponent<Renderer>();
			}

			public void PrepareForUse()
			{
				this.AtomCube.transform.localPosition = Vector3.zero;
				this.SetColor(Color.black);
			}

			public void SetColor(Color color)
			{
				this.renderer.sharedMaterial = this.pool.materialsCache.GetOrCreate(color, this.pool.materialSample);
			}

			public void Release()
			{
				this.AtomCube.transform.localPosition = new Vector3(1000, 1000, 0);
				this.AtomCube.transform.parent = null;
				this.pool.Release(this);
			}
		}
	}
}
