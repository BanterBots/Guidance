using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GDLibrary;
using JigLibX.Collision;
using JigLibX.Geometry;
using System;
using GDApp._3DTileEngine;

namespace GDApp
{
    public class Main : Microsoft.Xna.Framework.Game
    {
        #region Variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private BasicEffect wireframeEffect, texturedPrimitiveEffect, texturedModelEffect;

        private ObjectManager objectManager;
        private MouseManager mouseManager;
        private KeyboardManager keyboardManager;
        private CameraManager cameraManager;
        private PhysicsManager physicsManager;

        private GenericDictionary<string, Texture2D> textureDictionary;
        private GenericDictionary<string, IVertexData> vertexDictionary;
        private GenericDictionary<string, DrawnActor3D> objectDictionary;
        private GenericDictionary<string, Model> modelDictionary;
        private GenericDictionary<string, SpriteFont> fontDictionary;
        private GenericDictionary<string, Transform3DCurve> curveDictionary;
        private GenericDictionary<string, RailParameters> railDictionary;

        private Vector2 screenCentre;

        //temp vars
        private ModelObject drivableModelObject;
        private EventDispatcher eventDispatcher;


        PlayerObject playerObject;
        #endregion

        #region Properties
        public GraphicsDeviceManager Graphics
        {
            get
            {
                return this.graphics;
            }
        }
        public Vector2 ScreenCentre
        {
            get
            {
                return this.screenCentre;
            }
        }
        public MouseManager MouseManager
        {
            get
            {
                return this.mouseManager;
            }
        }
        public KeyboardManager KeyboardManager
        {
            get
            {
                return this.keyboardManager;
            }
        }
        public CameraManager CameraManager
        {
            get
            {
                return this.cameraManager;
            }
        }
        public PhysicsManager PhysicsManager
        {
            get
            {
                return this.physicsManager;
            }
        }
        #endregion

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            int width = 1024, height = 768;
            int worldScale = 2000;


            #region Statics & Graphics
            InitializeStaticReferences();
            InitializeGraphics(width, height);
            InitializeEffects();
            #endregion

            #region Resource Managers & Dictionaries
            InitializeManagers(); 
            InitializeDictionaries();
            #endregion

            #region Resources
            //LoadFonts();
            LoadModels();
            LoadTextures(); 
            //LoadVertices();
            //LoadPrimitiveArchetypes();
            #endregion

            #region Draw Game Objects

            //InitializeHelperObjects();
            //InitializeWireframeObjects();
            //InitializeSkyAndGround(worldScale);
            //InitializeProps(); //cubes etc
            //InitializeFoliage(); //trees and shrubs etc
            InitializeArchitecture(); //walls and buildings etc
            InitializeModels();  //3DS Max or Maya FBX format models
            InitializeMaze(0);
            #endregion

            InitializeStaticCollidableGround(worldScale);

            #region Cameras
            //InitializeCameraTracks();
            InitializeCamera(new Vector3(0, 10, 20), -Vector3.UnitZ, Vector3.UnitY);
            #endregion

            InitializeStaticTriangleMeshObjects();

            #region Demo
            //InitializeCurveDemo();
            #endregion
            base.Initialize();
        }

        private void InitializeCameraTracks()
        {
            Transform3DCurve curve = null;

            #region Curve1
            curve = new Transform3DCurve(CurveLoopType.Oscillate);  
            curve.Add(new Vector3(0, 10, 200),
                    -Vector3.UnitZ, Vector3.UnitY, 0);

            curve.Add(new Vector3(0, 10, 20),
                   -Vector3.UnitZ, Vector3.UnitX, 2);

            this.curveDictionary.Add("room_action1", curve);
            #endregion

        }

        private void InitializeCurveDemo()
        {
            
        }

        private void InitializeStaticReferences()
        {
            Actor.game = this;
            Camera3D.game = this;
            Controller.game = this;
        }

        private void InitializeGraphics(int width, int height)
        {
            this.graphics.PreferredBackBufferWidth = width;
            this.graphics.PreferredBackBufferHeight = height;
            this.graphics.ApplyChanges();

            //or we can set full screen
            //this.graphics.IsFullScreen = true;
            //this.graphics.ApplyChanges();

            //records screen centre point - used by mouse to see how much the mouse pointer has moved
            this.screenCentre = new Microsoft.Xna.Framework.Vector2(this.graphics.PreferredBackBufferWidth / 2.0f, this.graphics.PreferredBackBufferHeight / 2.0f);
        }

