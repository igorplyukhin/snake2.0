using System.Drawing;

namespace SnakeGame
{
    interface ICreature
    {
        string GetName();
        Point GetPosition();
        bool DeadInConflict(ICreature conflictedObject);
        void ActInConflict(ICreature conflictedObject, Game game);
        void ActInConflict(ILiveCreature conflictedObject, Game game);
    }
}
