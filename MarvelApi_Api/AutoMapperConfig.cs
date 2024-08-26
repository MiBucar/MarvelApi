using AutoMapper;
using Azure.Core;
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
            .ForMember(dest => dest.CharacterRelationships, opt => opt.Ignore())
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => ConvertToByteArray(src.Image)))
            .ForMember(dest => dest.ImageType, opt => opt.MapFrom(src => src.Image.ContentType));

			CreateMap<CharacterCreateDTO, Character>()
			.ForMember(dest => dest.Image, opt => opt.MapFrom(src => ConvertToByteArray(src.Image)))
			.ForMember(dest => dest.ImageType, opt => opt.MapFrom(src => src.Image.ContentType));

			CreateMap<Character, CharacterDTO>()
            .ForMember(dest => dest.EnemyIds, opt => opt.MapFrom(src => src.CharacterRelationships
                .Where(cr => cr.IsEnemy)
                .Select(cr => cr.RelatedCharacterId)))
            .ForMember(dest => dest.AllyIds, opt => opt.MapFrom(src => src.CharacterRelationships
                .Where(cr => !cr.IsEnemy)
                .Select(cr => cr.RelatedCharacterId)))
            .ForMember(dest => dest.Team, opt => opt.MapFrom(src => src.Team.Name))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => $"https://localhost:7228/api/character/GetImage/{src.Id}"));

                CreateMap<Team, TeamDTO>()
                .ForMember(dest => dest.MembersIds, opt => opt.MapFrom(src => src.Members.Select(x => x.Id)));

                CreateMap<TeamCreateDTO, Team>()
                .ForMember(dest => dest.Members, opt => opt.Ignore());

                CreateMap<TeamUpdateDTO, Team>()
                .ForMember(dest => dest.Members, opt => opt.Ignore());
        }

		private byte[] ConvertToByteArray(IFormFile formFile)
		{
			if (formFile == null)
				return null;

			using (var ms = new MemoryStream())
			{
				formFile.CopyTo(ms);
				return ms.ToArray();
			}
		}
	}
}