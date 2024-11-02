using System;
using System.Collections.Generic;
using System.Threading;

class ConsoleMazeSolver
{
    //this sets up the true or falses to see if the spot has been visited
    private const int MazeWidth = 21;
    private const int MazeHeight = 21;
    private static bool[,] maze;
    private static bool[,] visited;
    private static Stack<(int x, int y)> dfsStack;
    private static Queue<(int x, int y)> bfsQueue;
    private static bool useDFS;

    public static void Main()
    {
        Console.CursorVisible = false;
        GenerateMaze();
        DisplayMaze();

        // Ask the user to choose between DFS and BFS alg
        Console.WriteLine("Choose a search algorithm to solve the maze:");
        Console.WriteLine("1. DFS (Depth-First Search)");
        Console.WriteLine("2. BFS (Breadth-First Search)");
        Console.Write("Enter 1 or 2: ");

        // Get chioce and set algoritum
        string choice = Console.ReadLine();
        if (choice == "1")
        {
            useDFS = true;
            StartDFS();
        }
        else if (choice == "2")
        {
            useDFS = false;
            StartBFS();
        }
        else
        {
            Console.WriteLine("Invalid choice. Please restart and enter 1 or 2.");
            return;
        }

        // Keep running for animation
        Console.ReadKey();
    }

    // this generates a random maze, I used a combo of ChatGPT and youtube to fighure this out
    private static void GenerateMaze()
    {
        maze = new bool[MazeWidth, MazeHeight];

        visited = new bool[MazeWidth, MazeHeight];

        for (int x = 0; x < MazeWidth; x++)
            for (int y = 0; y < MazeHeight; y++)
                maze[x, y] = true;

        Random rand = new Random();
        void CarvePath(int x, int y)
        {
            maze[x, y] = false;
            var directions = new List<(int dx, int dy)> { (0, -2), (-2, 0), (0, 2), (2, 0) };

            directions.Sort((a, b) => rand.Next().CompareTo(rand.Next()));

            foreach (var (dx, dy) in directions)
            {
                int nx = x + dx, ny = y + dy;

                if (nx > 0 && ny > 0 && nx < MazeWidth - 1 && ny < MazeHeight - 1 && maze[nx, ny])
                {
                    maze[x + dx / 2, y + dy / 2] = false;
                    CarvePath(nx, ny);
                }
            }
        }
        CarvePath(1, 1);
    }

    //This gets was was generated and displates
    private static void DisplayMaze()
    {
        Console.Clear();
        for (int y = 0; y < MazeHeight; y++)
        {
            for (int x = 0; x < MazeWidth; x++)
            {
                if (x == 1 && y == 1)
                    Console.Write("S"); // Starting point
                else if (x == MazeWidth - 2 && y == MazeHeight - 2)
                    Console.Write("E"); // Ending point
                else
                    Console.Write(maze[x, y] ? "#" : " ");
            }
            Console.WriteLine();
        }
    }

    //these start the algs
    private static void StartDFS()
    {
        dfsStack = new Stack<(int, int)>();
        dfsStack.Push((1, 1));
        AnimateDFS();
    }

    private static void StartBFS()
    {
        bfsQueue = new Queue<(int, int)>();
        bfsQueue.Enqueue((1, 1));
        AnimateBFS();
    }

    //this animates DFS, draws a * where its been and makes that spot visited
    private static void AnimateDFS()
    {
        while (dfsStack.Count > 0)
        {
            var (x, y) = dfsStack.Pop();
            if (x == MazeWidth - 2 && y == MazeHeight - 2) break;

            visited[x, y] = true;
            DrawPosition(x, y, '*');

            foreach (var (dx, dy) in new (int, int)[] { (0, -1), (-1, 0), (0, 1), (1, 0) })
            {
                int nx = x + dx, ny = y + dy;
                if (IsInBounds(nx, ny) && !visited[nx, ny] && !maze[nx, ny]) 

                {
                    dfsStack.Push((nx, ny));
                }
            }
            Thread.Sleep(50); 
        }
    }
    //same with BFS
    private static void AnimateBFS()
    {
        while (bfsQueue.Count > 0)
        {
            var (x, y) = bfsQueue.Dequeue();
            if (x == MazeWidth - 2 && y == MazeHeight - 2) break;

            visited[x, y] = true;
            DrawPosition(x, y, '*');

            foreach (var (dx, dy) in new (int, int)[] { (0, -1), (-1, 0), (0, 1), (1, 0) })
            {
                int nx = x + dx, ny = y + dy;
                if (IsInBounds(nx, ny) && !visited[nx, ny] && !maze[nx, ny])

                {
                    bfsQueue.Enqueue((nx, ny));
                }
            }
            Thread.Sleep(50); // Delay for visualization
        }
    }

    private static void DrawPosition(int x, int y, char symbol)
    {

        Console.SetCursorPosition(x, y);

        Console.Write(symbol);

    }
    //this checks to make sure stuff stays in the maze
    private static bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < MazeWidth && y >= 0 && y < MazeHeight;
    }
}
