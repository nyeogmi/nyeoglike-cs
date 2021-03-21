using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.Level.Elements {
    public enum BlockType { 
        Normal = 0,
        Exit = 1
    }

    public struct Block: IComparable<Block> {
        public BlockType Type;

        public static Block Normal => new Block { Type = BlockType.Normal };
        public static Block Exit => new Block { Type = BlockType.Exit };

        public int CompareTo(Block other) => Type.CompareTo(other.Type);
    }
}
