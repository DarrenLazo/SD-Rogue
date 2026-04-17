using RogueLib.Dungeon;
using RogueLib.Engine;
using RogueLib.Utilities;
using SandBox01.Levels;
using SandBox01.Levels.Enemies;
using SandBox01.Levels.Items;
using TileSet = System.Collections.Generic.HashSet<RogueLib.Utilities.Vector2>;

namespace RlGameNS;

// -----------------------------------------------------------------------
// The Level is the model, all the game world objects live in the model. 
// player input updates the model, the model updates the view, and the 
// controller runs the whole thing. 
//
// Scene is the base class for all game scenes (levels). Scene is an 
// abstract class that implements IDrawable and ICommandable. 
// 
// A dungeon level is a collection or rooms and tunnels in a 78x25 grid. 
// each tile is at a point, or grid location, represented by a Vector2. 
// 
// *TileSets* are HashSets of grid points, TileSets can be used to tell 
// GameScreen what tiles to draw. TileSets can be combined with Union and 
// Intersect to create complex tile sets.
// -----------------------------------------------------------------------
public class Level : Scene
{
    // ---- level config ---- 
    protected string? _map;
    protected int _senseRadius = 8;
    protected int _round = 1;

    // --- Tile Sets -----
    // used to keep track of state of tiles on the map
    protected TileSet _walkables; // walkable tiles 
    protected TileSet _floor;
    protected TileSet _exit; // exit tile
    protected TileSet _tunnel;
    protected TileSet _door;
    protected TileSet _decor; // walls and other decorations, always visible once discovered
    protected TileSet _discovered; // tiles the player has seen
    protected TileSet _inFov;      // current fov of player

    protected List<Item> _items = [];
    protected List<Enemy> _enemies = [];
    protected List<string> _maps;

    public Level(Player p, /*string map*/List<string> maps, Game game)
    {
        if (game == null || p == null)
            throw new ArgumentNullException("game, player, or map cannot be null");

        _player = p;
        _player.Pos = new Vector2(4, 12); // random, or at stairs

        _maps = maps;
        _map = _maps[0];
        _game = game;

        initMapTileSets(_map);
        _walkables.Remove(_player.Pos);
        updateDiscovered();
        registerCommandsWithScene();

        spreadGold();

        // Simple adding of enemies FOR NOW.
 
        _enemies.Add(new Orc(new Vector2(40, 12), _player));
        _enemies.Add(new Troll(new Vector2(55, 12), _player));
        _enemies.Add(new Goblin(new Vector2(20, 10), _player)); // 20 , 10

        // Simple adding of items FOR NOW.

        _items.Add(new Weapon("Dagger", 1, new Vector2(10, 5)));
        _items.Add(new Armor("Leather Armor", 1, new Vector2(12, 7)));
        _items.Add(new HealingPotion(5, new Vector2(4, 10)));

        // it can only walk if it is in any _walkable tiles.
        foreach (var e in _enemies)
        {
            e.SetWalkCheck(CanEnemyMoveTo);
        }
    }

    // Check for enemies -> will be used for player collision.
    public Enemy? GetEnemyAt(Vector2 pos)
    {
        foreach (var e in _enemies)
            if (e.Pos == pos)
                return e;

        return null;
    }

    private bool CanEnemyMoveTo(Vector2 pos)
    {
        return _walkables.Contains(pos) && GetEnemyAt(pos) == null;
    }
    private void spreadGold()
    {
        var rng = new Random();
        var am = rng.Next(10, 20);

        for (int i = 0; i < am; i++)
        {
            var tile = _floor.ElementAt(rng.Next(_floor.Count));
            _items.Add(new Gold(tile, rng.Next(100, 200)));
        }
    }

    protected void updateDiscovered()
    {
        _inFov = fovCalc(_player!.Pos, _senseRadius);

        if (_discovered is null)
            _discovered = new TileSet();

        _discovered.UnionWith(_inFov);
    }

    protected TileSet fovCalc(Vector2 pos, int sens)
       => Vector2.getAllTiles()
          .Where(t => (pos - t).RookLength < sens)
          .Where(t => HasLineOfSight(pos, t))
          .ToHashSet();

    private bool HasLineOfSight(Vector2 from, Vector2 to)
    {
        foreach (var tile in GetLine(from, to).Skip(1))
        {
            if (tile == to)
                return true;

            if (BlocksSight(tile))
                return false;
        }

        return true;
    }

