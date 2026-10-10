using GymManagementSystem.BLL.Services.Members.Interfaces;
using GymManagementSystem.BLL.Services.Members.ViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.Members.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IRepository<Member> _membersRepository;
        public MemberService(IRepository<Member> membersRepository)
        {
            _membersRepository = membersRepository;
        }

        public async Task<bool> AddMemberAsync(CreateMemberViewModel model)
        {
            try
            {
                var members = await _membersRepository.GetAllAsync();

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
                return await _membersRepository.AddAsync(Member) > 0;
            }
            catch { return false; }

        }

        public async Task<IEnumerable<MemberViewModel>> GetMembersAsync()
        {
            var members = await _membersRepository.GetAllAsync();

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
    }
}
