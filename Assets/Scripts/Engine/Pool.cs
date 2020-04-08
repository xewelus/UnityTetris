using System.Collections.Generic;

namespace Assets.Scripts.Engine
{
	public class Pool<T> where T : class, new()
	{
		private readonly Stack<T> objs = new Stack<T>();
		public T Get()
		{
			T obj;
			lock (this.objs)
			{
				obj = this.objs.Count == 0 ? null : this.objs.Pop();
			}
			if (obj == null)
			{
				obj = new T();
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
