using AutoMapper;
using Company.DAL.Models;
using Company.PL.ViewModels;

namespace Company.PL.MappingProfiles
{
    public class DepartmentProfiles:Profile
    {
        public DepartmentProfiles()
        {
                CreateMap<DepartmentViewModel,Department>().ReverseMap();
        }
    }
}
