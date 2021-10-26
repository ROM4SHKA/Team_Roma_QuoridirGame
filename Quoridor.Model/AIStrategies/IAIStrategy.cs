using System;
using System.Collections.Generic;
using System.Text;

namespace Quoridor.Model.AIStrategies
{
    public interface IAIStrategy
    {
        void PerformMove(Player player, Map map);
    }
}
