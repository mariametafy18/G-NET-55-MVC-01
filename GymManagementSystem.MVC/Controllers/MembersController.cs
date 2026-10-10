using GymManagementSystem.BLL.Services.Members.Interfaces;
using GymManagementSystem.BLL.Services.Members.ViewModels;
using Microsoft.AspNetCore.Mvc;
namespace GymManagementSystem.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;
        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var members =  await _memberService.GetMembersAsync();
            return View(members);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _memberService.AddMemberAsync(model);
            if (result)
                return RedirectToAction(nameof(Index));
            return View(model);
        }
    }
}
