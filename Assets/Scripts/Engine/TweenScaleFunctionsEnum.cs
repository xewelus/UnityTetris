using System;
using DigitalRuby.Tween;

namespace Assets.Scripts.Engine
{
	public enum TweenScaleFunctionsEnum
	{
		Linear,
		QuadraticEaseIn,
		QuadraticEaseOut,
		QuadraticEaseInOut,
		CubicEaseIn,
		CubicEaseOut,
		CubicEaseInOut,
		QuarticEaseIn,
		QuarticEaseOut,
		QuarticEaseInOut,
		QuinticEaseIn,
		QuinticEaseOut,
		QuinticEaseInOut,
		SineEaseIn,
		SineEaseOut,
		SineEaseInOut
	}

	public static class TweenScaleFunctionsEnumExtensions
	{
		public static Func<float, float> ToFunction(this TweenScaleFunctionsEnum value)
		{
			switch (value)
			{
				case TweenScaleFunctionsEnum.Linear: return TweenScaleFunctions.Linear;
				case TweenScaleFunctionsEnum.QuadraticEaseIn: return TweenScaleFunctions.QuadraticEaseIn;
				case TweenScaleFunctionsEnum.QuadraticEaseOut: return TweenScaleFunctions.QuadraticEaseOut;
				case TweenScaleFunctionsEnum.QuadraticEaseInOut: return TweenScaleFunctions.QuadraticEaseInOut;
				case TweenScaleFunctionsEnum.CubicEaseIn: return TweenScaleFunctions.CubicEaseIn;
				case TweenScaleFunctionsEnum.CubicEaseOut: return TweenScaleFunctions.CubicEaseOut;
				case TweenScaleFunctionsEnum.CubicEaseInOut: return TweenScaleFunctions.CubicEaseInOut;
				case TweenScaleFunctionsEnum.QuarticEaseIn: return TweenScaleFunctions.QuarticEaseIn;
				case TweenScaleFunctionsEnum.QuarticEaseOut: return TweenScaleFunctions.QuarticEaseOut;
				case TweenScaleFunctionsEnum.QuarticEaseInOut: return TweenScaleFunctions.QuarticEaseInOut;
				case TweenScaleFunctionsEnum.QuinticEaseIn: return TweenScaleFunctions.QuinticEaseIn;
				case TweenScaleFunctionsEnum.QuinticEaseOut: return TweenScaleFunctions.QuinticEaseOut;
				case TweenScaleFunctionsEnum.QuinticEaseInOut: return TweenScaleFunctions.QuinticEaseInOut;
				case TweenScaleFunctionsEnum.SineEaseIn: return TweenScaleFunctions.SineEaseIn;
				case TweenScaleFunctionsEnum.SineEaseOut: return TweenScaleFunctions.SineEaseOut;
				case TweenScaleFunctionsEnum.SineEaseInOut: return TweenScaleFunctions.SineEaseInOut;
			}
			throw new NotSupportedException(value.ToString());
		}
	}
}
