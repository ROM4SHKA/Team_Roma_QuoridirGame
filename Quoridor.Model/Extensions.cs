using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Quoridor.Model
{
    public static class Extensions
    {
        public static (int, int) GetPlayerCoords(this int[,] field, int playerId)
        {
            int size = field.GetLength(0);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (field[i, j] == playerId) return (i, j);
                }
            }

            throw new InvalidOperationException($"There is no player with Id '{playerId}' on the map.");
        }

        public static void AddWallSelection(this bool[,] pathfindView, WallSelection selection)
        {
            int x = selection.Coords.Item1;
            int y = selection.Coords.Item2;

            if (selection.IsHorizontal)
            {
                for (int i = 0; i < 5; i++)
                {
                    pathfindView[x * 2 + i, (y + 1) * 2] = true;
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    pathfindView[(x + 1) * 2, y * 2 + i] = true;
                }
            }
        }
    }
}
