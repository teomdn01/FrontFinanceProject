using Org.Front.Core.Contracts.Models.Deserialization;
using System;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Org.Front.Core
{
    public static class StringExtensions
    {
        private static readonly JsonSerializerOptions CamelCaseJsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private static readonly JsonSerializerOptions DefaultJsonOptions = new JsonSerializerOptions()
        {
        };

        private static readonly JsonSerializerOptions CaseInsensitiveJsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private static readonly JsonSerializerOptions SnakeCaseJsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance,
            AllowTrailingCommas = true
        };

        public static bool IsValidRegExPattern(this string pattern)
        {
            if (!string.IsNullOrWhiteSpace(pattern))
            {
                try
                {
                    Regex.Match("", pattern);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;

        }

        public static Guid ToGuid(this string value) =>
            Guid.TryParse(value, out Guid result) ? result : Guid.Empty;


        public static string Left(this string str, int length)
        {
            return string.IsNullOrEmpty(str)
                ? str
                : str.Substring(0, Math.Min(str.Length, length));
        }

        public static string MakeHtmlParagraph(this string str)
        {
            return string.IsNullOrEmpty(str)
                ? str
                : string.Join("", str.Split("\n").Select(s => "<p>" + s + "</p>"));
        }

        public static string ToJson<T>(this T value, bool isCamelCase = true)
        {
            return value == null
                ? null
                : JsonSerializer.Serialize(value, isCamelCase ? CamelCaseJsonOptions : DefaultJsonOptions);
        }

        public static string ToJson<T>(this T value, JsonSerializerBehaviour behaviour)
        {
            var options = GetJsonSerializerOptions(behaviour);

            return value == null
                ? null
                : JsonSerializer.Serialize(value, options);
        }

        public static string ToJson<T>(this T value, JsonSerializerOptions serializerOptions) =>
            value == null ? null : JsonSerializer.Serialize(value, serializerOptions);

        public static T FromJson<T>(this string value, JsonSerializerBehaviour behaviour = JsonSerializerBehaviour.CamelCase)
        {
            var options = GetJsonSerializerOptions(behaviour);

            return string.IsNullOrEmpty(value)
                ? default
                : JsonSerializer.Deserialize<T>(value, options);
        }

        public static T TryFromJson<T>(this string value, JsonSerializerBehaviour behaviour = JsonSerializerBehaviour.CamelCase)
        {
            try
            {
                return value.FromJson<T>(behaviour);
            }
            catch
            {
                return default;
            }
        }

        public static string ToXmlUtf8<T>(this T value)
        {
            var serializer = new XmlSerializer(typeof(T));

            using var stringWriter = new Utf8StringWriter();
            serializer.Serialize(stringWriter, value);
            return stringWriter.ToString();
        }

        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }

        public static string CalculateHash(this string value, HashAlgorithmType hashFynctionType = HashAlgorithmType.None)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);

            var hashFunctionName = hashFynctionType switch
            {
                HashAlgorithmType.Md5 => HashAlgorithmName.MD5,
                HashAlgorithmType.Sha1 => HashAlgorithmName.SHA1,
                HashAlgorithmType.Sha256 => HashAlgorithmName.SHA256,
                HashAlgorithmType.Sha384 => HashAlgorithmName.SHA384,
                HashAlgorithmType.Sha512 => HashAlgorithmName.SHA512,

                HashAlgorithmType.None => HashAlgorithmName.SHA1,
                _ => HashAlgorithmName.SHA1,
            };


            using var mySHA = HashAlgorithm.Create(hashFunctionName.ToString());
            byte[] hash = mySHA.ComputeHash(bytes);
            var hashString = string.Concat(hash.Select(b => string.Format("{0:x2}", b)));
            return hashString;
        }

        public static string FirstCharToUpper(this string input)
        {
            return input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input.First().ToString().ToUpper() + input.Substring(1)
            };
        }

        private static JsonSerializerOptions GetJsonSerializerOptions(JsonSerializerBehaviour behaviour)
        {
            return behaviour switch
            {
                JsonSerializerBehaviour.Default => DefaultJsonOptions,
                JsonSerializerBehaviour.CamelCase => CamelCaseJsonOptions,
                JsonSerializerBehaviour.CaseInsensitive => CaseInsensitiveJsonOptions,
                JsonSerializerBehaviour.SnakeCase => SnakeCaseJsonOptions,
                _ => throw new ArgumentOutOfRangeException(nameof(behaviour), behaviour, null)
            };
        }

        public static bool ContainsSameWords(this string first, string second, string[] tokensToIgnore)
        {
            var tokens = ToLowerWords(first);
            var tokens2 = ToLowerWords(second);

            tokens = tokens.Except(tokensToIgnore).ToArray();
            tokens2 = tokens2.Except(tokensToIgnore).ToArray();

            return tokens.SequenceEqual(tokens2);
        }

        private static string[] ToLowerWords(string value)
        {
            if(value == null)
            {
                return Array.Empty<string>();
            }

            return value.Split(new[] { ' ' }).Select(x => x.ToLowerInvariant().Trim(',')).ToArray();
        }
    }

    /// <summary>
    /// Specifies JsonSerializerOptions
    /// </summary>
    public enum JsonSerializerBehaviour
    {
        /// <summary>
        /// Default JsonSerializerOptions are used
        /// </summary>
        Default,
        /// <summary>
        /// PropertyNamingPolicy is CamelCase
        /// </summary>
        CamelCase,
        /// <summary>
        /// Property names are case insensitive
        /// </summary>
        CaseInsensitive,
        /// <summary>
        /// Property names are of snake_case type
        /// </summary>
        SnakeCase
    }
}
