using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Sessions.Interfaces;
using GymManagementSystem.BLL.Services.Sessions.ViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Models.Enums;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.Sessions.Classes;

public class SessionService : ISessionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result> CreateSessionAsync(CreateSessionViewModel model)
    {
        try
        {
            if (model.EndDate <= model.StartDate)
                return Result.Validation("EndDate must be after StartDate");

            if (model.StartDate <= DateTime.Now)
                return Result.Validation("StartDate must be in the future");

            if (model.Capacity < 1 && model.Capacity > 25)
                return Result.Validation("Capacity must be between 1 and 25");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if (trainer is null) return Result.NotFound("Trainer is not found");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId);
            if (category is null) return Result.NotFound("Category is not found");

            var isValid = Enum.TryParse<Speciality>(category.CategoryName, true, out var categorySpeciality);

            if (!isValid || trainer.Speciality != categorySpeciality)
                return Result.Validation("cannot create this session because trainer doesn't match with category");

            var session = _mapper.Map<Session>(model);

            await _unitOfWork.sessionRepository.AddAsync(session);

            var result = await _unitOfWork.SaveChanges() > 0;
            return result ? Result.OK() : Result.Fail("Failed to create session");

        }
        catch (Exception ex) { return Result.Fail("InternalServer Error"); }
    }

    public async Task<Result<IEnumerable<SessionViewModel>?>> GetAllSessionsAsync()
    {
       var sessions = await _unitOfWork.sessionRepository.GetAllSessionsWithTrainerAndCategory();

        #region Comments
        //if (sessions == null || !sessions.Any()) return null;
        // Manual Mapping
        //var MappedSessions = sessions.Select(s => new SessionViewModel()
        //{
        //    Id = s.Id,
        //    Capacity = s.Capacity,
        //    CategoryName = s.Category.CategoryName,
        //    TrainerName = s.Trainer.Name,
        //    EndDate = s.EndDate,
        //    StartDate = s.StartDate,
        //}); 
        #endregion

        // automatic mapping 
        var MappedSessions = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);
        foreach (var session in MappedSessions)
        {
            session.AvailableSlots = 
                session.Capacity - await _unitOfWork.sessionRepository.GetCountOfBookedSlotsAync(session.Id);
        }

        return Result<IEnumerable<SessionViewModel>?>.OK(MappedSessions);
    
    }

    public async Task<Result<IEnumerable<CategorySelectedViewModel>>> GetCategoryForDropDownAsync()
    {
        var result =  _unitOfWork.GetRepository<Category>().GetAll().ToList();
        var model = _mapper.Map<List<CategorySelectedViewModel>>(result);
       
        return Result<IEnumerable<CategorySelectedViewModel>>.OK(model);
    }

    public async Task<Result<IEnumerable<TrainerSelectedViewModel>>> GetTrainerForDropDownAsync()
    {
        var result = _unitOfWork.GetRepository<Trainer>().GetAll().ToList();
        var model = _mapper.Map<List<TrainerSelectedViewModel>>(result);
       
        return Result<IEnumerable<TrainerSelectedViewModel>>.OK(model);
    }
}
