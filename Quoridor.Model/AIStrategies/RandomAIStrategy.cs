using System;
using System.Collections.Generic;
using System.Text;

namespace Quoridor.Model.AIStrategies
{
    public class RandomAIStrategy : IAIStrategy
    {
        private Random rand = new Random();

        public void PerformMove(Player player, Map map)
        {
            if (player.WallPairsLeft == 0 || rand.NextDouble() > 0.5)
            {
                MovePlayer();
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    var selection = new WallSelection()
                    {
                        IsHorizontal = rand.NextDouble() > 0.5,
                        Coords = (rand.Next(map.Size - 1), rand.Next(map.Size - 1)),
                    };

                    selection.UpdateIsValid(map);

                    if (selection.IsValid)
                    {
                        player.TakeAwayWallPair();
                        map.SetWall(selection);
                        return;
                    }
                }

                MovePlayer();
            }

            void MovePlayer()
            {
                var availMoves = map.GetPlayerAvailMoves(player);
                map.MovePlayer(player, availMoves[rand.Next(availMoves.Count)]);
            }
        }
    }
}
