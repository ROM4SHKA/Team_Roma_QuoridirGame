using Quoridor.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quoridor.View
{
    public class SelectMenuRenderer : Renderer
    {
        public SelectMenuRenderer() : base() { }

        public void Render(SelectMenu menu)
        {
            Console.BackgroundColor = Constants.BackgroundColor;
            Console.Clear();

            for (int i = 0; i < menu.Options.Length; i++)
            {
                Console.SetCursorPosition(offX, offY);
                Console.ForegroundColor = Constants.TextColor;
                Console.Write(menu.Title);
                Console.SetCursorPosition(offX, offY + 4 + 3 * i);
                if (i == menu.Position)
                {
                    Console.ForegroundColor = Constants.HighlightedTextColor;
                    Console.Write($"> {menu.Options[i]}");
                }
                else
                {
                    Console.ForegroundColor = Constants.TextColor;
                    Console.Write(menu.Options[i]);
                }
            }
        }
    }
}
