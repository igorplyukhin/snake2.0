using System.Drawing;

namespace SnakeGame
{
    interface ICreature
    {
        string GetName();
        Point GetPosition();
        bool DeadInConflict(ICreature conflictedObject);
        void ActInConflict(ICreature conflictedObject, Game game);
        void ActInConflict(ICreature conflictedObject, IAliveCreature aliveConflictedObject, Game game);
        void ActInConflict(IAliveCreature conflictedObject, Game game);
    }
}
