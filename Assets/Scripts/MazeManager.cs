using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Mode {
    Basic,
    Custom
}
public class MazeManager : MonoBehaviour
{
    public Mode mode;
    public GameObject start;
    public float threshold = 0.3f;
    public float baseChance = 0f;
    public int subBranchDepth = 5;
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject doorPrefab;

    [Header("Basic Mode Parameters")]
    public int width = 10;
    public int height = 10;
    public Vector2Int end;

    [Header("Custom Mode Parameters")]
    public int leftPadding = 10;
    public int rightPadding = 10;
    public int forwardPadding = 10;
    public int backwardPadding = 0;
    public Vector2Int endOffset;


    float tileCount;
    float totalArea;
    private int[,] maze;

    void Start()
    {
        if(mode == Mode.Basic) {
          GenerateMaze();
        } else {
          GenerateMazeVariant();
        }
    }

    void GenerateMaze()
    {
        int w = width;
        int h = height;
        tileCount = 0;
        totalArea = w * h;
        maze = new int[h * 2 + 1, w * 2 + 1];

        // Initialize Wall Grid
        for (int y = 0; y < h * 2 + 1; y++)
            for (int x = 0; x < w * 2 + 1; x++)
                maze[y, x] = 1;

        // Dig tunnel
        bool[,] visited = new bool[h, w];
        List<Vector2Int> path = new List<Vector2Int>();
        DFSGoal(0, 0, visited, end, path);

        // To generate full maze config, just use code below
        //DFS(0, 0, visited);

        // Set path to destination visited and redig other branches 
        path.Reverse();
        visited = new bool[h, w];

        // Count path to tiles 
        tileCount += path.Count;

        CreateSubBranch(path, visited);

        RenderMaze();
    }


    void GenerateMazeVariant()
    {
        int w = leftPadding + rightPadding + 1;
        int h = backwardPadding + forwardPadding + 1;
        tileCount = 0;
        totalArea = w * h;
        maze = new int[h * 2 + 1, w * 2 + 1];

        // Initialize Wall Grid
        for (int y = 0; y < h * 2 + 1; y++)
            for (int x = 0; x < w * 2 + 1; x++)
                maze[y, x] = 1;

        // Dig tunnel
        bool[,] visited = new bool[h, w];
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int endIndex = new Vector2Int(leftPadding, 0) + endOffset;
        DFSGoal(leftPadding, backwardPadding, visited, endIndex, path);

        // Set path to destination visited and redig other branches 
        path.Reverse();
        visited = new bool[h, w];

        // Count path to tiles 
        tileCount += path.Count;

        CreateSubBranch(path, visited);

        RenderMaze();
    }


