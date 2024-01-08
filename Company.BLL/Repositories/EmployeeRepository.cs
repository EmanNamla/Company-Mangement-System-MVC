using Company.BLL.Interfaces;
using Company.DAL.Contexts;
using Company.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repositories
{
    public class EmployeeRepository:GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly CompanyDbContext dbContext;

        public EmployeeRepository(CompanyDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> AnyAsync(Expression<Func<Employee, bool>> predicate)
        {
            return await dbContext.Employees.AnyAsync(predicate); 
        }

        public IQueryable<Employee> GetEmployeeByName(string name)
        {
            return  dbContext.Employees.Where(e => e.Name.ToLower().Contains(name.ToLower()));
        }
    }
}
