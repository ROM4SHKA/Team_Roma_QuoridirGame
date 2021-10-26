using System;
using System.Collections.Generic;
using System.Text;

namespace Quoridor.Model
{
#pragma warning disable CS0660 
#pragma warning disable CS0661 
    public struct Input
#pragma warning restore CS0661 
#pragma warning restore CS0660 
    {
        public InputType Type;

        public Direction Direction;
        public int Number;
        public ConsoleKey Key;

        public bool IsDirectional
        {
            get
            {
                return Type == InputType.Direction;
            }
        }

        public bool IsNumeric
        {
            get
            {
                return Type == InputType.Number;
            }
        }

        public static bool operator ==(Input input, ConsoleKey key)
        {
            return input.Type == InputType.Key && input.Key == key;
        }

        public static bool operator !=(Input input, ConsoleKey key)
        {
            return !(input == key);
        }

        public static bool operator ==(ConsoleKey key, Input input)
        {
            return input == key;
        }

        public static bool operator !=(ConsoleKey key, Input input)
        {
            return !(input == key);
        }

        public static bool operator ==(Input input, Direction direction)
        {
            return input.Type == InputType.Direction && input.Direction == direction;
        }

        public static bool operator !=(Input input, Direction direction)
        {
            return !(input == direction);
        }

        public static bool operator ==(Direction direction, Input input)
        {
            return input == direction;
        }

        public static bool operator !=(Direction direction, Input input)
        {
            return !(input == direction);
        }
    }

    public enum InputType
    {
        Invalid = 0,
        Direction,
        Number,
        Key,
    }
}
