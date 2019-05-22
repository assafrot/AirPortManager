
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{

  public HomeController(IHostingEnvironment evn)
  {
      _evn = evn;
  }

  IHostingEnvironment _evn;
  
  public IActionResult Index()
  {
    return File(System.IO.File.OpenRead(_evn.WebRootPath+"\\index.html"), "text/html");
  }

}