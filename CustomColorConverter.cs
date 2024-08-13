using System;
using System.Globalization;
using System.Text.RegularExpressions;
using JsonType;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RaiRai.ColorConverter
{
    public class CustomColorConverter : JsonConverter
    {
        /// <summary>
        /// Ensure that only TaxonomyColor enums are converted
        /// </summary>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TaxonomyColor);
        }

        /// <summary>
        /// Regex for determining if a string is a valid hex color code. Accepts both short-hand (#F00) and full-length (#FF00000) color codes.
        /// </summary>
        private static readonly Regex ColorCodeRegex = new Regex(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{8}|[A-Fa-f0-9]{3})$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        /// <summary>
        /// Simple writer method, purely to satisfy the implementation requirement for a <see cref="JsonConverter"/>.
        /// Simply reroutes to a <see cref="StringEnumConverter"/>.
        /// </summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            new StringEnumConverter().WriteJson(writer, value, serializer);
        }

        /// <summary>
        /// The most important method of the converter - custom logic for parsing <see cref="TaxonomyColor"/> values that can accept standard enum values,
        /// integers and custom color code strings (ex: "#FF00AA"). Custom color codes get converted into <see cref="int"/> representations of hex codes.
        /// </summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Everything is wrapped in a try/catch block for more controlled and informative exception messages
            try
            {
                // Depending on the TokenType, there will be different value conversion being done
                switch (reader.TokenType)
                {
                    case JsonToken.String:
                        {
                            // Read the string value
                            var valueString = reader.Value?.ToString();

                            if (string.IsNullOrWhiteSpace(valueString))
                            {
                                throw new JsonSerializationException("Invalid string value encountered.");
                            }

                            // We try parsing the string value normally
                            if (Enum.TryParse<TaxonomyColor>(valueString, ignoreCase: true, out var enumValue))
                            {
                                return enumValue;
                            }

                            // If parsing normally fails, then we check if it's a valid color code
                            var regexMatch = ColorCodeRegex.Match(valueString);

                            if (!regexMatch.Success)
                            {
                                throw new JsonSerializationException("Invalid color code string encountered.");
                            }

                            // The Regex string has a capture group defined, so we grab the first group's first capture
                            var colorCodeString = regexMatch.Groups[1].Captures[0].Value;

                            // If the color code only has three letter, it means it's a short-hand representation and needs to be expanded before continuing
                            if (colorCodeString.Length == 3)
                            {
                                // A super janky way of duplicating each symbol, but it works and that's good enough
                                colorCodeString = $"{colorCodeString[0]}{colorCodeString[0]}{colorCodeString[1]}{colorCodeString[1]}{colorCodeString[2]}{colorCodeString[2]}";
                            }

                            var colorCodeAsInt = int.Parse(colorCodeString, NumberStyles.HexNumber);
                            // Avoiding overlap with actual enum values
                            colorCodeAsInt += Enum.GetValues(typeof(TaxonomyColor)).Length;

                            // Once we have a proper 6-symbol color code, we simply parse that into an integer value and cast it to our enum
                            return (TaxonomyColor)colorCodeAsInt;
                        }
                    case JsonToken.Integer:
                        {
                            // Integer values are converted as they would normally - no special logic here
                            var intValue = reader.ReadAsInt32();
                            return intValue.HasValue ? (TaxonomyColor)intValue.Value : TaxonomyColor.@default;
                        }
                    case JsonToken.Null:
                        throw new JsonSerializationException("Null value encountered.");
                    default:
                        throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing TaxonomyColor enum.");
                }
            }
            catch (Exception ex)
            {
                throw new JsonSerializationException($"Failed to read JSON value for TaxonomyColor. TokenType -> {reader.TokenType}, Value -> {PrintValueForLogging(reader.Value)}", ex);
            }
        }

        /// <summary>
        /// A simple method for properly printing values for logging purposes
        /// </summary>
        private static string PrintValueForLogging(object value)
        {
            if (value == null)
            {
                return "{null}";
            }

            return (value is string s) ? @"""" + s + @"""" : value.ToString();
        }
    }
}