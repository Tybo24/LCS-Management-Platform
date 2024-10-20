using LCS_Management_Platform.Data;
using LCS_Management_Platform.Helpers;
using LCS_Management_Platform.Services;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Web.Http;
public static class ExceptionHelper
{
    public static void HandleTooManyRequests()
    {
        if (AppState.timerCountdown != 0)
        {
            AppState.waitUntil = DateTime.UtcNow.AddSeconds(AppState.timerCountdown / 1000);
        }
        else
        {
            AppState.waitUntil = DateTime.UtcNow.AddMinutes(5);
        }

        throw new Exception($"Too Many Requests. Try again after {DateTimeHelper.BritishTime(AppState.waitUntil)} ");
    }
}
