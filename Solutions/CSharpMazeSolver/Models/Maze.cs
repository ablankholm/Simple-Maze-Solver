using System;
using System.Collections.Generic;
using System.Drawing;

namespace CSharpMazeSolver.Models
{
    public class Maze : Dictionary<Point, TileKind>
    {
        public string           Label { get; set; }
        public int              Width { get; set; }
        public int              Height { get; set; }
        public Point            Start { get; set; } = new Point();
        public Point            End { get; set; } = new Point();
        public int              Score { get; set; } = int.MaxValue;
        public bool             IsSolved { get; set; } = true;
        public List<Point>      Path { get; set; } = new List<Point>();



        //Returns a string representation of the tile value formatted for output. 
        public string GetTileString(TileKind tile)
        {
            //A switch pattern, though long-winded, is used deliberately for clarity
            switch (tile)
            {
                case TileKind.Passage:
                    return " ";
                case TileKind.Wall:
                    return "#";
                case TileKind.Path:
                    return "X";
                case TileKind.PathStart:
                    return "S";
                case TileKind.PathEnd:
                    return "E";
                default:
                    return "%";
            }
        }



        public string GetLayoutString()
        {
            string output = string.Empty;
            Point tile = new Point();

            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    tile = new Point()
                    {
                        X = x,
                        Y = y
                    };
                    if (this.ContainsKey(tile))
                    {
                        output = output + GetTileString(this[tile]);
                    }
                }

                output = output + Environment.NewLine;
            }

            return output;
        }



        public void PrettyPrint()
        {
            Point tile = new Point();

            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    tile = new Point()
                    {
                        X = x,
                        Y = y
                    };
                    if (this.ContainsKey(tile))
                    {
                        switch (this[tile])
                        {
                            case TileKind.Passage:
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.BackgroundColor = ConsoleColor.Blue;

                                Console.Write(GetTileString(TileKind.Passage));

                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.BackgroundColor = ConsoleColor.Black;
                                break;
                            case TileKind.Wall:
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.BackgroundColor = ConsoleColor.Black;

                                Console.Write(GetTileString(TileKind.Wall));

                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.BackgroundColor = ConsoleColor.Black;
                                break;
                            case TileKind.Path:
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.BackgroundColor = ConsoleColor.DarkGreen;

                                Console.Write(GetTileString(TileKind.Path));

                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.BackgroundColor = ConsoleColor.Black;
                                break;
                            case TileKind.PathStart:
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.BackgroundColor = ConsoleColor.Cyan;

                                Console.Write(GetTileString(TileKind.PathStart));

                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.BackgroundColor = ConsoleColor.Black;
                                break;
                            case TileKind.PathEnd:
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.BackgroundColor = ConsoleColor.Magenta;

                                Console.Write(GetTileString(TileKind.PathEnd));

                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.BackgroundColor = ConsoleColor.Black;
                                break;
                            default:
                                break;
                        }
                    }
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(Environment.NewLine);
            }
        }



        public void PlacePathOnGrid()
        {
            foreach (var tile in Path)
            {
                this[tile] = TileKind.Path;
            }
        }
    }
}
