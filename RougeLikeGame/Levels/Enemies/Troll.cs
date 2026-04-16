using RogueLib.Dungeon;
using Vector2 = RogueLib.Utilities.Vector2;


namespace SandBox01.Levels.Enemies
{
    internal class Troll : Enemy
    {
        // removed it and used protected at Enemy class for convenience.
        //private Player _player;
        private int _turnCounter = 0;

        // passing in ConsoleColor.Magenta? I removed that for now.
        public Troll(Vector2 pos, Player player) : base('T', pos, hp: 20)
        {
            _player = player;
            Color = ConsoleColor.Magenta;
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

            _turnCounter++;
            if (_turnCounter % 3 != 0) return;

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
