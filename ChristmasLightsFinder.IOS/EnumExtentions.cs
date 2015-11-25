using System;
using System.ComponentModel;

namespace ChristmasLightsFinder.IOS
{
	public static class EnumExtensions
	{
		public static string Description(this Enum value)
		{
			var type = value.GetType();
			var fieldInfo = type.GetField(value.ToString());
			if (fieldInfo != null)
			{
				var attribs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
				if (attribs != null && attribs.Length > 0)
				{
					return attribs[0].Description;
				} 
			}

			return value.ToString();
		}

		public static T FromDescription<T>(this string value)
		{
			if (Enum.IsDefined(typeof(T), value))
			{
				return (T)Enum.Parse(typeof(T), value, true);
			}
			else
			{
				string[] enumNames = Enum.GetNames(typeof(T));
				foreach (string enumName in enumNames)
				{  
					object e = Enum.Parse(typeof(T), enumName);
					if (value == ((Enum)e).Description())
					{
						return (T)e;
					}
				}
			}
			throw new ArgumentException("The value '" + value 
				+ "' does not match a valid enum name or description.");
		}



	}
}

