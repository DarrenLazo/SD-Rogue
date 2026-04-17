using RogueLib.Dungeon;
using RogueLib.Engine;
using RogueLib.Utilities;
using SandBox01.Levels;
using TileSet = System.Collections.Generic.HashSet<RogueLib.Utilities.Vector2>;


namespace RlGameNS;


public static class Program {

   static void Main(string[] args) {
      while (true) {
         Game game = new MyGame();
         game.run();

         if (game.IsPlayerAlive)
            break;

         if (!PromptForNewGame())
            break;
      }
   }

   static bool PromptForNewGame() {
      var deathScene = new DeathScene();
      Console.Clear();
      Console.ForegroundColor = deathScene.Color;
      Console.WriteLine(deathScene.Glyph);
      Console.ResetColor();
      Console.WriteLine("You died. Start a new game? (y/n)");

      while (true) {
         var answer = Console.ReadLine()?.Trim().ToLowerInvariant();
         if (answer == "y" || answer == "yes")
            return true;
         if (answer == "n" || answer == "no")
            return false;

         Console.WriteLine("Please type y or n.");
      }
   }
}