        private void InitializeEffects()
        {
            this.wireframeEffect = new BasicEffect(graphics.GraphicsDevice);
            this.wireframeEffect.VertexColorEnabled = true;

            this.texturedPrimitiveEffect = new BasicEffect(graphics.GraphicsDevice);
            this.texturedPrimitiveEffect.VertexColorEnabled = true;
            this.texturedPrimitiveEffect.TextureEnabled = true;

            this.texturedModelEffect = new BasicEffect(graphics.GraphicsDevice);
            //this.texturedModelEffect.VertexColorEnabled = true;
            this.texturedModelEffect.TextureEnabled = true;
            this.texturedModelEffect.EnableDefaultLighting();
            this.texturedModelEffect.PreferPerPixelLighting = true;
            this.texturedModelEffect.SpecularPower = 128;

        }

        private void InitializeManagers()
        {
            //CD-CR
            this.physicsManager = new PhysicsManager(this);
            Components.Add(physicsManager);

            bool bDebugMode = true; //show wireframe CD-CR surfaces
            this.objectManager = new ObjectManager(this, "gameObjects", bDebugMode);
            Components.Add(this.objectManager);

            this.objectManager = new ObjectManager(this, "gameObjects", true);
            Components.Add(this.objectManager);

            this.mouseManager = new MouseManager(this, false);
            this.mouseManager.SetPosition(this.ScreenCentre);
            Components.Add(this.mouseManager);

            this.keyboardManager = new KeyboardManager(this);
            Components.Add(this.KeyboardManager);

            this.cameraManager = new CameraManager(this);
            Components.Add(this.cameraManager);
        }

        private void InitializeDictionaries()
        {
            this.textureDictionary = new GenericDictionary<string, Texture2D>("texture dictionary");
            
            this.vertexDictionary = new GenericDictionary<string, IVertexData>("vertex dictionary");
            
            this.objectDictionary = new GenericDictionary<string, DrawnActor3D>("object dictionary");

            this.modelDictionary = new GenericDictionary<string, Model>("model dictionary");

            this.fontDictionary = new GenericDictionary<string, SpriteFont>("font dictionary");

            this.curveDictionary = new GenericDictionary<string, Transform3DCurve>("curve dictionary");
        }

        //Triangle mesh objects wrap a tight collision surface around complex shapes - the downside is that TriangleMeshObjects CANNOT be moved
        private void InitializeStaticTriangleMeshObjects()
        {
            
        }

        private void LoadFonts()
        {
            //to do...
        }

        private void LoadModels()
        {
            this.modelDictionary.Add("torus", Content.Load<Model>("Assets/Models/torus"));

            this.modelDictionary.Add("box", Content.Load<Model>("Assets/Models/box"));

            this.modelDictionary.Add("room", Content.Load<Model>("Assets/Models/Guidance/TexturedBaseTiles/Tile_Room"));
            this.modelDictionary.Add("corner", Content.Load<Model>("Assets/Models/Guidance/TexturedBaseTiles/Tile_Corner"));
            this.modelDictionary.Add("tJunction", Content.Load<Model>("Assets/Models/Guidance/TexturedBaseTiles/Tile_Junction"));
            this.modelDictionary.Add("straight", Content.Load<Model>("Assets/Models/Guidance/TexturedBaseTiles/Tile_Straight"));
            this.modelDictionary.Add("cross", Content.Load<Model>("Assets/Models/Guidance/TexturedBaseTiles/Tile_Cross"));
            this.modelDictionary.Add("deadEnd", Content.Load<Model>("Assets/Models/Guidance/TexturedBaseTiles/Tile_DeadEnd"));
            this.modelDictionary.Add("puzzle", Content.Load<Model>("Assets/Models/Guidance/TexturedBaseTiles/Tile_Puzzle"));
            this.modelDictionary.Add("potion", Content.Load<Model>("Assets/Models/Guidance/Items/Pickups/m_potion"));
            //Add more models...
            
        }

