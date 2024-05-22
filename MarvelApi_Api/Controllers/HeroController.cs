using System.Net;
using AutoMapper;
using MarvelApi_Api.ActionFilters.Hero;
using MarvelApi_Api.Models;
using MarvelApi_Api.Models.DTOs;
using MarvelApi_Api.Models.DTOs.Hero;
using MarvelApi_Api.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MarvelApi_Api.Controllers
{
    [Controller]
    [Route("controller")]
    public class HeroController : ControllerBase
    {
        private readonly IHeroRepository _heroRepository;
        private readonly IMapper _autoMapper;

        public HeroController(IHeroRepository heroRepository, IMapper autoMapper)
        {
            _heroRepository = heroRepository;
            _autoMapper = autoMapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetHeroes(){
            var heroes = await _heroRepository.GetAllAsync();
            return Ok(heroes);
        }

        [HttpGet("{id:int}")]
        [ServiceFilter(typeof(ValidateHeroExistsAttribute))]
        public async Task<IActionResult> GetHero(int id){
            var hero = await _heroRepository.GetAsync(x => x.Id == id);

            var response = new ApiResponse{
                Result = hero,
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHero([FromBody] HeroCreateDTO hero){
            var mappedHero = _autoMapper.Map<HeroCreateDTO, Hero>(hero);
            await _heroRepository.CreateAsync(mappedHero);
            return CreatedAtAction(nameof(GetHero),new {id = mappedHero.Id}, mappedHero);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateHero(int id, [FromBody] HeroUpdateDTO hero){
            var mappedHero = _autoMapper.Map<HeroUpdateDTO, Hero>(hero);
            await _heroRepository.UpdateAsync(mappedHero);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteHero(int id){
            var hero = await _heroRepository.GetAsync(x => x.Id == id);
            await _heroRepository.DeleteAsync(id);
            return Ok(hero);
        }
    }
}