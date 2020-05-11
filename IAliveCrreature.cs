﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    interface IAliveCreature
    {
        string GetName();
        Point GetPosition();
        Direction GetDirection();
        LinkedList<Point> GetBody();
        int GetScore();
        void AddScore(int ads);
        void ActInConflict(ICreature conflictedObject, Game game);
        void TryChangeDirection(Direction direction);
        void Move(Game game);
    }
}
