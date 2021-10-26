using System;
using System.Collections.Generic;
using System.Text;

namespace Quoridor.Model
{
    public class SelectMenu
    {
        public event Action<SelectMenu> PositionChanged;

        public string Title { get; private set; }
        public string[] Options { get; private set; }
        public int Position { get; private set; }

        public SelectMenu(string title, params string[] options)
        {
            Title = title;
            Options = options;
        }

        public string CurrentOption()
        {
            return Options[Position];
        }

        public void BringToTop()
        {
            PositionChanged?.Invoke(this);
        }

        public void Next()
        {
            if (Position == Options.Length - 1) Position = 0;
            else Position++;

            PositionChanged?.Invoke(this);
        }

        public void Previous()
        {
            if (Position == 0) Position = Options.Length - 1;
            else Position--;

            PositionChanged?.Invoke(this);
        }
    }
}
