using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.BogusHandling
{
    public static class UserAndRoleSeed
    {
        public static void SeedUsersAndRoles(ModelBuilder modelBuilder)
        {
            IdentityRole<int> appRole = new()
            {
                Id = 1,
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString() //bu ifade sisteminizin yeni bir Guid yaratmasını saglar
            };

            modelBuilder.Entity<IdentityRole<int>>().HasData(appRole);

            //Normalde şifreleme işlemleri gibi yapılar Identity kütüphanesi sayesinde otomatik gelmektedir..Ama biz bu seviyede hardcoded veriler ekledigimiz icin o kütüphane aslında calısmayacaktır...

            PasswordHasher<AppUser> passwordHasher = new PasswordHasher<AppUser>();

            AppUser appUser = new()
            {
                Id = 1,
                UserName = "cgr123",
                Email ="cagri@email.com",
                NormalizedEmail = "CAGRI@EMAIL.COM",
                NormalizedUserName = "CGR123",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = passwordHasher.HashPassword(null,"cgr123")
            };

            modelBuilder.Entity<AppUser>().HasData(appUser);

            IdentityUserRole<int> appUserRole = new()
            {
                RoleId = 1,
                UserId = 1
            };

            modelBuilder.Entity<IdentityUserRole<int>>().HasData(appUserRole);
        }
    }
}
