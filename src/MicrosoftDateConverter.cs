using System;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Xero.NetStandard.OAuth2.Json.Converters
{
	public class MicrosoftDateConverter : DateTimeConverterBase
	{
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.Value is DateTime dt)
			{
				if (objectType == typeof(DateTime))
					return dt;
				else if (objectType == typeof(DateTimeOffset))
					return new DateTimeOffset(dt);
			}

			if (reader.Value is DateTimeOffset dto)
			{
				if (objectType == typeof(DateTimeOffset))
					return dto;
				else if (objectType == typeof(DateTime))
					return dto.DateTime;
			}

			return Convert.ToDateTime(reader.Value.ToString());
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var oldDateFormatHandling = writer.DateFormatHandling;
			var oldDateTimeZoneHandling = writer.DateTimeZoneHandling;

			writer.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
			writer.DateTimeZoneHandling = DateTimeZoneHandling.Local;

			writer.WriteValue(value);

			writer.DateFormatHandling = oldDateFormatHandling;
			writer.DateTimeZoneHandling = oldDateTimeZoneHandling;
		}
	}
}
