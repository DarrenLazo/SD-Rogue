using RogueLib.Dungeon;
using RogueLib.Utilities;

namespace SandBox01.Levels;

internal static class Inventory
{
    static List<Item> ItemsInInventory = new List<Item>();

    public static Item GetItem(int index) => ItemsInInventory[index];

    public static void AddItem(Player player, Item item) => ItemsInInventory.Add(item);

    public static void UseItem(Item item) => item.Use();

    // NEEDS WORK //
    public static void DropItem(Item item)
    {
        if (ItemsInInventory.Contains(item))
        {
            ItemsInInventory.Remove(item);
        }
    }


}