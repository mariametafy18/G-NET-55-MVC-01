using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.MVC.Controllers
{
    public class PlansController : Controller
    {
        private IPlanRepository _planRepository;

        public PlansController(IPlanRepository planRepository)
        {
            //PlanRepository = new PlanRepository();
            _planRepository = planRepository;
        }
        public async Task<IActionResult> IndexAsync()

        {
            var plans = await _planRepository.GetAllAsync();
            return View(plans);
        }

        public async Task<IActionResult> Details(int id)
        {
            var plan = await _planRepository.GetByIdAsync(id);

            if (plan is null)
                return RedirectToAction("Index");
            else
                return View(plan);
        }
    }
}
