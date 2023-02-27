using AutoMapper;
using static Azure.Core.HttpHeader;
using System.Threading.Channels;

namespace NZWalks.API.Profiles
{
    public class RegionsProfile:Profile
    {
        // why we use DTO

        //By using DTOs, you can define a common language for communication between layers and
        //make it easier to change the domain objects without affecting other layers.For example,
        //you may want to change the structure of your domain objects without affecting
        //the presentation layer. With DTOs, you can make these changes without affecting
        //the presentation layer, as long as the DTO structure remains the same.
        public RegionsProfile()
    {
        CreateMap<Models.Domain.Region, Models.DTO.Region>()
        .ReverseMap();
        //.ForMember(dest => dest.Id,options => options.MapFrom(src => src.Id));   
        }
    }
}
