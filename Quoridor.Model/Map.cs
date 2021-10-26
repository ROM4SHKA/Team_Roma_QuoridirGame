using System;
using System.Collections.Generic;
using System.Linq;

namespace Quoridor.Model
{
    public class Map
    {
        public event Action<Player> PlayerWon;

        public readonly int Size;
        public List<Player> Players { get; private set; }
        public int[,] PlayerFields { get; private set; }
        public bool[,] HorizontalWalls { get; private set; }
        public bool[,] VerticalWalls { get; private set; }
        public string[] Comments { get; set; }
        public bool[,] PathfindView { get; private set; }

        public Map(List<Player> players, int size = 9)
        {
            Size = size;
            Players = players;
            Initiallize();
        }

        public void Initiallize()
        {
            PlayerFields = new int[Size, Size];
            HorizontalWalls = new bool[Size, Size - 1];
            VerticalWalls = new bool[Size - 1, Size];
            PathfindView = new bool[2 * Size + 1, 2 * Size + 1];
            SetPlayersOnField();
        }

        private void SetPlayersOnField()
        {
            foreach (var player in Players)
            {
                if (player.WinDirection == Direction.Up)
                {
                    PlayerFields[Size / 2, Size - 1] = player.Id;
                }
                else if (player.WinDirection == Direction.Down)
                {
                    PlayerFields[Size / 2, 0] = player.Id;
                }
                else if (player.WinDirection == Direction.Left)
                {
                    PlayerFields[Size - 1, Size / 2] = player.Id;
                }
                else if (player.WinDirection == Direction.Right)
                {
                    PlayerFields[0, Size / 2] = player.Id;
                }
            }
        }

        public bool CheckIfPlayerWon(Player player)
        {
            if (player.WinDirection == Direction.Up)
            {
                for (int i = 0; i < Size; i++)
                {
                    if (PlayerFields[i, 0] == player.Id) return true;
                }
            }
            else if (player.WinDirection == Direction.Down)
            {
                for (int i = 0; i < Size; i++)
                {
                    if (PlayerFields[i, Size - 1] == player.Id) return true;
                }
            }

            return false;
        }

        public Player GetPlayerByIdOrDefault(int id)
        {
            return Players.SingleOrDefault(p => p.Id == id);
        }

        public void MovePlayer(Player player, (int, int) newCoords)
        {
            var oldCoords = PlayerFields.GetPlayerCoords(player.Id);

            PlayerFields[oldCoords.Item1, oldCoords.Item2] = 0;
            PlayerFields[newCoords.Item1, newCoords.Item2] = player.Id;

            if (CheckIfPlayerWon(player)) PlayerWon?.Invoke(player);
        }

