namespace AirBB.Models;

public class AirBBSession
{
    private const string LocKey = "filter_loc";
    private const string GuestsKey = "filter_guests";
    private const string StartKey = "filter_start";
    private const string EndKey = "filter_end";
    private const string ResListKey = "res_list"; // for badge (list of ReservationIds)

    private ISession S { get; }

    public AirBBSession(ISession s) => S = s;

    // filters
    public void SetFilters(string loc, string guests, string start, string end)
    {
        S.SetString(LocKey, loc ?? "All");
        S.SetString(GuestsKey, string.IsNullOrWhiteSpace(guests) ? "1" : guests);
        S.SetString(StartKey, start ?? "");
        S.SetString(EndKey, end ?? "");
    }

    public string GetLoc() => S.GetString(LocKey) ?? "All";
    public string GetGuests() => S.GetString(GuestsKey) ?? "1";
    public string GetStart() => S.GetString(StartKey) ?? "";
    public string GetEnd()   => S.GetString(EndKey) ?? "";

    // badge helpers (just a count)
    public void SetReservationIds(List<int> ids) => S.SetObject(ResListKey, ids);
    public List<int> GetReservationIds() => S.GetObject<List<int>>(ResListKey) ?? new();
    public int? GetReservationCount() => GetReservationIds().Count;
}
