using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Http;


namespace AirBB.Models;

public class AirBBCookies
{
    private const string ResCookie = "airbb_reservation_ids";
    private readonly IHttpContextAccessor _http;

    public AirBBCookies(IHttpContextAccessor http) => _http = http;

    public List<int> GetReservationIds()
    {
        var req = _http.HttpContext!.Request;
        if (!req.Cookies.TryGetValue(ResCookie, out var json) || string.IsNullOrEmpty(json)) return new();
        try { return JsonSerializer.Deserialize<List<int>>(json) ?? new(); }
        catch { return new(); }
    }

    public void SaveReservationIds(List<int> ids)
    {
        var res = _http.HttpContext!.Response;
        var options = new CookieOptions { Expires = DateTimeOffset.Now.AddDays(7), HttpOnly = true, IsEssential = true };
        res.Cookies.Append(ResCookie, JsonSerializer.Serialize(ids), options);
    }

    public void Clear() => _http.HttpContext!.Response.Cookies.Delete(ResCookie);
}
