using System;
using Foundation;
using Parse;

namespace ChristmasLightsFinder.IOS
{
	public class House
	{
        public House()
        {

        }

        public House(NSDictionary data, string key)
        {
			Address = data["Address"].ToString();
            City = data["City"].ToString();
			Province = data["Province"].ToString();
			ImagePath = data["ImagePath"].ToString();
			Longitude = Double.Parse(data["Longitude"].ToString());
			Latitude = Double.Parse(data["Latitude"].ToString());
			Id = key;
		}

        public string Id { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
		
		public string City{ get; set; }

		
		public string Province{ get; set; }


		public int Likes { get; set; }

		
		public string ImagePath{ get; set; }

		
		public double Longitude{ get; set; }

	
		public double Latitude{ get; set; }

		public string FullAddress {get { return string.Format("{0} {1}", Address,City);}}
	}
}

