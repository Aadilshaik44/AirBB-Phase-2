using Microsoft.AspNetCore.Mvc;


namespace AirBB.Controllers
{
public class ExperienceController : Controller
{
public IActionResult List(string id = "All") =>
Content($"Area=Public, Controller=Experience, Action=List, id={id}");
}
}