# Inlämningsuppgift 2: Robust Inköpslista

## Identifierade buggar och ågatder

1. **Tomma rader i filen kraschade `Load()`**
   - **Vad som hände:** Om filen innehöll en tom rad kraschade programmet med ett undantag vid filinläsning.
   - **Varför:** Metoden saknade kontroll för tomma rader efter `Trim()`.
   - **Lösning:** Lagt till en kontroll med `cleanLine == ""` för att hoppa över tomma rader vid inläsning.

2. **Dolda `\r`-tecken vid filinläsning**
   - **Vad som hände:** Osynliga tecken som `\r` låg kvar i strängarna vilket gjorde att sökfunktionen `Find()` misslyckades.
   - **Varför:** Filen lästes in utan att rensa osynliga radbrytningstecken.
   - **Lösning:** Använder `.Trim()` på alla inlästa rader samt jämför med `StringComparison.OrdinalIgnoreCase`.

3. **Tomt catch-block och felaktigt meddelande i `Save()`**
   - **Vad som hände:** Programmet visade "Listan är sparad" även om filskrivningen misslyckades.
   - **Varför:** `Save()` hade ett tomt `catch`-block och meddelandet låg utanför `try`.
   - **Lösning:** Ändrat `Save()` så att den returnerar `bool`, flyttat framgångsmeddelandet till `try`-blocket och visar felmeddelande i `catch`.

4. **Saknad validering i `Item`-klassen**
   - **Vad som hände:** Det gick att skapa varor med tomt namn eller negativt pris.
   - **Varför:** Konstruktorn saknade valideringslogik.
   - **Lösning:** Lagt till validering i konstruktorn på `Item` som kastar `ArgumentException` vid tomt namn och `ArgumentOutOfRangeException` vid negativt pris.

5. **Saknad felhantering vid skapande av vara i `Program.cs`**
   - **Vad som hände:** Programmet kraschade om felaktig indata angavs.
   - **Varför:** `new Item()` anropades utan felhantering.
   - **Lösning:** Omslutit `new Item()` med ett `try-catch`-block för att fånga upp valideringsfel och visa tydliga felmeddelanden för användaren.

6. **Saknad hantering av budgettak**
   - **Vad som hände:** ShoppingList hade inget budgettak.
   - **Varför:** Det fanns ingen gräns för den totala summan i listan.
   - **Lösning:** Lagt till en property `BudgetCap` (2000 kr) och ändrat `Add()` så att den returnerar `false` om budgeten överskrids.

---

## Designval för felhantering

Jag har valt att låta `ShoppingList`-klassen returnera `bool` vid operationsfel (t.ex. i `Add` för budgettak och i `Save` för filskrivning) samt kasta undantag i `Item`-konstruktorn vid ogiltig indata.

**Motivering:**
- **Separation of Concerns:** Klasserna `Item` och `ShoppingList` ska inte direkt anropa `Console.WriteLine()`, eftersom de representerar logik och data, inte användargränssnittet.
- **Robusthet:** Genom att kasta undantag i `Item` garanterar vi att inga ogiltiga objekt någonsin kan skapas. `Program.cs` ansvarar sedan för att fånga dessa undantag och presentera felmeddelanden på ett snyggt sätt för användaren.

---

## Klassdiagram

```mermaid
classDiagram
    class Program {
        +Main(string[] args)
    }

    class ShoppingList {
        -List~Item~ items
        -int budgetCap
        +Add(Item item) bool
        +RemoveAt(int index)
        +Find(string name) Item
        +Total() int
        +Save() bool
    }

    class Item {
        +string Name
        +int Price
        +Item(string name, int price)
    }

    Program --> ShoppingList
    ShoppingList o-- Item
