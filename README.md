# Inköpslista - Robust Konsolapplikation

Detta är en robust konsolapplikation i C# för att hantera en inköpslista. Programmet har refaktorerats för att förhindra krascher, hantera felaktiga inmatningar samt respektera ett budgettak.

## Felrapport (Rättade buggar)

1. **Filhantering (`FileNotFoundException`):**
   - *Vad som hände:* Programmet kraschade vid start om filen `old_items.txt` inte fanns.
   - *Varför:* Metoden försökte läsa en fil utan att kontrollera dess existens.
   - *Lösning:* Lade till en kontroll med `File.Exists()` innan filen läses in.

2. **Felaktig inmatning av siffror (`FormatException`):**
   - *Vad som hände:* Programmet kraschade om användaren skrev bokstäver istället för siffror i menyn eller vid pris/index.
   - *Varför:* `int.Parse` eller `double.Parse` kastade ett undantag vid ogiltig text.
   - *Lösning:* Bytte ut `Parse` mot `int.TryParse` och `double.TryParse` för säker validering.

3. **Felaktigt startindex i totalsumma:**
   - *Vad som hände:* Totalsumman räknades fel och hoppade över den första varan.
   - *Varför:* Loopen i `Total()` startade från index `1` istället för `0`.
   - *Lösning:* Ändrade loopens startindex till `i = 0`.

4. **Index ur range vid borttagning (`ArgumentOutOfRangeException`):**
   - *Vad som hände:* Programmet kraschade om man angav ett nummer som inte fanns i listan (t.ex. 10).
   - *Varför:* Listans `RemoveAt` anropades utan gränskontroll.
   - *Lösning:* Lade till en kontroll att indexet ligger mellan `0` och `items.Count - 1`.

5. **Sökfunktion saknade feedback:**
   - *Vad som hände:* Om en vara inte hittades visades inget meddelande för användaren.
   - *Varför:* Det saknades ett `else`-block när `Find()` returnerade `null`.
   - *Lösning:* Lade till en kontroll för `null` och visar meddelandet `"Varan finns inte i listan."`.

6. **Validering i konstruktorn (`Item`):**
   - *Vad som hände:* Ogiltiga objekt (tomma namn eller negativa priser) kunde skapas.
   - *Varför:* Konstruktorn saknade valideringslogik.
   - *Lösning:* Konstruktorn kastar nu `ArgumentException` vid tomt namn och `ArgumentOutOfRangeException` vid negativt pris.

## Designval (Budgettak)

När budgettaket överskrids i `ShoppingList.Add()` har jag valt att **returnera `false`** (eller kasta ett undantag). 

- *Motivering:* Genom att hantera situationen på detta sätt hålls `ShoppingList` ren från konsolutskrifter (`Console.WriteLine`). Det blir istället `Program.cs` ansvar att ta emot svaret och visa ett begripligt meddelande för användaren om att köpet överstiger budgeten.

## Klassdiagram

```mermaid
classDiagram
    class Program {
        +Main(string[] args)
    }

    class ShoppingList {
        -List~Item~ items
        -double budgetCap
        +Add(Item item) bool
        +RemoveAt(int index)
        +Find(string name) Item
        +Total() double
        +Save()
    }

    class Item {
        +string Name
        +double Price
        +Item(string name, double price)
    }

    Program --> ShoppingList
    ShoppingList o-- Item