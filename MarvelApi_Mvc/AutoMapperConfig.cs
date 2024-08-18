using AutoMapper;
using MarvelApi_Mvc.Models.DTOs.CharacterDTOs;
using MarvelApi_Mvc.Models.DTOs.TeamDTOs;

namespace MarvelApi_Mvc
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<CharacterUpdateDTO, CharacterDTO>().ReverseMap();
            CreateMap<TeamUpdateDTO, TeamDTO>().ReverseMap();

            CreateMap<CharacterDTO, CharacterUpdateDTO>()
            .ForMember(dest => dest.Team, opt => opt.Ignore());
        }
    }
}
