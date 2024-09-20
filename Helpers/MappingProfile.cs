using AutoMapper;
using devC_Jwt.Models;

namespace devC_Jwt.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser,AuthenModel>().ReverseMap();
        }
    }
}
