using GDApp._3DTileEngine.Objects.Items;
using GDLibrary;
using JigLibX.Collision;
using JigLibX.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GDApp._3DTileEngine
{
    class TileGrid
    {
        #region Properties
        // Grid that holds all tiles
        public ModelTileObject[,] grid;
        public List<DrawnActor> itemList;

        // Size of our grid, and size of each tile
        public int gridSize;
        public float tileSize;
        
        // Graphical data
        public BasicEffect effect;
        public Texture2D texture;
        public Model[] models;

        // Directional data
        public int[] tileInfo;

        // Generation settings
        private int minTiles = 5;
        private int tiles = 0;

        // Regeneration list
        List<Integer3> regenCoords = new List<Integer3>();

        // RNG
        Random random = new Random();
        private Texture2D potionTexture;

        //Potions

        #endregion

        #region Constructors
        // Random maze
        public TileGrid(int gridSize, float tileSize, Model[] models, BasicEffect effect, Texture2D texture, Texture2D potionTexture)
        {
            this.gridSize = gridSize;
            this.tileSize = tileSize;
            this.models = models;
            this.texture = texture;
            this.effect = effect;
            this.grid = new ModelTileObject[gridSize, gridSize];
            this.potionTexture = potionTexture;
            initializeInfo();
        }
        #endregion

        #region Maze Creation
        #region Random Gen
        public void generateRandomGrid()
        {
            while (tiles < minTiles)
            {
                // Reset values upon failed generation
                regenCoords = new List<Integer3>();
                tiles = 0;
                grid = new ModelTileObject[gridSize,gridSize];
                itemList = new List<DrawnActor>();

                
                

                // Hardcoded item
                
  

                // Create two random chains that link with start and finish
                createRandomChainAt(0, 1, 3);      
                createRandomChainAt(gridSize - 1, gridSize - 2, 1);

                //  Create hardcoded start and finish
                createTileAt(0, 0, 0, 0);
                createTileAt(gridSize - 1, gridSize - 1, 0, 2);


                regenCoords.Add(new Integer3(gridSize - 1, gridSize - 2, 1));
           }
            
            regenerateGaps();
        }
        #endregion

        #region Input Gen
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
                        ObjectType.CollidableGround,
                        transform,
                        effect,
                        Color.White,
                        1,
                        texture,
                        models[modelTypes[x, y]],
                        modelTypes[x, y],
                        x,
                        y);
                }
            }
        }
        #endregion
        #endregion

        #region Random generation
        // Initialising directional data for tiles with bits
        private void initializeInfo()
        {
            tileInfo = new int[7];
            tileInfo[0] = 8;
            tileInfo[1] = 10;
            tileInfo[2] = 9;
            tileInfo[3] = 13;
            tileInfo[4] = 15;
            tileInfo[5] = 10;
            tileInfo[6] = 8;
        }

        private bool createRandomChainAt(int x, int y, int originDir) // originDir represents the direction where we came from
        {
            bool n = true;          // can we go north
            bool e = true;          // can we go east
            bool s = true;          // can we go south
            bool w = true;          // can we go west
            int allowedDirs = 15;   // directions we can go

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
            *  3.5) The previous code will have blocked passage to the room we're originating from, so, we set our origin direction to be true    
            **/

            allowedDirs = setBit(allowedDirs, originDir-1, true);

            /**
            *  3.6) Initialising random generation variables and random loops
            **/

            int possibleDirs = 0;
            int modelNumber = 0;
            bool created = false;

            // Loops through trying models for the next tile
            for(int tries = 0; tries < 5; tries++)
            {   
                /**
                *   4) We choose a random model based on probability
                **/
                modelNumber = randomTile();
                possibleDirs = tileInfo[modelNumber];


                /**
                *   5) We compare the possible directions and the directions of our model + rotations.
                **/
                int rotation = -1;

                for (int otries = 0; otries < 4; otries++)
                {
                    if (compareDirs(allowedDirs, possibleDirs, originDir))
                    {
                        createTileAt(x, y, modelNumber, rotation);
                        tries = 5;
                        created = true;
                        
                    }
                    else
                    {
                        possibleDirs = rotateDirs(possibleDirs);
                        System.Console.Write("Rotating");
                        rotation++;
                    }
                }
                if(created)
                {
                    tries = 5;
                }
            }

            /**
            *   5.5) We create further chains/link up other chains with this one
            **/

            if (grid[x,y] != null)
            {

                if (isBitSet(possibleDirs, 2))      // If we can go south...
                {
                    if(grid[x + 1, y] == null)
                    {
                        createRandomChainAt(x + 1, y, 4);           // We create a new chain heading with north as origin
                    }
                    else
                    {
                        regenCoords.Add(new Integer3(x + 1, y, 4)); // Regenerate tile to the south
                    }
                }
                if (isBitSet(possibleDirs, 3))      // If we can go west...
                {
                    if (grid[x, y - 1] == null)
                    {
                        createRandomChainAt(x, y - 1, 1);           // We create a new chain heading east
                    }
                    else
                    {
                        regenCoords.Add(new Integer3(x, y - 1, 1)); // Regenerate tile to the west
                    }
                }
                if (isBitSet(possibleDirs, 4))      // If we can go north...
                {
                    if (grid[x - 1, y] == null)
                    {
                        createRandomChainAt(x - 1, y, 2);           // We create a new chain heading north with south (2) as origin
                    }
                    else
                    {
                        regenCoords.Add(new Integer3(x - 1, y, 2)); // Regenerate the tile to the north
                    }
                }              
                if (isBitSet(possibleDirs, 1))      // If we can go east...
                {
                    if (grid[x, y + 1] == null)
                    {
                        createRandomChainAt(x, y + 1, 3);           // We create a new chain heading east with west (3) as origin
                    }
                    else
                    {
                        regenCoords.Add(new Integer3(x, y + 1, 3)); // Regenerate the tile to the east
                    }
                }
                return true;
            }
            else
            {
                /**
                *   6) At this stage, generation has failed - we throw in dead ends at each opening
                **/

                if (originDir == 4) // coming from north
                {
                    //south dead end
                    createTileAt(x, y, 0, -1);
                }
                else if (originDir == 2) // coming from south
                {
                    // north dead end
                    createTileAt(x, y, 0, 1);
                }
                else if (originDir == 3) // coming from west
                {
                    // east dead end
                    createTileAt(x, y, 0, 2);
                }
                else if (originDir == 1) // coming from east
                {
                    // west dead end
                    createTileAt(x, y, 0, 0);
                }
            }
            return false;
        }
      
        private int randomTile()
        {
            int modelNumber = -1;
            int rand = random.Next(1, 110);

            if(rand > 60)
            {
                modelNumber = 4;
            }
            else if(rand > 16)
            {
                modelNumber = 2;
            }
            
            else if(rand > 10)
            {
                modelNumber = 3;
            }
            else if (rand > 8)
            {
                modelNumber = 1;
            }
            else
            {
                modelNumber = 3;
            }
            //else if (rand > 5)
           // {
            //    modelNumber = 5;
            //}
            //else if(rand > 0)
            //{
            //    modelNumber = 6;
           // }
            //else if (rand > 15)
            //{
            //    modelNumber = 0;
            //}

            return modelNumber;
          
        }
        #endregion

        #region Bitwise functions
        private int rotateDirs(int dirs)
        {
            int newBits = 0;

            // extract the 4 bits and reorder them.
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
            return newBits;
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
        #endregion

        #region Regeneration
        // After our maze gen, we regenerate invalid tiles to fit together
        private void regenerateGaps()
        {
            foreach (Integer3 regenCoord in regenCoords)
            {
                Regenerate(regenCoord.X, regenCoord.Y, regenCoord.Z);
            }
        }

        private void Regenerate(int x, int y, int originDir)
        {
            int allowedDirs = getDirsFromModelAndRotation(x, y);        // Get directional data of x y tile

            allowedDirs = setBit(allowedDirs, originDir - 1, true);      // Add the value of our origin

            genRandomTile(x, y, allowedDirs, originDir);                // Replace the x,y with a new tile
        }

        private int getDirsFromModelAndRotation(int x, int y)
        {
            int modelNo = grid[x, y].modelNo;
            int rotation = grid[x, y].rotation;
            int possibleDirs = tileInfo[modelNo];

            for (int i = 0; i <= rotation; i++)
            {
                possibleDirs = rotateDirs(possibleDirs);
            }
            return possibleDirs;
        }

        private void genRandomTile(int x, int y, int requiredDirs, int originDir)
        {
            System.Console.WriteLine("Required direction: " + requiredDirs);
            int possibleDirs = 0;
            int modelNumber = 0;
            bool created = false;

            for (int tries = 0; tries < tileInfo.Length; tries++)
            {
                System.Console.WriteLine("Trying model no: " + modelNumber);
                int rotation = -1;
                possibleDirs = tileInfo[modelNumber];

                for (int rotations = 0; rotations < 4; rotations++)
                {
                    System.Console.WriteLine("Possible directions: " + possibleDirs + "\n");
                    if (requiredDirs == possibleDirs)
                    {
                        createTileAt(x, y, modelNumber, rotation);
                        tries = tileInfo.Length;
                        rotations = 4;
                        created = true;
                    }
                    else
                    {
                        possibleDirs = rotateDirs(possibleDirs);
                        rotation++;
                    }
                }
                if (created)
                {
                    tries = tileInfo.Length;
                }
                modelNumber++;
                rotation = -1;
            }
        }
        #endregion

        // This function is used to create all tiles
        private void createTileAt(int x, int y, int model, int rotation)
        {
            Transform3D transform = new Transform3D(
                new Vector3(x * tileSize, 0, y * (-1) * tileSize),
                new Vector3(0, rotation * -90, 0),
                new Vector3(0.1f, 0.1f, 0.1f),
                Vector3.UnitX,
                Vector3.UnitY);

            ModelTileObject mazeObject = new ModelTileObject(
               "maze(" + x + "," + y + ")",
               ObjectType.CollidableGround,
               transform,
               effect,
               Color.White,
               1,
               texture,
               models[model],
               model,
               x,
               y);

            mazeObject.rotation = rotation;
            tiles++;
            grid[x, y] = mazeObject;

            //random items
            int randPotion = random.Next(1, 40);
            if(randPotion == 1)
                createPotionAt(x, y, effect, this.potionTexture);
        }

        public void createPotionAt(int x, int y, BasicEffect effect, Texture2D potionTex)
        {
            ModelObject potion = null;

            Transform3D transform = new Transform3D(
                new Vector3(x * tileSize, 4, y * (-1) * tileSize),
                new Vector3(0, 0 * -90, 0),
                new Vector3(0.08f, 0.08f, 0.08f),
                Vector3.UnitX,
                Vector3.UnitY);

            potion = new ModelObject(
               "maze(" + x + "," + y + ")",
               ObjectType.Pickup,
               transform,
               effect,
               potionTex,
               models[7],
               Color.White,
               1);
            

            potion.AddController(new PotionController("updown", potion, new Vector3(0, 0.1f, 0)));
            itemList.Add(potion);
            
            PotionZoneObject potionZone = new PotionZoneObject(
                "potionZone(" + x + "," + y + ")", 
                ObjectType.CollidableTriggerZone, 
                transform, 
                effect, 
                Color.White, 
                0, 
                null, 
                false,
                potion);

            //no mass so we disable material properties
            potionZone.AddPrimitive(
                new Box(
                    new Vector3(transform.Translation.X, transform.Translation.Y +300, transform.Translation.Z), 
                    Matrix.Identity, 
                    new Vector3(transform.Scale.X*300, transform.Scale.Y*150, transform.Scale.Z*300)));
            //enabled by default
            potionZone.Enable(true);
            itemList.Add(potionZone);
        }
    }
}
