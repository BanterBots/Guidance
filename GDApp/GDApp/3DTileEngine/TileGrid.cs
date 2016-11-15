using GDLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDApp._3DTileEngine
{
    class TileGrid
    {
        public int gridSize;
        public ModelTileObject[,] grid;
        public int tileSize;
        public Model[] models;
        public BasicEffect effect;
        public Texture2D texture;
        public int[] tileInfo;
        Random random = new Random();

        // Hardcoded maze entry, need seperate constructor for random generated one
        public TileGrid(int gridSize, int tileSize, Model[] models, BasicEffect effect, Texture2D texture, int[,] modelTypes, int[,] modelRotations)
        {
            this.gridSize = gridSize;
            this.tileSize = tileSize;
            this.models = models;
            this.texture = texture;
            this.effect = effect;
            this.grid = new ModelTileObject[gridSize, gridSize];

            initializeInfo();
            generateGridFromArrays(modelTypes, modelRotations);
        }

        public TileGrid(int gridSize, int tileSize, Model[] models, BasicEffect effect, Texture2D texture)
        {
            this.gridSize = gridSize;
            this.tileSize = tileSize;
            this.models = models;
            this.texture = texture;
            this.effect = effect;
            this.grid = new ModelTileObject[gridSize, gridSize];
            initializeInfo();
            generateRandomGrid();
        }

        private void initializeInfo()
        {
            //  0
            //3   1
            //  2                    
            //                                                          R = 0   R = 1   R = 2   R = 3   R = 0
            /*                                                          uldr    uldr    uldr    uldr   decimal
            "deadEnd"   index:0     connection:l        bytes:          0100                              4
            "straight"  index:1     connection:l-r      bytes:          0101                              5
            "corner"    index:2     connection:l-d      bytes:          0110                              6
            "tJunction" index:3     connection:l-d-u    bytes:          1110    1101    1011    0111      14
            "cross"     index:4     connection:l-d-u-r  bytes:          1111                              15  
            "box"       index:5     connection:l-d      bytes:          0110                              6
            */

            tileInfo = new int[6];
            tileInfo[0] = 8;
            tileInfo[1] = 10;
            tileInfo[2] = 9;
            tileInfo[3] = 13;
            tileInfo[4] = 15;
            tileInfo[5] = 6;
        }

        private void generateRandomGrid()
        {

            //  0
            //3   1
            //  2
            //           x  y  model  rotation
            createTileAt(0, 0, 0, 0); // start
            //createTileAt(gridSize-1, gridSize-1, 0, 2); // finish
            createRandomChainAt(3, 3, 3);

            
        }

        private bool createRandomChainAt(int x, int y, int originDir) // originDir represents the direction where we came from
        {
            bool n = true;          // can we go north
            bool e = true;          // can we go east
            bool s = true;          // can we go south
            bool w = true;          // can we go west
            int allowedDirs = 15;   // directions we can go

            //  0
            //3   1
            //  2                    
            //                                                          R = 0   R = 1   R = 2   R = 3
            /*                                                          uldr    uldr    uldr    uldr
            "deadEnd"   index:0     connection:l        bytes:          0100                
            "straight"  index:1     connection:l-r      bytes:          0101
            "corner"    index:2     connection:l-d      bytes:          0110
            "tJunction" index:3     connection:l-d-u    bytes:          1110    1101    1011    0111
            "cross"     index:4     connection:l-d-u-r  bytes:          1111
            "box"       index:5     connection:l-d      bytes:          0110
            */

            /**
            *   0.5) We check if we are asked to go out of bounds
            **/

            if(x < 0 || x > gridSize-1)
            {
                return false;
            }
            if (y < 0 || y > gridSize-1)
            {
                return false;
            }

            /**
            *   1) We check if this tile already exists
            **/

            if (grid[x,y] != null)
            {
                return false;
            }

            /**
            *   2) We check the dimensions of our array against provided coordinates
            **/

            if (x == 0)                  // We cant go north, hit border of array
            {
                n = false;
            }
            else if(x == gridSize - 1)  // We cant go south, hit border of array
            {
                s = false;
            }

            if(y == 0)                  // We cant go west, hit border of array
            {
                w = false;
            }
            else if(y == gridSize - 1)  // We cant go east, hit border of array
            {
                e = false;
            }

            /**
            *   3) We check the rooms around the coordinates and find which directions we can go in
            **/

            if (n)                      // If we can go north, check if there's already a room there
            {
                if(grid[x - 1, y] != null)
                {
                    allowedDirs -= 8;
                }
            }
            else
            {
                allowedDirs -= 8;
            }
            if (e)                      // If we can go east, check if there's already a room there
            {
                if(grid[x, y + 1] != null)
                {
                    allowedDirs -= 1;
                }
            }
            else
            {
                allowedDirs -= 1;
            }
            if (s)                      // If we can go south, check if there's already a room there
            {
                if(grid[x + 1, y] != null)
                {
                    allowedDirs -= 2;
                }
            }
            else
            {
                allowedDirs -= 2;
            }
            if (w)                      // If we can go west, check if there's already a room there
            {
                if(grid[x, y - 1] != null)
                {
                    allowedDirs -= 4;
                }
            }
            else
            {
                allowedDirs -= 4;
            }

            /**
            *  3.5) The previous code will have blocked passage to the room we're originating from, so...     
            **/
            System.Console.Write("allowed dirs after collision checks " + allowedDirs + "\n");
            allowedDirs = setBit(allowedDirs, originDir-1, true);
            System.Console.Write("allowed dirs after origin calc " + allowedDirs + "\n");

            /**
            *   4) We choose a random model based on probability
            **/
            int modelNumber = randomTile();
            System.Console.Write(modelNumber);

            /**
            *   5) We compare the possible directions and the directions of our model + rotations.
            **/

            int possibleDirs = tileInfo[modelNumber];
            System.Console.Write("possible dirs coming from array: " + possibleDirs + "\n");
            int rotation = -1;

            for(int tries = 0; tries < 4; tries++)
            {
                if (compareDirs(allowedDirs, possibleDirs, originDir))
                {
                    createTileAt(x, y, modelNumber, rotation);
                    tries = 4;
                }
                else
                {
                    possibleDirs = rotateDirs(possibleDirs);
                    System.Console.Write("Rotating");
                    rotation++;
                }
            }

            if(grid[x,y] != null)
            {
                // Following 
                //   0
                // 3   1
                //   2
                // For creating further chains
                System.Console.Write("possible dirs " + possibleDirs+"\n");
                System.Console.Write("allowed dirs  " + allowedDirs + "\n");
                if (isBitSet(possibleDirs, 4))      // If we can go north...
                {
                    createRandomChainAt(x - 1, y, 2);                   // We create a new chain heading north with south (2) as origin
                }

                if (isBitSet(possibleDirs, 1))      // If we can go east...
                {
                   createRandomChainAt(x, y + 1, 3);                   // We create a new chain heading east with west (3) as origin
                }

                if (isBitSet(possibleDirs, 2))      // If we can go south...
                {
                  createRandomChainAt(x + 1, y, 4);                   // We create a new chain heading with north as origin
                }

                if (isBitSet(possibleDirs, 3))      // If we can go west...
                {
                    createRandomChainAt(x, y - 1, 1);                   // We create a new chain heading east
                }

                return true;
            }
            System.Console.Write("Failed generation");
            return false;
        }

        private int rotateDirs(int dirs)
        {
            int newBits = 0;

            // extract the 4 bits and reorder them.
            System.Console.Write("incoming dirs for rotation " + dirs + "\n");
            if (isBitSet(dirs, 4))
            {
                newBits += 1;
            }
            if (isBitSet(dirs, 3))
            {
                newBits += 8;
            }
            if (isBitSet(dirs, 2))
            {
                newBits += 4;
                //paddy was here 15/11/16
            }
            if (isBitSet(dirs, 1))
            {
                newBits += 2;
            }
            System.Console.Write("outgoing dirs after rotation " + newBits + "\n");
            return newBits;
        }

        private int randomTile()
        {
            int modelNumber = -1;
            int rand = random.Next(1, 101);

            if (rand > 90)       // 10 in 100 for a dead end
            {   // Dead End
                modelNumber = 0;
            }
            else if (rand > 70)  // 20 in 100 for a straight
            {   // Straight
                modelNumber = 1;
            }
            else if (rand > 50)  // 20 in 100 for a corner
            {   // Corner
                modelNumber = 2;
            }
            else if (rand > 35)  // 15 in 100 for a T junction
            {   // T Junction
                modelNumber = 3;
            }
            else if (rand > 25)  // 10 in 100 for a cross
            {   // Cross
                modelNumber = 4;
            }
            else if (rand > 0)   // 25 in 100 for a box;
            {   // Box
                modelNumber = 5;
            }
           // modelNumber = 1;
            return modelNumber;
          
        }

        private void createTileAt(int x, int y, int model, int rotation)
        {
            Transform3D transform = new Transform3D(
                new Vector3(x * tileSize, 0, y * (-1)*tileSize),
                new Vector3(0, rotation * -90, 0),
                new Vector3(0.1f, 0.1f, 0.1f),
                Vector3.UnitX,
                Vector3.UnitY);

            ModelTileObject mazeObject = new ModelTileObject(
               "maze(" + x + "," + y + ")",
               ActorType.Pickups,
               transform,
               effect,
               Color.White,
               1,
               texture,
               models[model],
               model,
               x,
               y);

            grid[x, y] = mazeObject;
            //game.objectManager.Add(grid[x, y]);

        }

        private bool compareDirs(int allowedDirs, int possibleDirs, int originDir)
        {
            System.Console.WriteLine("origin direction: " + originDir);
            if(!isBitSet(possibleDirs, originDir))
            {
                return false;
            }

            if (!isBitSet(allowedDirs, 4))
            {
                //if(originDir)
                if (isBitSet(possibleDirs, 4))
                {
                    return false;
                }
                else
                {
                    //match
                }
            }

            if (!isBitSet(allowedDirs, 3))
            {
                if (isBitSet(possibleDirs, 3))
                {
                    return false;
                }
                else
                {
                    //match
                }
            }

            if (!isBitSet(allowedDirs, 2))
            {
                if (isBitSet(possibleDirs, 2))
                {
                    return false;
                }
                else
                {
                    //match
                }
            }

            if (!isBitSet(allowedDirs, 1))
            {
                if (isBitSet(possibleDirs, 1))
                {
                    return false;
                }
                else
                {
                    //match
                }
            }

            return true;
        }

        //private void shift(int rotation, int )
        private bool isBitSet(int bytes, int bitNumber)
        {
            return (bytes & (1 << bitNumber - 1)) != 0;
        }

        private int setBit(int bytes, int position, bool value)
        {
            int mask = 1 << position;
            int result;
            if (value == true)
            {
                result = bytes | mask;
            }
            else
            {
                mask = ~mask;
                result = bytes & mask;
            }
            return result;
        }

        private void generateGridFromArrays(int[,] modelTypes, int[,] modelRotations)
        {
            Transform3D transform;
            ModelTileObject mazeObject;

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    transform = new Transform3D(
                        new Vector3(x * tileSize, 0, y * tileSize),
                        new Vector3(0, modelRotations[x, y] * 90, 0),
                        new Vector3(0.1f, 0.1f, 0.1f),
                        Vector3.UnitX,
                        Vector3.UnitY);

                    grid[x, y] = mazeObject = new ModelTileObject(
                        "maze(" + x + "," + y + ")",
                        ActorType.Pickups,
                        transform,
                        effect,
                        Color.White,
                        1,
                        texture,
                        models[modelTypes[x, y]],
                        modelTypes[x,y],
                        x,
                        y);
                }
            }
        }
    }
}
