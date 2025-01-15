using Project.Bll.DtoClasses;
using Project.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Bll.Managers.Abstracts
{
    public interface IManager<T,U> where T:BaseDto where U:class,IEntity
    {
        //Business Logic For Queries

        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        List<T> GetActives();
        List<T> GetPassives();
        List<T> GetModifieds();

        //Business Logic for Commands
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task<string> RemoveAsync(T entity);
        Task MakePassiveAsync(T entity);

        Task CreateRangeAsync(List<T> list);
        Task UpdateRangeAsync(List<T> list);
        Task<string> RemoveRangeAsync(List<T> list);
    }
}
