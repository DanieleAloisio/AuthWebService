using AuthWebService.Models;
using AutoMapper;


namespace AuthWebService.Dto
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<User, UserDto>();
            CreateMap<Role, RoleDto>();
        }
    }
}
