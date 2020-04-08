using System;
using UnityEngine;

namespace Assets.Scripts.Engine
{
	public class KeyboardController
	{
		public event Action NeedLeft;
		public event Action NeedRight;

		private float? lastMoveSideTime;
		private Side? lastMoveSide;
		private Side? lastPressed;
		private float moveSideDelay = 0.2f;

		public void Update()
		{
			bool leftPressed = Input.GetKey(KeyCode.LeftArrow);
			bool rightPressed = Input.GetKey(KeyCode.RightArrow);

			Side? side = null;
			Side? pressed = null;

			if (leftPressed && rightPressed)
			{
				if (this.lastPressed == Side.Left)
				{
					side = Side.Right;
				}
				else if (this.lastPressed == Side.Right)
				{
					side = Side.Left;
				}
				else
				{
					side = this.lastMoveSide;
				}
			}
			else if (leftPressed)
			{
				pressed = Side.Left;
				side = Side.Left;
			}
			else if (rightPressed)
			{
				pressed = Side.Right;
				side = Side.Right;
			}

			if (this.lastPressed != pressed)
			{
				this.lastMoveSideTime = null;
				this.lastPressed = pressed;
			}

			if (side != null)
			{
				Action action = side == Side.Left ? this.NeedLeft : this.NeedRight;
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
