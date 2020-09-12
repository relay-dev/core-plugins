using Core.Utilities;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Plugins.Utilities
{
    public class SystemJsonSerializer : IJsonSerializer
    {
        public string Serialize(object obj, SerializerOptions options = null)
        {
            return JsonSerializer.Serialize(obj, ToJsonOptions(options));
        }

        public async Task<string> SerializeAsync(object objectToSerialize, CancellationToken cancellationToken, SerializerOptions options = null)
        {
            await using var stream = new MemoryStream();

            await JsonSerializer.SerializeAsync(stream, objectToSerialize, objectToSerialize.GetType(), ToJsonOptions(options), cancellationToken);

            stream.Position = 0;

            using var reader = new StreamReader(stream);

            return await reader.ReadToEndAsync();
        }

        public TReturn Deserialize<TReturn>(string obj, SerializerOptions options = null)
        {
            return JsonSerializer.Deserialize<TReturn>(obj, ToJsonOptions(options));
        }

        public async Task<TReturn> DeserializeAsync<TReturn>(string json, CancellationToken cancellationToken, SerializerOptions options = null)
        {
            await using var stream = new MemoryStream();

            return await JsonSerializer.DeserializeAsync<TReturn>(stream, ToJsonOptions(options), cancellationToken);
        }

        private JsonSerializerOptions ToJsonOptions(SerializerOptions options)
        {
            if (options == null)
            {
                return null;
            }

            var jsonOptions = new JsonSerializerOptions
            {
                AllowTrailingCommas = options.AllowTrailingCommas,
                DefaultBufferSize = options.DefaultBufferSize,
                IgnoreNullValues = options.IgnoreNullValues,
                IgnoreReadOnlyProperties = options.IgnoreReadOnlyProperties,
                MaxDepth = options.MaxDepth,
                PropertyNameCaseInsensitive = options.PropertyNameCaseInsensitive,
                WriteIndented = options.WriteIndented,
                
            };

            if (!string.IsNullOrEmpty(options.DictionaryKeyPolicy))
            {
                jsonOptions.DictionaryKeyPolicy = GlobalHelper.ParseEnum<JsonNamingPolicy>(options.DictionaryKeyPolicy);
            }

            if (!string.IsNullOrEmpty(options.Encoder))
            {
                jsonOptions.Encoder = GlobalHelper.ParseEnum<JavaScriptEncoder>(options.Encoder);
            }

            if (!string.IsNullOrEmpty(options.PropertyNamingPolicy))
            {
                jsonOptions.PropertyNamingPolicy = GlobalHelper.ParseEnum<JsonNamingPolicy>(options.PropertyNamingPolicy);
            }

            if (!string.IsNullOrEmpty(options.ReadCommentHandling))
            {
                jsonOptions.ReadCommentHandling = GlobalHelper.ParseEnum<JsonCommentHandling>(options.ReadCommentHandling);
            }

            return jsonOptions;
        }
    }
}
