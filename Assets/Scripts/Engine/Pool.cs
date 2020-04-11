using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Engine
{
	public class Pool<T> where T : class
	{
		private readonly HashSet<T> objs = new HashSet<T>();
		private readonly Func<Pool<T>, T> createFunc;

		public Pool(Func<Pool<T>, T> createFunc)
		{
			this.createFunc = createFunc;
		}

		public T Get()
		{
			T obj;
			lock (this.objs)
			{
				if (this.objs.Count == 0)
				{
					obj = null;
				}
				else
				{
					obj = this.objs.First();
					this.objs.Remove(obj);
				}
			}
			if (obj == null)
			{
				obj = this.createFunc(this);
			}
			if (obj is IPoolable)
			{
				IPoolable poolable = (IPoolable)obj;
				poolable.PrepareForUse();
			}
			return obj;
		}

		public void Release(T obj)
		{
			lock (this.objs)
			{
				if (!this.objs.Contains(obj))
				{
					this.objs.Add(obj);
				}
			}
		}
	}

	public interface IPoolable
	{
		void PrepareForUse();
	}
}