        private void LoadTextures()
        {

            this.textureDictionary.Add("egypt",
                Content.Load<Texture2D>("Assets/Textures/Guide/BaseTile01_Diffuse"));
            
            #region debug
            this.textureDictionary.Add("ml",
               Content.Load<Texture2D>("Assets/Textures/Debug/ml"));
            this.textureDictionary.Add("checkerboard",
                Content.Load<Texture2D>("Assets/Textures/Debug/checkerboard"));
            #endregion 

            #region ground
            this.textureDictionary.Add("grass1", 
                Content.Load<Texture2D>("Assets/Textures/Foliage/Ground/grass1"));
            #endregion
            
            #region sky
            this.textureDictionary.Add("skybox_back",
                Content.Load<Texture2D>("Assets/Textures/Skybox/back"));
            this.textureDictionary.Add("skybox_front",
                Content.Load<Texture2D>("Assets/Textures/Skybox/front"));
            this.textureDictionary.Add("skybox_left",
                Content.Load<Texture2D>("Assets/Textures/Skybox/left"));
            this.textureDictionary.Add("skybox_right",
                Content.Load<Texture2D>("Assets/Textures/Skybox/right"));
            this.textureDictionary.Add("skybox_sky",
                Content.Load<Texture2D>("Assets/Textures/Skybox/sky"));
            #endregion
            
            #region trees & shrubs
            for (int i = 1; i <= 5; i++)
            {
                this.textureDictionary.Add("tree"  + i,
                    Content.Load<Texture2D>("Assets/Textures/Foliage/Trees/tree" + i));
            }

            for (int i = 1; i <= 3; i++)
            {
                this.textureDictionary.Add("shrub" + i,
                    Content.Load<Texture2D>("Assets/Textures/Foliage/Shrubs/shrub" + i));
            }
            #endregion

            #region fences
            for (int i = 1; i <= 4; i++)
            {
                this.textureDictionary.Add("fence" + i,
                    Content.Load<Texture2D>("Assets/Textures/Architecture/Fence/fence" + i));
            }

            #region walls
            this.textureDictionary.Add("backwall",
                  Content.Load<Texture2D>("Assets/Textures/Architecture/Walls/backwall"));
            this.textureDictionary.Add("sidewall",
                Content.Load<Texture2D>("Assets/Textures/Architecture/Walls/sidewall"));
            #endregion

            #region boxes
            this.textureDictionary.Add("crate1",
                   Content.Load<Texture2D>("Assets/Textures/Props/Crates/crate1"));
            this.textureDictionary.Add("crate2",
                    Content.Load<Texture2D>("Assets/Textures/Props/Crates/crate2"));
            #endregion

            #endregion

            #region other
            this.textureDictionary.Add("abstract1",
            Content.Load<Texture2D>("Assets/Textures/Abstract/abstract1"));
            #endregion


        }

        private void LoadVertices()
        {
            
        }

        private void LoadPrimitiveArchetypes()
        {
            Transform3D transform = null;
            TexturedPrimitiveObject texturedQuad = null;

            #region Textured Quad Archetype
            transform = new Transform3D(Vector3.Zero, Vector3.Zero, Vector3.One, Vector3.UnitZ, Vector3.UnitY);
            texturedQuad = new TexturedPrimitiveObject("textured quad archetype", ActorType.Decorator,
                     transform, this.texturedPrimitiveEffect, this.vertexDictionary["textured_quad"],
                     this.textureDictionary["checkerboard"]); //or  we can leave texture null since we will replace it later

            this.objectDictionary.Add("textured_quad", texturedQuad);
            #endregion
        }
        
        private void InitializeHelperObjects()
        {
            //wireframe origin helper
            Transform3D transform = new Transform3D(new Vector3(0, 10, 0), Vector3.Zero, 10 * Vector3.One,
                Vector3.UnitZ, Vector3.UnitY);

            PrimitiveObject originHelper = new PrimitiveObject("origin1", ActorType.Helper,
                    transform, this.wireframeEffect, this.vertexDictionary["wireframe_origin_helper"]);

            this.objectManager.Add(originHelper);
        }

