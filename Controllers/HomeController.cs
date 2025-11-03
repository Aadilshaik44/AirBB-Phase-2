using AirBB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AirBB.Controllers;

public class HomeController : Controller
{
    private readonly AirBnbContext _ctx;
    private readonly AirBBCookies _cookies;

    public HomeController(AirBnbContext ctx, IHttpContextAccessor http)
    {
        _ctx = ctx;
        _cookies = new AirBBCookies(http);
    }

    // GET /  — Filter panel + results (PRG target)
    [HttpGet]
    public IActionResult Index(HomeViewModel model)
    {
        // load cookie-based reservation ids into session badge on first hit
        var sess = new AirBBSession(HttpContext.Session);
        if (sess.GetReservationIds().Count == 0)
            sess.SetReservationIds(_cookies.GetReservationIds());

        // restore filters from session if empty
        var loc = !string.IsNullOrWhiteSpace(model.ActiveLocationId) ? model.ActiveLocationId : sess.GetLoc();
        var guests = !string.IsNullOrWhiteSpace(model.ActiveGuests) ? model.ActiveGuests : sess.GetGuests();
        var start = !string.IsNullOrWhiteSpace(model.ActiveStart) ? model.ActiveStart : sess.GetStart();
        var end   = !string.IsNullOrWhiteSpace(model.ActiveEnd)   ? model.ActiveEnd   : sess.GetEnd();

        // apply to session to retain during navigation
        sess.SetFilters(loc, guests, start, end);

        // build locations dropdown (default "All")
        var locations = _ctx.Locations
            .OrderBy(l => l.Name)
            .Select(l => new SelectListItem { Text = l.Name, Value = l.LocationId.ToString() })
            .ToList();
        locations.Insert(0, new SelectListItem { Text = "All", Value = "All" });

        // base query
        var q = _ctx.Residences.Include(r => r.Location).AsQueryable();

        if (int.TryParse(loc, out var locId)) q = q.Where(r => r.LocationId == locId);
        if (int.TryParse(guests, out var g)) q = q.Where(r => r.GuestNumber >= g);

        // date overlap filter — use Start/End overlap
        // Only filter if both dates present & valid
        if (DateTime.TryParse(start, out var sdt) && DateTime.TryParse(end, out var edt))
        {
            q = q.Where(r => !_ctx.Reservations
                .Any(res => res.ResidenceId == r.ResidenceId
                            && res.ReservationStartDate <= edt
                            && res.ReservationEndDate >= sdt));
        }

        model.Locations = locations;
        model.Residences = q.OrderBy(r => r.Location!.Name).ThenBy(r => r.Name).ToList();
        model.ActiveLocationId = loc;
        model.ActiveGuests = guests;
        model.ActiveStart = start;
        model.ActiveEnd = end;
        model.Reservations = GetReservationsForBadge(sess.GetReservationIds());

        return View(model);
    }

    // POST /Home/Filter  — PRG: set filters, redirect to Index
    [HttpPost]
    public IActionResult Filter(HomeViewModel model)
    {
        var sess = new AirBBSession(HttpContext.Session);
        sess.SetFilters(model.ActiveLocationId, model.ActiveGuests, model.ActiveStart, model.ActiveEnd);
        return RedirectToAction(nameof(Index));
    }

    private List<Reservation> GetReservationsForBadge(List<int> ids) =>
        ids.Count == 0 ? new() :
        _ctx.Reservations.Include(r => r.Residence).Where(r => ids.Contains(r.ReservationId)).ToList();

    public IActionResult Index() => View();


    public IActionResult Support() =>
    Content("Area=Public, Controller=Home, Action=Support");


    public IActionResult CancellationPolicy() =>
    Content("Area=Public, Controller=Home, Action=CancellationPolicy");


    public IActionResult Terms() =>
    Content("Area=Public, Controller=Home, Action=Terms");


    public IActionResult Cookies() =>
    Content("Area=Public, Controller=Home, Action=Cookies");
}
