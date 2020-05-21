using System.Drawing;

namespace SnakeGame
{
    interface ICreature
    {
        string GetName();
        Point GetPosition();
        void ActInConflict(ICreature conflictedObject, IAliveCreature aliveConflictedObject, int mapWidth, int mapHeight);
        void ActInConflict(IAliveCreature conflictedObject, int mapWidth, int mapHeight);
        bool DeadInConflict(ICreature conflictedObject);
        bool DeadInConflict(IAliveCreature conflictedObject);
    }
}
