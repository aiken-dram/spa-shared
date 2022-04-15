using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Shared.Application.Extensions;

/// <summary>
/// Extensions for ILogger
/// </summary>
public static class LoggerExtensions
{
    /// <summary>
    /// Log to debug with json of object
    /// </summary>
    /// <param name="logger">logger</param>
    /// <param name="message">name of object</param>
    /// <param name="json">object to generate json for</param>
    public static void JsonLogDebug(this ILogger logger, string message, object json)
    {
        logger.LogDebug("{0}: {1}", message, JsonConvert.SerializeObject(json, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
    }
}
