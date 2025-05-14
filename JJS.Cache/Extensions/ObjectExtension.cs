using System.Text.Json;

namespace JJS.Cache.Extensions;

public static class ObjectExtension
{
    /// <summary>
    /// Deep copies an object using JSON serialization and deserialization.
    /// </summary>
    /// <typeparam name="T">Type of object</typeparam>
    /// <param name="self">Object to copy</param>
    /// <returns>Copy of the object</returns>
    public static T DeepCopy<T>(this T self)
    {
        var json = JsonSerializer.Serialize(self);
        return JsonSerializer.Deserialize<T>(json)!;
    }
}
