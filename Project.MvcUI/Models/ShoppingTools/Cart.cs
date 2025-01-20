using Newtonsoft.Json;

namespace Project.MvcUI.Models.ShoppingTools
{
    [Serializable]
    public class Cart
    {
        [JsonProperty("_myCart")]
        readonly Dictionary<int,CartItem> _myCart;

        public Cart()
        {
            _myCart = new Dictionary<int,CartItem>();

            //_myCart[1] normal şartlarda index parantezleri ilgili index'teki elemanı secme ifadesini söyler...Fakat bir Dictionary koleksiyonu söz konusu oldugunda bu index parantezi ilgili key'e sahip anahtarı sec demektir...
        }

        [JsonProperty("GetCartItems")]
        public List<CartItem> GetCartItems
        {
            get
            {
                return _myCart.Values.ToList();
            }
        }

        public void AddToCart(CartItem item)
        {
            if (_myCart.ContainsKey(item.Id)) 
            {
                _myCart[item.Id].Amount++;
                return;
            }
            _myCart.Add(item.Id, item);
        }

        public void Decrease(int id)
        {
            _myCart[id].Amount--;
            if (_myCart[id].Amount == 0) _myCart.Remove(id);
        }

        //Odev: Adedini Update ettirin

        public void RemoveFromCart(int id)
        {
            _myCart.Remove(id);
        }

        [JsonProperty("TotalPrice")]
        public decimal TotalPrice
        {
            get
            {
                return _myCart.Values.Sum(x => x.SubTotal);
            }
        }
    }
}
