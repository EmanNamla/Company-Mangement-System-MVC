using Company.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Interfaces
{
    public interface IEmployeeRepository:IGenericRepository<Employee>
    {
        IQueryable<Employee>GetEmployeeByName(string name);

        Task<bool> AnyAsync(Expression<Func<Employee, bool>> predicate);
    }
}
