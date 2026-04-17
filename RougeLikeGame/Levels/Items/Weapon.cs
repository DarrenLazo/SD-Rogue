using RogueLib.Dungeon;
using RogueLib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SandBox01.Levels.Items;
public class Weapon : Item
{
    public override string Name { get; }
    public int DamageBonus { get; }

    public Weapon(string name, int dmgBonus, Vector2 pos)
        : base(')', pos)
    {
        Name = name;
        DamageBonus = dmgBonus;
    }

    public override void Draw(IRenderWindow disp)
    {
        disp.Draw(Glyph, Pos, ConsoleColor.Yellow);
    }

    public override void Use()
    {
        
    }
}
