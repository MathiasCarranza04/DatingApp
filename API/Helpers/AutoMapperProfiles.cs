using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //mapeo con origen AppUser al MemberDto

            CreateMap<AppUser, MemberDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom
             (src => src.Photos.FirstOrDefault(x => x.IsMain).Url)) //lleno el valor de photourl de member dto buscando la foto en entity appuser

             .ForMember(dest => dest.Age, opt => opt.MapFrom
             (src => src.DateOfBirth.CalculateAge()));

            CreateMap<Photo, PhotoDto>(); //mapeo la entity Photo con el dto 
            CreateMap<MemberUpdateDto, AppUser>();
        }

    }
}