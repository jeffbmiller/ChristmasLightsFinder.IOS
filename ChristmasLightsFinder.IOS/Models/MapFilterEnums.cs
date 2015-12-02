using System;
using System.ComponentModel;

namespace ChristmasLightsFinder.IOS
{
	public enum MapFilter
	{
		[Description("Show All")]
		All,
		[Description("Top 3 - Most Likes")]
		Top3,
		[Description("Recently Added")]
		RecentlyAdded
	}
}

