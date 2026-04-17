using RogueLib.Dungeon;
using Vector2 = RogueLib.Utilities.Vector2;

namespace SandBox01.Levels.Enemies
{
    internal class Goblin : Enemy
    {
        // removed it and used protected at Enemy class for convenience. MOSTLY BECAUSE OF A DAMN BUG
        //// I referenced the player here to let the goblin to chase them each round
        //private Player _player;

        // passing in ConsoleColor.Green? I removed that for now.
        public Goblin(Vector2 pos, Player player) : base('g', pos, hp: 6)
        {
            _player = player;
            Color = ConsoleColor.Green;
        }

        public override void Update()
        {
            if (!IsAlive) return;

            // Attack if adjacent
            if (IsAdjacentToPlayer())
            {
                AttackPlayer();
                return;
            }

            // This calculus is made to move exactly one tile of movement bringing the enemy closer to the player.
            int dx = (_player.Pos.X > Pos.X) ? 1 : (_player.Pos.X < Pos.X ? -1 : 0);
            int dy = (_player.Pos.Y > Pos.Y) ? 1 : (_player.Pos.Y < Pos.Y ? -1 : 0);
            Vector2 newPos = new Vector2(Pos.X + dx, Pos.Y + dy);

            if (CanMoveTo != null && CanMoveTo(newPos))
            {
                Pos = newPos;
            }

        }
    }
}
