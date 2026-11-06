using System;

ShoppingList list = new ShoppingList("items.txt");

// Läs in data från filen vid programstart
list.Load();

while (true)
{
    Console.WriteLine();
    list.Print();
    Console.WriteLine();
    Console.WriteLine("1. Lägg till vara");
    Console.WriteLine("2. Ta bort vara");
    Console.WriteLine("3. Spara");
    Console.WriteLine("4. Sök vara");
    Console.WriteLine("5. Avsluta");
    Console.Write("Välj: ");

    int choice; 
    if (!int.TryParse(Console.ReadLine(), out choice))
    {
        Console.WriteLine("Fel: Ogiltigt val!");
        continue;
    }

    // Alternativ 1: Lägg till vara
    if (choice == 1)
    {
        Console.Write("Namn: ");
        string name = Console.ReadLine();

        Console.Write("Pris: ");
        if (!int.TryParse(Console.ReadLine(), out int price))
        {
            Console.WriteLine("Fel: Ogiltigt pris!");
        }
        else
        {
            try
            {
                Item newItem = new Item(name, price);
                if (list.Add(newItem))
                {
                    Console.WriteLine("Varan lades till!");
                }
                else
                {
                    Console.WriteLine($"Fel: Budgettaket på {list.BudgetCap} kr överskrids!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fel: {ex.Message}");
            }
        }
    }
    // Alternativ 2: Ta bort vara
    else if (choice == 2)
    {
        Console.Write("Nummer: ");
        if (int.TryParse(Console.ReadLine(), out int number))
        {
            list.RemoveAt(number);  
        }
        else
        {
           Console.WriteLine("Fel: Ogiltigt nummer!");   
        }
    }
    // Alternativ 3: Spara till fil
    else if (choice == 3)
    {
        list.Save();
    }
    // Alternativ 4: Sök efter vara
    else if (choice == 4)
    {
        Console.Write("Namn att söka efter: ");
        string wanted = Console.ReadLine();
        Item found = list.Find(wanted);

        if (found == null)
        {
            Console.WriteLine("Varan finns inte i listan.");
        }
        else
        {
            Console.WriteLine($"Hittade: {found}");
        }
    }
    // Alternativ 5: Avsluta programmet
    else if (choice == 5)
    {
        break;
    }
    else
    {
        Console.WriteLine("Fel: Välj ett alternativ mellan 1 och 5.");
    }
}
