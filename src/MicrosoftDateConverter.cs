using System;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Xero.NetStandard.OAuth2.Json.Converters
{
	public class MicrosoftDateConverter : DateTimeConverterBase
	{
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return Convert.ToDateTime(reader.Value.ToString());
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var dateFormatHandling = writer.DateFormatHandling;

			writer.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;

			writer.WriteValue(value);

			writer.DateFormatHandling = dateFormatHandling;
		}
	}
}
