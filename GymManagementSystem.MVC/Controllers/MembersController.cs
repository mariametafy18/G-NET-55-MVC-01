using GymManagementSystem.BLL.Services.Members.Interfaces;
using GymManagementSystem.BLL.Services.Members.ViewModels;
using GymManagementSystem.DAL.Models;
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
        {
            TempData["SuccessMessage"] = "Member Created Successfully";
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData["ErrorMessage"] = "Member Creation Failed";
            return RedirectToAction(nameof(Create));
        }
    }

    public async Task<IActionResult> Details(int? id)
    {
        var member = await _memberService.GetMemberAsync(id);
        if(member == null)
        {
            TempData["ErrorMessage"] = "Member not found";
            return RedirectToAction(nameof(Index));
        }
        return View(member);
    }
    public async Task<IActionResult> HealthRecordDetails(int? id)
    {
        var model = await _memberService.GetHealthRecordDetailsAsync(id);
        if (model == null)
        {
            TempData["ErrorMessage"] = "Health Record not found";
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    public async Task<IActionResult> EditMember(int? id)
    {
        var member = await _memberService.GetMemberToUpdateAsync(id);

        if (member == null) 
        {
            TempData["ErrorMessage"] = "Member not found";
            return RedirectToAction(nameof(Index));
        }
        return View(member);
    }

    [HttpPost]
    public async Task<IActionResult> EditMember(MemberToUpdateViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await _memberService.UpdateMemberAsync(model);

        if (result)
        {
            TempData["SuccessMessage"] = "Member Updated Successfully";
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData["ErrorMessage"] = "Member Update Failed";
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

        if (result)
        {
            TempData["SuccessMessage"] = "Member deleted Successfully";
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData["ErrorMessage"] = "Member Delete Failed";
            return View(Id);
        }
    }
}
