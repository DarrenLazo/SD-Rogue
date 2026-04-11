using RogueLib.Utilities;

namespace RogueLib.Dungeon
{
    public abstract class Enemy : IActor, IDrawable
    {
      
            public char Glyph { get; }

            public ConsoleColor Color { get; protected set; }

            public Vector2 Pos { get; set; }
            public int Hp { get; protected set; }
            
            public int MaxHp { get; protected set;  }

            public bool IsAlive => Hp > 0;

            protected Enemy(char glyph, Vector2 pos, int hp)
            {
                Glyph = glyph;
                Pos = pos;
                Hp = hp;
                MaxHp = hp;
                Color = ConsoleColor.Red;
            }

            public virtual void Update()
            {

            }

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
            }
        }

    }

