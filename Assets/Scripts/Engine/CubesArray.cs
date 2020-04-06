using System.Collections.Generic;
using System.Drawing;

namespace Assets.Scripts
{
	public class CubesArray
	{
		public CubesArray(int width, int height)
		{

		}

		private enum ItemType
		{
			None,
			Fixed,
			Figure
		}

		private class ItemInfo
		{
			public ItemType Type;
		}
	}
}