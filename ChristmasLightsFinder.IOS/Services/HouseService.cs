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
			else if (mapFilter == MapFilter.Top5) {
				var houses = await  query.FindAsync ();
				return houses.GroupBy (x => x.Likes).OrderByDescending(x=>x.Key).Take (5).SelectMany (x => x);
			}


			var results = await  query.FindAsync ();

			return results;
		}
	}
}

