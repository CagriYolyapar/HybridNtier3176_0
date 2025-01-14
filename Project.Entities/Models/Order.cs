using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class Order : BaseEntity
    {
        public string ShippingAddress { get; set; } //Kullanıcının siparişi talep ettigi (siparişin gönderilecegi) adres
        public int? AppUserId { get; set; } //null gecildiyse anlayın ki kullanıcı üye degildir
        public string? Email { get; set; } //Üye olmayan bir kullanıcının (AppUserId'si bos olan) Email'i özel olarak burada tutulur...Burada söyle bir logic vardır : EMail bossa AppUserId doludur (kullanıcı üyedir)...AppUserId bossa Email doludur(kullanıcı üye degildir biz o Email'i Order'da tanıyarak kullanıcı ile o Email üzerinden haberlesmeye geceriz)
        public string? NameDescription { get; set; } //Üye olmayan bir kullanıcının isim acılkaması burada tutulur(Null gecildiyse anlayın ki üye olarak alısveriş yapmıstır)
        public decimal Price { get; set; }

        //Relational Properties
        public virtual AppUser AppUser { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }



    }
}