        private void InitializeWireframeObjects()
        {
            //wireframe triangle
            Transform3D transform = new Transform3D(new Vector3(-10, 10, 0), Vector3.Zero, 4 * new Vector3(2, 3, 1),
                    Vector3.UnitZ, Vector3.UnitY);

            PrimitiveObject triangleObject = new PrimitiveObject("triangle1", ActorType.Decorator,
                    transform, this.wireframeEffect, this.vertexDictionary["wireframe_triangle"]);

            triangleObject.AttachController(new RotationController("rotControl1", ControllerType.Rotation,
                            0.1875f * Vector3.UnitY));

            this.objectManager.Add(triangleObject);
        }

        private void InitializeSkyAndGround(int worldScale)
        {
            TexturedPrimitiveObject archTexturedPrimitiveObject = null, cloneTexturedPrimitiveObject = null;

            #region Archetype
            //we need to do an "as" typecast since the dictionary holds DrawnActor3D types
            archTexturedPrimitiveObject = this.objectDictionary["textured_quad"] as TexturedPrimitiveObject;
            archTexturedPrimitiveObject.Transform3D.Scale *= worldScale;
            #endregion
            /*
            #region Grass
            cloneTexturedPrimitiveObject = (TexturedPrimitiveObject)archTexturedPrimitiveObject.Clone();
            cloneTexturedPrimitiveObject.ID = "ground";
            cloneTexturedPrimitiveObject.Transform3D.Translation = new Vector3(0, 0, 0);
            cloneTexturedPrimitiveObject.Transform3D.Rotation = new Vector3(-90, 0, 0);
            cloneTexturedPrimitiveObject.Texture = this.textureDictionary["grass1"];
            this.objectManager.Add(cloneTexturedPrimitiveObject);
            #endregion
    */
            #region Skybox
            //back
            cloneTexturedPrimitiveObject = (TexturedPrimitiveObject)archTexturedPrimitiveObject.Clone();
            cloneTexturedPrimitiveObject.ID = "skybox_back";
            cloneTexturedPrimitiveObject.Transform3D.Translation = new Vector3(0, 0, -worldScale / 2.0f);
            cloneTexturedPrimitiveObject.Texture = this.textureDictionary["skybox_back"];
            this.objectManager.Add(cloneTexturedPrimitiveObject);

            //left
            cloneTexturedPrimitiveObject = (TexturedPrimitiveObject)archTexturedPrimitiveObject.Clone();
            cloneTexturedPrimitiveObject.ID = "skybox_left";
            cloneTexturedPrimitiveObject.Transform3D.Translation = new Vector3(-worldScale / 2.0f, 0, 0);
            cloneTexturedPrimitiveObject.Transform3D.Rotation = new Vector3(0, 90, 0);
            cloneTexturedPrimitiveObject.Texture = this.textureDictionary["skybox_left"];
            this.objectManager.Add(cloneTexturedPrimitiveObject);

            //right
            cloneTexturedPrimitiveObject = (TexturedPrimitiveObject)archTexturedPrimitiveObject.Clone();
            cloneTexturedPrimitiveObject.ID = "skybox_right";
            cloneTexturedPrimitiveObject.Transform3D.Translation = new Vector3(worldScale / 2.0f, 0, 0);
            cloneTexturedPrimitiveObject.Transform3D.Rotation = new Vector3(0, -90, 0);
            cloneTexturedPrimitiveObject.Texture = this.textureDictionary["skybox_right"];
            this.objectManager.Add(cloneTexturedPrimitiveObject);

            //front
            cloneTexturedPrimitiveObject = (TexturedPrimitiveObject)archTexturedPrimitiveObject.Clone();
            cloneTexturedPrimitiveObject.ID = "skybox_front";
            cloneTexturedPrimitiveObject.Transform3D.Translation = new Vector3(0, 0, worldScale / 2.0f);
            cloneTexturedPrimitiveObject.Transform3D.Rotation = new Vector3(0, 180, 0);
            cloneTexturedPrimitiveObject.Texture = this.textureDictionary["skybox_front"];
            this.objectManager.Add(cloneTexturedPrimitiveObject);

            //top
            cloneTexturedPrimitiveObject = (TexturedPrimitiveObject)archTexturedPrimitiveObject.Clone();
            cloneTexturedPrimitiveObject.ID = "skybox_sky";
            cloneTexturedPrimitiveObject.Transform3D.Translation = new Vector3(0, worldScale / 2.0f, 0);
            cloneTexturedPrimitiveObject.Transform3D.Rotation = new Vector3(90, -90, 0);
            cloneTexturedPrimitiveObject.Texture = this.textureDictionary["skybox_sky"];
            this.objectManager.Add(cloneTexturedPrimitiveObject);
            #endregion
        }

