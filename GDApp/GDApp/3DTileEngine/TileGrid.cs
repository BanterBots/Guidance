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

        // Hardcoded maze entry, need seperate constructor for random generated one
        public TileGrid(int gridSize, int tileSize, Model[] models, BasicEffect effect, Texture2D texture, int[,] modelTypes, int[,] modelRotations)
        {
            this.gridSize = gridSize;
            this.tileSize = tileSize;
            this.models = models;
            this.texture = texture;
            this.effect = effect;
            this.grid = new ModelTileObject[gridSize, gridSize];

            generateGridFromArrays(modelTypes, modelRotations);
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
                        new Vector3(y * tileSize, 0, x * tileSize),
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
                        x,
                        y);
                }
            }
        }
    }
}
