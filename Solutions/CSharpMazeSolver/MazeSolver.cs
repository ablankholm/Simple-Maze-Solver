using System;
using System.Collections.Generic;
using System.Linq;
using CSharpMazeSolver.Models;
using System.Drawing;

namespace CSharpMazeSolver
{
    public class MazeSolver
    {
        private Maze            challenge { get; set; }
        private Maze            solution { get; set; }
        private HashSet<Point>  deadEnds { get; set; }
        private List<Point>     path { get; set; }
        private bool            IsSolveable { get; set; }
        private bool            IsSolved { get; set; }



        public MazeSolver()
        {
            challenge   = new Maze();
            solution    = challenge;
            deadEnds    = new HashSet<Point>();
            path        = new List<Point>();
            IsSolveable = true;
            IsSolved    = false;
        }



        public MazeSolver(Maze maze)
        {
            challenge   = maze;
            solution    = challenge;
            deadEnds    = new HashSet<Point>();
            path        = new List<Point>();
            IsSolveable = true;
            IsSolved    = false;
        }



        public Maze Solve()
        {
            //Convenience
            Point goal = challenge.End;

            //Trackers
            Point currentTile = new Point();
            List<Point> neighbours = new List<Point>();

            //Set start of path
            path.Add(challenge.Start);

            //Setup initial tile to check
            path.Add(GetNextTile(path.Last()));

            //Backtracking best first search
            while (IsSolveable && !IsSolved)
            {
                //Testing / debug
                //Console.WriteLine(solution.GetLayoutString());

                //Get current tile
                currentTile = path.Last();
                solution[currentTile] = TileKind.Path;

                //Get neighbours
                neighbours = GetNeighbours(currentTile);

                //Check if goal reached
                if (neighbours.Contains(challenge.End))
                {
                    IsSolved = true;
                    solution.IsSolved = IsSolved;
                    solution.Path = path;
                    solution.Score = path.Count;
                    break;
                }

                //Prune neighbours
                neighbours = neighbours
                    .Where(n => challenge.ContainsKey(n) && IsPathable(n))
                    .ToList();

                //Update path
                if (neighbours.Count < 1)
                {
                    //Short-circuiting check
                    if (currentTile.Equals(challenge.Start))
                    {
                        IsSolveable = false;
                        solution.IsSolved = false;
                        break;
                    }
                    solution[currentTile] = TileKind.Passage;
                    path.Remove(currentTile);
                    deadEnds.Add(currentTile);
                }
                else
                {
                    path.Add(GetNextTile(neighbours));
                }
            }


            return solution;
        }



        //Returns the Manhattan distance between the origin and target tiles 
        public int GetTileDistance(Point origin, Point target, int stepCost = 1)
        {
            return stepCost * (Math.Abs(origin.X - target.X) + Math.Abs(origin.Y - target.Y));
        }



        public Point GetNeighbour(Point tile, Cardinality direction)
        {
            switch (direction)
            {
                case Cardinality.North:
                    return new Point()
                    {
                        X = tile.X,
                        Y = tile.Y - 1
                    };
                case Cardinality.South:
                    return new Point()
                    {
                        X = tile.X,
                        Y = tile.Y + 1
                    };
                case Cardinality.East:
                    return new Point()
                    {
                        X = tile.X + 1,
                        Y = tile.Y
                    };
                case Cardinality.West:
                    return new Point()
                    {
                        X = tile.X - 1,
                        Y = tile.Y
                    };
                default:
                    return tile;
            }
        }



        public List<Point> GetNeighbours(Point tile)
        {
            //foreach (var direction in Enum.GetValues(typeof(Cardinality)).Cast<IEnumerable<Cardinality>>())
            //{

            //}

            return new List<Point>()
            {
                GetNeighbour(tile, Cardinality.North),
                GetNeighbour(tile, Cardinality.South),
                GetNeighbour(tile, Cardinality.East),
                GetNeighbour(tile, Cardinality.West)
            };
        }



        public Point GetNextTile(Point currentTile)
        {
            return GetNeighbours(currentTile)
                .Where(t => challenge.ContainsKey(t))
                .OrderBy(t => GetTileDistance(t, challenge.End))
                .FirstOrDefault();
        }



        public Point GetNextTile(List<Point> neighbours)
        {
            return neighbours
                .OrderBy(t => GetTileDistance(t, challenge.End))
                .FirstOrDefault();
        }



        public bool IsPathable(Point tile)
        {
            if (challenge[tile] == TileKind.Passage 
                && deadEnds.Contains(tile) == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
