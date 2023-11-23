using Company.BLL.Interfaces;
using Company.DAL.Contexts;
using Company.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly CompanyDbContext dbContext;

        public GenericRepository(CompanyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddAsyn(T item)
        {
           await dbContext.AddAsync(item);
           
        }

        public void Delete(T item)
        {
            dbContext.Remove(item);
      
        }

        public async Task< T> GetAsyn(int id)
        => await dbContext.Set<T>().FindAsync(id);

        public async Task <IEnumerable<T>> GetAllAsyn()
        {
            if(typeof(T)==typeof(Employee))
            {
              return  (IEnumerable<T>) await  dbContext.Employees.Include(e => e.department).ToListAsync();
            }
            return await dbContext.Set<T>().ToListAsync();
        }
      

        public void Update(T item)
        {
            dbContext.Update(item);
           
        }
    }
}
