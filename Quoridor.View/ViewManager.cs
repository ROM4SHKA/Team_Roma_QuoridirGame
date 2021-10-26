using Quoridor.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quoridor.View
{
    public class ViewManager
    {
        private readonly GameManager _GameManger;
        private readonly MapRenderer _MapRenderer;
        private readonly SelectMenuRenderer _SelectMenuRenderer;

        public ViewManager(GameManager gameManager)
        {
            _GameManger = gameManager;
            _GameManger.CurrentSelectMenuChanged += UpdateCurrentSelectMenuEvent;
            _GameManger.MapUpdated += DrawMap;
            _MapRenderer = new MapRenderer(_GameManger.Map);
            _SelectMenuRenderer = new SelectMenuRenderer();

            UpdateCurrentSelectMenuEvent();
        }

        void UpdateCurrentSelectMenuEvent()
        {
            if (_GameManger.CurrentSelectMenu != null)
            {
                _GameManger.CurrentSelectMenu.PositionChanged += DrawSelectMenu;
                DrawSelectMenu(_GameManger.CurrentSelectMenu);
            }
        }

        void DrawSelectMenu(SelectMenu menu)
        {
            _SelectMenuRenderer.Render(menu);
        }

        void DrawMap(MapMask[] mapMasks)
        {
            _MapRenderer.Render(mapMasks);
        }
    }
}
