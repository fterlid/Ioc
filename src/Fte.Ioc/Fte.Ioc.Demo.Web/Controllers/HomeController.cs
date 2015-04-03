using Fte.Ioc.Demo.Services;
using System.Web.Mvc;

namespace Fte.Ioc.Demo.Web.Controllers
{
	public class HomeController : Controller
	{
		public HomeController(ISomeOtherService someOtherService)
		{
		}

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}