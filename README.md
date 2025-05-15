[![GitHub License](https://img.shields.io/github/license/jjs98/cache-service)](LICENSE)
[![NuGet Version](https://img.shields.io/nuget/v/JJS.Cache)](https://www.nuget.org/packages/JJS.Cache/)

# JJS.Cache

A CacheService to cache values in memory

## Installation
```powershell
 dotnet add package JJS.Cache
 ```

## Usage
```csharp
using JJS.Cache;

var builder = WebApplication.CreateBuilder();
builder.Services.UseCacheService(builder.Configuration);

var app = builder.Build();

```

## Example
```csharp

using JJS.Cache;
using System.Threading.Tasks;

public class MyService(ICacheService cacheService)
{
    public async Task<string> GetCachedValueAsync()
    {
        var value = await cacheService.GetOrSetAsync<string>("test-key", async () =>
        {
            // Simulate a long-running operation
            await Task.Delay(1000);
            return "Hello, World!";
        });

        if (value == null)
        {
            return "valueFactory returned null";
        }

        return value;
    }
}
```

## Configuration
Configure the default expiration time for the cache in milliseconds in the appsettings.json file. If not set, the default value is 60000 milliseconds (1 minute).
```json
{
  "Cache": {
	"DefaultExpiration": 60000
  }
}
```

## Dependencies
`Microsoft.Extensions.Logging.ILogger` is used for logging.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributing
If you would like to contribute to this project, please fork the repository and submit a pull request. We welcome contributions of all kinds, including bug fixes, new features, and documentation improvements.
