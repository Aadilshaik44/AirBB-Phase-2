using AirBB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirBB.Controllers;

public class ResidenceController : Controller
{
    private readonly AirBnbContext _ctx;
    private readonly AirBBCookies _cookies;

    public ResidenceController(AirBnbContext ctx, IHttpContextAccessor http)
    {
        _ctx = ctx;
        _cookies = new AirBBCookies(http);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var sess = new AirBBSession(HttpContext.Session);
        var model = _ctx.Residences.Include(r => r.Location).FirstOrDefault(r => r.ResidenceId == id);
        if (model == null) return RedirectToAction("Index", "Home");

        // keep current filters in ViewData so the Detail page can “Back to Home” retaining state
        ViewData["loc"] = sess.GetLoc();
        ViewData["gu"] = sess.GetGuests();
        ViewData["st"] = sess.GetStart();
        ViewData["en"] = sess.GetEnd();
        return View(model);
    }

    // POST /Residence/Reserve
    [HttpPost]
    public IActionResult Reserve(int id, string start, string end)
    {
        if (!DateTime.TryParse(start, out var sdt) || !DateTime.TryParse(end, out var edt) || sdt > edt)
        {
            TempData["message"] = "Please select a valid date range.";
            return RedirectToAction("Details", new { id });
        }

        // availability check (overlap)
        var overlap = _ctx.Reservations.Any(r => r.ResidenceId == id &&
                                                 r.ReservationStartDate <= edt &&
                                                 r.ReservationEndDate >= sdt);
        if (overlap)
        {
            TempData["message"] = "Sorry, those dates are unavailable.";
            return RedirectToAction("Details", new { id });
        }

        var res = new Reservation
        {
            ResidenceId = id,
            ReservationStartDate = sdt.Date,
            ReservationEndDate = edt.Date
        };
        _ctx.Reservations.Add(res);
        _ctx.SaveChanges();

        // update badge (session + cookie store ids only)
        var sess = new AirBBSession(HttpContext.Session);
        var ids = sess.GetReservationIds();
        ids.Add(res.ReservationId);
        sess.SetReservationIds(ids);
        _cookies.SaveReservationIds(ids);

        TempData["message"] = "Reservation confirmed!";
        return RedirectToAction("Index", "Home"); // PRG back to filter page
    }
}