        private void InitializeProps()
        {

        }

        private void InitializeFoliage()
        {
            //to do...
        }

        private void InitializeArchitecture()
        {
            //to do...
        }

        private void InitializeStaticCollidableGround(int scale)
        {
            //CollidableObject collidableObject = null;
            //Transform3D transform3D = null;
            //Texture2D texture = null;
            
            //Model model = this.modelDictionary["box"];
            //texture = this.textureDictionary["grass1"];
            //transform3D = new Transform3D(new Vector3(-0.5f, 0, 0), new Vector3(0, 0, 0),
            //    new Vector3(scale, 0.01f, scale), Vector3.UnitX, Vector3.UnitY);

            //collidableObject = new CollidableObject("ground", ActorType.CollidableGround, transform3D, this.texturedModelEffect, Color.White, 1, texture, model);
            //collidableObject.AddPrimitive(new Box(transform3D.Translation, Matrix.Identity, transform3D.Scale), new MaterialProperties(0.8f, 0.8f, 0.7f));
            //collidableObject.Enable(true, 1); //change to false, see what happens.
            //this.objectManager.Add(collidableObject);
        }

        private void InitializeModels()
        {
            //Transform3D transform = new Transform3D(
            //    new Vector3(0, 2, 0),
            //    new Vector3(0, 0, 0), new Vector3(0.2f, 0.2f, 0.2f),
            //    Vector3.UnitX, Vector3.UnitY);

            //this.playerObject = new PlayerObject(
            //    "box",
            //    ActorType.Pickup,
            //    transform,
            //    this.texturedModelEffect,
            //    Color.White,
            //    0.6f,
            //    this.textureDictionary["checkerboard"],
            //    this.modelDictionary["box"],
            //    AppData.PlayerMoveKeys, 0.5f, 0.2f, AppData.PlayerMoveSpeed, -AppData.PlayerMoveSpeed, new Vector3(0,2,0));
            
            //this.objectManager.Add(this.playerObject);
            
        }

        private void InitializeMaze(int size)
        {
            // size is hardcoded
            size = 15;
 
            Model[] mazeTiles = new Model[]{
                this.modelDictionary["deadEnd"],    //0
                this.modelDictionary["straight"],   //1
                this.modelDictionary["corner"],     //2
                this.modelDictionary["tJunction"],  //3
                this.modelDictionary["cross"],      //4
                this.modelDictionary["room"],      //5
                this.modelDictionary["puzzle"],    //6
                this.modelDictionary["potion"]
            };

            

            // is a tilegrid class even necessary? maybe just tilegridcreator to handle map generation
            //TileGrid tg = new TileGrid(size, 76, mazeTiles, this.texturedModelEffect, this.textureDictionary["crate1"], modelTypes, modelRotations);
            TileGrid tg = new TileGrid(5, 76, mazeTiles, this.texturedModelEffect, this.textureDictionary["egypt"]);
            tg.generateRandomGrid();

            for (int i = 0; i < tg.gridSize; i++)
            {
                for (int j = 0; j < tg.gridSize; j++)
                {
                    if(tg.grid[i,j] != null)
                    {
                        this.objectManager.Add(tg.grid[i, j]);
                    }
                }
            }

            foreach(DrawnActor3D model in tg.itemList)
            {
                this.objectManager.Add(model);
            }
        }

