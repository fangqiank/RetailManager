using System.Threading.Tasks;
using RMWPFUi.Library.Models;

namespace RMWPFUi.Library.Api
{
    public interface ISaleEndPoint
    {
        Task PostSale(SaleModel sale);
    }
}