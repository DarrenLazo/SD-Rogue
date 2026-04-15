using RogueLib.Dungeon;
using Vector2 = RogueLib.Utilities.Vector2;

namespace SandBox01.Levels
{
    internal class Goblin : Enemy
    {
        // I referenced the player here to let the goblin to chase them each round
        private Player _player;

        public Goblin(Vector2 pos, Player player) : base('g', ConsoleColor.Green, pos, hp: 6)
        {
            _player = player;
        }

        public override void Update()
        {
            // This calculus is made to move exactly one tile of movement bringing the enemy closer to the player.
            int dx = (_player.Pos.X > Pos.X) ? 1 : (_player.Pos.X < Pos.X ? -1 : 0);
            int dy = (_player.Pos.Y > Pos.Y) ? 1 : (_player.Pos.Y < Pos.Y ? -1 : 0);
            Pos = new Vector2(Pos.X + dx, Pos.Y + dy);
        }
    }
}
