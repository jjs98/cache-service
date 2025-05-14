namespace JJS.Cache.Interfaces;

/// <summary>
/// Interface for a caching service that provides methods to set, get, and manage cache entries.
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Sets a value in the cache with a specified key.
    /// </summary>
    /// <typeparam name="T">Type of the value in the cache entry</typeparam>
    /// <param name="key">Key to identify the cache entry</param>
    /// <param name="value">The value of the cache entry</param>
    void Set<T>(string key, T? value)
        where T : notnull;

    /// <summary>
    /// Sets a value in the cache with a specified key and expiration time.
    /// </summary>
    /// <typeparam name="T">Type of the value in the cache entry</typeparam>
    /// <param name="key">Key to identify the cache entry</param>
    /// <param name="value">The value of the cache entry</param>
    /// <param name="expiration">Timespan after when the cache entry expires</param>
    void Set<T>(string key, T? value, TimeSpan expiration)
        where T : notnull;

    /// <summary>
    /// Tries to get a value from the cache with a specified key.
    /// </summary>
    /// <typeparam name="T">Type of the value in the cache entry</typeparam>
    /// <param name="key">Key to identify the cache entry</param>
    /// <param name="value">The value of the cache entry</param>
    /// <returns>Value of cache entry or default for the requested type</returns>
    bool TryGet<T>(string key, out T? value);

    /// <summary>
    /// Gets a value from the cache with a specified key, or sets it using the provided factory function if not found.
    /// </summary>
    /// <typeparam name="T">Type of the value in the cache entry</typeparam>
    /// <param name="key">Key to identify the cache entry</param>
    /// <param name="valueFactory">Function to get the value if the cache entry does not exists</param>
    /// <returns>Cache entry if exists, otherwise result of valueFactory</returns>
    T? GetOrSet<T>(string key, Func<T> valueFactory)
        where T : notnull;

    /// <summary>
    /// Gets a value from the cache with a specified key, or sets it using the provided factory function if not found.
    /// </summary>
    /// <typeparam name="T">Type of the value in the cache entry</typeparam>
    /// <param name="key">Key to identify the cache entry</param>
    /// <param name="valueFactory">Function to get the value if the cache entry does not exists</param>
    /// <param name="expiration">Timespan after when the cache entry expires</param>
    /// <returns>Cache entry if exists, otherwise result of valueFactory</returns>
    T? GetOrSet<T>(string key, Func<T> valueFactory, TimeSpan expiration)
        where T : notnull;

    /// <summary>
    /// Asynchronously gets a value from the cache with a specified key, or sets it using the provided factory function if not found.
    /// </summary>
    /// <typeparam name="T">Type of the value in the cache entry</typeparam>
    /// <param name="key">Key to identify the cache entry</param>
    /// <param name="valueFactory">Asynchronous function to get the value if the cache entry does not exists</param>
    /// <returns>Cache entry if exists, otherwise result of valueFactory</returns>
    Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> valueFactory)
        where T : notnull;

    /// <summary>
    /// Asynchronously gets a value from the cache with a specified key, or sets it using the provided factory function if not found.
    /// </summary>
    /// <typeparam name="T">Type of the value in the cache entry</typeparam>
    /// <param name="key">Key to identify the cache entry</param>
    /// <param name="valueFactory">Asynchronous function to get the value if the cache entry does not exists</param>
    /// <param name="expiration">Timespan after when the cache entry expires</param>
    /// <returns>Cache entry if exists, otherwise result of valueFactory</returns>
    Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> valueFactory, TimeSpan expiration)
        where T : notnull;

    /// <summary>
    /// Removes a value from the cache with a specified key.
    /// </summary>
    /// <param name="key">Key to identify the cache entry</param>
    void Remove(string key);

    /// <summary>
    /// Removes all items from the collection.
    /// </summary>
    void Clear();
}
