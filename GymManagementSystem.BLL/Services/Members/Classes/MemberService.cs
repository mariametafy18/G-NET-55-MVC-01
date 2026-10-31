using GymManagementSystem.BLL.Services.Members.Interfaces;
using GymManagementSystem.BLL.Services.Members.ViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.BLL.Services.Members.Classes;

public class MemberService : IMemberService
{
    //    private readonly IRepository<Member> _membersRepository;
    //    private readonly IRepository<Membership> _membershipRepository;
    //    private readonly IRepository<Plan> _planRepository;
    //    private readonly IRepository<HealthRecord> _healthRecordRepository;
    //public MemberService(IRepository<Member> membersRepository, IRepository<HealthRecord> healthRecordRepository, IRepository<Membership> membershipRepository, IRepository<Plan> planRepository)
    //{
    //    _membersRepository = membersRepository;
    //    _healthRecordRepository = healthRecordRepository;
    //    _membershipRepository = membershipRepository;
    //    _planRepository = planRepository;
    //}

    // unit of work
    private readonly IUnitOfWork _unitOfWork;

    public MemberService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> AddMemberAsync(CreateMemberViewModel model)
    {
        try
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync();

            bool IsNotUniquePhone = members.Any(x => x.Phone == model.Phone);
            bool IsNotUniqueEmail = members.Any(x => x.Email == model.Email);

            if (IsNotUniqueEmail || IsNotUniquePhone)
                return false;

            var Member = new Member()
            {
                DateOfBirth = model.DateOfBirth,
                Email = model.Email,
                Gender = model.Gender,
                Phone = model.Phone,
                Name = model.Name,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    Street = model.Street,
                    City = model.City,
                },
                HealthRecord = new HealthRecord()
                {
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Height = model.HealthRecordViewModel.Height.ToString(),
                    Weight = model.HealthRecordViewModel.Weight.ToString(),
                    Note = model.HealthRecordViewModel.Note
                }
            };
            await _unitOfWork.GetRepository<Member>().AddAsync(Member);

            return await _unitOfWork.SaveChanges() > 0;

        }
        catch { return false; }

    }

    public async Task<bool> DeleteMemberAsync(int? Id)
    {
        try
        {
            if (Id == null || Id.Value == 0)
                return false;

            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(Id.Value);

            if (member == null) return false;

            await _unitOfWork.GetRepository<Member>().DeleteAsync(member);
            return await _unitOfWork.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }

    public async Task<HealthRecordViewModel?> GetHealthRecordDetailsAsync(int? Id)
    {
        var healthRecords = _unitOfWork.GetRepository<HealthRecord>().GetAll();

        if (healthRecords.Any())
        {
            var healthRecord = await healthRecords.Where(x => x.MemberId == Id).FirstOrDefaultAsync();

            if (healthRecord != null)
            {
                var model = new HealthRecordViewModel()
                {
                    BloodType = healthRecord.BloodType,
                    Height = decimal.Parse(healthRecord.Height), // error
                    Weight = decimal.Parse(healthRecord.Weight), // error
                    Note = healthRecord.Note
                };
                return model;
            }
        }
        return null;
    }

    public async Task<MemberDetailsViewModel> GetMemberAsync(int? Id)
    {
        if (Id == null || Id.Value == 0)
            return null;

        var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(Id.Value);

        if (member == null) return null;

        var model = new MemberDetailsViewModel()
        {
            Email = member.Email,
            Gender = member.Gender.ToString(),
            Name = member.Name,
            Phone = member.Phone,
            Photo = member.Photo,
            DateOfBirth = member.DateOfBirth.ToString("d/M/yyyy"),
            Address = $"{member.Address?.BuildingNumber} - {member.Address?.Street} - {member.Address?.City}",
            MembershipStartDate = member.Memberships?.Where(x => x.IsActive).ToList().Select(x => x.CreatedAt.ToString("d/M/yyyy")).FirstOrDefault(),
            MembershipEndDate = member.Memberships?.Where(x => x.IsActive).ToList().Select(x => x.EndDate.ToString("d/M/yyyy")).FirstOrDefault(),
            PlanName = member.Memberships?.Where(x => x.IsActive).ToList().Select(x => x.Plan.Name).FirstOrDefault()
        };

        //var memberships = _membershipRepository.GetAllAsync();

        //if(memberships!= null) // --> should be if(memberships.Any()) --> error as the async cannot have the any
        //{
        //    //var membershipActive = await memberships.Where // --> error 
        //}
        return model;
    }

    public async Task<IEnumerable<MemberViewModel>> GetMembersAsync()
    {
        var members = await _unitOfWork.GetRepository<Member>().GetAllAsync();

        return members.Select(x => new MemberViewModel()
        {
            Phone = x.Phone,
            Photo = x.Photo,
            Name = x.Name,
            Email = x.Email,
            Gender = x.Gender.ToString(),
            Id = x.Id,
        });
    }

    public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int? Id)
    {
        if (Id == null || Id.Value == 0)
            return null;

        var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(Id.Value);

        if (member == null) return null;

        var model = new MemberToUpdateViewModel()
        {
            Id = member.Id,
            Email = member.Email,
            Name = member.Name,
            Phone = member.Phone,
            Photo = member.Photo,
            BuildingNumber = member.Address?.BuildingNumber ?? 0,
            Street = member.Address?.Street ?? "Not Found",
            City = member.Address?.City ?? "Not Found",
        };
        return model;
    }

    public async Task<bool> UpdateMemberAsync(MemberToUpdateViewModel model)
    {
        try
        {
            if (model == null)
                return false;

            var members = _unitOfWork.GetRepository<Member>().GetAll();

            bool IsNotUniquePhone = members.Any(x => x.Phone == model.Phone && x.Id != model.Id);
            bool IsNotUniqueEmail = members.Any(x => x.Email == model.Email && x.Id != model.Id);

            if (IsNotUniqueEmail || IsNotUniquePhone)
                return false;

            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(model.Id);

            if (member == null) return false;

            model.Email = model.Email;
            model.Phone = model.Phone;
            model.BuildingNumber = model.BuildingNumber;
            model.Street = model.Street;
            model.City = model.City;

            await _unitOfWork.GetRepository<Member>().UpdateAsync(member);
            return await _unitOfWork.SaveChanges() > 0;
        }
        catch(Exception ex)
        {
            return false;
        }
    }
}
