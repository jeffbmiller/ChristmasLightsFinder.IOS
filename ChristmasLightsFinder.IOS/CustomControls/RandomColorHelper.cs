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
			var colorIndex = rnd.Next(0, 2);

			var colors = new List<UIColor> (){UIColor.Blue, UIColor.Red, UIColor.Purple};

			return colors[colorIndex];
		}
	}
}

