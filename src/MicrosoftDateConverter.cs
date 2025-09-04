using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Xero.NetStandard.OAuth2.Json.Converters
{
	public class MicrosoftDateConverter : JsonConverter<DateTime>
	{
		public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var value = reader.GetString();
			if (DateTime.TryParse(value, out var result))
			{
				return result;
			}
			
			// Handle Microsoft JSON date format: /Date(ticks)/
			if (value.StartsWith("/Date(") && value.EndsWith(")/"))
			{
				var ticksString = value.Substring(6, value.Length - 8);
				if (long.TryParse(ticksString, out var ticks))
				{
					return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(ticks);
				}
			}
			
			throw new JsonException($"Unable to convert \"{value}\" to DateTime.");
		}

		public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			var milliseconds = (long)(value.ToUniversalTime() - epoch).TotalMilliseconds;
			writer.WriteStringValue($"/Date({milliseconds})/");
		}
	}

	public class NullableMicrosoftDateConverter : JsonConverter<DateTime?>
	{
		public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.Null)
			{
				return null;
			}

			var value = reader.GetString();
			if (DateTime.TryParse(value, out var result))
			{
				return result;
			}
			
			// Handle Microsoft JSON date format: /Date(ticks)/
			if (value.StartsWith("/Date(") && value.EndsWith(")/"))
			{
				var ticksString = value.Substring(6, value.Length - 8);
				if (long.TryParse(ticksString, out var ticks))
				{
					return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(ticks);
				}
			}
			
			throw new JsonException($"Unable to convert \"{value}\" to DateTime.");
		}

		public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}

			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			var milliseconds = (long)(value.Value.ToUniversalTime() - epoch).TotalMilliseconds;
			writer.WriteStringValue($"/Date({milliseconds})/");
		}
	}
}
