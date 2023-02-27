using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;
        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyAsync")]
        public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.GetAsync(id);
            if(walkDifficulty == null)
            {
                return NotFound();
            }
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultiesAsync()
        {
            var walkDifficulty = await walkDifficultyRepository.GetAllAsync();
            var walkDifficultyDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }


        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            //DTO to domain
            var walkDifficulty = new Models.Domain.WalkDifficulty()
            {
                Code = addWalkDifficultyRequest.Code,
            };

            //call repository
            walkDifficulty = await walkDifficultyRepository.AddAsync(walkDifficulty);

            //convert domain to DTO

            var walkDifficultyDTO = new Models.DTO.WalkDifficulty()
            {
                Id = walkDifficulty.Id,
                Code = walkDifficulty.Code,
            };

            //return response
            return CreatedAtAction(nameof(GetWalkDifficultyAsync), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute]Guid id,[FromBody]UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            //convert DTO to Domain
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty()
            {
                Code = updateWalkDifficultyRequest.Code,
            };

            //call repo to update
            walkDifficultyDomain = await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomain);

            //null check
            if(walkDifficultyDomain==null)
            {
                return NotFound();
            }
            var walkDifficultyDTO = new Models.DTO.WalkDifficulty()
            {
                Id = walkDifficultyDomain.Id,
                Code = walkDifficultyDomain.Code,
            };

            return Ok(walkDifficultyDTO);


        }

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteWalkDifficulty(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.DeleteAsync(id);
            if( walkDifficulty==null)
            {
                return NotFound();
            }
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);
            return Ok(walkDifficultyDTO);
        }
    }
}
