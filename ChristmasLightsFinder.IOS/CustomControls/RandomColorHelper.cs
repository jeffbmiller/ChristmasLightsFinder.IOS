using System;
using UIKit;
using System.Collections.Generic;
using System.Linq;

namespace ChristmasLightsFinder.IOS
{
	public static class RandomColorHelper
	{

		private static readonly Random random = new Random();
		private static readonly object syncLock = new object();
		public static UIColor GetRandomColor()
		{
			var colors = new List<UIColor> (){UIColor.Blue, UIColor.Red, UIColor.Green,  UIColor.Purple, UIColor.Orange};

			lock(syncLock) { // synchronize
				return colors.ElementAt(random.Next(0, 4));
			}
		}
	}
}

