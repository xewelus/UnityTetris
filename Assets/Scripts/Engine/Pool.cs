using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Engine
{
	public class Pool<T> where T : class
	{
		private readonly Stack<T> objs = new Stack<T>();
		private readonly Func<T> createFunc;

		public Pool(Func<T> createFunc)
		{
			this.createFunc = createFunc;
		}

		public T Get()
		{
			T obj;
			lock (this.objs)
			{
				obj = this.objs.Count == 0 ? null : this.objs.Pop();
			}
			if (obj == null)
			{
				obj = this.createFunc();
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
				this.objs.Push(obj);
			}
		}
	}

	public interface IPoolable
	{
		void PrepareForUse();
	}
}
