# Weather API

A RESTful API that wraps [Visual Crossing's](https://www.visualcrossing.com/weather-api) weather data,
with Redis caching and rate limiting. 
Built for the [roadmap.sh Weather API project]
(https://roadmap.sh/projects/weather-api-wrapper-service).

## Features

- Fetches real-time weather from a 3rd-party API
- Redis caching (12-hour expiration) to reduce external calls
- Rate limiting (10 req/min per IP)
- Secrets kept out of source control via `appsettings.Development.json` / user-secrets

## Tech Stack

ASP.NET Core (C#) · StackExchange.Redis · AspNetCoreRateLimit

## Getting Started

**Prerequisites:** .NET SDK 8+, Docker (for Redis), a free [Visual Crossing API key](https://www.visualcrossing.com/weather-api)

```bash
git clone https://github.com/Nasyidashvili/WeatherAPI.git
cd WeatherAPI

# start Redis
docker run -d --name redis-weather -p 6379:6379 redis

# add your API key (appsettings.Development.json is gitignored)
dotnet user-secrets init
dotnet user-secrets set "WeatherApiSettings:ApiKey" "YOUR_API_KEY"

dotnet run
```

Swagger UI available at `/swagger`.

## Usage

GET /api/weather/{location}


**Example:** `GET /api/weather/tbilisi`

```json
{
  "dateTime": "2026-09-23",
  "temperatureC": 21.1,
  "humidity": 73.2
}
```

Returns `400` for a missing location, `429` if the rate limit is exceeded.

## Notes

- No pagination, authentication, or authorization (out of scope)
- Temperatures in Celsius
