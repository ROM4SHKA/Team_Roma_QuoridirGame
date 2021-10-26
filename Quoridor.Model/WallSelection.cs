using System;
using System.Collections.Generic;
using System.Text;

namespace Quoridor.Model
{
    public class WallSelection
    {
        public bool IsHorizontal { get; set; }
        public bool IsValid { get; private set; } = true;
        public (int, int) Coords { get; set; }

        public void Move(Direction direction)
        {
            if (direction == Direction.Up) Coords = (Coords.Item1, Coords.Item2 - 1);
            else if (direction == Direction.Down) Coords = (Coords.Item1, Coords.Item2 + 1);
            else if (direction == Direction.Left) Coords = (Coords.Item1 - 1, Coords.Item2);
            else if (direction == Direction.Right) Coords = (Coords.Item1 + 1, Coords.Item2);
        }
        
        public void UpdateIsValid(Map map)
        {
            IsValid = CheckIsValid(map);
        }

        private bool CheckIsValid(Map map)
        {
            if ((IsHorizontal && (map.HorizontalWalls[Coords.Item1, Coords.Item2] || map.HorizontalWalls[Coords.Item1 + 1, Coords.Item2]))
                || (!IsHorizontal && (map.VerticalWalls[Coords.Item1, Coords.Item2] || map.VerticalWalls[Coords.Item1, Coords.Item2 + 1])))
            {
                return false;
            }

            bool[,] grid = new bool[map.PathfindView.GetLength(0), map.PathfindView.GetLength(1)];
            Array.Copy(map.PathfindView, grid, map.PathfindView.Length);
            grid.AddWallSelection(this);

            foreach (var player in map.Players)
            {
                var playerCoords = map.PlayerFields.GetPlayerCoords(player.Id);
                if (!new PathFinder(grid, player.WinDirection).Solve(playerCoords.Item1 * 2 + 1, playerCoords.Item2 * 2 + 1))
                    return false;
            }

            return true;
        }
    }
}
