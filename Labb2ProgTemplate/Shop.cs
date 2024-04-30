namespace Labb2ProgTemplate
{

    public class Shop
    {
        private Membership CurrentCustomer { get; set; }

        private List<Membership> CustomerList = new List<Membership>();

        private List<Product> Products = new List<Product>()
        {
            new Product("Whip", 15.99),
            new Product("Bitch-slap", 27.99),
            new Product("Enter Uranus", 69.69),
            new Product("Pork chop", 16.89),
        };

        private ShopFunctions ShopFunctions = new ShopFunctions();

        public Shop()
        {
            RegisterCustomerDataToFile();
            LoadCustomerDataFromFile();
            MainMenu();
        }

        private string[] customerOption { get; set; }

        private string _currencyCode;

        private bool _loginSuccess = false;

        private ConsoleKeyInfo _keyPress;

        public void RegisterCustomerDataToFile()
        {
            var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Phu");
            Directory.CreateDirectory(directory);

            string customerDataFile = Path.Combine(directory, "customerData.txt");
            using (StreamWriter sWriter = new StreamWriter(customerDataFile, true))
            {
                sWriter.WriteLine("Name:knatte\nPassword:123\nMembership:gold");
                sWriter.WriteLine("Name:fnatte\nPassword:321\nMembership:silver");
                sWriter.WriteLine("Name:tjatte\nPassword:213\nMembership:bronze");
            }
        }

        public void LoadCustomerDataFromFile()
        {
            var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Phu");
            string customerDataPath = Path.Combine(directory, "customerData.txt");

            using (StreamReader sReader = new StreamReader(customerDataPath))
            {
                string line;
                while ((line = sReader.ReadLine()) != null)
                {
                    if (line.StartsWith("Name:"))
                    {
                        string name = line.Substring(5).Trim();
                        string password = sReader.ReadLine();
                        string membership = sReader.ReadLine();

                        if (password != null && password.StartsWith("Password:") &&
                            membership != null && membership.StartsWith("Membership:"))
                        {
                            string storedPassword = password.Substring(9).Trim();
                            string storedMembership = membership.Substring(11).Trim();

                            Membership customer = new Membership(name, storedPassword, storedMembership);
                            CustomerList.Add(customer);
                        }
                    }
                }
            }
        }

        public void MainMenu()
        {
            customerOption = new string[] { "Login", "Register", "" ,"Exit" };

            do
            {
                Console.Clear();

                ShopFunctions.Title();

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nWelcome to the Bitchmarket!\nFor all your sassy needs.");
                Console.ResetColor();
                Console.WriteLine("\nPlease login or register to continue.\n");

                ShopFunctions.OptionHighlight(customerOption, ShopFunctions._selectOption);
                
                var keyPress = Console.ReadKey(true).Key;

                var optionActions = new Dictionary<int, Action>
                {
                    { 
                        Array.IndexOf(customerOption, "Login"), () =>
                        {
                            Console.Clear();
                            ShopFunctions.Title();
                            Console.WriteLine("\nLOGIN\n");
                            Login();
                            _loginSuccess = true;
                        }
                    },
                    {
                        Array.IndexOf(customerOption, "Register"), () =>
                        {
                            Console.Clear();
                            ShopFunctions.Title();
                            Console.WriteLine("REGISTRATION\n");
                            Register();
                            _loginSuccess = true;
                        }
                    },
                    {
                        Array.IndexOf(customerOption, "Exit"), () =>
                        {
                            Console.WriteLine("\nThank you for visiting us! \nWe hope to see you soon again.");
                            Environment.Exit(0);
                        }
                    }
                };

                ShopFunctions.KeyOption(keyPress, ShopFunctions._selectOption, optionActions, customerOption.Length);

            } while (!_loginSuccess);
        }

        private void Login()
        {
            do
            {
                ShopFunctions.BlackWhiteText("Name:");
                var name = Console.ReadLine().ToLower();

                ShopFunctions.BlackWhiteText("Password:");
                var password = ShopFunctions.HidePassword();

                Membership customer = CustomerList.FirstOrDefault(c => c.Name.Equals(name) && c.CheckPassword(password));

                if (customer != null)
                {
                    Console.WriteLine("\nLogin successful!");
                    Thread.Sleep(1000);
                    CurrentCustomer = customer;
                    ShopMenu();
                }
                else
                {
                    Console.WriteLine("\nInvalid name or password.");
                    Console.WriteLine("Or press ESC to return to main menu and register.");
                }

                _keyPress = Console.ReadKey(true);

            } while (_keyPress.Key != ConsoleKey.Escape);

            MainMenu();
        }

        private void Register()
        {
            var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Phu");
            string customerData = Path.Combine(directory, "customerData.txt");

            ShopFunctions.BlackWhiteText("Enter your name:");
            string name = Console.ReadLine();

            ShopFunctions.BlackWhiteText("\nEnter a password:");
            string password = Console.ReadLine();

            Membership newCustomer = new Membership(name, password, membership:"basic");
            CustomerList.Add(newCustomer);

            CurrentCustomer = newCustomer;

            using (StreamWriter sWriter = new StreamWriter(customerData, true))
            {
                sWriter.WriteLine($"Name:{name}\nPassword:{password}\nMembership:basic");
            }

            Console.WriteLine("\nYou've have successfully been registered.");
            Thread.Sleep(1000);

            ShopMenu();
        }

        private void ShopMenu()
        {
            customerOption = new string[] {

                $"{Products[0].Name}\t\t\t{Products[0].Price}SEK", 
                $"{Products[1].Name}\t\t{Products[1].Price}SEK",
                $"{Products[2].Name}\t\t{Products[2].Price}SEK",
                $"{Products[3].Name}\t\t{Products[3].Price}SEK",
                "",
                "\u001b[2mView cart\u001b[0m", 
                "" ,
                "\u001b[2mCheck out\u001b[2m", 
                "",
                "\u001b[2mLog out\u001b[2m","\u001b[2mExit\u001b[2m",
            };

            do
            {
                Console.Clear();

                ShopFunctions.Title();

                Console.WriteLine(CurrentCustomer.MembershipStatus());

                Console.WriteLine(CurrentCustomer.ToString());

                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("\n-SHOP MENU-");
                Console.ResetColor();

                Console.WriteLine("\n\u001b[4m*Products*\u001b[0m");
                
                ShopFunctions.OptionHighlight(customerOption, ShopFunctions._selectOption);

                var keyPress = Console.ReadKey(true).Key;

                var optionActions = new Dictionary<int, Action>
                {
                    {
                        Array.IndexOf(customerOption, $"{Products[0].Name}\t\t\t{Products[0].Price}SEK"), () =>
                        {
                            CurrentCustomer.AddToCart(Products[0]);
                        }
                    },
                    {
                        Array.IndexOf(customerOption, $"{Products[1].Name}\t\t{Products[1].Price}SEK"), () =>
                        {
                            CurrentCustomer.AddToCart(Products[1]);
                        }
                    },
                    {
                        Array.IndexOf(customerOption, $"{Products[2].Name}\t\t{Products[2].Price}SEK"), () =>
                        {
                            CurrentCustomer.AddToCart(Products[2]);
                        }
                    },
                    {
                        Array.IndexOf(customerOption, $"{Products[3].Name}\t\t{Products[3].Price}SEK"), () =>
                        {
                            CurrentCustomer.AddToCart(Products[3]);
                        }
                    },
                    {
                        Array.IndexOf(customerOption, "\u001b[2mView cart\u001b[0m"), () =>
                        {
                            Console.WriteLine("\nCART:");
                            ViewCart();

                        }
                    },
                    {
                        Array.IndexOf(customerOption, "\u001b[2mCheck out\u001b[2m"), () =>
                        {
                            Console.WriteLine("\nProducts in cart:");
                            Checkout();
                        }
                    },
                    {
                        Array.IndexOf(customerOption, "\u001b[2mLog out\u001b[2m"), () =>
                        {
                            Console.WriteLine("Thank you");
                            MainMenu();
                        }
                    },
                    {
                        Array.IndexOf(customerOption, "\u001b[2mExit\u001b[2m"), () =>
                        {
                            Console.WriteLine($"\nHope to see you soon again {CurrentCustomer.Name.ToUpper()}!");
                            Environment.Exit(0);
                        }
                    }
                };

                ShopFunctions.KeyOption(keyPress, ShopFunctions._selectOption, optionActions, customerOption.Length);

            } while (true);

        }

        private void ViewCart()
        {
            do
            {
                Console.Clear();

                Console.WriteLine("Currently in your cart:\n");

                if (CurrentCustomer.Cart.Count == 0)
                {
                    Console.WriteLine("\nYour cart is empty.\n");
                    Console.ReadKey();
                    return;
                }

                for (int i = 0; i < CurrentCustomer.Cart.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {CurrentCustomer.Cart[i].Name} - {CurrentCustomer.Cart[i].Price}kr");
                }

                double total = CurrentCustomer.CartTotal();
                Console.WriteLine($"Current total: {total}kr\n");

                Console.WriteLine("Enter the number of the item you want to remove (or Enter to go back):");
                if (int.TryParse(Console.ReadLine(), out int selectedIndex) && selectedIndex >= 1 &&
                    selectedIndex <= CurrentCustomer.Cart.Count)
                {
                    Product removedItem = CurrentCustomer.Cart[selectedIndex - 1];
                    CurrentCustomer.RemoveFromCart(removedItem);
                    Console.WriteLine($"-Item '{removedItem.Name}' removed from the cart.\n");

                }
                else if (selectedIndex == 0)
                {
                    _loginSuccess = true;
                    return;
                }
                else
                {
                    Console.WriteLine("-Please enter a valid number.\n");
                }

            } while (!_loginSuccess);

        }

        private void Checkout()
        {
            double total = CurrentCustomer.CartTotal();

            string reciept = CurrentCustomer.CustomerCurrentProducts("SEK", 1);
            Console.WriteLine(reciept);

            if (total > 0)
            {
                Console.WriteLine($"TOTAL:\t\t\t{total}SEK\n");

                if (CurrentCustomer.MembershipTier.ToLower() == "gold" ||
                    CurrentCustomer.MembershipTier.ToLower() == "silver" ||
                    CurrentCustomer.MembershipTier.ToLower() == "bronze")
                {
                    double discount = GetDiscountForMembershipTier();
                    total *= (1 - discount);
                    Console.WriteLine($"With a {CurrentCustomer.MembershipTier.ToUpper()} membership you get a {discount*100}% discount.");
                    Console.WriteLine($"Total with discount:\t{ Math.Round(total, 2, MidpointRounding.AwayFromZero)}SEK");
                }
                
                Console.Write("\nEnter the currency you want to pay in (SEK/EURO/USD): ");
                _currencyCode = Console.ReadLine().ToUpper();
                double currencyConverted = Currency();
                Console.WriteLine($"Total in currency:\t{currencyConverted}{_currencyCode}");

                Console.WriteLine("\nPress any key to continue with the payment.");
                Console.ReadKey();

                Console.WriteLine("\n\u001b[4m*RECIEPT:*\u001b[0m");
                reciept = CurrentCustomer.CustomerCurrentProducts(_currencyCode, GetExchangeRate());
                Console.WriteLine(reciept);
                Console.WriteLine($"TOTAL:\t\t\t{currencyConverted}{_currencyCode}");

                Console.WriteLine($"\nThank you {CurrentCustomer.Name.ToUpper()} for your purchase!");
                Console.ReadKey();
                CurrentCustomer.Cart.Clear();
                ShopMenu();
            }
            else
            {
                Console.WriteLine("Your cart is empty.\n");
                Console.ReadKey();
            }
        }

        public double GetDiscountForMembershipTier()
        {
            Dictionary<string, double> MembershipDiscount = new Dictionary<string, double>
            {
                { "gold", 0.15 },
                { "silver", 0.1 },
                { "bronze", 0.05 }
            };

            if (MembershipDiscount.TryGetValue(CurrentCustomer.MembershipTier, out double discount))
            {

                return discount;
            }
            else
            {
                return 0;
            }
        }

        public double Currency()
        {

            double sum = CurrentCustomer.CartTotal();

            while (true)
            {
                double exchangeRate = GetExchangeRate();
                if (sum != 0)
                {
                    sum /= exchangeRate;
                    return Math.Round(sum, 2, MidpointRounding.AwayFromZero);
                }
            }

        }

        public double GetExchangeRate()
        {
            var currency = new Dictionary<string, double>();
            currency.Add("EURO", 11.60);
            currency.Add("USD", 11.02);
            currency.Add("SEK", 1);

            while (true)
            {
                if (currency.ContainsKey(_currencyCode.ToUpper()))
                {
                    return currency[_currencyCode.ToUpper()];
                }
                else
                {
                    Console.WriteLine("Invalid currency.");
                    _currencyCode = Console.ReadLine();
                }
            }

        }

    }
}



