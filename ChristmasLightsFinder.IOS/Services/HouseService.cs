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
			var results = await  query.FindAsync ();

			if (mapFilter == MapFilter.RecentlyAdded) {
				return results.Where(x=>x.CreatedAt >= DateTime.Today.AddDays(-3));
			}
			else if (mapFilter == MapFilter.Top5) {
				return results.OrderByDescending(x=>x.Likes).Take(5);
			}
			else{
				return results;
			}
		}
	}
}

