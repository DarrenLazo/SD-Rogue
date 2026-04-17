using RogueLib.Dungeon;
using RogueLib.Utilities;

namespace SandBox01.Levels;


public class DeathScene : IDrawable
{
    public Vector2 Pos;
    public ConsoleColor Color { get; } = ConsoleColor.Red;
    public string Glyph { get; } = $@"
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
          |                  ||
          |                  ||
  *       | *   **    * **   |**      **
   \))ejm97/.,(//,,..,,\||(,,.,\\,.((//
        ";

    void IDrawable.Draw(IRenderWindow disp) =>
        disp.Draw(Glyph, Pos, Color);
}