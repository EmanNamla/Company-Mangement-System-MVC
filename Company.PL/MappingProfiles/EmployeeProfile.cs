using AutoMapper;
using Company.DAL.Models;
using Company.PL.ViewModels;

namespace Company.PL.MappingProfiles
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
        }
    }
}
