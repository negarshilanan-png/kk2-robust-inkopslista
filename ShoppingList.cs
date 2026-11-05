// Holds the items and takes care of loading and saving them.
class ShoppingList
{
    private List<Item> items = new List<Item>();
    private string path;

    public ShoppingList(string path)
    {
        this.path = path;
    }

    // Definiera ett budgettak på 2000 kr
public double BudgetCap { get; set; } = 2000;

// Lägger till en vara om den inte överskrider budgettaket
public bool Add(Item item)
{
    // Kontrollera om den nya totalen överskrider budgeten
    if (Total() + item.Price > BudgetCap)
    {
        return false; // Kan inte lägga till varan
    }

    items.Add(item);
    return true; // Varan lades till
}


    // Removes the item the user sees as number 1, 2, 3 ...
    public void RemoveAt(int number)
    {
        int index = number - 1;
        if (index >= 0 && index < items.Count)
        {
          items.RemoveAt(number - 1);  
        }
        else
        {
            Console.WriteLine("Fel: Numret finns inte i listan!");
        }   
    }

    // Adds up the price of every item on the list.
    public int Total()
    {
        int sum = 0;

        for (int i = 0; i < items.Count; i++)
        {
            sum += items[i].Price;
        }

        return sum;
    }

    // Söker efter en vara baserat på namn
public Item Find(string name)
{
    if (string.IsNullOrWhiteSpace(name)) return null;

    foreach (Item item in items)
    {
        // Jämför namn och rensar bort osynliga tecken
        if (item.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            return item;
        }
    }
    return null;
}

    public void Print()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {items[i]}");
        }

        Console.WriteLine($"Totalt: {Total()} kr");
    }

    // Sparar alla varor till filen på ett säkert sätt
public bool Save()
{
    try
    {
        using (StreamWriter writer = new StreamWriter(path))
        {
            foreach (Item item in items)
            {
                writer.WriteLine($"{item.Name};{item.Price}");
            }
        }
        Console.WriteLine("Listan är sparad.");
        return true;
    }
    catch (Exception ex)
    {
        Console.WriteLine("Kunde inte spara listan: " + ex.Message);
        return false;
    }
}

    // Läser in varor från filen på ett enkelt och säkert sätt
public void Load()
{
    // Kontrollera om filen finns
    if (!File.Exists(path))
    {
        return;
    }

    try
    {
        items.Clear(); // Rensar listan

        // Läser alla rader från filen
        string[] lines = File.ReadAllLines(path);

        foreach (string line in lines)
        {
            // Ta bort osynliga tecken som \r och extra utrymme
            string cleanLine = line.Trim();

            // Om raden är tom, hoppa över den
            if (cleanLine == "")
            {
                continue;
            }

            // Dela raden vid semikolon: parts[0] = namn, parts[1] = pris
            string[] parts = cleanLine.Split(';');

            // Kontrollera att det finns två delar
            if (parts.Length == 2)
            {
                string name = parts[0].Trim();
                
                // Omvandla priset säkert
                if (int.TryParse(parts[1], out int price))
                {
                    items.Add(new Item(name, price));
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Fel vid inläsning: " + ex.Message);
    }
}

}
