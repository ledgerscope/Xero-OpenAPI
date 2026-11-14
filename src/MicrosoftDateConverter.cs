using System;
using System.Globalization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Xero.NetStandard.OAuth2.Json.Converters
{
	/// <summary>
	/// Reads and writes Microsoft-style /Date(...)/ values. Everything is kept in UTC:
	/// values read are always returned with <see cref="DateTimeKind.Utc"/>, and values written are always sent as UTC.
	/// </summary>
	public class MicrosoftDateConverter : DateTimeConverterBase
	{
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var targetType = Nullable.GetUnderlyingType(objectType) ?? objectType;

			if (reader.TokenType == JsonToken.Null)
			{
				if (targetType != objectType)
					return null;

				throw new JsonSerializationException($"Cannot convert null value to {objectType}.");
			}

			DateTime utc;
			if (reader.Value is DateTime dt)
				utc = ToUtc(dt);
			else if (reader.Value is DateTimeOffset dto)
				utc = dto.UtcDateTime;
			else
				utc = DateTime.Parse(reader.Value.ToString(), CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);

			if (targetType == typeof(DateTimeOffset))
				return new DateTimeOffset(utc);

			return utc;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var oldDateFormatHandling = writer.DateFormatHandling;
			var oldDateTimeZoneHandling = writer.DateTimeZoneHandling;

			writer.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
			writer.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

			// Written as a zero-offset DateTimeOffset so the output carries an explicit UTC offset,
			// e.g. /Date(1630022400000+0000)/ (Json.NET omits the offset for a UTC DateTime)
			if (value is DateTime dt)
				writer.WriteValue(new DateTimeOffset(ToUtc(dt), TimeSpan.Zero));
			else if (value is DateTimeOffset dto)
				writer.WriteValue(dto.ToUniversalTime());
			else
				writer.WriteValue(value);

			writer.DateFormatHandling = oldDateFormatHandling;
			writer.DateTimeZoneHandling = oldDateTimeZoneHandling;
		}

		/// <summary>
		/// Renders a date the way Xero's *String counterpart properties do (e.g. 2017-11-30T00:00:00), in UTC.
		/// </summary>
		public static string ToDateString(DateTime? value)
		{
			if (value == null || value.Value == default(DateTime))
				return null;

			return ToUtc(value.Value).ToString("yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Normalises a value to UTC. Local values are converted; Unspecified values are taken to already be UTC.
		/// </summary>
		public static DateTime ToUtc(DateTime value)
		{
			switch (value.Kind)
			{
				case DateTimeKind.Local:
					return value.ToUniversalTime();
				case DateTimeKind.Unspecified:
					return DateTime.SpecifyKind(value, DateTimeKind.Utc);
				default:
					return value;
			}
		}
	}
}
