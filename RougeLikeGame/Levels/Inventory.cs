using RogueLib.Dungeon;
using RogueLib.Utilities;

namespace SandBox01.Levels;

internal static class Inventory
{
    static List<Item> ItemsInInventory = new List<Item>();

    public static Item GetItem(int index) => ItemsInInventory[index];

    public static void AddItem(Item item) => ItemsInInventory.Add(item);

    public static void UseItem(Item item) => item.Use();

    // NEEDS WORK //
    public static void Open()
    {
        if (ItemsInInventory.Count == 0)
            return;

        foreach(Item i in ItemsInInventory)
        {
            Console.WriteLine($"{i.Name}");
        }
    }


}