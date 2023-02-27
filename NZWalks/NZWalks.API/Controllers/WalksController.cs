using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            //fetch data from database
            var walksDomain = await walkRepository.GetAllAsync();
            //Convert domain Walks to DTO walks
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);
            //return response
            return Ok(walksDTO);
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //get domain object from database
            var walkDomain = await walkRepository.GetAsync(id);
            //Check for NULL
            if(walkDomain == null)
            {
                return NotFound();
            }
            //Convert to DTO object
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            //return Response
            return Ok(walkDTO);

        }
        [HttpPost]
        

        public async Task<IActionResult> AddWalkAsync([FromBody]Models.DTO.AddWalkRequest addWalkRequest)
        {
            //Convert the DTO object to domain object
            var walkDomain = new Models.Domain.Walk()
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId,
            };
            //pass domain object to repository to save into the database
            walkDomain = await walkRepository.AddAsync(walkDomain);
            //convert the domain object to DTO
            var walkDTO = new Models.DTO.Walk()
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId,
            };
            //send OK response
            return CreatedAtAction(nameof(GetWalkAsync), new {id = walkDTO.Id},walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]

        public async Task<IActionResult> UpdateWalkAsync([FromRoute]Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //DTO to domain object
            var walkDomain = new Models.Domain.Walk()
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId,
            };

            //pass details to repository
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);
            //handle NULL
            if(walkDomain == null)
            {
                return NotFound();
            }
            //convert back to DTO
            var walkDTO = new Models.DTO.Walk()
            {
                Id = walkDomain.Id,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                Length = walkDomain.Length,
                WalkDifficultyId = walkDomain.WalkDifficultyId,
            };
            //return response
            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync([FromRoute]Guid id)
        {
            //call repository to delete walk
            var walkDomain = await walkRepository.DeleteAsync(id);
            //handle NULL
            if(walkDomain == null)
            {
                return NotFound();
            }

            //convert Domain object to DTO object
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            //return Response
            return Ok(walkDTO);

        }
    }

}
