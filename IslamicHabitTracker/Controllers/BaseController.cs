using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

public class BaseController : ControllerBase
{
    protected string GetUserTimeZone()
    {
        return Request.Headers["X-TimeZone"].FirstOrDefault() ?? "UTC";
    }

    protected DateTime ToUserTime(DateTime utcDateTime)
    {
        var timeZoneId = GetUserTimeZone();
        try
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZone);
        }
        catch
        {
            return utcDateTime;
        }
    }

    protected DateTime ToUtcTime(DateTime localDateTime)
    {
        var timeZoneId = GetUserTimeZone();
        try
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeToUtc(localDateTime, timeZone);
        }
        catch
        {
            return localDateTime;
        }
    }
} 