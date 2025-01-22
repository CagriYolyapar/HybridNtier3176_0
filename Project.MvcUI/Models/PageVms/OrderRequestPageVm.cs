using Project.Entities.Models;
using Project.MvcUI.Models.PaymentAPITools;

namespace Project.MvcUI.Models.PageVms
{
    public class OrderRequestPageVm
    {
        public Order Order { get; set; }
        public PaymentRequestModel PaymentRequestModel { get; set; }
    }
}
