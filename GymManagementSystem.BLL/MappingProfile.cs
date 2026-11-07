using AutoMapper;
using GymManagementSystem.BLL.Services.Members.ViewModels;
using GymManagementSystem.BLL.Services.Sessions.ViewModels;
using GymManagementSystem.DAL.Models;

namespace GymManagementSystem.BLL;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        MemberProfile();
        SessionProfile();
    }

    private void MemberProfile()
    {
        CreateMap<Member, MemberDetailsViewModel>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString("d / M / yyyy")));

        CreateMap<Member, MemberViewModel>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()));


        CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();
        //CreateMap<HealthRecord, HealthRecordViewModel>();

        CreateMap<Member, MemberToUpdateViewModel>()
            .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street));

        CreateMap<MemberToUpdateViewModel, Member>()
            .ForMember(dest => dest.Name, opt => opt.Ignore())
            .ForMember(dest => dest.Photo, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.Address.City = src.City;
                dest.Address.Street = src.Street;
                dest.Address.BuildingNumber = src.BuildingNumber;
            });


        CreateMap<CreateMemberViewModel, Member>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address()
            {
                BuildingNumber = src.BuildingNumber,
                City = src.City,
                Street = src.Street,
            }))
            .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));
    }
    private void SessionProfile()
    {
        CreateMap<Session, SessionViewModel>()
            .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));

        CreateMap<Trainer, TrainerSelectedViewModel>();
        CreateMap<Category, CategorySelectedViewModel>();
        CreateMap<CreateSessionViewModel, Session>();
    }

}
