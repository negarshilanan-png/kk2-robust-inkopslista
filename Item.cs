// One item on the shopping list.
class Item
{
    public string Name { get; set; }
    public int Price { get; set; }

    public Item(string name, int price)
    {
        // Rensa bort osynliga tecken och extra utrymme
        string cleanName = name?.Trim();

        // Validera att namnet inte är tomt eller endast innehåller mellanslag
        if (string.IsNullOrWhiteSpace(cleanName))
        {
            throw new ArgumentException("Namnet får inte vara tomt.", nameof(name));
        }

        // Validera att priset inte är negativt
        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Priset får inte vara negativt.");
        }

        Name = cleanName;
        Price = price;
    }

    public override string ToString()
    {
        return $"{Name} - {Price} kr";
    }
}
