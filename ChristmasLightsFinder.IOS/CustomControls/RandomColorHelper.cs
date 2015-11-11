using System;
using UIKit;
using System.Collections.Generic;

namespace ChristmasLightsFinder.IOS
{
	public static class RandomColorHelper
	{
		public static UIColor GetRandomColor()
		{
			var rnd = new Random();
			var colorIndex = rnd.Next(0, 3);

			var colors = new List<UIColor> (){UIColor.Blue, UIColor.Red, UIColor.Green,  UIColor.Purple};

			return colors[colorIndex];
		}
	}
}

