using RogueLib.Dungeon;
using Vector2 = RogueLib.Utilities.Vector2;


namespace SandBox01.Levels
{
    internal class Orc : Enemy
    {
        // removed it and used protected at Enemy class for convenience.
        //private Player _player;
        private int _turnCounter = 0;

        // passing in ConsoleColor.Red? I removed that for now.
        public Orc(Vector2 pos, Player player) : base('o', pos, hp: 12)
        {
            _player = player;
            Color = ConsoleColor.Red;
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
            if (_turnCounter % 2 != 0) return;

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
