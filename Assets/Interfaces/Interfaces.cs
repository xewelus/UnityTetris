namespace Assets.Interfaces
{
	public interface IOnValidate
	{
		void OnValidate();
	}

	public interface IOnGUI
	{
		void OnGUI();
	}

	public interface IAwake
	{
		void Awake();
	}

	public interface IStart
	{
		void Start();
	}

	public interface IUpdate
	{
		void Update();
	}

	public static class Usings
	{
		public static void Use()
		{
			object obj = new object();
			((IOnValidate)obj).OnValidate();
			((IOnGUI)obj).OnGUI();
			((IStart)obj).Start();
			((IUpdate)obj).Update();
			((IAwake)obj).Awake();
		}
	}
}