    private bool BlocksSight(Vector2 pos)
    {
        if (_walkables.Contains(pos))
            return false;

        return _decor.Contains(pos) || !_floor.Contains(pos);
    }

    private IEnumerable<Vector2> GetLine(Vector2 from, Vector2 to)
    {
        int x0 = from.X;
        int y0 = from.Y;
        int x1 = to.X;
        int y1 = to.Y;
        int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            yield return new Vector2(x0, y0);

            if (x0 == x1 && y0 == y1)
                yield break;

            int e2 = err * 2;

            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }
    // -----------------------------------------------------------------------
    public override void Update()
    {
        _player!.Update();
        foreach (var enemy in _enemies.ToList())
        {
            if (!_player.isAlive)
            {
                _levelActive = false;
                break;
            }

            if (enemy.IsAlive)
            {
                enemy.Update();
            }
        }
        _enemies.RemoveAll(e => !e.IsAlive);

        if (!_player.isAlive)
        {
            _levelActive = false;
        }
    }

    public override void Draw(IRenderWindow? disp)
    {
        // Draw floor tiles ONLY if discovered or in FOV - FIXES ISSUES WITH ENEMIES AND ITEMS BEING EXPOSED -> Tunnels still show items though
        foreach (var p in _floor)
        {
            if (_discovered.Contains(p) || _inFov.Contains(p))
                disp.Draw('.', p, ConsoleColor.DarkGray);
        }
        // using custom RenderWindow, cast to my RenderWindow
        var tilesToDraw = new TileSet(_decor);
        tilesToDraw.IntersectWith(_discovered);
        tilesToDraw.UnionWith(_inFov);

        disp.fDraw(tilesToDraw, _map, ConsoleColor.Gray);

        var rng = new Random();
        if (_player.Turn % 5 == 0)
            _player._color = (ConsoleColor)rng.Next(10, 16);
        _player!.Draw(disp);
        // disp.Draw(_player!.Glyph, _player!.Pos, ConsoleColor.Cyan);

        drawItems(disp);
        drawEnemies(disp);
        disp.Draw(_player.HUD, new Vector2(0, 23), ConsoleColor.Green);
        disp.Draw(new string(' ', 78), new Vector2(0, 24), ConsoleColor.White); // The only way so far that I can clear the log messages -> If I dont do this, log messages will retain some messages so msg ex: 'The Goblin dies! in for 1 damage!'
        disp.Draw(LogSystem.Message, new Vector2(0, 24), ConsoleColor.White); // Log message here
    }

    public override void DoCommand(Command command)
    {
        if (!_player!.isAlive)
        {
            _levelActive = false;
            return;
        }

        // player ctl  
        if (command.Name == "up")
        {
            MovePlayer(Vector2.N);
        }
        else if (command.Name == "down")
        {
            MovePlayer(Vector2.S);
        }
        else if (command.Name == "left")
        {
            MovePlayer(Vector2.W);
        }
        else if (command.Name == "right")
        {
            MovePlayer(Vector2.E);
        }
        else if (command.Name == "inventory")
        {
            Inventory.Open();
        }
        else if (command.Name == "quit")
        {
            _levelActive = false;
        }
        // game ctl
    }

    // -------------------------------------------------------------------------

    private void drawItems(IRenderWindow disp)
    {
        foreach (var item in _items)
        {
            if (_inFov.Contains(item.Pos))
            {
                disp.Draw(item.Glyph, item.Pos, ConsoleColor.Yellow);
            }

        }
    }

    private void drawEnemies(IRenderWindow disp)
    {
        foreach (var enemy in _enemies)
        {
            if (_inFov.Contains(enemy.Pos)) // Fixes trail bug + not in fov but displays enemies anyway
                enemy.Draw(disp);
        }
    }

