using Microsoft.EntityFrameworkCore;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Bogus.DataSets;

namespace Project.Dal.BogusHandling
{
    public static class CategoryDataSeed
    {
        public static void SeedCategories(ModelBuilder modelBuilder)
        {
            //Bu seviyede Dal katmanında oldugumuz icin rahatca Domain Entity ile calısabilirsinzi
            List<Category> categories = new();

            //Eger saf durumda Database'e hardcoded bir veri eklemesi yapacaksanız Identity sistemi devreye girmez.... O  yüzden Id'lerin degerini elle vermek zorunda kalırsınız

            for (int i = 1; i < 11; i++)
            {
                Category c = new()
                {
                    Id = i,
                    CategoryName = new Commerce("tr").Categories(1)[0],
                    Description = new Lorem("tr").Sentence(10),
                    CreatedDate = DateTime.Now,
                    Status = Entities.Enums.DataStatus.Inserted
                };

                categories.Add(c);
            }

            modelBuilder.Entity<Category>().HasData(categories);

        }
    }
}
