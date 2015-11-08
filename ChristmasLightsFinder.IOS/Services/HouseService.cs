using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Parse;

namespace ChristmasLightsFinder.IOS
{
	public class HouseService
	{
		public async Task<IEnumerable<House>> GetHousesAsync()
		{
			var query = new ParseQuery<House>();
			var results = await  query.FindAsync ();

			return results;
		}
	}
}

