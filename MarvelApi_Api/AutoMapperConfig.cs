using AutoMapper;
using MarvelApi_Api.Models;
using MarvelApi_Api.Models.DTOs.Hero;

namespace MarvelApi_Api
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<HeroCreateDTO, Hero>().ReverseMap();
            CreateMap<HeroUpdateDTO, Hero>().ReverseMap();
        }
    }
}