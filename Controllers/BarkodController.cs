using Microsoft.AspNetCore.Mvc;
using StokBarkodSistemi.Services;

namespace StokBarkodSistemi.Controllers
{
    public class BarkodController : Controller
    {
        private readonly BarkodService _barkodService;

        public BarkodController(BarkodService barkodService)
        {
            _barkodService = barkodService;
        }

        [HttpPost]
        public IActionResult CreateBarkod(string stokNo, decimal gelenMiktar)
        {
            try
            {
                _barkodService.GenerateBarkod(stokNo, gelenMiktar);
                return RedirectToAction("Barkodlar", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
    }
}
