using System;
using UnityEngine;

namespace Assets.Scripts.Engine
{
	public class KeyboardController
	{
		public event Action NeedLeft;
		public event Action NeedRight;

		private float? lastMoveSideTime;
		private bool? lastMoveSide;
		private bool? lastPressed;
		private float moveSideDelay = 0.2f;

		public void Update()
		{
			bool leftPressed = Input.GetKey(KeyCode.LeftArrow);
			bool rightPressed = Input.GetKey(KeyCode.RightArrow);

			bool? side = null;
			bool? pressed = null;

			if (leftPressed && rightPressed)
			{
				side = !this.lastPressed ?? this.lastMoveSide;
			}
			else if (leftPressed || rightPressed)
			{
				pressed = leftPressed;
				side = leftPressed;
			}

			if (this.lastPressed != pressed)
			{
				this.lastMoveSideTime = null;
				this.lastPressed = pressed;
			}

			if (side != null)
			{
				Action action = side.Value ? this.NeedLeft : this.NeedRight;
				if (action != null)
				{
					float now = Time.time;
					if (this.lastMoveSideTime == null || this.lastMoveSideTime.Value + this.moveSideDelay < now)
					{
						this.lastMoveSideTime = now;
						this.lastMoveSide = side;
						action.Invoke();
					}
				}
			}
		}

		private enum Side
		{
			Left,
			Right
		}
	}
}
