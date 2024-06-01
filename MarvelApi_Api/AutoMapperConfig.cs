using AutoMapper;
using MarvelApi_Api.Models;
using MarvelApi_Api.Models.DTOs.CharacterDTOS;
using MarvelApi_Api.Models.DTOs.Team;

namespace MarvelApi_Api
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<CharacterCreateDTO, Character>().ReverseMap();
            CreateMap<CharacterUpdateDTO, Character>().ReverseMap();

            CreateMap<CharacterUpdateDTO, Character>()
            .ForMember(dest => dest.CharacterRelationships, opt => opt.Ignore());

            CreateMap<Character, CharacterDTO>()
            .ForMember(dest => dest.EnemyIds, opt => opt.MapFrom(src => src.CharacterRelationships
                .Where(cr => cr.IsEnemy)
                .Select(cr => cr.RelatedCharacterId)))
            .ForMember(dest => dest.AllyIds, opt => opt.MapFrom(src => src.CharacterRelationships
                .Where(cr => !cr.IsEnemy)
                .Select(cr => cr.RelatedCharacterId)))
            .ForMember(dest => dest.Team, opt => opt.MapFrom(src => src.Team.Name));

                CreateMap<Team, TeamDTO>()
                .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Members.Select(x => x.Name)));

                CreateMap<TeamCreateDTO, Team>()
                .ForMember(dest => dest.Members, opt => opt.Ignore());

                CreateMap<TeamUpdateDTO, Team>()
                .ForMember(dest => dest.Members, opt => opt.Ignore());
        }
    }
}