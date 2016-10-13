using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _3DTileEngine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private BasicEffect effect;
        private Camera3D camera;
        private BasicEffect texturedEffect;

        public Camera3D Camera
        {
            get
            {
                return this.camera;
            }
        }

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            InitializeDictionaries(); //to do...
            LoadTextures(); //to do...
            InitializeEffects();
            InitializeCamera();
            InitializeWireframeObjects();
            InitializeTexturedObjects();
            base.Initialize();
        }

        private void InitializeDictionaries()
        {
           // throw new System.NotImplementedException();
        }

        private void LoadTextures()
        {
           // throw new System.NotImplementedException();
        }

        private void InitializeEffects()
        {
            this.effect = new BasicEffect(graphics.GraphicsDevice);
            this.effect.VertexColorEnabled = true;

            this.texturedEffect = new BasicEffect(graphics.GraphicsDevice);
            this.texturedEffect.TextureEnabled = true;

        }

        private void InitializeWireframeObjects()
        {
            OriginHelper o1 = new OriginHelper(
                this, new Vector3(0, 5, 0),
                new Vector3(0, 0, 0), 4 * Vector3.One, 
                this.effect);
            Components.Add(o1);
        }

        private void InitializeTexturedObjects()
        {
            Texture2D grassTexture = Content.Load<Texture2D>("Assets/Textures/Debug/ml");
            int size = 10;
            int mapscale = 10;
            Tile[,] tileArray = new Tile[10,10];
            for (int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    tileArray[i, j] = new Tile(i, j);
                }
            }


            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    //tileArray[i, j] = new Tile(i, j);
                    Components.Add(new TexturedTile(
                       this, new Vector3(i, 0, j),
                       new Vector3(-90, 0, 0),
                       mapscale * Vector3.One,
                       this.texturedEffect,
                       grassTexture));
                }
            }

        }
        private void InitializeCamera()
        {
            this.camera = new Camera3D(this,
                new Vector3(0, 100, 5), -Vector3.UnitY,
                Vector3.UnitX, MathHelper.PiOver2,
                16.0f / 9, 1, 1000);
            Components.Add(camera);

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
