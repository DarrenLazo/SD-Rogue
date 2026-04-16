using RogueLib.Dungeon;
using RogueLib.Utilities;

namespace SandBox01.Levels;

internal class DeathScene : IDrawable
{
    public Vector2 Pos;
    public ConsoleColor _color = ConsoleColor.Red;
    public string Glyph = $@"
                  _  /)
                 mo / )
                 |/)\)
                  /\_
                  \__|=
                 (    )
                 __)(__
           _____/      \\_____
          |  _     ___   _   ||
          | | \     |   | \  ||
          | |  |    |   |  | ||
          | |_/     |   |_/  ||
          | | \     |   |    ||
          | |  \    |   |    ||
          | |   \. _|_. | .  ||
          |                  ||
          | player name here ||
          |                  ||
  *       | *   **    * **   |**      **
   \))ejm97/.,(//,,..,,\||(,,.,\\,.((//
        ";

    void IDrawable.Draw(IRenderWindow disp) =>
        disp.Draw(Glyph, Pos, _color);
}