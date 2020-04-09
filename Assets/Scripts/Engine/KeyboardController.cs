using System;
using UnityEngine;

namespace Assets.Scripts.Engine
{
	public class KeyboardController
	{
		private readonly GameParameters.KeyboardParameters parameters;
		private readonly Timing timing;

		public event Action MoveLeft;
		public event Action MoveRight;
		public event Action MoveDown;
		public event Action RotateLeft;
		public event Action RotateRight;
		public event Action Pause;

		private float? lastMoveSideTime;
		private bool? lastMoveSide;
		private bool? lastPressed;

		private float? lastMoveDownTime;

		public KeyboardController(GameParameters.KeyboardParameters parameters, Timing timing)
		{
			this.parameters = parameters;
			this.timing = timing;
		}

		public void Update()
		{
			this.CheckMoveSide();
			this.CheckMoveDown();

			if (Input.GetKey(this.parameters.RotateLeft))
			{
				this.RotateLeft?.Invoke();
			}
			if (Input.GetKey(this.parameters.RotateRight))
			{
				this.RotateRight?.Invoke();
			}
			if (Input.GetKey(this.parameters.Pause))
			{
				this.Pause?.Invoke();
			}
		}

		private void CheckMoveSide()
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
				Action action = side.Value ? this.MoveLeft : this.MoveRight;
				if (action != null)
				{
					float now = this.timing.time;
					if (this.lastMoveSideTime == null || this.lastMoveSideTime.Value + this.parameters.MoveSideDelay < now)
					{
						this.lastMoveSideTime = now;
						this.lastMoveSide = side;
						action.Invoke();
					}
				}
			}
		}

		private void CheckMoveDown()
		{
			if (this.MoveDown != null)
			{
				if (!Input.GetKey(this.parameters.Down)) return;

				float now = this.timing.time;
				if (this.lastMoveDownTime == null || this.lastMoveDownTime.Value + this.parameters.MoveDownDelay < now)
				{
					this.lastMoveDownTime = now;
					this.MoveDown.Invoke();
				}
			}
		}
	}
}
