namespace RogueLib.Dungeon;


//Just changed the class to interface and added the update method
public interface IActor
{
    char Glyph { get; }


    void Update();
}