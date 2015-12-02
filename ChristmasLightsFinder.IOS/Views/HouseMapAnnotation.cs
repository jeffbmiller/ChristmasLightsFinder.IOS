using System;
using MapKit;
using CoreLocation;

namespace ChristmasLightsFinder.IOS
{
	public class HouseMapAnnotation : MKAnnotation
	{
		private  CLLocationCoordinate2D coordinate;
		string title, subtitle;
		public override string Title { get{ return title; }}
		public override string Subtitle { get{ return subtitle; }}
		public House House { get; set; }
		public string Rank { get; set; }

		public HouseMapAnnotation (CLLocationCoordinate2D coordinate, string title, House house, string rank) {
			this.coordinate = coordinate;
			this.title = title;
			this.House = house;
			this.subtitle = string.Format ("({0}) Likes",house.Likes);
			this.Rank = rank;
		}

		public void RefreshAnnotationView()
		{
			DidChangeValue ("subtitle");
		}

		public override CLLocationCoordinate2D Coordinate {
			get {
				return coordinate;
			}
		}

	}
}

