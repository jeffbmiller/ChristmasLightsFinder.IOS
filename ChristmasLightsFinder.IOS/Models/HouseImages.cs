using System;
using SQLite;

namespace ChristmasLightsFinder.IOS
{
	public class HouseImages
	{
		[PrimaryKey, AutoIncrement]
		public int Id {get;set;}
		public byte[] Thumbnail {get;set;}
		public byte[] Image { get; set; }
		public string ObjectId { get; set; }
	}
}

