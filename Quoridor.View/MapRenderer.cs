using Quoridor.Model;
using System;

namespace Quoridor.View
{
    public class MapRenderer : Renderer
    {
        private readonly Map Map;

        public MapRenderer(Map map) : base()
        {
            Map = map;
        }

        public void Render(MapMask[] mapMasks = null)
        {
            Console.Clear();

            for (int x = 0; x < Map.Size; x++)
            {
                for (int y = 0; y < Map.Size; y++)
                {
                    RenderField(x, y, Map.GetPlayerByIdOrDefault(Map.PlayerFields[x, y]));
                    if (x < Map.Size - 1 && Map.VerticalWalls[x, y]) RenderVertWall(x, y);
                    if (y < Map.Size - 1 && Map.HorizontalWalls[x, y]) RenderHorWall(x, y);
                }
            }

            if (mapMasks != null)
            {
                foreach (var mask in mapMasks)
                {
                    RenderMapMask(mask);
                }
            }

            RenderComments(Map.Comments);
        }

        private void RenderMapMask(MapMask mask)
        {
            ConsoleColor color = mask.Type switch
            {
                MaskType.MoveMask => Constants.MoveMaskColor,
                MaskType.WallMask => Constants.WallMaskColor,
                MaskType.InvalidWallMask => Constants.InvalidWallMaskColor,
                _ => throw new NotImplementedException(),
            };

            Console.BackgroundColor = color;

            for (int i = 0; i < mask.Fields.Count; i++)
            {
                int x = mask.Fields[i].Item1;
                int y = mask.Fields[i].Item2;
                if (mask.IsHorizontalWall == null)
                {
                    Console.SetCursorPosition(offX + 4 * x, offY + 2 * y);
                    Console.Write(mask.WriteIndex ? " " + i : "  ");
                }
                else
                {
                    if (mask.IsHorizontalWall == true)
                    {
                        RenderHorWall(x, y, color);
                    }
                    else
                    {
                        RenderVertWall(x, y, color);
                    }
                }
            }
        }

        private void RenderField(int x, int y, Player player)
        {
            bool isFree = player == null;
            Console.SetCursorPosition(offX + 4 * x, offY + 2 * y);
            if (isFree)
            {
                Console.BackgroundColor = Constants.FieldColor;
                Console.Write("  ");
            }
            else
            {
                Console.BackgroundColor = player.Color;
                Console.ForegroundColor = Constants.TextColor;
                Console.Write($"P{player.Id}");
            }
        }

        private void RenderHorWall(int x, int y, ConsoleColor color = Constants.WallColor)
        {
            Console.SetCursorPosition(offX + 4 * x, offY + 2 * y + 1);
            Console.BackgroundColor = Constants.BackgroundColor;
            Console.ForegroundColor = color;
            Console.Write($"──");
        }

        private void RenderVertWall(int x, int y, ConsoleColor color = Constants.WallColor)
        {
            Console.SetCursorPosition(offX + 4 * x + 2, offY + 2 * y);
            Console.BackgroundColor = Constants.BackgroundColor;
            Console.ForegroundColor = color;
            Console.Write($" │");
        }

        private void RenderComments(string[] comments)
        {
            Console.BackgroundColor = Constants.BackgroundColor;
            Console.ForegroundColor = Constants.SecondaryTextColor;

            for (int i = 0; i < comments.Length; i++)
            {
                Console.SetCursorPosition(offX, offY + 2 * Map.Size + 2 + i);
                Console.Write(comments[i]);
            }
        }
    }
}
