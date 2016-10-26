using BabyStore.DAL;

namespace BabyStore.Models
{
    public class Basket
    {
        public string BasketId { get; set; }
        public const string BasketSessionKey = "BasketId";
        private StoreContext db = new StoreContext();
    }
}