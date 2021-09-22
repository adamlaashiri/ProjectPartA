using System;

namespace ProjectPartA_A2
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
        static int nrArticles = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Project Part A\n");
            int menuSel = 5;
            do
            {
                menuSel = MenuSelection();
                MenuExecution(menuSel);

            } while (menuSel != 5);
        }

        private static int MenuSelection()
        {
            int menuSel = 5;
            bool correctInput = false;

            do
            {
                Console.WriteLine($"{nrArticles} out of {_maxNrArticles} articles entered.");
                Console.WriteLine("Menu");
                Console.WriteLine("1 - Enter an article");
                Console.WriteLine("2 - Remove an article");
                Console.WriteLine("3 - Print receipt sorted by price");
                Console.WriteLine("4 - Print receipt sorted by name");
                Console.WriteLine("5 - Quit");

                try
                {
                    menuSel = int.Parse(Console.ReadLine());

                    if (!(menuSel > 0 && menuSel <= 5))
                    {
                        PrintError("\nPlease enter a number between 1 and 5\n");
                        continue;
                    }
                    correctInput = true;
                }
                catch (Exception ex) //the throw from Parse is catched here in case of error
                {
                    PrintError("\nError: Enter a valid number\n");
                    continue;
                }
            } while (!correctInput);

            return menuSel;
        }
        private static void MenuExecution(int menuSel)
        {
            try
            {
                switch (menuSel)
                {
                    case 1:
                        ReadAnArticle();
                        break;
                    case 2:
                        RemoveAnArticle();
                        break;
                    case 3:
                        SortArticles();
                        break;
                    case 4:
                        SortArticles(true);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                //Because the user is brought back to the menu after error, let's handle all error in the top level catch block
                PrintError($"Error: {ex.Message}\n");
            }
        }

        private static void ReadAnArticle()
        {
            //Your code to enter an article
            if (nrArticles <= _maxNrArticles)
            {
                Console.Write("\n");
                Console.WriteLine($"Please enter name and price for article #{nrArticles} in the format name:price (example Beer:2,25):");
                string input = Console.ReadLine();
                Console.WriteLine("\n");

                if (input != string.Empty && input.Trim().Contains(':') && !input.Trim().Contains(' '))
                {
                    int delimiterIndex = input.IndexOf(':');
                    bool nameError;
                    bool priceError;

                    string itemName = input.Substring(0, delimiterIndex);
                    decimal itemPrice;

                    nameError = itemName == string.Empty || itemName.Length > _maxArticleNameLength;
                    priceError = !decimal.TryParse(input.Substring(delimiterIndex + 1), out itemPrice);

                    if (nameError && priceError)
                        throw new Exception("name and price input format error");

                    if (nameError)
                        throw new Exception("name input format error");

                    if (priceError)
                        throw new Exception("price input format error");

                    //if input check completed without any error, assign new article and return
                    articles[nrArticles++] = new Article { Name = itemName, Price = itemPrice };
                    PrintConfirmation($"Success: Added {itemName} to articles\n");
                    return;
                }
                throw new Exception("Article input format error!");
                
            }
            throw new Exception($"Exceeded maximum articles of {_maxNrArticles}");
        }
        private static void RemoveAnArticle()
        {
            //Your code to remove an article

            if (nrArticles > 0)
            {
                Console.Write("\n");
                Console.WriteLine("Please enter name of article to remove (example Coffe):");
                string input = Console.ReadLine();
                Console.WriteLine("\n");

                if (input != string.Empty)
                {
                    try
                    {
                        int index = FindIndexByName(input);
                        RemoveArticleFromArray(index);
                        nrArticles--;
                        PrintConfirmation($"\nSuccess: Removed {input} from articles\n");
                        return;
                    }
                    catch (Exception e) // this is where the throw is catched from FindIndexByName when inputed article is not found
                    {
                        throw new Exception(e.Message);
                    }
                }
                throw new Exception("name input format error!");
            }
            throw new Exception("there are no articles entered");
        }

        private static void PrintReciept(string title)
        {
            //Your code to print out a reciept
            decimal total = 0;

            Console.Write("\n");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine($"Reciept Purchased Articles sorted by {title}");
            Console.WriteLine($"Purchase date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            Console.Write("\n");
            Console.WriteLine($"Number of items purchased: {nrArticles}");

            Console.Write("\n");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"{"#",-5}{"Name",-10}{"Price",20}");
            Console.ForegroundColor = ConsoleColor.White;

            for (int i = 0; i < nrArticles; i++)
            {
                total += articles[i].Price;
                Console.WriteLine($"{i, -5}{articles[i].Name, -10}{articles[i].Price, 20:C2}");
            }

            Console.Write("\n");
            Console.WriteLine($"{"Total purchase:", -20}{total, 15:c2}");
            Console.WriteLine($"{"Includes VAT (25%):", -20}{total * _vat, 15:c2}");
            Console.WriteLine("-------------------------------------------");
        }

        private static void SortArticles(bool sortByName = false)
        {
            //selection sort
            if (nrArticles > 0)
            {
                if (sortByName)
                {
                    Article temp;
                    int smallest;
                    for (int i = 0; i < nrArticles - 1; i++)
                    {
                        smallest = i;
                        for (int j = i + 1; j < nrArticles; j++)
                        {
                            if (articles[j].Name.ToLower()[0] < articles[smallest].Name.ToLower()[0]) //using String.Chars[] to access the first char. Comparing chars numerically
                            {
                                smallest = j;
                            }
                            temp = articles[smallest];
                            articles[smallest] = articles[i];
                            articles[i] = temp;
                        }
                    }
                    PrintReciept("name");
                } else
                {
                    Article temp;
                    int smallest;
                    for (int i = 0; i < nrArticles - 1; i++)
                    {
                        smallest = i;
                        for (int j = i + 1; j < nrArticles; j++)
                        {
                            if (articles[j].Price < articles[smallest].Price)
                            {
                                smallest = j;
                            }
                            temp = articles[smallest];
                            articles[smallest] = articles[i];
                            articles[i] = temp;
                        }
                    }
                    PrintReciept("number");
                }
                PressToContinue();
            }
            throw new Exception("there are no articles entered to sort");
        }

        //helper methods
        private static void PrintError(string input)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(input);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void PrintConfirmation(string input)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(input);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void PressToContinue()
        {
            Console.WriteLine("\nPress any button to continue...");
            Console.ReadKey();
            Console.WriteLine();
        }

        private static int FindIndexByName(string name)
        {
            int index = 0;

            for (int i = 0; i < nrArticles; i++)
            {
                if (articles[i].Name.ToLower() == name.ToLower())
                {
                    return index;
                }
                index++;
            }
            throw new Exception($"Article {name} not found. Cannot remove.");
        }
        private static void RemoveArticleFromArray(int index)
        {
            Article[] tmpArticles = new Article[_maxNrArticles];

            int newIndex = 0;
            for (int i = 0; i < nrArticles; i++)
            {
                if (i == index)
                    continue;
                //array size is set at compilation time (can't add or remove elements), 
                //therefore I need to copy elements one by one and ignore copying the element with target index while also "not disturbing" the index of the new array
                tmpArticles[newIndex++] = articles[i];
            }
            articles = tmpArticles;
        }
    }
}
