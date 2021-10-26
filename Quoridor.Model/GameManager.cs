using Quoridor.Model.AIStrategies;
using System;
using System.Collections.Generic;

namespace Quoridor.Model
{
    public class GameManager
    {
        public event Action<MapMask[]> MapUpdated;
        public event Action CurrentSelectMenuChanged;

        private SelectMenu _selectMenu;
        public SelectMenu CurrentSelectMenu
        {
            get
            {
                return _selectMenu;
            }
            private set
            {
                _selectMenu = value;
                CurrentSelectMenuChanged?.Invoke();
            }
        }
        public Map Map { get; private set; }
        public Player CurrentPlayer { get; private set; }
        public GameState State { get; private set; } = GameState.MainMenu;
        public MapMask AvailFieldsForMove { get; private set; }
        public MapMask ChosenWallPosition { get; private set; }
        public WallSelection WallSelection { get; private set; }
        public bool OnePlayer { get; private set; }
        public IAIStrategy AIStrategy { get; private set; }

        public GameManager()
        {
            Map = new Map(
                new List<Player>()
                {
                    new Player(1, Direction.Up, ConsoleColor.Blue),
                    new Player(2, Direction.Down, ConsoleColor.Red),
                }, Settings.MapSize);
            Map.PlayerWon += ShowPlayerWon;

            AIStrategy = new RandomAIStrategy();
        }

        public void Initialize()
        {
            SetMainMenu();
        }

        private void StartNewGame(bool onePlayer = false)
        {
            OnePlayer = onePlayer;

            AvailFieldsForMove = null;
            ChosenWallPosition = null;
            CurrentPlayer = null;

            foreach (var player in Map.Players)
            {
                player.SetUpWallPairs();
            }

            Map.Initiallize();
            NextPlayer();
            StartMoveChoosing();
        }

        public bool ConsumeInput(Input input)
        {
            if (State == GameState.MainMenu)
            {              
                if (input == ConsoleKey.Enter)
                {

                    if (CurrentSelectMenu.Position == 0) // 1 player
                    {
                        StartNewGame(true);
                    }
                    else if (CurrentSelectMenu.Position == 1) // 2 players
                    {
                        StartNewGame();
                    }
                    else // exit
                    {
                        return false;
                    }
                }

                ChangeCurrentSelectMenuIfNeeded();
            }
            else if (input == ConsoleKey.Escape || State == GameState.PlayerWon)
            {
                SetMainMenu();
            }
            else if (State == GameState.MoveChoosing)
            {
                if (input.IsNumeric)
                {
                    if (input.Number < AvailFieldsForMove.Fields.Count)
                    {
                        Map.MovePlayer(CurrentPlayer, AvailFieldsForMove.Fields[input.Number]);
                        StartMoveChoosingForNextPlayer();
                    }
                }
                else if (input == ConsoleKey.Tab)
                {
                    StartWallChoosing();
                }
            }
            else if (State == GameState.WallChoosing)
            {
                if (input.IsDirectional)
                {
                    TryMoveWallSelection(input.Direction);
                }
                else if (input == ConsoleKey.Enter && WallSelection.IsValid && CurrentPlayer.WallPairsLeft > 0)
                {
                    SetWall();
                    StartMoveChoosingForNextPlayer();
                }
                else if (input == ConsoleKey.Q)
                {
                    RotateWall();
                }
                else if (input == ConsoleKey.Tab)
                {
                    StartMoveChoosing();
                }
            }

            void ChangeCurrentSelectMenuIfNeeded()
            {
                if (input == Direction.Down)
                {
                    CurrentSelectMenu.Next();
                }
                else if (input == Direction.Up)
                {
                    CurrentSelectMenu.Previous();
                }
            }

            return true;
        }

        public void ShowPlayerWon(Player player)
        {
            State = GameState.PlayerWon;
            CurrentSelectMenu = new SelectMenu($"Player {player.Id} won!", "OK");
        }

        private void SetWall()
        {
            Map.SetWall(WallSelection);
            CurrentPlayer.TakeAwayWallPair();
            ChosenWallPosition = null;
            UpdateMap();
        }

        private void TryMoveWallSelection(Direction direction)
        {
            if (WallSelection.Coords.Item1 == 0 && direction == Direction.Left ||
                WallSelection.Coords.Item2 == 0 && direction == Direction.Up ||
                WallSelection.Coords.Item1 == Map.Size - 2 && direction == Direction.Right ||
                WallSelection.Coords.Item2 == Map.Size - 2 && direction == Direction.Down) return;

            WallSelection.Move(direction);
            WallSelection.UpdateIsValid(Map);
            UpdateMap();
        }

        private void SetMainMenu()
        {
            State = GameState.MainMenu;
            CurrentSelectMenu = new SelectMenu("Welcome to THE QUORIDOR!", "1 Player", "2 Players", "Exit");
            CurrentSelectMenu.BringToTop();
        }

        private void NextPlayer()
        {
            CurrentPlayer = CurrentPlayer == null ? Map.Players[0] : Map.NextPlayer(CurrentPlayer);
        }

        private void StartMoveChoosingForNextPlayer()
        {
            NextPlayer();
            if (OnePlayer)
            {
                AIStrategy.PerformMove(CurrentPlayer, Map);
                NextPlayer();
            }
            if (State != GameState.PlayerWon) StartMoveChoosing();
        }

        private void StartMoveChoosing()
        {
            State = GameState.MoveChoosing;
            Map.Comments = new[]
            {
                $"> Player {CurrentPlayer.Id} <             {CurrentPlayer.WallPairsLeft,2} walls left",
                "",
                "Press button [0-9] to move to a field,",
                "Press [Tab] to switch to wall mode.",
            };

            ChosenWallPosition = null;
            WallSelection = null;
            AvailFieldsForMove = new MapMask(MaskType.MoveMask, true)
            {
                Fields = Map.GetPlayerAvailMoves(CurrentPlayer)
            };

            UpdateMap();
        }

        private void StartWallChoosing()
        {
            State = GameState.WallChoosing;
            Map.Comments = new[]
            {
                $"> Player {CurrentPlayer.Id} <             {CurrentPlayer.WallPairsLeft,2} walls left",
                "",
                "Press [WASD] to move the wall, [Q] to rotate it.",
                "Press [Tab] to switch to move mode.",
            };

            WallSelection = new WallSelection()
            {
                IsHorizontal = true,
                Coords = (Map.Size / 2, Map.Size / 2),
            };
            AvailFieldsForMove = null;
            ChosenWallPosition = new MapMask(MaskType.WallMask, false, WallSelection.IsHorizontal);

            WallSelection.UpdateIsValid(Map);
            UpdateMap();
        }

        private void RotateWall()
        {
            WallSelection.IsHorizontal = !WallSelection.IsHorizontal;
            WallSelection.UpdateIsValid(Map);
            UpdateMap();
        }

        private void UpdateMap()
        {
            var masks = new List<MapMask>();
            if (AvailFieldsForMove != null) masks.Add(AvailFieldsForMove);
            if (ChosenWallPosition != null)
            {
                ChosenWallPosition.FromWallSelection(WallSelection);
                masks.Add(ChosenWallPosition);
            }

            MapUpdated?.Invoke(masks.ToArray());
        }
    }
}
