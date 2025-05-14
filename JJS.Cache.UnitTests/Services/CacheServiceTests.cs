using FluentAssertions;
using JJS.Cache.Services;
using Microsoft.Extensions.Logging;

namespace JJS.Cache.UnitTests.Services;

public class CacheServiceTests
{
    private readonly CacheService _sut;
    private readonly ILogger<CacheService> _logger = NSubstitute.Substitute.For<
        ILogger<CacheService>
    >();

    public CacheServiceTests()
    {
        _sut = new CacheService(_logger, TimeSpan.FromMilliseconds(100));
    }

    [Fact]
    public void Set_ShouldStoreValueInCache_WhenValueIsNotNull()
    {
        // Arrange
        var key = "testKey";
        var value = "testValue";

        // Act
        _sut.Set(key, value);

        // Assert
        var existsInCache = _sut.TryGet(key, out string? cachedValue);
        existsInCache.Should().BeTrue();
        cachedValue.Should().Be(value);
    }

    [Fact]
    public void Set_ShouldStoreValueInCacheWithExpiration_WhenValueIsNotNull()
    {
        // Arrange
        var key = "testKey";
        var value = "testValue";
        var expiration = TimeSpan.FromSeconds(5);
        // Act
        _sut.Set(key, value, expiration);
        // Assert
        var existsInCache = _sut.TryGet(key, out string? cachedValue);
        existsInCache.Should().BeTrue();
        cachedValue.Should().Be(value);
    }

    [Fact]
    public void Set_ShouldNotStoreValueInCache_WhenValueIsNull()
    {
        // Arrange
        var key = "testKey";
        string? value = null;

        // Act
        _sut.Set(key, value);

        // Assert
        var existsInCache = _sut.TryGet(key, out string? cachedValue);
        existsInCache.Should().BeFalse();
        cachedValue.Should().BeNull();
    }

    [Fact]
    public void Set_ShouldNotStoreMutableValueInCache_WhenValueIsMutable()
    {
        // Arrange
        var key = "testKey";
        var expectedValue = new List<string> { "testValue" };
        var value = new List<string> { "testValue" };

        // Act
        _sut.Set(key, value);
        value = ["modifiedValue"];

        // Assert
        var existsInCache = _sut.TryGet(key, out List<string>? cachedValue);
        existsInCache.Should().BeTrue();
        cachedValue.Should().BeEquivalentTo(expectedValue);
    }

    [Fact]
    public void TryGet_ShouldReturnTrueAndValue_WhenKeyExists()
    {
        // Arrange
        var key = "testKey";
        var value = "testValue";
        _sut.Set(key, value);

        // Act
        var existsInCache = _sut.TryGet(key, out string? cachedValue);

        // Assert
        existsInCache.Should().BeTrue();
        cachedValue.Should().Be(value);
    }

    [Fact]
    public void TryGet_ShouldReturnFalseAndNull_WhenKeyDoesNotExist()
    {
        // Arrange
        var key = "nonExistentKey";

        // Act
        var existsInCache = _sut.TryGet(key, out string? cachedValue);

        // Assert
        existsInCache.Should().BeFalse();
        cachedValue.Should().BeNull();
    }

    [Fact]
    public void TryGet_ShouldReturnFalseAndNull_WhenKeyExistsButIsExpired()
    {
        // Arrange
        var key = "testKey";
        var value = "testValue";
        var expiration = TimeSpan.FromMilliseconds(1);
        _sut.Set(key, value, expiration);
        // Wait for the cache entry to expire
        Thread.Sleep(2);
        // Act
        var existsInCache = _sut.TryGet(key, out string? cachedValue);
        // Assert
        existsInCache.Should().BeFalse();
        cachedValue.Should().BeNull();
    }

    [Fact]
    public void GetOrSet_ShouldReturnValue_WhenKeyExists()
    {
        // Arrange
        var key = "testKey";
        var value = "testValue";
        _sut.Set(key, value);
        // Act
        var cachedValue = _sut.GetOrSet(key, () => "newValue");
        // Assert
        cachedValue.Should().Be(value);
    }

    [Fact]
    public void GetOrSet_ShouldStoreValueInCache_WhenKeyDoesNotExist()
    {
        // Arrange
        var key = "testKey";
        var value = "testValue";
        // Act
        var cachedValue = _sut.GetOrSet(key, () => value);
        // Assert
        cachedValue.Should().Be(value);
        var existsInCache = _sut.TryGet(key, out string? retrievedValue);
        existsInCache.Should().BeTrue();
        retrievedValue.Should().Be(value);
    }

    [Fact]
    public void GetOrSet_ShouldStoreValueInCache_WhenKeyExistsButIsExpired()
    {
        // Arrange
        var key = "testKey";
        var value = "testValue";
        var expiration = TimeSpan.FromMilliseconds(1);
        _sut.Set(key, value, expiration);
        // Wait for the cache entry to expire
        Thread.Sleep(2);
        // Act
        var cachedValue = _sut.GetOrSet(key, () => "newValue");
        // Assert
        cachedValue.Should().Be("newValue");
    }

    [Fact]
    public void GetOrSet_ShouldStoreValueInCacheWithExpiration_WhenKeyDoesNotExist()
    {
        // Arrange
        var key = "testKey";
        var value = "testValue";
        var expiration = TimeSpan.FromSeconds(5);
        // Act
        var cachedValue = _sut.GetOrSet(key, () => value, expiration);
        // Assert
        cachedValue.Should().Be(value);
        var existsInCache = _sut.TryGet(key, out string? retrievedValue);
        existsInCache.Should().BeTrue();
        retrievedValue.Should().Be(value);
    }
}
