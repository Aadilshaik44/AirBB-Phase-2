using AirBB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirBB.Controllers;

public class ReservationsController : Controller
{
    private readonly AirBnbContext _ctx;
    private readonly AirBBCookies _cookies;
    public ReservationsController(AirBnbContext ctx, IHttpContextAccessor http)
    {
        _ctx = ctx; _cookies = new AirBBCookies(http);
    }

    [HttpGet]
    public IActionResult Index()
    {
        var sess = new AirBBSession(HttpContext.Session);
        var ids = sess.GetReservationIds();

        var list = (ids.Count == 0)
            ? new List<Reservation>()
            : _ctx.Reservations.Include(r => r.Residence).ThenInclude(x => x!.Location)
                  .Where(r => ids.Contains(r.ReservationId))
                  .OrderByDescending(r => r.ReservationId).ToList();

        return View(list);
    }

    [HttpPost]
    public IActionResult Cancel(int id)
    {
        var res = _ctx.Reservations.Find(id);
        if (res != null)
        {
            _ctx.Reservations.Remove(res);
            _ctx.SaveChanges();

            // also drop from session/cookie id list
            var sess = new AirBBSession(HttpContext.Session);
            var ids = sess.GetReservationIds();
            ids.Remove(id);
            sess.SetReservationIds(ids);
            _cookies.SaveReservationIds(ids);

            TempData["message"] = "Reservation canceled.";
        }
        return RedirectToAction(nameof(Index));
    }
}
