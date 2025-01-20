using Project.Bll.DtoClasses;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Bll.Managers.Abstracts
{
    public interface IProductManager : IManager<ProductDto,Product>
    {
        //Todo : !Sadece test amaclı yazılan metotlardır...Bir Manager size hicbir zaman Domain Entity döndürmez...Burada ek refactorler size bırakılmıstır
        Task<List<Product>> GetDomainProducts();
        List<Product> GetDomainProductsByCategoryId(int categoryId);
    }
}
