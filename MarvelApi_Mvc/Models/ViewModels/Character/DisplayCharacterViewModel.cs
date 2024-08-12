﻿using MarvelApi_Mvc.Models.DTOs.CharacterDTOs;

namespace MarvelApi_Mvc.Models.ViewModels.Character
{
	public class DisplayCharacterViewModel
	{
		public CharacterDTO Character { get; set; }
		public List<CharacterDTO> Allies { get; set; }
		public List<CharacterDTO> Enemies { get; set; }
	}
}
