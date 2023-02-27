using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            //using automapper
            var regions = await regionRepository.GetAllAsync();
            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionsDTO);

            ////return DTO regions(from database)

            //var regionsDTO = new List<Models.Domain.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new Models.Domain.Region()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        Area = region.Area,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population,
            //    };
            //    regionsDTO.Add(regionDTO);
            //});
            //return Ok(regionsDTO);





            //static data

            //var regions = new List<Region>()
            //{
            //    new Region
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Wellington",
            //        Code = "WLG",
            //        Area=226654,
            //        Lat = -6.4654,
            //        Long = 56.87,
            //        Population = 500000
            //    },
            //    new Region
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Auckland",
            //        Code = "AUCK",
            //        Area=456745,
            //        Lat = -62.4654,
            //        Long = 34.8787,
            //        Population = 700000
            //    }
            //};

        }
        //[HttpPost]
        //public async Task<IActionResult> GetAllRegions1()
        //{
        //    var regions = await regionRepository.GetAllAsync();
        //    var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);
        //    Console.WriteLine(regionsDTO);
        //    return Ok(regionsDTO);
        //}



        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);
            if(region == null)
            {
                return NotFound();
            }
            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //Request(DTO) to domain model 

            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population,
            };

            //pass details to repo


            region = await regionRepository.AddAsync(region);

            //Convert the data back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population,
            };

            return CreatedAtAction(nameof(GetRegionAsync),new {id = regionDTO.Id},regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //get the region from the database
            var region = await regionRepository.DeleteAsync(id);

            //handel if not found 
            if(region == null)
            {
                return NotFound();
            }
            //convert response back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population,
            };

            //return an OK response
            return Ok(regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute]Guid id,[FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            //Convert the DTO to domain model
            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Name = updateRegionRequest.Name,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Population = updateRegionRequest.Population,
            };
            //update region using repository
            region = await regionRepository.UpdateAsync(id, region);
            //if null then not found
            if(region == null)
            {
                return NotFound();
            }
            //convert domain back to TDO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population,
            };
            //return Ok response

            return Ok(regionDTO);
        }
    }
}
