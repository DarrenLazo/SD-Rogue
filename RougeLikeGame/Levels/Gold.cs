using RogueLib.Dungeon;
using RogueLib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SandBox01.Levels;

internal class Gold : Item
{
    public Gold(Vector2 pos, int amount) : base ('*', pos) 
    {
        Amount = amount;
    }

    public ConsoleColor Color => ConsoleColor.Yellow;

    public int Amount { get; init; }
    public override void Draw(IRenderWindow disp)
    {
        disp.Draw(Glyph, Pos, Color);
    }
}