    private void initMapTileSets(string map)
    {
        var lines = map.Split('\n');

        // ------ rules for map ------
        // . - floor, walkable and transparent.
        // + - door, walkable and transparent // # - tunnel, walkable and transparent
        // ' ' - solid stone, not walkable, not transparent.
        // '|' - wall, not walkable, not transparent, but discoverable.'
        //  others are treated the same as wall.
        // tunnel, wall, and doorways are decor, once discovered they are visible.
        _discovered = new TileSet();
        _inFov = new TileSet();
        _floor = new TileSet();
        _tunnel = new TileSet();
        _door = new TileSet();
        _decor = new TileSet();
        _exit = new TileSet();

        foreach (var (c, p) in Vector2.Parse(map))
        {
            if (c == '.') _floor.Add(p);
            else if (c == '+') _door.Add(p);
            else if (c == '#') _tunnel.Add(p);
            else if (c == 'E') _exit.Add(p);
            else if (c != ' ') _decor.Add(p);
        }

        _walkables = _floor.Union(_tunnel).Union(_door).Union(_exit).ToHashSet();

        //      for (int row = 0; row < lines.Length; ++row) {
        //         for (int col = 0; col < lines[row].Length; ++col) {
        //            char tile = lines[row][col];
        //
        //            if (tile == '.' || tile == '+' || tile == '#') {
        //               _walkables.Add(new Vector2(col, row));
        //               _decor.Add(new Vector2(col, row));
        //            } else if (tile != ' ') {
        //               _decor.Add(new Vector2(col, row));
        //            }
        //         }
        //      }
    }

    // ------------------------------------------------------
    // Commands 
    // ------------------------------------------------------


    private void registerCommandsWithScene()
    {
        RegisterCommand(ConsoleKey.UpArrow, "up");
        RegisterCommand(ConsoleKey.W, "up");
        RegisterCommand(ConsoleKey.K, "up");

        RegisterCommand(ConsoleKey.DownArrow, "down");
        RegisterCommand(ConsoleKey.S, "down");
        RegisterCommand(ConsoleKey.J, "down");

        RegisterCommand(ConsoleKey.LeftArrow, "left");
        RegisterCommand(ConsoleKey.A, "left");
        RegisterCommand(ConsoleKey.H, "left");

        RegisterCommand(ConsoleKey.RightArrow, "right");
        RegisterCommand(ConsoleKey.D, "right");
        RegisterCommand(ConsoleKey.L, "right");

        RegisterCommand(ConsoleKey.I, "inventory");

        RegisterCommand(ConsoleKey.Q, "quit");
    }


    public void MovePlayer(Vector2 delta)
    {
        if (!_player!.isAlive)
        {
            _levelActive = false;
            return;
        }

        var newPos = _player!.Pos + delta;

        // 1. Check for enemy collision
        var enemy = GetEnemyAt(newPos);
        if (enemy != null)
        {
            _player.Attack(enemy);
            return; // do NOT move into the enemy tile
        }

        
        // 2. Normal movement
        if (_walkables.Contains(newPos))
        {
            var oldPos = _player!.Pos;
            _player!.Pos = newPos;
            _walkables.Remove(newPos); // new tile is now occupied
            _walkables.Add(oldPos);    // old tile is now free
            updateDiscovered();

            checkItemPickup(newPos);

            //Checking to see if Player is trying to walk into the exit
            if (_exit.Contains(newPos))
            {
                _player.Pos = new Vector2(3, 4);
                
                int index = _maps.IndexOf(_map);
                initMapTileSets(_maps[index + 1]);
                _map = _maps[index + 1];
                LogSystem.Log("next level");
            }
            
        }
    }

    private void checkItemPickup(Vector2 pos)
    {
        var item = _items.FirstOrDefault(i => i.Pos == pos);
        if (item == null) return; // We switched to switch statment : if (item is Gold gold) was the old null safety check.

        switch (item)
        {
            case Gold gold:
                _player!.PickUpGold(gold.Amount);
                LogSystem.Log($"You pick up {gold.Amount} gold.");
                _items.Remove(item);
                break;

            case HealingPotion potion:
                // Inventory stuff add here!
                LogSystem.Log("You pick up a healing potion.");
                Inventory.AddItem(potion);
                _items.Remove(item);
                break;

            case Weapon weapon:
                // Inventory stuff add here!
                LogSystem.Log($"You pick up a {weapon.Name}.");
                Inventory.AddItem(weapon);
                _items.Remove(item);
                break;

            case Armor armor:
                // Inventory stuff add here!
                LogSystem.Log($"You pick up {armor.Name}.");
                Inventory.AddItem(armor);
                _items.Remove(item);
                break;
        }

    }
    public void QuitLevel()
    {
        _levelActive = false;
    }
}
