using GymManagementSystem.BLL.Services.Members.Interfaces;
using GymManagementSystem.BLL.Services.Members.ViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.MVC.Controllers;
using Microsoft.AspNetCore.Mvc;
namespace GymManagementSystem.PL.Controllers;

public class MembersController : Controller
{
    private readonly IMemberService _memberService;
    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }
    public async Task<IActionResult> IndexAsync()
    {
        var result = _memberService.GetMembersAsync();
        if (result.Success)
            return View(result.Value);

        return RedirectToAction(nameof(Index), nameof(HomeController));
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
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Member Created Successfully";
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Create));
        }
    }

    public async Task<IActionResult> Details(int? id)
    {
        var result = await _memberService.GetMemberAsync(id);
        if(!result.Success)
        {
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Value);
    }
    public async Task<IActionResult> HealthRecordDetails(int? id)
    {
        var result = await _memberService.GetHealthRecordDetailsAsync(id);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Value);
    }

    public async Task<IActionResult> EditMember(int? id)
    {
        var result = await _memberService.GetMemberToUpdateAsync(id);

        if (!result.Success) 
        {
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> EditMember(MemberToUpdateViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await _memberService.UpdateMemberAsync(model);

        if (result.Success)
        {
            TempData["SuccessMessage"] = "Member Updated Successfully";
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(model));
        }
    }

    public async Task<IActionResult> Delete(int? Id)
    {
        return View(Id);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int? Id)
    {
        var result = await _memberService.DeleteMemberAsync(Id);

        if (result.Success)
        {
            TempData["SuccessMessage"] = "Member deleted Successfully";
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData["ErrorMessage"] = result.error;
            return View(Id);
        }
    }
}
