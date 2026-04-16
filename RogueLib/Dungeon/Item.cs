using System;
using RogueLib.Utilities;
using System.Collections.Generic;
using System.Text;

namespace RogueLib.Dungeon;
public abstract class Item : IDrawable
{
    public Vector2 Pos { get; set; }
    public char Glyph { get; init; }

    public Item (char c, Vector2 pos)
    {
        Pos = pos;
        Glyph = c;
    }

    public abstract void Use();

    public abstract void Draw(IRenderWindow disp);
}

