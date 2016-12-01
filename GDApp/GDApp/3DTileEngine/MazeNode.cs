using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;


/*
    //Usage Example:
    int width = 10, height = 8;
    int offValue = 0, visitedValue = 8;
    int[,] maze = {
                                    {0,0,0,1,0,0,0,0,0,0},
                                    {1,1,1,1,0,0,0,0,0,0},
                                    {0,0,0,1,1,1,1,1,0,0},
                                    {0,0,0,0,0,1,0,1,0,0},
                                    {0,0,0,0,0,0,0,1,0,1},
                                    {0,0,0,1,1,1,1,1,0,1},
                                    {0,0,0,0,0,1,0,1,0,1},
                                    {0,0,0,0,0,0,0,1,1,1},
                              };

    MazeNode start = new MazeNode(1, 0); //start
    MazeNode end = new MazeNode(4,9);    //end
    System.Diagnostics.Debug.WriteLine(MazeNode.AreConnected(start, end, ref maze, width, height, offValue, visitedValue));
 
 */

namespace GDLibrary
{
    public enum PathStatusType : sbyte
    {
        Processing,
        Connected,
        NotConnected
    }

    public class MazeNode
    {
        #region Variables
        private int x, y;
        private bool bProcessed;
        static readonly Vector2[] directionOffsets = { -Vector2.UnitX, Vector2.UnitY, Vector2.UnitX, -Vector2.UnitY };
        #endregion

        #region Properties
        public bool Processed
        {
            get
            {
                return this.bProcessed;
            }
            set
            {
                this.bProcessed = value;
            }
        }
        public int X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = (value >= 0) ? value : 0;
            }
        }
        public int Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = (value >= 0) ? value : 0;
            }
        }
        #endregion

        public MazeNode()
            : this(0, 0)
        {
        }
        public MazeNode(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.bProcessed = false;
        }

        public override string ToString()
        {
            return "[" + this.x + "," + this.y + "]";
        }

        public List<MazeNode> GetValidConnectedNodesExcl(ref MazeNode excludeNode, ref int[,] maze, 
                                int width, int height, int offValue, int visitedValue)
        {
            MazeNode newNode = null;
            int row = 0, col = 0;
            List<MazeNode> connectedNodes = new List<MazeNode>();
            foreach (Vector2 offset in directionOffsets)
            {
                row = (int)(this.x + offset.X);
                col = (int)(this.y + offset.Y);

                if ((row >= 0) && (row < height) && (col >= 0) && (col < width))
                {
                    if ((maze[row, col] != offValue) && (maze[row, col] != visitedValue))
                    {
                        newNode = new MazeNode(row, col);
                        if (!newNode.Equals(excludeNode) && (!newNode.Processed))
                            connectedNodes.Add(new MazeNode(row, col));
                    }
                }
            }
            return connectedNodes;
        }

        public override int GetHashCode()
        {
            return this.x.GetHashCode() + 13 * this.y.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            MazeNode other = obj as MazeNode;
            return this.x == other.X
                    && this.y == other.Y;
        }

        public static void PrintMaze(ref int[,] maze, int width, int height, MazeNode currentNode, MazeNode previousNode)
        {
            System.Diagnostics.Debug.Write("\n\n");
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    if (currentNode.X == row && currentNode.Y == col)
                        System.Diagnostics.Debug.Write("X ");
                    else
                        System.Diagnostics.Debug.Write(maze[row, col] + " ");
                }
                System.Diagnostics.Debug.Write("\n");
            }
        }


        public static PathStatusType AreConnected(MazeNode start, MazeNode end, ref int[,] maze, int width, int height, int offValue, int visitedValue)
        {
            Stack<MazeNode> pathStack = new Stack<MazeNode>();
            MazeNode previousNode = null, currentNode = start;
            PathStatusType statusType = PathStatusType.Processing;

            //copy the original in case we need to preserve it.
            int[,] copyMaze = new int[height, width];
            Array.Copy(maze, copyMaze, width * height); 

            do
            {
                MazeNode.PrintMaze(ref copyMaze, width, height, currentNode, previousNode);
                List<MazeNode> connectedNodeList = currentNode.GetValidConnectedNodesExcl(ref previousNode, ref copyMaze, width, height, offValue, visitedValue);
                foreach (MazeNode node in connectedNodeList)
                {
                    if (!node.Equals(end) && !node.Processed)
                        pathStack.Push(node);
                    else
                        statusType = PathStatusType.Connected; //no more nodes left to process
                }

                if (statusType == PathStatusType.Processing)
                {
                    currentNode.Processed = true;
                    previousNode = currentNode;
                    copyMaze[previousNode.X, previousNode.Y] = visitedValue; //set as visited

                    if (pathStack.Count == 0)
                        statusType = PathStatusType.NotConnected;  //no more nodes left to process
                    else
                        currentNode = pathStack.Pop();
                }
            } while (statusType == PathStatusType.Processing);

            return statusType;
        }

    }
}
