using CSharpMazeSolver.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CSharpMazeSolver
{
    class Program
    {
        private static string appPath = Application.StartupPath;

        //For controlling console ui maximization
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;



        static void Main(string[] args)
        {
            Program         app             = new Program();
            List<string>    inputFiles      = new List<string>();
            string          mazeChoices     = string.Empty;
            int             mazeFileNumber  = int.MaxValue;
            Maze            mazeToSolve     = new Maze();

            //Load sample data
            inputFiles.Add(appPath + @"\App_Data\input.txt");
            inputFiles.Add(appPath + @"\App_Data\large_input.txt");
            inputFiles.Add(appPath + @"\App_Data\medium_input.txt");
            inputFiles.Add(appPath + @"\App_Data\small.txt");
            inputFiles.Add(appPath + @"\App_Data\sparse_medium.txt");

            //Configure console window to properly display larger samples outputs
            //May still not display properly on small displays
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);
            Console.BufferHeight = Int16.MaxValue - 1;

            
            //Generate maze options
            for (int i = 0; i < inputFiles.Count; i++)
            {
                mazeChoices += $"{i}: {inputFiles[i]}" + Environment.NewLine;
            }

            //Ask the user to pick a maze to solve
            Console.WriteLine(
                Environment.NewLine + "Hi!"
                + Environment.NewLine + "The following mazes are available to solve:" + Environment.NewLine
                + Environment.NewLine + mazeChoices
                + Environment.NewLine + "Please enter the number of the maze you'd like to solve..."
                );

            if (int.TryParse(Console.ReadLine(), out mazeFileNumber))
            {
                if (mazeFileNumber < inputFiles.Count)
                {
                    //Try to solve selected maze
                    mazeToSolve = MazeFile.Parse(inputFiles[mazeFileNumber]);
                    Console.WriteLine(Environment.NewLine + $"Attempting to solve {mazeToSolve.Label} with the following layout:");
                    Console.WriteLine(Environment.NewLine + mazeToSolve.GetLayoutString());

                    MazeSolver solver = new MazeSolver(mazeToSolve);
                    Maze solution = solver.Solve();

                    if (solution.IsSolved)
                    {
                        Console.WriteLine(Environment.NewLine + "Maze solved:");
                        solution.PrettyPrint();
                    }
                    else
                    {
                        Console.WriteLine(Environment.NewLine + "Maze not solveable.");
                    }

                    Console.WriteLine(Environment.NewLine + "Press any key to exit...");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine(Environment.NewLine + $"{mazeFileNumber} is not a valid option.");
                    Console.WriteLine(Environment.NewLine + "Press any key to exit...");
                    Console.ReadLine();
                    return;
                }
            }
            else
            {
                Console.WriteLine(Environment.NewLine + "Invalid input.");
                Console.WriteLine(Environment.NewLine + "Press any key to exit...");
                Console.ReadLine();
                return;
            }
        }
    }
}
