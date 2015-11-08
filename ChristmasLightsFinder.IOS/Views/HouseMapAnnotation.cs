﻿using System;
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

		public HouseMapAnnotation (CLLocationCoordinate2D coordinate, string title, House house) {
			this.coordinate = coordinate;
			this.title = title;
			this.House = house;
		}

		public override CLLocationCoordinate2D Coordinate {
			get {
				return coordinate;
			}
		}

	}
}

