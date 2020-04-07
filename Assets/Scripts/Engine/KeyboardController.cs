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
		private Side? lastMoveSide;
		private Pressed? lastPressed;
		private float moveSideDelay = 0.2f;

		public void Update()
		{
			bool leftPressed = Input.GetKey(KeyCode.LeftArrow);
			bool rightPressed = Input.GetKey(KeyCode.RightArrow);

			Side? side;
			bool instant;
			Pressed? pressed;

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
			else
			{
				pressed = null;
			}

			if (this.lastPressed == null)
			{
				instant = true;
				if (leftPressed ^ rightPressed)
				{
					// что-то одно стало нажато
					side = leftPressed ? Side.Left : Side.Right;
				}
				else
				{
					side = null;
				}
			}
			else
			{
				if (leftPressed ^ rightPressed)
				{
					// что-то одно до сих пор нажато
					side = leftPressed ? Side.Left : Side.Right;
					instant = this.lastPressed != pressed;
				}
				else if (leftPressed)
				{
					// обе нажаты
					if (this.lastPressed == Pressed.Left)
					{
						side = Side.Right;
						instant = true;
					}
					else if (this.lastPressed == Pressed.Right)
					{
						side = Side.Left;
						instant = true;
					}
					else
					{
						side = this.lastMoveSide;
						instant = false;
					}
				}
				else
				{
					// ничего не нажато
					side = null;
					instant = true;
				}
			}

			this.lastPressed = pressed;

			if (instant)
			{
				this.lastMoveSideTime = null;
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
