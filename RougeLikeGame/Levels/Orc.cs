using RogueLib.Dungeon;
using Vector2 = RogueLib.Utilities.Vector2;


namespace SandBox01.Levels
{
    internal class Orc : Enemy
    {
        private Player _player;
        private int _turnCounter = 0;

        public Orc(Vector2 pos, Player player) : base('o', ConsoleColor.Red, pos, hp: 12)
        {
            _player = player;
        }
    }

    public override void Update()
        {
            _turnCounter++;
            if (_turnCounter % 2 != 0) return;

            int dx = (_player.Pos.X > Pos.X) ? 1 : (_player.Pos.X < Pos.X ? -1 : 0);
            int dy = (_player.Pos.Y > Pos.Y) ? 1 : (_player.Pos.Y < Pos.Y ? -1 : 0);
            Pos = new Vector2(Pos.X + dx, Pos.Y + dy);
        }


    }
}
