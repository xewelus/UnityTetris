using System;
using UnityEngine;

namespace Assets.Scripts.Engine
{
	public class KeyboardController
	{
		private readonly GameParameters.KeyboardParameters parameters;

		public event Action NeedLeft;
		public event Action NeedRight;

		private float? lastMoveSideTime;
		private bool? lastMoveSide;
		private bool? lastPressed;

		public KeyboardController(GameParameters.KeyboardParameters parameters)
		{
			this.parameters = parameters;
		}

		public void Update()
		{
			bool leftPressed = Input.GetKey(this.parameters.Left);
			bool rightPressed = Input.GetKey(this.parameters.Right);

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
					if (this.lastMoveSideTime == null || this.lastMoveSideTime.Value + this.parameters.MoveSideDelay < now)
					{
						this.lastMoveSideTime = now;
						this.lastMoveSide = side;
						action.Invoke();
					}
				}
			}
		}
	}
}
