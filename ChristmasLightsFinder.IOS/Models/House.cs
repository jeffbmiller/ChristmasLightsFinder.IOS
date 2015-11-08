using System;
using Parse;

namespace ChristmasLightsFinder.IOS
{
	[ParseClassName("House")]
	public class House : ParseObject
	{
		[ParseFieldName("address")]
		public string Address
		{
			get { return GetProperty<string>(); }
			set { SetProperty<string>(value); }
		}

		[ParseFieldName("city")]
		public string City
		{
			get { return GetProperty<string>(); }
			set { SetProperty<string>(value); }
		}

		[ParseFieldName("province")]
		public string Province
		{
			get { return GetProperty<string>(); }
			set { SetProperty<string>(value); }
		}

		[ParseFieldName("country")]
		public string Country
		{
			get { return GetProperty<string>(); }
			set { SetProperty<string>(value); }
		}

		public string FullAddress {get { return string.Format("{0} {1} {2}", Address,City, Country);}}
	}
}

