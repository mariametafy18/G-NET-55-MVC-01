using AutoMapper;
using GymManagementSystem.BLL.Common;
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
    // auto mapper
    private readonly IMapper mapper;

    public MemberService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<Result> AddMemberAsync(CreateMemberViewModel model)
    {
        try
        {
            var members =  _unitOfWork.GetRepository<Member>().GetAllAsync();

            bool IsNotUniquePhone = members.Any(x => x.Phone == model.Phone);
            bool IsNotUniqueEmail = members.Any(x => x.Email == model.Email);

            if (IsNotUniqueEmail)
                return Result.Fail("Email is already used");
            if (IsNotUniquePhone)
                return Result.Fail("Phone is already used");


            #region Comments
            //var Member = new Member()
            //{
            //    DateOfBirth = model.DateOfBirth,
            //    Email = model.Email,
            //    Gender = model.Gender,
            //    Phone = model.Phone,
            //    Name = model.Name,
            //    Address = new Address()
            //    {
            //        BuildingNumber = model.BuildingNumber,
            //        Street = model.Street,
            //        City = model.City,
            //    },
            //    HealthRecord = new HealthRecord()
            //    {
            //        BloodType = model.HealthRecordViewModel.BloodType,
            //        Height = model.HealthRecordViewModel.Height.ToString(),
            //        Weight = model.HealthRecordViewModel.Weight.ToString(),
            //        Note = model.HealthRecordViewModel.Note
            //    }
            //}; 
            #endregion

            var Member = mapper.Map<Member>(model);

            await _unitOfWork.GetRepository<Member>().AddAsync(Member);

            var result = await _unitOfWork.SaveChanges() > 0;

            if (result) return Result.OK();

            return Result.Fail("Member ia not Added");
        }
        catch { return Result.Fail("Internal Server Error"); }

    }

    public async Task<Result> DeleteMemberAsync(int? Id)
    {
        try
        {
            if (Id == null || Id.Value == 0)
                return Result.Fail("Member is not found");

            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(Id.Value);

            if (member == null) return Result.Fail("model is not found");

            await _unitOfWork.GetRepository<Member>().DeleteAsync(member);
            var result = await _unitOfWork.SaveChanges() > 0;

            if (result) return Result.OK();

            return Result.Fail("Member cannot be deleted");
        }
        catch
        {
            return Result.Fail("Internal Server Error");
        }
    }

    public async Task<Result<HealthRecordViewModel?>> GetHealthRecordDetailsAsync(int? Id)
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
                return Result<HealthRecordViewModel?>.OK(model);
            }
        }
        return Result<HealthRecordViewModel?>.Fail("Healty Record is not found"); ;
        ;
    }

    public async Task<Result<MemberDetailsViewModel?>> GetMemberAsync(int? Id)
    {
        if (Id == null || Id.Value == 0)
            return Result<MemberDetailsViewModel?>.Fail("Id is not found");

        var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(Id.Value);

        if (member == null) return Result<MemberDetailsViewModel?>.Fail("Member is not found"); ;

        #region Comments
        //var model = new MemberDetailsViewModel()
        //{
        //    Email = member.Email,
        //    Gender = member.Gender.ToString(),
        //    Name = member.Name,
        //    Phone = member.Phone,
        //    Photo = member.Photo,
        //    DateOfBirth = member.DateOfBirth.ToString("d/M/yyyy"),
        //    Address = $"{member.Address?.BuildingNumber} - {member.Address?.Street} - {member.Address?.City}",
        //    MembershipStartDate = member.Memberships?.Where(x => x.IsActive).ToList().Select(x => x.CreatedAt.ToString("d/M/yyyy")).FirstOrDefault(),
        //    MembershipEndDate = member.Memberships?.Where(x => x.IsActive).ToList().Select(x => x.EndDate.ToString("d/M/yyyy")).FirstOrDefault(),
        //    PlanName = member.Memberships?.Where(x => x.IsActive).ToList().Select(x => x.Plan.Name).FirstOrDefault()
        //}; 
        #endregion

        var model = mapper.Map<MemberDetailsViewModel>(member);
        //var model = mapper.Map<Member, MemberDetailsViewModel>(member);

        var memberships = _unitOfWork.GetRepository<Membership>().GetAll().Where(x => x.MemberId == Id).ToList();

        if (memberships != null)
        {
            var membershipActive = memberships.Where(x => x.IsActive).FirstOrDefault();
            if (membershipActive != null)
            {
                model.MembershipStartDate = membershipActive.CreatedAt.ToString("d/M/yyyy");
                model.MembershipEndDate = membershipActive.EndDate.ToString("d/M/yyyy");

                var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(membershipActive.PlanId);

                if (plan != null)
                    model.PlanName = plan.Name;
            }
        }
        return Result<MemberDetailsViewModel?>.OK(model);
    }

    public Result<IEnumerable<MemberViewModel>> GetMembersAsync()
    {
        var members = _unitOfWork.GetRepository<Member>().GetAllAsync();

        #region Comments
        //return members.Select(x => new MemberViewModel()
        //{
        //    Phone = x.Phone,
        //    Photo = x.Photo,
        //    Name = x.Name,
        //    Email = x.Email,
        //    Gender = x.Gender.ToString(),
        //    Id = x.Id,
        //}); 
        #endregion

        return Result<IEnumerable<MemberViewModel>>.OK(mapper.Map<IEnumerable<MemberViewModel>>(members));
    }

    public async Task<Result<MemberToUpdateViewModel?>> GetMemberToUpdateAsync(int? Id)
    {
        if (Id == null || Id.Value == 0)
            return Result<MemberToUpdateViewModel?>.Fail("Id is not found");

        var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(Id.Value);

        if (member == null)
            return Result<MemberToUpdateViewModel?>.Fail("Member is not found");

        #region Comments
        //var model = new MemberToUpdateViewModel()
        //{
        //    Id = member.Id,
        //    Email = member.Email,
        //    Name = member.Name,
        //    Phone = member.Phone,
        //    Photo = member.Photo,
        //    BuildingNumber = member.Address?.BuildingNumber ?? 0,
        //    Street = member.Address?.Street ?? "Not Found",
        //    City = member.Address?.City ?? "Not Found",
        //}; 
        #endregion

        var model = mapper.Map<MemberToUpdateViewModel>(member);
        return Result<MemberToUpdateViewModel?>.OK(model);
        ;
    }

    public async Task<Result> UpdateMemberAsync(MemberToUpdateViewModel model)
    {
        try
        {
            if (model == null)
                return Result.Fail("model is not found");


            var members = _unitOfWork.GetRepository<Member>().GetAll();

            bool IsNotUniquePhone = members.Any(x => x.Phone == model.Phone && x.Id != model.Id);
            bool IsNotUniqueEmail = members.Any(x => x.Email == model.Email && x.Id != model.Id);


            if (IsNotUniqueEmail)
                return Result.Fail("Email is already used");
            if (IsNotUniquePhone)
                return Result.Fail("Phone is already used");

            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(model.Id);

            if (member == null) return Result.Fail("Member is not found");


            #region Comments
            //model.Email = model.Email;
            //model.Phone = model.Phone;
            //model.BuildingNumber = model.BuildingNumber;
            //model.Street = model.Street;
            //model.City = model.City; 
            #endregion

            var mappedMember = mapper.Map(model, member);

            await _unitOfWork.GetRepository<Member>().UpdateAsync(mappedMember);
            var result = await _unitOfWork.SaveChanges() > 0;

            if (result)
                return Result.OK();

            return Result.Fail("Member cannot be updated");
        }
        catch (Exception ex)
        {
            return Result.Fail("Internal Server Error");
        }
    }
}
