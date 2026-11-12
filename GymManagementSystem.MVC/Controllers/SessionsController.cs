using GymManagementSystem.BLL.Services.Sessions.Interfaces;
using GymManagementSystem.BLL.Services.Sessions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymManagementSystem.PL.Controllers;

public class SessionsController : Controller
{
    private readonly ISessionService _sessionService;

    public SessionsController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _sessionService.GetAllSessionsAsync();

        //if (sessions == null) { 
        return View(result.Value);
    
    }

    public async Task<IActionResult> Create()
    {
       await LoadTrainersAndCategoriesForSelectedList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSessionViewModel model)
    {
        var result = await _sessionService.CreateSessionAsync(model);
       
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Session Created Successfully";
            return RedirectToAction(nameof(Index));
        }
        await LoadTrainersAndCategoriesForSelectedList();
        return View(model);
    }
    #region Helper Methods

    private async Task LoadTrainersAndCategoriesForSelectedList()
    {
        var trainers = await _sessionService.GetTrainerForDropDownAsync();
        ViewBag.Trainers = new SelectList(trainers.Value, "Id", "Name");

        var categories = await _sessionService.GetCategoryForDropDownAsync();
        ViewBag.Categories = new SelectList(categories.Value, "Id", "CategoryName");
    }
    #endregion
}
