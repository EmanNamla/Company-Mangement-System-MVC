using Company.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Interfaces
{
    public interface IGenericRepository<T>
    {
        Task AddAsyn(T item);
        void Delete(T item);
        Task< T> GetAsyn(int id);

        Task< IEnumerable<T>> GetAllAsyn();

        void Update(T item);
    }
}
