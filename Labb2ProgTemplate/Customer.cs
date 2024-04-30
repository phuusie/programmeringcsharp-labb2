using System.Text;

namespace Labb2ProgTemplate
{
    public class Customer
    {
        public string Name { get; private set; }

        private string Password { get; set; }

        private List<Product> _cart;

        public List<Product> Cart
        {
            get { return _cart; }
        }

        public Customer(string name, string password)
        {
            Name = name;
            Password = password;
            _cart = new List<Product>();

        }

        public override string ToString()
        {

            return $"Welcome {Name.ToUpper()}!\nPassword: {Password}\n\n\u001b[4mCART\u001b[0m" +
                   $"\n{CustomerCurrentProducts("SEK", 1)}\nTotal:\t\t\t{CartTotal()}SEK";
        }

        public bool CheckPassword(string password)
        {
            return Password == password;
        }

        public void AddToCart(Product product)
        {
            Console.WriteLine($"\nHow many {product.Name}s do you want?");
            string input = Console.ReadLine();
            int quantity;

            if (int.TryParse(input, out quantity) && quantity >= 0)
            {
                for (int i = 0; i < quantity; i++)
                {
                    _cart.Add(product);
                }

                if (quantity == 1)
                {
                    Console.WriteLine($"{quantity} x {product.Name} has been added to your cart.");
                }
                else
                {
                    Console.WriteLine($"{quantity} x {product.Name}s have been added to your cart.");
                }

                Thread.Sleep(1000);
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid quantity.");
            }
            
        }

        public void RemoveFromCart(Product product)
        {
            _cart.Remove(product);
        }

        public double CartTotal()
        {
            double total = 0.0;
            
            foreach (var item in _cart)
            {
                total += item.Price;
            }

            double roundUp = Math.Round(total, 2, MidpointRounding.AwayFromZero);

            return roundUp;
        }

        public string CustomerCurrentProducts(string currency, double currencyChange)
        {
            Dictionary<string, int> productQuantities = new Dictionary<string, int>();
            Dictionary<string, double> productTotals = new Dictionary<string, double>();

            foreach (Product product in _cart)
            {
                if (productQuantities.ContainsKey(product.Name))
                {
                    productQuantities[product.Name]++;
                }
                else
                {
                    productQuantities[product.Name] = 1;
                }

                if (productTotals.ContainsKey(product.Name))
                {
                    productTotals[product.Name] += product.Price;
                }
                else
                {
                    productTotals[product.Name] = product.Price;
                }
            }

            StringBuilder reciept = new StringBuilder();

            if (productQuantities.Count > 0)
            {
                foreach (var product in productQuantities)
                {
                    string productName = product.Key;
                    int quantity = product.Value;
                    double totalPriceForProducts = productTotals[productName] / currencyChange;
                    double individualPrice = _cart.First(p => p.Name == productName).Price / currencyChange;

                    reciept.AppendLine($"{productName} - {quantity} x {Math.Round(individualPrice, 2, MidpointRounding.AwayFromZero)}{currency}/each " +
                                       $"= {Math.Round(totalPriceForProducts, 2, MidpointRounding.AwayFromZero)}{currency}");
                }
            }
            return reciept.ToString();
        }

    }
}
