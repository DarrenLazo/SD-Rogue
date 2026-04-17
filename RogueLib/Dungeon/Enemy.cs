using RogueLib.Utilities;

namespace RogueLib.Dungeon
{
    public abstract class Enemy : IActor, IDrawable
    {

        public char Glyph { get; }

        public ConsoleColor Color { get; protected set; }

        public Vector2 Pos { get; set; }
        public int Hp { get; protected set; }

        public int MaxHp { get; protected set; }

        public bool IsAlive => Hp > 0;

        // Func to check if can move to.
        protected Func<Vector2, bool>? CanMoveTo;

        // Keep track of the player
        protected Player _player;

        protected Enemy(char glyph, Vector2 pos, int hp)
        {
            Glyph = glyph;
            Pos = pos;
            Hp = hp;
            MaxHp = hp;
            Color = ConsoleColor.Red;
        }

        public abstract void Update();

        public virtual void Draw(IRenderWindow disp)
        {
            if (IsAlive)
            {
                disp.Draw(Glyph, Pos, Color);
            }
        }

        public virtual void TakeDamage(int amount)
        {
            Hp = Math.Max(0, Hp - amount);

            if (Hp <= 0)
            {
                Hp = 0;
                LogSystem.Log($"The {GetType().Name} dies!"); // Log here
            }
        }


        // Setting our walk check.
        public void SetWalkCheck(Func<Vector2, bool> checker)
        {
            CanMoveTo = checker;
        }

        // Check if its adjacent to player, using provided helper methods.
        protected bool IsAdjacentToPlayer()
        {
            return (_player.Pos - Pos).RookLength == 1;
        }


        protected virtual void AttackPlayer()
        {
            int dmg = 1;
            _player.TakeDamage(dmg);
            //LogSystem.Log($"The {this.GetType().Name} hits you for {dmg} damage."); // would have to prob multiline
        }
    }

}

