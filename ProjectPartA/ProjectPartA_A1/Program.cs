using System;

namespace ProjectPartA_A1
{
    class Program
    {
        struct Article
        {
            public string Name;
            public decimal Price;
        }

        const int _maxNrArticles = 10;
        const int _maxArticleNameLength = 20;
        const decimal _vat = 0.25M;

        static Article[] articles = new Article[_maxNrArticles];
        static int nrArticles;

        static void Main(string[] args)
        {
            ReadArticles();
            PrintReciept();
        }
        
        private static void ReadArticles()
        {
            //Your code to enter the articles
            while (true)
            {
                Console.WriteLine($"How many articles do you want (between 1 and {_maxNrArticles})");

                //input error handling
                if (int.TryParse(Console.ReadLine(), out nrArticles))
                {
                    break;
                }
                Console.WriteLine("\nWrong input, try again!\n");
            }


            for (int i = 0; i < nrArticles; i++)
            {

                while (true)
                {
                    Console.Write("\n");
                    Console.WriteLine($"Please enter name and price for article #{i} in the format name:price (example Beer:2,25)");
                    string input = Console.ReadLine();

                    if (input != string.Empty && input.Trim().Contains(':') && !input.Trim().Contains(' '))
                    {
                        int delimiterIndex = input.IndexOf(':');
                        bool nameError = false;
                        bool priceError = false;

                        string itemName = input.Substring(0, delimiterIndex);
                        decimal itemPrice;

                        //item name and price error handling
                        nameError = itemName == string.Empty || itemName.Length > _maxArticleNameLength;
                        priceError = !decimal.TryParse(input.Substring(delimiterIndex + 1), out itemPrice);

                        if (nameError && priceError)
                        {
                            Console.WriteLine("\nName and price error\n");
                            continue;
                        } 
                        
                        if (nameError)
                        {
                            Console.WriteLine("\nName error\n");
                            continue;
                        }

                        if(priceError)
                        {
                            Console.WriteLine("\nPrice error\n");
                            continue;
                        }

                        //if input check completed without any error, assign new article
                        articles[i] = new Article { Name = itemName, Price = itemPrice};                

                        break; //onto next article
                    }
                    Console.WriteLine("\nFormat error!\n");
                }
            }
        }
        private static void PrintReciept()
        {
            //Your code to print out a reciept
            decimal total = 0;

            Console.Write("\n\n");
            Console.WriteLine("Reciept Purchased Articles");
            Console.WriteLine($"Purchase date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            
            Console.Write("\n");
            Console.WriteLine($"Number of items purchased: {nrArticles}");

            Console.Write("\n");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"{"#", -5}{"Name", -10}{"Price", 20}");
            Console.ForegroundColor = ConsoleColor.White;

            for (int i = 0; i < nrArticles; i++)
            {
                total += articles[i].Price;
                Console.WriteLine($"{i, -5}{articles[i].Name, -10}{articles[i].Price,20:C2}");
            }

            Console.Write("\n");
            Console.WriteLine($"{"Total purchase:", -20}{total, 15:c2}");
            Console.WriteLine($"{"Includes VAT (25%):", -20}{total * _vat, 15:c2}");
        }

    }
}