        private void InitializeCamera(Vector3 position, Vector3 look, Vector3 up)
        {
            Transform3D transform = null;
            Camera3D camera = null;
            string cameraLayout = "";

            #region Layout 1x1
            cameraLayout = "1x1";

            #region First Person Camera
            transform = new Transform3D(new Vector3(0, 20, 0), -Vector3.UnitZ, Vector3.UnitY);
            camera = new Camera3D("Static", ActorType.Camera, transform,
                ProjectionParameters.StandardMediumSixteenNine,
                new Viewport(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));

            ModelObject player = new ModelObject(
                "player", 
                ActorType.Player, 
                new Transform3D(new Vector3(-5, 10, 0), Vector3.Right, Vector3.Up), 
                texturedModelEffect, 
                Color.White, 
                -1,
                textureDictionary["ml"], 
                modelDictionary["box"]);
            
            camera.AttachController(new CollidableFirstPersonController(
                "firstPersControl1",
                ControllerType.FirstPerson, 
                AppData.CameraMoveKeys,
                AppData.CameraMoveSpeed*8, 
                AppData.CameraStrafeSpeed*2, 
                AppData.CameraRotationSpeed*15,
                2f, // radius
                20f, // height
                10f,// acceleration
                2f, // deceleration
                1,  // mass
                new Vector3(0,10,0),
                player));

            

            //camera.AttachController(new ThirdPersonController("thirdPersControl1", ControllerType.ThirdPerson,
            //    this.playerObject, 2,
            //    AppData.CameraThirdPersonScrollSpeedDistanceMultiplier/2,
            //    160,
            //    AppData.CameraThirdPersonScrollSpeedElevatationMultiplier/2,
            //    AppData.CameraLerpSpeedSlow));

            //add the new camera to the approriate K, V pair in the camera manager dictionary i.e. where key is "1x2"
            this.cameraManager.Add(cameraLayout, camera);
            #endregion
            #endregion

           // ProjectionParameters pm = Matrix.CreateOrthographic(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, )
                
            //ViewPort projection = Matrix.CreateOrthographicOffCenter(-GraphicsDevice.Viewport.Width/2f, GraphicsDevice.Viewport.Width/2f, -GraphicsDevice.Viewport.Height/2f, GraphicsDevice.Viewport.Height/2f, 1, 10000);
            Matrix projection = Matrix.CreateOrthographic(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 1.0f, 500.0f);


            #region Layout Map
            cameraLayout = "Map";
            #region Map View
            transform = new Transform3D(new Vector3(300, 1000, -500), Vector3.Down, -1 * Vector3.Right);
            camera = new Camera3D("Static", ActorType.Camera, transform,
                ProjectionParameters.StandardMediumSixteenNine,
                new Viewport(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));

            camera.ProjectionParameters.Projection = projection;

           // camera.Viewport.
            //camera.AttachController(new FirstPersonController("firstPersControl1",
            //ControllerType.FirstPerson, AppData.CameraMoveKeys,
            //AppData.CameraMoveSpeed, AppData.CameraStrafeSpeed, AppData.CameraRotationSpeed));

            //add the new camera to the approriate K, V pair in the camera manager dictionary i.e. where key is "1x2"
            this.cameraManager.Add(cameraLayout, camera);
            #endregion
            #endregion

            this.cameraManager.SetActiveCameraLayout("1x1");

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            this.fontDictionary.Dispose();
            this.modelDictionary.Dispose();
            this.textureDictionary.Dispose();
            this.vertexDictionary.Dispose();
            this.objectDictionary.Dispose();
            //this.railDictionary.Dispose();
            this.curveDictionary.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            demoCameraLayoutSwitching();
            base.Update(gameTime);
        }

        private void demoCameraLayoutSwitching()
        {
            if (this.keyboardManager.IsKeyDown(Keys.F1))
            {
                this.cameraManager.SetActiveCameraLayout("1x1");
                Window.Title = "1x1 Camera Layout [FirstPerson]";
            }
            else if (this.keyboardManager.IsKeyDown(Keys.F2))
            {
                this.cameraManager.SetActiveCameraLayout("Map");
                Window.Title = "Map Camera Layout [MapView]";
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            foreach (Camera3D camera in this.cameraManager)
            {
                //set the viewport based on the current camera
                graphics.GraphicsDevice.Viewport = camera.Viewport;
                base.Draw(gameTime);

                //set which is the active camera (remember that our objects use the CameraManager::ActiveCamera property to access View and Projection for rendering
                this.cameraManager.ActiveCameraIndex++;
            }
        }
    }
}
