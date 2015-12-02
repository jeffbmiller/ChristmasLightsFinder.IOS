using System;
using System.ComponentModel;

namespace ChristmasLightsFinder.IOS
{
	public enum MapFilter
	{
		[Description("Show All")]
		All,
		[Description("Top 10 - Most Likes")]
		Top10,
		[Description("Recently Added")]
		RecentlyAdded
	}
}