        public List<(int, int)> GetPlayerAvailMoves(Player p)
        {
            var coords = PlayerFields.GetPlayerCoords(p.Id);

            List<(int, int)> availDirections = new List<(int, int)>();

            if (!(coords.Item1 == 0 || HasLeftWall(coords.Item1, coords.Item2)))
            {
                if (HasPlayer(coords.Item1 - 1, coords.Item2))
                {
                    if (InBounds(coords.Item1 - 2, coords.Item2) && !HasPlayer(coords.Item1 - 2, coords.Item2) && !HasLeftWall(coords.Item1 - 1, coords.Item2))
                    {
                        availDirections.Add((coords.Item1 - 2, coords.Item2));
                    }
                    else
                    {
                        if (InBounds(coords.Item1 - 1, coords.Item2 - 1) && !HasPlayer(coords.Item1 - 1, coords.Item2 - 1) && !HasUpperWall(coords.Item1 - 1, coords.Item2))
                        {
                            availDirections.Add((coords.Item1 - 1, coords.Item2 - 1));
                        }
                        if (InBounds(coords.Item1 - 1, coords.Item2 + 1) && !HasPlayer(coords.Item1 - 1, coords.Item2 + 1) && !HasBottomWall(coords.Item1 - 1, coords.Item2))
                        {
                            availDirections.Add((coords.Item1 - 1, coords.Item2 + 1));
                        }
                    }
                }
                else
                {
                    availDirections.Add((coords.Item1 - 1, coords.Item2));
                }
            }

            if (!(coords.Item1 == Size - 1 || HasRightWall(coords.Item1, coords.Item2)))
            {
                if (HasPlayer(coords.Item1 + 1, coords.Item2))
                {
                    if (InBounds(coords.Item1 + 2, coords.Item2) && !HasPlayer(coords.Item1 + 2, coords.Item2) && !HasRightWall(coords.Item1 + 1, coords.Item2))
                    {
                        availDirections.Add((coords.Item1 + 2, coords.Item2));
                    }
                    else
                    {
                        if (InBounds(coords.Item1 + 1, coords.Item2 - 1) && !HasPlayer(coords.Item1 + 1, coords.Item2 - 1) && !HasUpperWall(coords.Item1 + 1, coords.Item2))
                        {
                            availDirections.Add((coords.Item1 + 1, coords.Item2 - 1));
                        }
                        if (InBounds(coords.Item1 + 1, coords.Item2 + 1) && !HasPlayer(coords.Item1 + 1, coords.Item2 + 1) && !HasBottomWall(coords.Item1 + 1, coords.Item2))
                        {
                            availDirections.Add((coords.Item1 + 1, coords.Item2 + 1));
                        }
                    }
                }
                else
                {
                    availDirections.Add((coords.Item1 + 1, coords.Item2));
                }
            }

            if (!(coords.Item2 == 0 || HasUpperWall(coords.Item1, coords.Item2)))
            {
                if (HasPlayer(coords.Item1, coords.Item2 - 1))
                {
                    if (InBounds(coords.Item1, coords.Item2 - 2) && !HasPlayer(coords.Item1, coords.Item2 - 2) && !HasUpperWall(coords.Item1, coords.Item2 - 1))
                    {
                        availDirections.Add((coords.Item1, coords.Item2 - 2));
                    }
                    else
                    {
                        if (InBounds(coords.Item1 - 1, coords.Item2 - 1) && !HasPlayer(coords.Item1 - 1, coords.Item2 - 1) && !HasLeftWall(coords.Item1, coords.Item2 - 1))
                        {
                            availDirections.Add((coords.Item1 - 1, coords.Item2 - 1));
                        }
                        if (InBounds(coords.Item1 + 1, coords.Item2 - 1) && !HasPlayer(coords.Item1 + 1, coords.Item2 - 1) && !HasRightWall(coords.Item1, coords.Item2 - 1))
                        {
                            availDirections.Add((coords.Item1 + 1, coords.Item2 - 1));
                        }
                    }
                }
                else
                {
                    availDirections.Add((coords.Item1, coords.Item2 - 1));
                }
            }

            if (!(coords.Item2 == Size - 1 || HasBottomWall(coords.Item1, coords.Item2)))
            {
                if (HasPlayer(coords.Item1, coords.Item2 + 1))
                {
                    if (InBounds(coords.Item1, coords.Item2 + 2) && !HasPlayer(coords.Item1, coords.Item2 + 2) && !HasBottomWall(coords.Item1, coords.Item2 + 1))
                    {
                        availDirections.Add((coords.Item1, coords.Item2 + 2));
                    }
                    else
                    {
                        if (InBounds(coords.Item1 - 1, coords.Item2 + 1) && !HasPlayer(coords.Item1 - 1, coords.Item2 + 1) && !HasLeftWall(coords.Item1, coords.Item2 + 1))
                        {
                            availDirections.Add((coords.Item1 - 1, coords.Item2 + 1));
                        }
                        if (InBounds(coords.Item1 + 1, coords.Item2 + 1) && !HasPlayer(coords.Item1 + 1, coords.Item2 + 1) && !HasRightWall(coords.Item1, coords.Item2 + 1))
                        {
                            availDirections.Add((coords.Item1 + 1, coords.Item2 + 1));
                        }
                    }
                }
                else
                {
                    availDirections.Add((coords.Item1, coords.Item2 + 1));
                }
            }

            return availDirections;
        }

        private bool HasPlayer(int x, int y)
        {
            return PlayerFields[x, y] != 0;
        }

        private bool HasLeftWall(int x, int y)
        {
            return VerticalWalls[x - 1, y];
        }

        private bool HasRightWall(int x, int y)
        {
            return VerticalWalls[x, y];
        }

        private bool HasUpperWall(int x, int y)
        {
            return HorizontalWalls[x, y - 1];
        }

        private bool HasBottomWall(int x, int y)
        {
            return HorizontalWalls[x, y];
        }

        private bool InBounds(int x, int y)
        {
            return x >= 0 && x < Size && y >= 0 && y < Size;
        }

        public Player NextPlayer(Player currentPlayer)
        {
            int currenntIndex = Players.IndexOf(currentPlayer);
            if (currenntIndex == Players.Count - 1) return Players[0];
            else return Players[currenntIndex + 1];
        }

        public void SetWall(WallSelection selection)
        {
            int x = selection.Coords.Item1;
            int y = selection.Coords.Item2;

            if (selection.IsHorizontal)
            {
                HorizontalWalls[x, y] = true;
                HorizontalWalls[x + 1, y] = true;
            }
            else
            {
                VerticalWalls[x, y] = true;
                VerticalWalls[x, y + 1] = true;
            }

            PathfindView.AddWallSelection(selection);
        }
    }
}
