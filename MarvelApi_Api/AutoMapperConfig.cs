using AutoMapper;
using MarvelApi_Api.Models;
using MarvelApi_Api.Models.DTOs.Character;

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
                .Select(cr => cr.RelatedCharacterId)));
        }
    }
}