using Backbone.General.Serialization.Json.Abstractions.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Backbone.General.Serialization.Json.Newtonsoft.Brokers;

/// <summary>
/// Provides JSON serialization settings for Newtonsoft serialization provider to customize serialization
/// </summary>
public class NewtonsoftJsonSerializationSettingsProvider : INewtonsoftJsonSerializationSettingsProvider
{
    private readonly Dictionary<string, JsonSerializerSettings> _settingsDictionary;

    public NewtonsoftJsonSerializationSettingsProvider()
    {
        _settingsDictionary = new Dictionary<string, JsonSerializerSettings>
        {
            [JsonSerializationConstants.GeneralSerializationSettings] = Configure(new JsonSerializerSettings()),
            [JsonSerializationConstants.GeneralSerializationWithTypeHandlingSettings] = ConfigureWithTypeHandling(new JsonSerializerSettings())
        };
    }

    public JsonSerializerSettings Get()
    {
        return new JsonSerializerSettings(_settingsDictionary[JsonSerializationConstants.GeneralSerializationSettings]);
    }

    public JsonSerializerSettings Get(string serializationSettingsKey)
    {
        return _settingsDictionary.TryGetValue(serializationSettingsKey, out var value)
            ? new JsonSerializerSettings(value)
            : throw new KeyNotFoundException("The specified JSON serialization settings with key does not exist.");
    }

    public JsonSerializerSettings GetWithTypeHandling()
    {
        return new JsonSerializerSettings(_settingsDictionary[JsonSerializationConstants.GeneralSerializationSettings]);
    }

    public void Add(string serializationSettingsKey, JsonSerializerSettings settings)
    {
        _settingsDictionary[serializationSettingsKey] = settings;
    }

    public void Update(string serializationSettingsKey, JsonSerializerSettings newSettings)
    {
        _settingsDictionary[serializationSettingsKey] = _settingsDictionary.ContainsKey(serializationSettingsKey)
            ? newSettings
            : throw new KeyNotFoundException("The specified JSON serialization settings with key does not exist.");
    }

    public bool Remove(string serializationSettingsKey)
    {
        return _settingsDictionary.Remove(serializationSettingsKey);
    }

    public JsonSerializerSettings Configure(JsonSerializerSettings settings)
    {
        // Configures the output JSON formatting for readability
        settings.Formatting = Formatting.Indented;

        // Configures reference loops when serializing objects with circular references
        settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

        // Configures the contract resolver to use camelCase for property names
        settings.ContractResolver = new DefaultContractResolver();

        // Ignores null values during serialization
        settings.NullValueHandling = NullValueHandling.Ignore;

        return settings;
    }

    public JsonSerializerSettings ConfigureWithTypeHandling(JsonSerializerSettings settings)
    {
        Configure(settings);

        // Configures the type name handling to include type information
        settings.TypeNameHandling = TypeNameHandling.All;

        return settings;
    }
}