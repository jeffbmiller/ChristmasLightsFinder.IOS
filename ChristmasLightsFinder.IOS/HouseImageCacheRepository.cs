using System;
using System.Threading.Tasks;
using SQLite;
using System.Linq;

namespace ChristmasLightsFinder.IOS
{
	public class HouseImageCacheRepository
	{
		private SQLiteAsyncConnection db;

		public HouseImageCacheRepository ()
		{
			string folder = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			db = new SQLiteAsyncConnection (System.IO.Path.Combine (folder, "houseImages.db"));
//			db.DropTableAsync<HouseImages> ().ContinueWith (t => {
//				Console.WriteLine ("Table Dropped!");
//			});
			db.CreateTableAsync<HouseImages>().ContinueWith (t => {
				Console.WriteLine ("Table created!");
			});

		}

		public async Task<HouseImages> GetHouseImagesFor(string objectId)
		{
			var query = db.Table<HouseImages>().Where (x => x.ObjectId == objectId);

			var result = await query.FirstOrDefaultAsync ();

			return result;
		}

		public async Task<bool> SaveHouseImages(HouseImages houseImages)
		{
			var id = await db.InsertAsync (houseImages);
			return id > 0;
		}

		public async Task<bool> UpdateHouseImages(HouseImages houseImages)
		{
			var id = await db.UpdateAsync (houseImages);
			return id > 0;
		}


	}
}

