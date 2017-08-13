using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpMazeSolver.Models
{
    public enum ParsingStage
    {
        WidthAndHeight  = 0,
        StartTile       = 1,
        EndTile         = 2,
        Grid            = 3
    }
}
