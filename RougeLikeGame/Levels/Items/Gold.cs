using RogueLib.Dungeon;
using RogueLib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SandBox01.Levels.Items;

internal class Gold : Item
{
    public override string Name { get; }
    public Gold(Vector2 pos, int amount) : base ('*', pos) 
    {
        Name = "Gold";
        Amount = amount;
    }

    public ConsoleColor Color => ConsoleColor.Yellow;

    public int Amount { get; init; }
    public override void Draw(IRenderWindow disp)
    {
        disp.Draw(Glyph, Pos, Color);
    }

    public override void Use()
    {
        
    }
}
