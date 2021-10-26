using System;
using System.Collections.Generic;
using System.Text;

namespace Quoridor.Model
{
    public class MapMask
    {
        public List<(int, int)> Fields { get; set; }
        public MaskType Type { get; private set; }
        public bool WriteIndex { get; private set; }
        public bool? IsHorizontalWall { get; private set; }

        public MapMask(MaskType type, bool writeIndex = false, bool? isHorizontalWall = null)
        {
            Type = type;
            WriteIndex = writeIndex;
            IsHorizontalWall = isHorizontalWall;
        }

        public void FromWallSelection(WallSelection selection)
        {
            Fields = new List<(int, int)>()
            {
                selection.Coords, selection.IsHorizontal ?
                (selection.Coords.Item1 + 1, selection.Coords.Item2) :
                (selection.Coords.Item1, selection.Coords.Item2 + 1)
            };
            IsHorizontalWall = selection.IsHorizontal;
            Type = selection.IsValid ? MaskType.WallMask : MaskType.InvalidWallMask;
        }
    }
}
