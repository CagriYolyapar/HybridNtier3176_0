using Project.Entities.Models;

namespace Project.MvcUI.Areas.Admin.Models.PageVms
{
    public class CreateProductPageVm
    {
        //Domain Entity kullanmayın!
        public Product Product { get; set; }

        public List<Category> Categories { get; set; }
    }
}
