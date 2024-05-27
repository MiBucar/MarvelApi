using AutoMapper;
using MarvelApi_Api.Models;
using MarvelApi_Api.Models.DTOs.Character;

namespace MarvelApi_Api
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<CharacterCreateDTO, Character>()
            .ForMember(dest => dest.Enemies, opt => opt.Ignore())
            .ForMember(dest => dest.Allies, opt => opt.Ignore());

            CreateMap<CharacterUpdateDTO, Character>().ReverseMap();

            CreateMap<Character, CharacterDTO>()
            .ForMember(dest => dest.Enemies, opt => opt.MapFrom(src => src.Enemies.Select(e => e.Name)))
            .ForMember(dest => dest.Allies, opt => opt.MapFrom(src => src.Allies.Select(a => a.Name)));

        CreateMap<CharacterDTO, Character>()
            .ForMember(dest => dest.Enemies, opt => opt.Ignore())
            .ForMember(dest => dest.Allies, opt => opt.Ignore());
        }
    }
}