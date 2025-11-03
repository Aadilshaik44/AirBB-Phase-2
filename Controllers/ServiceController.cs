using Microsoft.AspNetCore.Mvc;


namespace AirBB.Controllers
{
public class ServiceController : Controller
{
public IActionResult List(string id = "All") =>
Content($"Area=Public, Controller=Service, Action=List, id={id}");
}
}