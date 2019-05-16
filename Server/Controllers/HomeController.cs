
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

[Route("[controller]")]
public class HomeController : Controller
{

  public HomeController(IHostingEnvironment evn)
  {
      _evn = evn;
  }

  IHostingEnvironment _evn;
  
  public IActionResult Index()
  {
    return File(_evn.WebRootPath+"\\index.html", "text/html");
  }

}