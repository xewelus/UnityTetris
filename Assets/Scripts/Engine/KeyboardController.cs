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
		private Pressed? lastPressed;
		private float moveSideDelay = 0.2f;

		public void Update()
		{
			bool leftPressed = Input.GetKey(KeyCode.LeftArrow);
			bool rightPressed = Input.GetKey(KeyCode.RightArrow);

			Side? side = null;
			Pressed? pressed = null;

			if (leftPressed && rightPressed)
			{
				pressed = Pressed.Both;
			}
			else if (leftPressed)
			{
				pressed = Pressed.Left;
			}
			else if (rightPressed)
			{
				pressed = Pressed.Right;
			}

			if (leftPressed ^ rightPressed)
			{
				side = leftPressed ? Side.Left : Side.Right;
			}
			else if (pressed == Pressed.Both)
			{
				// обе нажаты
				if (this.lastPressed == Pressed.Left)
				{
					side = Side.Right;
				}
				else if (this.lastPressed == Pressed.Right)
				{
					side = Side.Left;
				}
				else
				{
					side = this.lastMoveSide;
				}
			}

			if (this.lastPressed != pressed)
			{
				this.lastMoveSideTime = null;
			}

			this.lastPressed = pressed;

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

		private enum Pressed
		{
			Left,
			Right,
			Both
		}

		private enum Side
		{
			Left,
			Right
		}
	}
}
