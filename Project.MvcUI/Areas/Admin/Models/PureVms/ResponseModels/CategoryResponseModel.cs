using System.ComponentModel.DataAnnotations;

namespace Project.MvcUI.Areas.Admin.Models.PureVms.ResponseModels
{
    public class CategoryResponseModel
    {
        public int Id { get; set; }

        [Display(Name ="Kategori ismi")]
        [Required(ErrorMessage = "{0} gerekli alandır")]
        public string CategoryName { get; set; }
        public string Description { get; set; }


    }
}
