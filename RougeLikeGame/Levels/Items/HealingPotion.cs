using RogueLib.Dungeon;
using RogueLib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SandBox01.Levels.Items;

public class HealingPotion : Item
{
    public int HealAmount { get; }

    public HealingPotion(int healAmount, Vector2 pos)
        : base('!', pos)
    {
        HealAmount = healAmount;
    }

    public override void Draw(IRenderWindow disp)
    {
        disp.Draw(Glyph, Pos, ConsoleColor.Magenta);
    }

    public override void Use()
    {
        
    }
}