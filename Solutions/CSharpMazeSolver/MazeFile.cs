using System;
using System.Collections.Generic;
using System.Linq;
using CSharpMazeSolver.Models;
using System.IO;
using System.Drawing;

namespace CSharpMazeSolver
{
    public class MazeFile
    {
        public static string Path;



        public MazeFile(string path)
        {
            Path = path;
        }



        public static Maze Parse()
        {
            return Parse(Path);
        }



        public static Maze Parse(string pathToMazeFile)
        {
            Maze maze = new Maze();
            List<int> numbers = new List<int>();
            ParsingStage parsingStage = ParsingStage.WidthAndHeight;
            int rowCount = 0;


            //Using File.ReadLines to keep memory consumption low if dealing with large files
            foreach (var line in File.ReadLines(pathToMazeFile))
            {
                switch (parsingStage)
                {
                    //Parse first line and update the width, height and name of the maze
                    case ParsingStage.WidthAndHeight:
                        maze.Label = pathToMazeFile;
                        numbers = line.Split(' ').Select(Int32.Parse).ToList();
                        maze.Width = numbers[0];
                        maze.Height = numbers[1];


                        //Transtion stage
                        parsingStage = ParsingStage.StartTile;
                        break;


                    //Parse second line and update maze starting position
                    case ParsingStage.StartTile:
                        numbers = line.Split(' ').Select(Int32.Parse).ToList();
                        maze.Start = new Point()
                        {
                            X = numbers[0],
                            Y = numbers[1]
                        };

                        //Transtion stage
                        parsingStage = ParsingStage.EndTile;
                        break;


                    //Parse third line and update maze end position
                    case ParsingStage.EndTile:
                        numbers = line.Split(' ').Select(Int32.Parse).ToList();
                        maze.End = new Point()
                        {
                            X = numbers[0],
                            Y = numbers[1]
                        };

                        //Transtion stage
                        parsingStage = ParsingStage.Grid;
                        break;

                    //Parse all remaining lines adding each to the maze grid in turn
                    case ParsingStage.Grid:
                        numbers = line.Split(' ').Select(Int32.Parse).ToList();
                        int colCount = 0;
                        Point tile = new Point();

                        foreach (var number in numbers)
                        {
                            tile = new Point()
                            {
                                X = colCount,
                                Y = rowCount
                            };

                            maze.Add(
                                tile, 
                                (TileKind)number);

                            colCount++;
                        }

                        rowCount++;
                        break;


                    default:
                        break;
                }
            }

            //Updatet the start and end tiles on the grid
            maze[maze.Start]    = TileKind.PathStart;
            maze[maze.End]      = TileKind.PathEnd;

            return maze;
        }
    }
}