    public void Generate(int width, int height, Vector2Int start, Vector2Int end)
    {
        // 0 - Wall, 1 - Path
        int[,] array = new int[width, height];

        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                array[i, j] = 0;
            }
        } 

    }


    public void DFS(int cx, int cy, bool[,] visited)
    {
        int w = visited.GetLength(1);
        int h = visited.GetLength(0);
        visited[cy, cx] = true;
        maze[cy * 2 + 1, cx * 2 + 1] = 0; // 当前路径格子

        List<Vector2Int> dirs = new List<Vector2Int> {
            Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left
        };
        Shuffle(dirs);

        foreach (var dir in dirs)
        {
            int nx = cx + dir.x;
            int ny = cy + dir.y;
            if (nx >= 0 && ny >= 0 && nx < w && ny < h && !visited[ny, nx])
            {
                // 打通墙体
                int wx = cx + nx + 1;
                int wy = cy + ny + 1;
                maze[wy, wx] = 0;
                
                DFS(nx, ny, visited);
            }
        }
    }


    public bool DFSGoal(int cx, int cy, bool[,] visited, Vector2Int goal, List<Vector2Int> path)
    {
        int w = visited.GetLength(1);
        int h = visited.GetLength(0);
        visited[cy, cx] = true;

        if (cx == goal.x && cy == goal.y)
        {
            Debug.Log("Found");
            maze[cy * 2 + 1, cx * 2 + 1] = 0;
            path.Add(goal);
            return true;
        }
        //maze[cy * 2 + 1, cx * 2 + 1] = 0; // 当前路径格子

        List<Vector2Int> dirs = new List<Vector2Int> {
            Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left
        };
        Shuffle(dirs);

        foreach (var dir in dirs)
        {
            int nx = cx + dir.x;
            int ny = cy + dir.y;
            if (nx >= 0 && ny >= 0 && nx < w && ny < h && !visited[ny, nx])
            {
                if(DFSGoal(nx, ny, visited, goal, path))
                {
                    int wallX = cx * 2 + 1 + dir.x;
                    int wallY = cy * 2 + 1 + dir.y;
                    maze[cy * 2 + 1, cx * 2 + 1] = 0;
                    maze[wallY, wallX] = 0;
                    path.Add(new Vector2Int(nx, ny));
                    return true;
                }       
            }
        }

        return false;
    }


    public void Redig(int cx, int cy, bool[,] visited, Vector2Int dir, float chance, int depth)
    {
        if(depth >= subBranchDepth || tileCount/totalArea > threshold)
        {
            return;
        }

        int w = visited.GetLength(1);
        int h = visited.GetLength(0);
        
        visited[cy, cx] = true;
        maze[cy * 2 + 1, cx * 2 + 1] = 0; // Current grid index
        tileCount++;

        List<Vector2Int> dirs = new List<Vector2Int> {
            Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left
        };

        // Filter out all invalid move
        for(int i = 0; i < dirs.Count; i++)
        {
            Vector2Int potentialDir = dirs[i];
            int nx = cx + potentialDir.x;
            int ny = cy + potentialDir.y;
            if (nx < 0 || ny < 0 || nx >= w || ny >= h || visited[ny, nx])
            {
                dirs.Remove(potentialDir);
                i -= 1;
            }
        }

        // If no possible move, stop digging
        if(dirs.Count == 0)
        {
            return;
        }

        int ty = cy + dir.y;
        int tx = cx + dir.x;
        // Check if we need to change direction or stay the same
        if (tx >= 0 && ty >= 0 && tx < w && ty < h && !visited[ty, tx] && Random.Range(0, 1.0f) >= chance)
        {
            int wallX = cx * 2 + 1 + dir.x;
            int wallY = cy * 2 + 1 + dir.y;
            maze[wallY, wallX] = 0;

            int nx = cx + dir.x;
            int ny = cy + dir.y;
            Redig(nx, ny, visited, dir, chance + 0.1f, depth);
        }
        else
        {
            Vector2Int nDir = dirs[Random.Range(0, dirs.Count)];
            int wallX = cx * 2 + 1 + nDir.x;
            int wallY = cy * 2 + 1 + nDir.y;
            maze[wallY, wallX] = 0;

            int nx = cx + nDir.x;
            int ny = cy + nDir.y;
            Redig(nx, ny, visited, nDir, baseChance, depth);
        }
    }

    public void CreateSubBranch(List<Vector2Int> existPath, bool[,] visited)
    {
        foreach (var p in existPath)
        {
            visited[p.y, p.x] = true;
        }

        List<Vector2Int> dirs = new List<Vector2Int> {
            Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left
        };

        float chance = baseChance;

        foreach (var p in existPath)
        {
            if (Random.Range(0, 1.0f) <= chance)
            {
                Redig(p.x, p.y, visited, dirs[Random.Range(0, 4)], baseChance, 0);
                chance = 0;
            }
            else
            {
                chance += 0.05f;
            }
        }
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[rnd];
            list[rnd] = temp;
        }
    }

    void RenderMaze()
    {
        for (int y = 0; y < maze.GetLength(0); y++)
        {
            for (int x = 0; x < maze.GetLength(1); x++)
            {
                
                Vector3 pos = new Vector3(x, -1, y);
                if(mode == Mode.Custom) {
                    pos += new Vector3(-2*leftPadding, 0, -2*backwardPadding);
                }
                    
                if (maze[y, x] == 1)
                {
                    //Instantiate(wallPrefab, pos, Quaternion.identity, transform);
                }
                else
                {
                    //pos += start.transform.position;
                    Instantiate(floorPrefab, pos, Quaternion.identity);
                } 
            }
        }

        Vector3 doorPos = new Vector3(endOffset.x, 0, endOffset.y);
        Instantiate(doorPrefab, doorPos, Quaternion.identity);
    }
}
