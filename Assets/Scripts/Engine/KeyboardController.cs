using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Engine
{
	public class KeyboardController
	{
		public event Action NeedLeft;
		public event Action NeedRight;
		public event Action NeedDown;

		private float? lastMoveSideTime;
		private float moveSideDelay = 0.2f;

		public void Update()
		{
			bool leftPressed = Input.GetKey(KeyCode.LeftArrow);
			bool rightPressed = Input.GetKey(KeyCode.RightArrow);
			if (leftPressed ^ rightPressed)
			{
				Action action = leftPressed ? this.NeedLeft : this.NeedRight;
				if (action != null)
				{
					float now = Time.time;
					if (this.lastMoveSideTime == null || this.lastMoveSideTime.Value + this.moveSideDelay < now)
					{
						this.lastMoveSideTime = now;
						action.Invoke();
					}
				}
			}
		}
	}
}
