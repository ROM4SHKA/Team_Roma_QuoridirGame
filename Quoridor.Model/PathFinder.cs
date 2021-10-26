using System;
using System.Collections.Generic;
using System.Text;

namespace Quoridor.Model
{
    public class PathFinder
    {
        const int Tried = 2;
        const int Path = 3;

        private bool[,] Grid;
        private int Size;
        private Direction DesiredDirection;

        private int[,] Map;

        public PathFinder(bool[,] grid, Direction desiredDirection)
        {
            int size = grid.GetLength(0);
            Grid = grid;
            Size = size;
            DesiredDirection = desiredDirection;

            Map = new int[size, size];
        }

        public bool Solve(int startX, int startY)
        {
            return Traverse(startX, startY);
        }

        private bool Traverse(int i, int j)
        {
            if (!IsValid(i, j))
            {
                return false;
            }

            if (IsEnd(i, j))
            {
                Map[i, j] = Path;
                return true;
            }
            else
            {
                Map[i, j] = Tried;
            }

            if (Traverse(i - 1, j))
            {
                Map[i - 1, j] = Path;
                return true;
            }

            if (Traverse(i, j + 1))
            {
                Map[i, j + 1] = Path;
                return true;
            }

            if (Traverse(i + 1, j))
            {
                Map[i + 1, j] = Path;
                return true;
            }

            if (Traverse(i, j - 1))
            {
                Map[i, j - 1] = Path;
                return true;
            }

            return false;
        }

        private bool IsEnd(int i, int j)
        {
            return DesiredDirection switch
            {
                Direction.Up => j == 0,
                Direction.Down => j == Size - 1,
                Direction.Left => i == 0,
                Direction.Right => i == Size - 1,
                _ => throw new NotImplementedException(),
            };
        }

        private bool IsValid(int i, int j)
        {
            if (InRange(i, j) && IsOpen(i, j) && !IsTried(i, j))
            {
                return true;
            }

            return false;
        }

        private bool IsOpen(int i, int j)
        {
            return !Grid[i, j];
        }

        private bool IsTried(int i, int j)
        {
            return Map[i, j] == Tried;
        }

        private bool InRange(int i, int j)
        {
            return i >= 0 && i < Size
                && j >= 0 && j < Size;
        }
    }
}
