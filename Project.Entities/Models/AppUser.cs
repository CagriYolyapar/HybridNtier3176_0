using Microsoft.AspNetCore.Identity;
using Project.Entities.Enums;
using Project.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class AppUser : IdentityUser<int>, IEntity
    {
        //Guid bilgisayarınızın mac adresi ip adresi ve sistem tarihini alıp bunları bir takım algoritmalarla hashleyip size bir kod olusturan bir tiptir...
        public Guid ActivationCode { get; set; } //Her kullanıcım icin sadece ona özgü unique bir kod olusturmak isterim ki onun Email'ine o kodu gönderebileyim. Böylece ben onun Email'ine gidip oradaki linke tıkladıgını o kodu bana geri ulastırdıgında anlarım...
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DataStatus Status { get; set; }

        //Relational properties
        public virtual AppUserProfile AppUserProfile { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

    }
}
