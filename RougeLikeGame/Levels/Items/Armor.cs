using RogueLib.Dungeon;
using RogueLib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SandBox01.Levels.Items;

public class Armor : Item
{
    public string Name { get; }
    public int ArmorBonus { get; }

    public Armor(string name, int armBonus, Vector2 pos)
        : base('[', pos)
    {
        Name = name;
        ArmorBonus = armBonus;
    }

    public override void Draw(IRenderWindow disp)
    {
        disp.Draw(Glyph, Pos, ConsoleColor.Cyan);
    }
}
