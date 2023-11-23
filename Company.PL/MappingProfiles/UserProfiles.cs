using AutoMapper;
using Company.DAL.Models;
using Company.PL.ViewModels;

namespace Company.PL.MappingProfiles
{
    public class UserProfiles:Profile
    {
        public UserProfiles()
        {
            CreateMap<ApplicationUser, UserViewModel>();
        }
    }
}
