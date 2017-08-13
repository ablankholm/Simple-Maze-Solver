using System.ComponentModel;

namespace CSharpMazeSolver.Models
{
    public enum TileKind
    {
        [Description(" ")]
        Passage     = 0,
        [Description("#")]
        Wall        = 1,
        [Description("X")]
        Path        = 2,
        [Description("S")]
        PathStart   = 3,
        [Description("E")]
        PathEnd     = 4
    }
}
