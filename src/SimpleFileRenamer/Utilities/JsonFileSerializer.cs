using Newtonsoft.Json;
using Serilog;
using SimpleFileRenamer.Abstractions.Utilities;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

namespace SimpleFileRenamer.Utilities;

public class JsonFileSerializer : IFileSerializer
{
    private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
    {
        NullValueHandling = NullValueHandling.Ignore,
        Formatting = Formatting.None,
        DateFormatHandling = DateFormatHandling.IsoDateFormat,
        DateParseHandling = DateParseHandling.DateTimeOffset,
    };

    static JsonFileSerializer()
    {
        _settings.Error += JsonErrorHandler;
    }

    public string Serialize<T>(T obj) => JsonConvert.SerializeObject(obj);

    public T? Deserialize<T>(string data) => JsonConvert.DeserializeObject<T>(data);

    private static void JsonErrorHandler(object? sender, ErrorEventArgs e)
    {
        // Only log an error once
        if (e.CurrentObject == e.ErrorContext.OriginalObject)
        {
            Log.Error(e.ErrorContext.Error.Message);
            e.ErrorContext.Handled = true;
        }
    }
}
