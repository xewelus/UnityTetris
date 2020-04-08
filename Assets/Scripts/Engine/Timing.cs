using UnityEngine;

namespace Assets.Scripts.Engine
{
	public class Timing
	{
		public bool IsPaused;
		public float time { get; private set; }
		private float prevTime;
		public float deltaTime { get; private set; }

		public void Update()
		{
			if (this.IsPaused) return;

			this.prevTime = this.time;
			this.time += Time.deltaTime;
			this.deltaTime = this.time - this.prevTime;
		}
	}
}
