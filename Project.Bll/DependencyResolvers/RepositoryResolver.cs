using Microsoft.Extensions.DependencyInjection;
using Project.Dal.Repositories.Abstracts;
using Project.Dal.Repositories.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Bll.DependencyResolvers
{
    public static class RepositoryResolver
    {
        public static void AddRepositoryService(this IServiceCollection services)
        {
            //Siz bir metodun parametresine bir tip verdiginiz zaman özellikle bir tipin karsılıgında programın nasıl bir tepki verecegini söylemek icin yazdıgınız Matematik'e IOC Container'a tip eklemek denir...

            //Mesela siz ICategoryRepository tipinden bahsedildigi zaman programın CategoryRepository instance'i ile tepki vermesini istiyorsanız burada teknik olarak sunu demeniz gerekir "IOC container'a ICategorRepository istedigimde CategoryRepository vermesi icin bir ekleme yaptım /tanımlama yaptım dersiniz...

            //IOC container'a tip tanımlaması yapmanızım 3 yolu vardır...Servis'e bunu ya AddTransient ya AddScoped ya da AddSingleton olarak eklersiniz...


            /*
             * 
             * AddSingleton : Sadece bir instance alınarak tüm program boyunca o instance kullanılır
             * 
             * AddScoped : Bir request icerisinde aynı tiplerde farklı tanımlamalar varsa  o spesifik request icin o tipten sadece 1 instance alınır
             * 
             * AddTransient : Tip kac kez tanımlandıysa her biri icin ayrı instance alınır
             
             public Constructor(ICategoryRepository categoryRepository,ICategoryRepository categoryRepository2)
            {


            }
             
             
             
             
             
             
             
             
             
             */

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<IAppUserProfileRepository, AppUserProfileRepository>();


        }
    }
}
