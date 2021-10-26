using System;
using System.Collections.Generic;

namespace Quoridor.Model
{
    public class Player
    {
        public int Id { get; private set; }
        public ConsoleColor Color { get; private set; }
        public int WallPairsLeft { get; private set; }
        public Direction WinDirection { get; private set; }

        public Player(int id, Direction winDirection, ConsoleColor color)
        {
            Id = id;
            WinDirection = winDirection;
            Color = color;
            SetUpWallPairs();
        }

        public void SetUpWallPairs()
        {
            WallPairsLeft = Settings.StartWallPairs;
        }

        public void TakeAwayWallPair()
        {
            if (WallPairsLeft <= 0) throw new InvalidOperationException("No wall pairs left.");
            WallPairsLeft--;
        }
    }
}
