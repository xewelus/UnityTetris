namespace Assets.Scripts.Engine
{
	public partial class GameLevel
	{
		private class KeyboardEvents
		{
			private readonly GameLevel gameLevel;
			private KeyboardEvents(GameLevel gameLevel)
			{
				this.gameLevel = gameLevel;
			}

			public static void Init(GameLevel gameLevel, KeyboardController keyboard)
			{
				KeyboardEvents events = new KeyboardEvents(gameLevel);
				keyboard.MoveLeft += events.Keyboard_MoveLeft;
				keyboard.MoveRight += events.Keyboard_MoveRight;
				keyboard.MoveDown += events.Keyboard_MoveDown;
				keyboard.RotateLeft += events.Keyboard_RotateLeft;
				keyboard.RotateRight += events.Keyboard_RotateRight;
				keyboard.Pause += events.Keyboard_Pause;
			}

			private void Keyboard_MoveLeft()
			{
				this.gameLevel.figureInfo?.MoveSide(true);
			}

			private void Keyboard_MoveRight()
			{
				this.gameLevel.figureInfo?.MoveSide(false);
			}

			private void Keyboard_MoveDown()
			{
				this.gameLevel.figureInfo?.MoveDown(true);
				this.gameLevel.lastTime = this.gameLevel.timing.time;
			}

			private void Keyboard_RotateLeft()
			{
				this.gameLevel.figureInfo?.Rotate(true);
			}

			private void Keyboard_RotateRight()
			{
				this.gameLevel.figureInfo?.Rotate(false);
			}

			private void Keyboard_Pause()
			{
				this.gameLevel.timing.IsPaused = !this.gameLevel.timing.IsPaused;
			}
		}
	}
}
