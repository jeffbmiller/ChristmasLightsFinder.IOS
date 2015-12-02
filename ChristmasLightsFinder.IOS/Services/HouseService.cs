using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Parse;
using System.Linq;

namespace ChristmasLightsFinder.IOS
{
	public class HouseService
	{
		public async Task<IEnumerable<House>> GetHousesAsync(MapFilter mapFilter)
		{
			var query = new ParseQuery<House>();


			if (mapFilter == MapFilter.RecentlyAdded) {
				query = query.WhereGreaterThanOrEqualTo("createdAt",DateTime.Today.AddDays(-3));
			}
			else if (mapFilter == MapFilter.Top10) {
				query = query.OrderByDescending("likes").Limit(10);
			}


			var results = await  query.FindAsync ();

			return results;
		}
	}
}

