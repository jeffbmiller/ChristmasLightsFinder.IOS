using System;
using System.ComponentModel;

namespace ChristmasLightsFinder.IOS
{
	public enum MapFilter
	{
		[Description("Show All")]
		All,
		[Description("Top 5 - Most Likes")]
		Top5,
		[Description("Recently Added")]
		RecentlyAdded
	}
}

