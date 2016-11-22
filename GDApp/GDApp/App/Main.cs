using GDApp._3DTileEngine;
using GDApp.GDLibrary;
using GDApp.Sockets;
using GDLibrary;
using JigLibX.Collision;
using JigLibX.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

#region Update Log
/* To Do: 
 * turn to face controller
 * alpha objects
 * rotation and translation on modelobject picking
 * Check the controller Clone on Rail
 * Add Camera3D::Reset()
 * Do all relevant classes have - clone, equals, hash code
 * Jitter on Curve3D possibly related to Curve1D tangents - seems to occur on odd repeats of the curve
 * RailCamera3D shows bug when rail passes over the object - up vector?
 * 


 * Fix look and up on camera
 * Add debugdrawer to show FPS, camera type etc
 * Spherical, normal, cylindrical billboard
 * Depth and transparency on picked objects
 */

/* Version: 20
  =============
 * Author: NMCG
 * Revisions:
 *  Added animated player demo and text to texture render demo
 *  Text and video on primitive or model object
 *  
 * Bugs/Fixes:
 * Fixed - menu double click - see MouseManager::IsLeftButtonClickedOnce - Georgijs
 * Fixed - Color0 run time bug on load player objects - using other effect - texturedPrimitiveObject 
 * Fixed - Snow black background replaced
 *  
 * Addition:
 * - Have RailCamera3D lag the target object slightly - configurable
 * - Test clone for ModelObject and MoveableModelObject
 */

/* Version: 19
 =============
* Author: NMCG
* Revisions:
* - Added IVertexData and associated classes to streamline hierarchy
* - Added controllers for Video and TextToTexture 
* - Added MenuManager
* - Added new event types and event category enums to simplify event processing in EventDispatcher
* - Added HashSet in EventDispatcher to prevent duplicate events within the same update cycle (i.e. playing audio 10 times in one update)
* - Added GenericDraweableManager to centralise pause, add, remove, delete methods across UIManager, ObjectManager
* - Added code in MenuManager to support volume up, down, mute
* - Added texture to MenuManager for controls
* 
* Bugs:
* - Jitter on Curve3D possibly related to Curve1D tangents - seems to occur on odd repeats of the curve
* - RailCamera3D shows bug when rail passes over the object - up vector?
* 
* Fixes:
*  * Fix look and up on camera
*  * Add debugdrawer to show FPS, camera type etc
*  
* Addition:
* - Have RailCamera3D lag the target object slightly - configurable
* - Test clone for ModelObject and MoveableModelObject
*/

/* Version: 18
  =============
 * Author: NMCG
 * Revisions:
 * - Added draw on ZoneObject to visualise the code
 * 
 * Bugs:
 * - Jitter on Curve3D possibly related to Curve1D tangents - seems to occur on odd repeats of the curve
 * - RailCamera3D shows bug when rail passes over the object - up vector?
 * 
 * Fixes:
 *  * Fix look and up on camera
 *  * Add debugdrawer to show FPS, camera type etc
 *  
 * Addition:
 * - Have RailCamera3D lag the target object slightly - configurable
 * - Test clone for ModelObject and MoveableModelObject
 */

/* Version: 17
  =============
 * Author: NMCG
 * Revisions:
 * - UIManager, UI Objects, and Transform2D to support UI elements
 * 
 * Bugs:
 * - Jitter on Curve3D possibly related to Curve1D tangents - seems to occur on odd repeats of the curve
 * - RailCamera3D shows bug when rail passes over the object - up vector?
 * 
 * Fixes:
 *  * Fix look and up on camera
 *  * Add debugdrawer to show FPS, camera type etc
 *  
 * Addition:
 * - Have RailCamera3D lag the target object slightly - configurable
 * - Test clone for ModelObject and MoveableModelObject
 */

/* Version: 16
  =============
 * Author: NMCG
 * Revisions:
 * - Added mouse control to CollidableFirstPersonController
 * - Added CollidableFirstPersonController
 * - Added ObjectType enums
 * - Added KeyData and GameData variables
 * - Fixed rotate bug in Transform3D
 * - Changed default FOV in ProjectionParameters to 60 degrees
 * 
 * Bugs:
 * - Jitter on Curve3D possibly related to Curve1D tangents - seems to occur on odd repeats of the curve
 * - RailCamera3D shows bug when rail passes over the object - up vector?
 * 
 * Fixes:
 *  * Fix look and up on camera
 *  * Add debugdrawer to show FPS, camera type etc
 *  
 * Addition:
 * - Have RailCamera3D lag the target object slightly - configurable
 * - Test clone for ModelObject and MoveableModelObject
 */

/* Version: 15
  =============
 * Author: NMCG
 * Revisions:
 * - Added JigLibX enabled mouse picking
 * - Added ZoneObject to allow us to represent collidable zone (e.g. end zone then play win audio)
 * - Added CharacterObject with capsule collision skin
 * - Added PlayerObject which supports player movement
 * - Added CameraZoneObject - to complete in class
 * 
 * Bugs:
 * - Jitter on Curve3D possibly related to Curve1D tangents - seems to occur on odd repeats of the curve
 * - RailCamera3D shows bug when rail passes over the object - up vector?
 * 
 * Fixes:
 *  * Alpha on ModelObject
 *  * CameraManager::ActiveCamera and CycleCamera
 *  
 * Addition:
 * - Have RailCamera3D lag the target object slightly - configurable
 * - Test clone for ModelObject and MoveableModelObject
 */

/* Version: 14
  =============
 * Author: NMCG
 * Revisions:
 * - Added JigLibX
 * - Added TargetController as common super for Rail and ThirdPersonController
 * - Tested ThirdPersonController and distance on mouse scroll
 * - Set camera type in Window.Title
 * 
 * Bugs:
 * - Jitter on Curve3D possibly related to Curve1D tangents - seems to occur on odd repeats of the curve
 * - RailCamera3D shows bug when rail passes over the object - up vector?
 * Fixes:
 * - None
 * 
 * Addition:
 * - Have RailCamera3D lag the target object slightly - configurable
 * - Test clone for ModelObject and MoveableModelObject
 */

/* Version: 13
  =============
 * Author: NMCG
 * Revisions:
 * - Added MouseManager ray methods to ultimately test for object collisions
 * - Added ThirdPersonController (untested)
 * Bugs:
 * - Jitter on Curve3D possibly related to Curve1D tangents
 * - RailCamera3D shows bug when rail passes over the object - up vector?
 * Fixes:
 * - None
 * Addition:
 * - Have RailCamera3D lag the target object slightly - configurable
 * - Test clone for ModelObject and MoveableModelObject
 */

/* Version: 12
  =============
 * Author: NMCG
 * Revisions:
 * - Added path finding and demo code 
 * - Added sound manager and demo code
 * Bugs:
 * - Jitter on Curve3D possibly related to Curve1D tangents
 * - RailCamera3D shows bug when rail passes over the object - up vector?
 * Fixes:
 * - None
 * Addition:
 * - Have RailCamera3D lag the target object slightly - configurable
 * - Test clone for ModelObject and MoveableModelObject
 */

/* Version: 11
  =============
 * Author: NMCG
 * Revisions:
 * - Added first version of the EventDispatcher
 * Bugs:
 * - Jitter on Curve3D possibly related to Curve1D tangents
 * - RailCamera3D shows bug when rail passes over the object - up vector?
 * Fixes:
 * - None
 * Addition:
 * - Have RailCamera3D lag the target object slightly - configurable
 * - Test clone for ModelObject and MoveableModelObject
 */

/* Version: 10
  =============
 * Author: NMCG
 * Revisions:
 * - Added IController, Controller, PawnCamera3D, and a rail camera controller example
 * - Added more VertexFactory methods, added security and 3rd person camera
 * Bugs:
 * - Jitter on Curve3D possibly related to Curve1D tangents
 * - RailCamera3D shows bug when rail passes over the object - up vector?
 * Fixes:
 * - None
 * Addition:
 * - Have RailCamera3D lag the target object slightly - configurable
 * - Test clone for ModelObject and MoveableModelObject
 */

/* Version: 9
  =============
 * Author: NMCG
 * Revisions:
 * - Added Curve3D and TrackCamera3D
 * Bugs:
 * - Jitter on Curve3D possibly related to Curve1D tangents
 * - RailCamera3D shows bug when rail passes over the object - up vector?
 * - Interaction between rail and track camera?
 *
 * Fixes:
 * - None
 * Addition:
 * - Have RailCamera3D lag the target object slightly - configurable
 * - Test clone for ModelObject and MoveableModelObject
 */

/* Version: 8
  =============
 * Author: NMCG
 * Revisions:
 * - Added MoveableModelObject
 * - Added RailParameters
 * - Added RailCamera3D (work in progress
 * - Added dictionary, utility, and debug bounding box drawer classes
 * Bugs:
 * - RailCamera3D shows bug when rail passes over the object - up vector?
 * Fixes:
 * - Fixed null on rail camera target object by moving InitialiseCamera()
 * Addition:
 * - Have RailCamera3D lag the target object slightly - configurable
 * - Test clone for ModelObject and MoveableModelObject
 */

/* Version: 7
  =============
 * Author: NMCG
 * Revisions:
 * - Added IActor, Actor, DrawnActor
 * - Modified drawn objects and camera3D to work with new hierarchy
 * Bugs:
 * - Null on RailCamera3D::targetObject
 * Fixes:
 */

/* Version: 6
  =============
 * Author: NMCG
 * Revisions:
 * - Added Camera3D::clone and test
 * - Added CameraManager and Camera3D code to Main::Draw()
 * - Solved reset in Transform3D and ProjectionParameters
 * - Replaced CameraManager key type from string to CameraLayout
 * - Added Main::demoCameraLayout() to switch on F1 and F2 between split and full screen layouts
 * - Added FirstPersonCamera3D::Clone() to allow us to copy this type and see movement in the cloned camera
 * - Added FirstPersonCamera3D mouse and keyboard movement
 * - Added FirstPersonCamera3D::speed and added value to GameData
 * - Added Camera3D::game static variable
 * - Added Main::InitializeGraphics to set resolution
 * Bugs:
 * Fixes:
 */

/* Version: 5
  =============
 * Author: NMCG
 * Revisions:
 * - Added Transform3D and ProjectionParameters clone()
 * Bugs:
 * Fixes:
 */

/* Version: 5
  =============
 * Author: NMCG
 * Revisions:
 * - Added Camera3D Equals and GetHashCode methods
 * - Added Camera3D viewport and properties
 * Bugs:
 * Fixes:
 */

/* Version: 4
  =============
 * Author: NMCG
 * Revisions:
 * - Added TexturedQuad (unbuffered - change to vertex buffer?)
 * Bugs:
 * Fixes:
 */

/* Version: 3.1 (3.0 was distributed during class Week 2 - 1/1/16)
  =============
 * Author: NMCG
 * Revisions:
 * - Added demo camera movement
 * - Added input managers - keybaord and mouse
 * - Added start code for Camera3D class but did not integrate in Main yet
 * - Added Transform3D
 * - Added ObjectType to indicate type of entity
 * - Added examples of buffered, and indexed and buffered primitive construction
 * Bugs:
 * Fixes:
 */

/* Version: 2
 =============
 * Author: NMCG
 * Revisions:
 * - Added ProjectionParameters
 * - Added examples of unbuffered primitive construction
 * Bugs:
 * Fixes:
 */

#endregion

namespace GDApp
{
    public class Main : Microsoft.Xna.Framework.Game
    {
        #region Fields
        /**
        *   GRAPHICS
        **/
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private BasicEffect primitiveEffect;
        private BasicEffect texturedPrimitiveEffect;
        private BasicEffect texturedModelEffect;
        private BasicEffect propModelEffect;

        private Effect billboardEffect;

        private Vector2 screenCentre;
        private Microsoft.Xna.Framework.Rectangle screenRectangle;

        /**
        *   MANAGERS
        **/
        private CameraManager cameraManager;
        private KeyboardManager keyboardManager;
        private MouseManager mouseManager;
        private ObjectManager objectManager;
        private SoundManager soundManager;
        private PhysicsManager physicsManager;
        private UIManager uiManager;
        private MenuManager menuManager;

        /**
        *   DICTIONARIES
        **/
        private GenericDictionary<string, Video> videoDictionary;
        private GenericDictionary<string, IVertexData> vertexDictionary;
        private GenericDictionary<string, Texture2D> textureDictionary;
        private GenericDictionary<string, SpriteFont> fontDictionary;
        private GenericDictionary<string, Model> modelDictionary;
        private GenericDictionary<string, Camera3DTrack> trackDictionary;

        private EventDispatcher eventDispatcher;

        //temp demo vars
        private MoveableModelObject playerActor;

        /**
        *   SOUND
        **/
        SoundEffect _bongoBongoLoop;
        SoundEffectInstance _bongoBongoInstance;

        AudioEmitter _emitter;
        AudioListener _listener;
        #endregion

        #region Properties
        public Microsoft.Xna.Framework.Rectangle ScreenRectangle
        {
            get
            {
                return screenRectangle;
            }
        }
        public SpriteBatch SpriteBatch
        {
            get
            {
                return this.spriteBatch;
            }
        }
        public PhysicsManager PhysicsManager
        {
            get
            {
                return this.physicsManager;
            }
        }
        public ObjectManager ObjectManager
        {
            get
            {
                return this.objectManager;
            }
        }
        public EventDispatcher EventDispatcher
        {
            get
            {
                return this.eventDispatcher;
            }
        }
        public Vector2 ScreenCentre
        {
            get
            {
                return this.screenCentre;
            }
        }
        //nmcg - 18.3.16
        public SoundManager SoundManager
        {
            get
            {
                return soundManager;
            }
        }
        public CameraManager CameraManager
        {
            get
            {
                return this.cameraManager;
            }
        }
        public KeyboardManager KeyboardManager
        {
            get
            {
                return this.keyboardManager;
            }
        }
        public MouseManager MouseManager
        {
            get
            {
                return this.mouseManager;
            }
        }
        #endregion

        #region Constructors
        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        #endregion

        #region Event Handling
        //handle the relevant menu events
        public virtual void eventDispatcher_MainMenuChanged(EventData eventData)
        {
            if (eventData.EventType == EventType.OnExit)
                Exit();
            else if (eventData.EventType == EventType.OnRestart)
                this.LoadGame();
        }
        #endregion

        #region Load Assets
        private void InitializeVideos()
        {
            //this.videoDictionary.Add("sample", Content.Load<Video>("Assets/Video/sample"));
        }

        private void InitalizeTextures()
        {
            // GUIDANCE TEXTURES
            this.textureDictionary.Add("egypt", Content.Load<Texture2D>("Assets/Models/Maze/BaseTile01_Diffuse"));
            this.textureDictionary.Add("redPotion", Content.Load<Texture2D>("Assets/Models/ModelTextures/FlatColourTexture"));
           
            //this.textureDictionary.Add("crate1", Content.Load<Texture2D>("Assets\\Textures\\Game\\Props\\Crates\\crate1"));
            //this.textureDictionary.Add("back", Content.Load<Texture2D>("Assets\\Textures\\Game\\Skybox\\back"));
            //this.textureDictionary.Add("sky", Content.Load<Texture2D>("Assets\\Textures\\Game\\Skybox\\sky"));
            //this.textureDictionary.Add("left", Content.Load<Texture2D>("Assets\\Textures\\Game\\Skybox\\left"));
            //this.textureDictionary.Add("right", Content.Load<Texture2D>("Assets\\Textures\\Game\\Skybox\\right"));
            //this.textureDictionary.Add("front", Content.Load<Texture2D>("Assets\\Textures\\Game\\Skybox\\front"));
            this.textureDictionary.Add("grass1", Content.Load<Texture2D>("Assets\\Textures\\Game\\old\\Foliage\\Ground\\grass1"));

            //this.textureDictionary.Add("tree1", Content.Load<Texture2D>("Assets\\Textures\\Game\\Foliage\\Trees\\tree1"));
            //this.textureDictionary.Add("slj", Content.Load<Texture2D>("Assets\\Debug\\Textures\\slj"));
            this.textureDictionary.Add("ml", Content.Load<Texture2D>("Assets\\Debug\\Textures\\ml"));

            //UI
            this.textureDictionary.Add("white", Content.Load<Texture2D>("Assets\\Textures\\UI\\white"));
            this.textureDictionary.Add("mouseicons", Content.Load<Texture2D>("Assets/Textures/UI/mouseicons"));

            //billboards
            //this.textureDictionary.Add("billboardtexture", Content.Load<Texture2D>("Assets/Textures/Game/Billboards/billboardtexture"));
            //this.textureDictionary.Add("snow1", Content.Load<Texture2D>("Assets/Textures/Game/Billboards/snow1"));
            //this.textureDictionary.Add("chevron1", Content.Load<Texture2D>("Assets/Textures/Game/Billboards/chevron1"));
            //this.textureDictionary.Add("chevron2", Content.Load<Texture2D>("Assets/Textures/Game/Billboards/chevron2"));
            //this.textureDictionary.Add("alarm1", Content.Load<Texture2D>("Assets/Textures/Game/Billboards/alarm1"));
            //this.textureDictionary.Add("alarm2", Content.Load<Texture2D>("Assets/Textures/Game/Billboards/alarm2"));
            //this.textureDictionary.Add("tv", Content.Load<Texture2D>("Assets/Textures/Game/Props/tv"));

            //menu
            this.textureDictionary.Add("mainmenu", Content.Load<Texture2D>("Assets/Textures/Menu/mainmenu"));
            this.textureDictionary.Add("audiomenu", Content.Load<Texture2D>("Assets/Textures/Menu/audiomenu"));
            this.textureDictionary.Add("controlsmenu", Content.Load<Texture2D>("Assets/Textures/Menu/controlsmenu"));
            this.textureDictionary.Add("exitmenuwithtrans", Content.Load<Texture2D>("Assets/Textures/Menu/exitmenuwithtrans"));

        }

        private void InitalizeModels()
        {
            // GUIDANCE MODELS
            this.modelDictionary.Add("room", Content.Load<Model>("Assets/Models/Maze/Tile_Room"));
            this.modelDictionary.Add("corner", Content.Load<Model>("Assets/Models/Maze/Tile_Corner"));
            this.modelDictionary.Add("tJunction", Content.Load<Model>("Assets/Models/Maze/Tile_Junction"));
            this.modelDictionary.Add("straight", Content.Load<Model>("Assets/Models/Maze/Tile_Straight"));
            this.modelDictionary.Add("cross", Content.Load<Model>("Assets/Models/Maze/Tile_Cross"));
            this.modelDictionary.Add("deadEnd", Content.Load<Model>("Assets/Models/Maze/Tile_DeadEnd"));
            this.modelDictionary.Add("puzzle", Content.Load<Model>("Assets/Models/Maze/Tile_Puzzle"));
            this.modelDictionary.Add("potion", Content.Load<Model>("Assets/Models/Items/m_potion"));

            this.modelDictionary.Add("cube", Content.Load<Model>("Assets\\Models\\cube"));
            
            this.modelDictionary.Add("box", Content.Load<Model>("Assets\\Models\\box"));
            /*
            this.modelDictionary.Add("torus", Content.Load<Model>("Assets\\Models\\torus"));
            this.modelDictionary.Add("sphere", Content.Load<Model>("Assets\\Models\\sphere"));

            this.modelDictionary.Add("dude", Content.Load<Model>("Assets/Models/Animated/dude"));
            */


        }

        private void InitializeFonts()
        {
            this.fontDictionary.Add("debug", Content.Load<SpriteFont>("Assets\\Debug\\Fonts\\debug"));
            this.fontDictionary.Add("ui", Content.Load<SpriteFont>("Assets\\Fonts\\ui"));
            this.fontDictionary.Add("menu", Content.Load<SpriteFont>("Assets\\Fonts\\menu"));
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Sound
            _bongoBongoLoop = Content.Load<SoundEffect>("Assets/Audio/bongbongoLoop");
            _bongoBongoInstance = _bongoBongoLoop.CreateInstance();
            _bongoBongoInstance.IsLooped = true;
            _bongoBongoInstance.Volume = 0.4f;
            _bongoBongoInstance.Play();
        }
        protected override void UnloadContent()
        {
            //unload all resources
            this.fontDictionary.Dispose();
            this.textureDictionary.Dispose();
            this.videoDictionary.Dispose();
            this.trackDictionary.Dispose();
            this.modelDictionary.Dispose();
            this.vertexDictionary.Dispose();
        }
        #endregion

        #region Initialize Core
        private void LoadGame()
        {
            //remove anything from a previous run in object and ui manager
            this.objectManager.Clear();
            this.uiManager.Clear();

            //setup the world and all its objects
            int worldScale = 1000;
            #region Non Collidable Primitives
            InitializeSkyBox(worldScale);
            InitializeNonCollidablePrimitives();
            InitializeNonCollidableBillboards();
 
            #endregion

            #region Non Collidable Models
            InitializeNonCollidableModels();

            #endregion

            #region Collidable
            // GUIDANCE
            int size = 0;
            InitializeMaze(size);

            InitializeStaticCollidableGround(worldScale);
            InitializeStaticTriangleMeshObjects();
            InitializeDynamicCollidableObjects();
            InitializePlayerObjects();
            InitializeAnimatedPlayerObjects();
            InitializeZoneObjects();
            #endregion

            InitializeCameraTracks();
            InitializeCameras();
            InitializeUI();
        }

        protected override void Initialize()
        {
            startServer();

            InitializeEventDispatcher();
            InitializeStatics();
            IntializeGraphics(1024, 768);

            InitializeDictionaries();
            InitializeFonts();
            InitalizeModels();
            InitalizeTextures();
            InitializeVideos();
            InitializeManagers();
            InitializeVertexData();
            InitializeEffects();

            //pulling out load game allows us to reload from the menu to restart, if we lose
            LoadGame();

            #region Event Handling
            this.eventDispatcher.MainMenuChanged += new EventDispatcher.MainMenuEventHandler(eventDispatcher_MainMenuChanged);
            #endregion

            #region Demos
            demoPathFinding();
            #endregion

            base.Initialize();
        }

        private void InitializeEventDispatcher()
        {
            this.eventDispatcher = new EventDispatcher(this, 50);
            Components.Add(this.eventDispatcher);
        }

        private void InitializeVertexData()
        {
            IVertexData vertexData = null;
            Microsoft.Xna.Framework.Graphics.PrimitiveType primitiveType;
            int primitiveCount;
            VertexPositionColor[] vertices = null;                  //anything wireframe
            VertexPositionColorTexture[] texturedVertices = null;   //anything with a texture!
            /*
            #region Origin Helper
            vertices = VertexInfo.GetVerticesPositionColorOriginHelper(out primitiveType, out primitiveCount);
            vertexData = new VertexData<VertexPositionColor>(vertices, primitiveType, primitiveCount);
            this.vertexDictionary.Add("origin", vertexData);
            #endregion

            #region Sphere
            vertices = VertexInfo.GetVerticesPositionColorSphere(1, 15, out primitiveType, out primitiveCount);
            vertexData = new VertexData<VertexPositionColor>(vertices, primitiveType, primitiveCount);
            this.vertexDictionary.Add("sphere", vertexData);
            #endregion
            */
            #region Textured Quad (e.g. sky, signs, billboards)
            texturedVertices = VertexInfo.GetVerticesPositionColorTextureQuad(1, out primitiveType, out primitiveCount);
            vertexData = new VertexData<VertexPositionColorTexture>(texturedVertices, primitiveType, primitiveCount);
            this.vertexDictionary.Add("texturedquad", vertexData);
            #endregion

        }

        private void InitializeUI()
        {
            InitializeUIInventoryMenu();
            InitializeUIMousePointer();
        }

        private void InitializeUIMousePointer()
        {
            /*
            Transform2D transform = null;
            Texture2D texture = null;
            Microsoft.Xna.Framework.Rectangle sourceRectangle;

            //texture
            texture = this.textureDictionary["mouseicons"];
            transform = new Transform2D(Vector2.One);

            //show first of three images from the file
            sourceRectangle = new Microsoft.Xna.Framework.Rectangle(0, 0, 128, 128);

            UITextureObject texture2DObject = new UIMouseObject("mouse icon",
                ObjectType.UITexture2D, transform, new Color(127, 127, 127, 50),
                SpriteEffects.None, 1, texture, 
                sourceRectangle,
                new Vector2(sourceRectangle.Width / 2.0f, sourceRectangle.Height / 2.0f),
                true);
            this.uiManager.Add(texture2DObject);
            */
        }

        private void InitializeUIInventoryMenu()
        {
            /*
            Transform2D transform = null;
            SpriteFont font = null;
            Texture2D texture = null;

            //text
            font = this.fontDictionary["ui"];
            String text = "help me!";
            Vector2 dimensions = font.MeasureString(text);
            transform = new Transform2D(new Vector2(50, 600), 0, Vector2.One, Vector2.Zero, new Integer2(dimensions));
            UITextObject textObject = new UITextObject("test1", ObjectType.UIText, transform, new Color(15, 15, 15, 150), SpriteEffects.None, 0, "help", font, true);
            this.uiManager.Add(textObject);

            //texture
            texture = this.textureDictionary["white"];
            transform = new Transform2D(new Vector2(40, 590), 0, new Vector2(4, 4), Vector2.Zero, new Integer2(texture.Width, texture.Height));
            UITextureObject texture2DObject = new UITextureObject("texture1", 
                ObjectType.UITexture2D, transform, new Color(127, 127, 127, 50), 
                SpriteEffects.None, 1, texture, true);
            this.uiManager.Add(texture2DObject);
            */
        }
       
        private void InitializeCameraTracks()
        {
            /*
            Camera3DTrack cameraTrack = null;

            cameraTrack = new Camera3DTrack(CurveLoopType.Oscillate);
            cameraTrack.Add(new Vector3(-20, 10, 10), -Vector3.UnitZ, Vector3.UnitY, 0);
            cameraTrack.Add(new Vector3(20, 5, 10), -Vector3.UnitZ, Vector3.UnitY, 5);
            cameraTrack.Add(new Vector3(50, 5, 10), -Vector3.UnitX, Vector3.UnitY, 10);

            this.trackDictionary.Add("simple", cameraTrack);

            cameraTrack = new Camera3DTrack(CurveLoopType.Oscillate);
            //start
            cameraTrack.Add(new Vector3(0, 2, 0), -Vector3.UnitY, Vector3.UnitZ, 0);
            //fast
            cameraTrack.Add(new Vector3(0, 100, 0), Vector3.UnitZ, Vector3.UnitY, 5);
            //slow
            cameraTrack.Add(new Vector3(0, 105, 0), Vector3.UnitZ, Vector3.UnitY, 7);
            //fall
            cameraTrack.Add(new Vector3(0, 2, 0), -Vector3.UnitY, Vector3.UnitZ, 8);

            this.trackDictionary.Add("puke", cameraTrack);
            */
        }

        private void InitializeDictionaries()
        {
            this.textureDictionary = new GenericDictionary<string, Texture2D>("textures");
            this.fontDictionary = new GenericDictionary<string, SpriteFont>("fonts");
            this.modelDictionary = new GenericDictionary<string, Model>("model");
            this.trackDictionary = new GenericDictionary<string, Camera3DTrack>("camera tracks");

            //stores vertices for use by primitive object and textured primitive object
            this.vertexDictionary = new GenericDictionary<string, IVertexData>("vertex data");

            this.videoDictionary = new GenericDictionary<string, Video>("vidoes");
        }
       
        private void IntializeGraphics(int width, int height)
        {
            this.graphics.PreferredBackBufferWidth = width;
            this.graphics.PreferredBackBufferHeight = height;
            this.screenCentre = new Vector2(width/2, height/2);
            this.screenRectangle = new Microsoft.Xna.Framework.Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            this.graphics.ApplyChanges();
        }

        private void InitializeStatics()
        {
            Actor.game = this;
            Controller.game = this;
            UIActor.game = this;
        }

        private void InitializeManagers()
        {
            //CD/CR
            this.physicsManager = new PhysicsManager(this);
            Components.Add(physicsManager);

            this.cameraManager = new CameraManager(this);
            Components.Add(this.cameraManager);

            this.keyboardManager = new KeyboardManager(this);
            Components.Add(this.keyboardManager);

            bool isMouseVisible = true;
            this.mouseManager = new MouseManager(this, isMouseVisible);
            //centre the mouse otherwise the movement for 1st person camera will be unpredictable
            this.mouseManager.SetPosition(this.screenCentre); 
            Components.Add(this.mouseManager);

            bool bDebugMode = false;
            this.objectManager = new ObjectManager(this, 10, 10, bDebugMode);
            this.objectManager.DrawOrder = 1;
            Components.Add(this.objectManager);

            //sound
            this.soundManager = new SoundManager(this,
                 "Content\\Assets\\Audio\\Demo2DSound.xgs",
                 "Content\\Assets\\Audio\\WaveBank1.xwb",
                "Content\\Assets\\Audio\\SoundBank1.xsb");
            Components.Add(this.soundManager);


            this.uiManager = new UIManager(this, 10, 10);
            this.uiManager.DrawOrder = 2; //always draw after object manager(1)
            Components.Add(this.uiManager);

            Texture2D[] menuTexturesArray = { 
                this.textureDictionary["mainmenu"], 
                this.textureDictionary["audiomenu"],
                this.textureDictionary["controlsmenu"],
                this.textureDictionary["exitmenuwithtrans"]
            };

            this.menuManager = new MenuManager(this, menuTexturesArray, this.fontDictionary["menu"], MenuData.MenuTexturePadding, MenuData.MenuTextureColor);
            this.menuManager.DrawOrder = 3; //always draw after ui manager(2)
            Components.Add(this.menuManager);
        }

        private void InitializeEffects()
        {
            // GUIDANCE EFFECTS
            this.texturedModelEffect = new BasicEffect(graphics.GraphicsDevice);
            //this.texturedModelEffect.VertexColorEnabled = true;
            this.texturedModelEffect.TextureEnabled = true;
            this.texturedModelEffect.EnableDefaultLighting();
            this.texturedModelEffect.PreferPerPixelLighting = true;
            this.texturedModelEffect.SpecularPower = 128;


            this.propModelEffect = new BasicEffect(graphics.GraphicsDevice);
            this.propModelEffect.TextureEnabled = true;
            this.propModelEffect.EnableDefaultLighting();
            this.propModelEffect.DirectionalLight1.Enabled = true;
            this.propModelEffect.DirectionalLight2.Enabled = true;
            this.propModelEffect.DirectionalLight0.Enabled = false;

            this.propModelEffect.AmbientLightColor = new Vector3(.6f, .6f, .6f);

            this.propModelEffect.DirectionalLight1.DiffuseColor = new Vector3(.1f, .1f, .1f);
            this.propModelEffect.DirectionalLight2.DiffuseColor = new Vector3(.1f, .1f, .1f);

            this.propModelEffect.DirectionalLight1.Direction = Vector3.Forward;
            this.propModelEffect.DirectionalLight2.Direction = Vector3.Right;


            this.propModelEffect.SpecularPower = 2048;
            this.propModelEffect.DiffuseColor = new Vector3(.9f, .9f, .9f);
            this.propModelEffect.SpecularColor = new Vector3(.9f, .9f, .9f);
            // END

            this.primitiveEffect = new BasicEffect(graphics.GraphicsDevice);
            this.primitiveEffect.VertexColorEnabled = true;

            /*
            this.texturedPrimitiveEffect = new BasicEffect(graphics.GraphicsDevice);
            this.texturedPrimitiveEffect.VertexColorEnabled = true;
            this.texturedPrimitiveEffect.TextureEnabled = true;*/

            this.texturedModelEffect = new BasicEffect(graphics.GraphicsDevice);
           // this.texturedModelEffect.VertexColorEnabled = true; 
            this.texturedModelEffect.TextureEnabled = true;

            //used for billboards
            this.billboardEffect = Content.Load<Effect>("Assets/Effects/Billboard");

            //used for animated models
            this.animatedModelEffect = Content.Load<Effect>("Assets/Effects/Animated");
        }

        private void InitializeCameras()
        {  
            PawnCamera3D clonePawnCamera = null;
            Camera3D cloneFixedCamera = null;
            Transform3D transform = Transform3D.Zero;
            Transform3D pawnTransform = new Transform3D(new Vector3(0, 11, 0), -Vector3.UnitZ, Vector3.UnitY);

            #region Camera Archetypes 

            PawnCamera3D pawnCameraArchetype = new PawnCamera3D("pawn camera archetype",
                ObjectType.PawnCamera,
                pawnTransform,
                ProjectionParameters.StandardMediumFourThree, 
                this.graphics.GraphicsDevice.Viewport);

            Camera3D fixedCameraArchetype = new Camera3D("fixed camera archetype", ObjectType.FixedCamera);

            #endregion

            string cameraLayoutName = "FirstPersonMazeCamera";
          
            #region FPS Camera
            clonePawnCamera = (PawnCamera3D)pawnCameraArchetype.Clone();
            clonePawnCamera.ID = "Collidable Maze Cam";
            clonePawnCamera.AddController(new CollidableFirstPersonController(
                clonePawnCamera + " controller",
                clonePawnCamera,
                KeyData.MoveKeys,
                GameData.CameraMoveSpeed * 6,
                GameData.CameraStrafeSpeed * 6,
                GameData.CameraRotationSpeed * 20,
                1f, // radius
                20f, // height
                10f,// acceleration
                2f, // deceleration
                1,  // mass
                new Vector3(0,0,0)));
            this.cameraManager.Add(cameraLayoutName, clonePawnCamera);
            #endregion

            #region Maze Camera
            transform = new Transform3D(
                new Vector3(300, 1000, -500), 
                Vector3.Down, 
                -1 * Vector3.Right);

            cloneFixedCamera = (Camera3D)fixedCameraArchetype.Clone();
            cloneFixedCamera.ID = "Top Maze Cam";
            cloneFixedCamera.Transform3D = transform;
            this.cameraManager.Add(cameraLayoutName, cloneFixedCamera);
            #endregion
            
            #region Nialls Stuff
            /*
                       #region Collidable First Person Camera
                       clonePawnCamera = (PawnCamera3D)pawnCameraArchetype.Clone();
                       clonePawnCamera.ID = "collidable 1st person front";
                       clonePawnCamera.AddController(new CollidableFirstPersonController(clonePawnCamera + " controller", clonePawnCamera, KeyData.MoveKeys, GameData.CameraMoveSpeed, 
                           GameData.CameraStrafeSpeed, GameData.CameraRotationSpeed, 2f, 5, 1, 1, 1, Vector3.Zero));
                       this.cameraManager.Add(cameraLayoutName, clonePawnCamera);
                       #endregion

                       #region Non-collidable 1st Person Front Camera
                       clonePawnCamera = (PawnCamera3D)pawnCameraArchetype.Clone();

                       clonePawnCamera.ID = "non-collidable 1st person front";
                       clonePawnCamera.Transform3D.Translation = new Vector3(-10, 0, 30);
                       clonePawnCamera.AddController(new FirstPersonController(clonePawnCamera + " controller", clonePawnCamera, KeyData.MoveKeys, GameData.CameraMoveSpeed, GameData.CameraStrafeSpeed, GameData.CameraRotationSpeed));
                       this.cameraManager.Add(cameraLayoutName, clonePawnCamera);
                       #endregion

                       #region Non-collidable Fixed (i.e. no controller) Left Camera
                       cloneFixedCamera = (Camera3D)fixedCameraArchetype.Clone();

                       cloneFixedCamera.ID = "non-collidable front left fixed";
                       cloneFixedCamera.Transform3D.Translation = new Vector3(-50, 5, 0); //on -ve X-axis 
                       cloneFixedCamera.Transform3D.Look = Vector3.UnitX; //looking at origin
                       cloneFixedCamera.Transform3D.Up = Vector3.UnitY; 
                       this.cameraManager.Add(cameraLayoutName, cloneFixedCamera);
                       #endregion

                       #region Non-collidable Fixed (i.e. no controller) Top Camera
                       cloneFixedCamera = (Camera3D)fixedCameraArchetype.Clone();

                       cloneFixedCamera.ID = "non-collidable front top fixed";
                       cloneFixedCamera.Transform3D.Translation = new Vector3(0, 50, 0); //on +ve Y-axis 
                       cloneFixedCamera.Transform3D.Look = -Vector3.UnitY; //looking down at origin
                       cloneFixedCamera.Transform3D.Up = -Vector3.UnitZ; 
                       this.cameraManager.Add(cameraLayoutName, cloneFixedCamera);
                       #endregion
                       */
            #endregion

            this.cameraManager.SetCameraLayout("FirstPersonMazeCamera");
        }
        #endregion

        #region Initialize Drawn Objects
        private void InitializeZoneObjects()
        {/*
            #region Camera Switcher Zone
            Transform3D transform3D = new Transform3D(new Vector3(40, 0, 0),
                Vector3.Zero, new Vector3(20, 5, 10),
                Vector3.UnitX, Vector3.UnitY); //look and up dont matter and arent used for anything

            CameraZoneObject zoneObject = new CameraZoneObject("alarm zone 1",
                ObjectType.CollidableTriggerZone, transform3D, 
                this.primitiveEffect, Color.Red, 1,
                false, "FirstPersonFullScreen", "non-collidable front left fixed");

            //no mass so we disable material properties
            zoneObject.AddPrimitive(new Box(transform3D.Translation, Matrix.Identity, transform3D.Scale));
            //enabled by default
            zoneObject.Enable(true);

            this.objectManager.Add(zoneObject);
            #endregion
            */
        }

        private void InitializePlayerObjects()
        {
            /*
            PlayerObject playerObject = null;
            Transform3D transform3D = null;

            transform3D = new Transform3D(new Vector3(60, 5, -50),
                Vector3.Zero, new Vector3(1.5f, 1.5f, 5), //scale?
                Vector3.UnitZ, Vector3.UnitY);

            playerObject = new PlayerObject("p1",
                ObjectType.Player, transform3D,
                this.texturedModelEffect,
                this.textureDictionary["slj"],
                this.modelDictionary["cube"], 
                Color.White,
                1, 
                KeyData.MoveKeysOther, 1, 5, 1.2f, 1, Vector3.Zero);
            playerObject.Enable(false, 1);

            this.objectManager.Add(playerObject);
            */
        }
        
        private void InitializeAnimatedPlayerObjects()
        {
            /*
            AnimatedPlayerObject playerObject = null;
            Transform3D transform3D = null;

            transform3D = new Transform3D(new Vector3(20, 20, 20),
                new Vector3(-90, 0, 0),
                 0.1f * Vector3.One, //y-z are reversed because the capsule is rotation by 90 degrees around X-axis - See CharacterObject constructor
                 -Vector3.UnitZ, Vector3.UnitY);

            playerObject = new AnimatedPlayerObject("ap1",
                ObjectType.AnimatedPlayer, transform3D,
                this.animatedModelEffect,
                this.textureDictionary["slj"],
                this.modelDictionary["dude"],
                Color.White,
                1,
                KeyData.MoveKeysAnimated, 1.5f, 5, 1, 1,
                "Take 001", new Vector3(0, -4, 0));
            playerObject.Enable(false, 1);
      

            this.objectManager.Add(playerObject);
            */
        }
        
        private void InitializeStaticTriangleMeshObjects()
        {
            /*
            CollidableObject collidableObject = null;
            Transform3D transform3D = null;

            transform3D = new Transform3D(new Vector3(-5, 0.5f, 0),
                new Vector3(45, 0, 0), 0.05f * Vector3.One, Vector3.UnitX, Vector3.UnitY);
            collidableObject = new TriangleMeshObject("torus", ObjectType.CollidableProp,
            transform3D, this.texturedModelEffect,
            this.textureDictionary["grass1"], this.modelDictionary["torus"],
                Color.White, 1,
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableObject.Enable(true, 1);
            collidableObject.ObjectType = ObjectType.Pickup;
            this.objectManager.Add(collidableObject);
            */
        }

        private void InitializeMaze(int size)
        {
            // size is hardcoded
            size = 5;

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
            TileGrid tg = new TileGrid(9, 76.20f, mazeTiles, this.texturedModelEffect, this.textureDictionary["egypt"], this.textureDictionary["redPotion"]);
            tg.generateRandomGrid();

            for (int i = 0; i < tg.gridSize; i++)
            {
                for (int j = 0; j < tg.gridSize; j++)
                {
                    if (tg.grid[i, j] != null)
                    {
                        this.objectManager.Add(tg.grid[i, j]);
                    }
                }
            }
            tg.createPotionAt(0, 1, this.propModelEffect, this.textureDictionary["redPotion"]);

            foreach (ModelObject model in tg.itemList)
            {
                this.objectManager.Add(model);
            }
        }

        private void InitializeStaticCollidableGround(int scale)
        {
            CollidableObject collidableObject = null;
            Transform3D transform3D = null;
            Texture2D texture = null;

            Model model = this.modelDictionary["cube"];
            texture = this.textureDictionary["grass1"];
            transform3D = new Transform3D(new Vector3(0, -5, 0), new Vector3(0, 0, 0),
                new Vector3(scale, 1, scale), Vector3.UnitX, Vector3.UnitY);

            collidableObject = new CollidableObject("ground", ObjectType.CollidableGround, transform3D, this.texturedModelEffect, texture, model, Color.White, 1);
            collidableObject.AddPrimitive(new Box(transform3D.Translation, Matrix.Identity, transform3D.Scale), new MaterialProperties(0.8f, 0.8f, 0.7f));
            collidableObject.Enable(true, 1); //change to false, see what happens.
            this.objectManager.Add(collidableObject);
            
        }

        private void InitializeDynamicCollidableObjects()
        {
            /*
            CollidableObject collidableObject = null;
            Transform3D transform3D = null;
            Texture2D texture = null;
            Model model = null;

            #region Spheres
            //these boxes, spheres and cylinders are all centered around (0,0,0) in 3DS Max
            model = this.modelDictionary["sphere"];
            texture = this.textureDictionary["crate1"];
            for (int i = 0; i < 10; i++)
            {
                transform3D = new Transform3D(
                    new Vector3(-5, 20 + 8 * i, i),
                    new Vector3(0, 0, 0),
                    0.082f * Vector3.One, //notice theres a certain amount of tweaking the radii with reference to the collision sphere radius of 2.54f below
                    Vector3.UnitX, Vector3.UnitY);
                collidableObject = new CollidableObject("sphere " + i,
                    ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, model,
                     Color.White, 1);
                collidableObject.AddPrimitive(
                    new Sphere(transform3D.Translation, 2.54f),
                    new MaterialProperties(0.2f, 0.8f, 0.7f));
                collidableObject.Enable(true, 1);
                collidableObject.ObjectType = ObjectType.Pickup;
                this.objectManager.Add(collidableObject);
            }
            #endregion

            #region Box
            model = this.modelDictionary["cube"];
            texture = this.textureDictionary["crate1"];

            for (int i = 0; i < 10; i++)
            {
                transform3D = new Transform3D(
                        new Vector3(-20, 5 + 8 * i, i),
                        new Vector3(0, 0, 0),
                        3 * Vector3.One, //notice theres a certain amount of tweaking the radii with reference to the collision sphere radius of 2.54f below
                        Vector3.UnitX, Vector3.UnitY);

                collidableObject = new CollidableObject("cube",
                    ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, model, Color.White, 1);

                collidableObject.AddPrimitive(
                    new Box(Vector3.Zero, Matrix.Identity, transform3D.Scale),
                    new MaterialProperties(0.2f, 0.8f, 0.7f));

                collidableObject.Enable(false, 1);
                collidableObject.ObjectType = ObjectType.Pickup;
                this.objectManager.Add(collidableObject);
            }

            #endregion*/
        }

        private void InitializeNonCollidableModels()
        {/*
            Transform3D transform = null;
            transform = new Transform3D(new Vector3(-30, 25, 20),
                new Vector3(45, 0, 45), 0.5f * Vector3.One,
                Vector3.UnitX, Vector3.UnitY);
            this.playerActor = new MoveableModelObject("m",
                ObjectType.Player, transform,
                this.texturedModelEffect,
                this.textureDictionary["crate1"],
                this.modelDictionary["box"],
                Color.White, 0.8f, 
                KeyData.MoveKeys);

            playerActor.AddController(new Video3DController("vc1", playerActor,
        this.textureDictionary["crate1"], this.videoDictionary["sample"], "sample.wmv", 0.1f, 0.5f));


            this.objectManager.Add(this.playerActor);*/
        }

        private void InitializeSkyBox(int scale)
        {
        /*
            VertexPositionColorTexture[] vertices = VertexFactory.GetTextureQuadVertices();

            Transform3D transform = null;
            TexturedPrimitiveObject texturedPrimitive = null, clone = null;
            IVertexData vertexData = null;
            int halfScale = scale / 2;

            //back
            transform = new Transform3D(new Vector3(0, 0, -halfScale), new Vector3(0, 0, 0), scale * Vector3.One, Vector3.UnitZ, Vector3.UnitY);
            vertexData = this.vertexDictionary["texturedquad"];
            texturedPrimitive = new TexturedPrimitiveObject("sky", ObjectType.Decorator,
            transform, vertexData, this.texturedPrimitiveEffect, Color.White, 1, this.textureDictionary["back"]);              
            this.objectManager.Add(texturedPrimitive);

            //top
            clone = (TexturedPrimitiveObject)texturedPrimitive.Clone();
            clone.Texture = this.textureDictionary["sky"];
            clone.Transform3D.Translation = new Vector3(0, halfScale, 0);
            clone.Transform3D.Rotation = new Vector3(90, -90, 0);
            this.objectManager.Add(clone);

            //left
            clone = (TexturedPrimitiveObject)texturedPrimitive.Clone();
            clone.Texture = this.textureDictionary["left"];
            clone.Transform3D.Translation = new Vector3(-halfScale, 0, 0);
            clone.Transform3D.Rotation = new Vector3(0, 90, 0);
            this.objectManager.Add(clone);

            //right
            clone = (TexturedPrimitiveObject)texturedPrimitive.Clone();
            clone.Texture = this.textureDictionary["right"];
            clone.Transform3D.Translation = new Vector3(halfScale, 0, 0);
            clone.Transform3D.Rotation = new Vector3(0, -90, 0);
            this.objectManager.Add(clone);

            //front
            clone = (TexturedPrimitiveObject)texturedPrimitive.Clone();
            clone.Texture = this.textureDictionary["front"];
            clone.Transform3D.Translation = new Vector3(0, 0, halfScale);
            clone.Transform3D.Rotation = new Vector3(0, 180, 0);
            this.objectManager.Add(clone);
            */
        }

        private void InitializeNonCollidablePrimitives()
        {
            /*
            //Origin
            Transform3D transform = null;
            PrimitiveObject primitiveObject = null;
            IVertexData vertexData = null;

            //origin helper
            transform = new Transform3D(new Vector3(0, 5, -10), Vector3.Zero, 4 * Vector3.One, Vector3.UnitX, Vector3.UnitY);
            vertexData = this.vertexDictionary["origin"];
            primitiveObject = new PrimitiveObject("origin", ObjectType.Helper, transform, vertexData, this.primitiveEffect, Color.White, 1);
            this.objectManager.Add(primitiveObject);
            */
            
        }

        private void AddNonCollidableVideoBillboards()
        {
            //BillboardPrimitiveObject billboardPrimitiveObject = null;
            //Texture2D texture = null;
            //Transform3D transform = null;
            //IVertexData vertexData = PrimitiveFactory.GetVertexDataInstance(this.GraphicsDevice, PrimitiveFactory.GeometryType.BillboardQuad, PrimitiveFactory.StorageType.Buffered);

            ////since its video it doesnt matter what the start texture is
            //texture = this.textureDictionary["tv"];
            ////rotate on +ve X just to look like its hanging on a wall
            ////notice that we scale very small because we later scale up by actual video width and height - see 4 lines down
            //transform = new Transform3D(new Vector3(0, 30, 20), new Vector3(22.5f, 0, 0), new Vector3(0.015f, 0.015f, 1), Vector3.UnitZ, Vector3.UnitY);
            //billboardPrimitiveObject = new BillboardPrimitiveObject(this, transform, this.billboardEffect, vertexData, texture, Color.White, 1f, BillboardType.Normal);

            //Video sampleVideo = this.videoDictionary["sample"];
            //billboardPrimitiveObject.Transform.Scale *= new Vector3(sampleVideo.Width, sampleVideo.Height, 1);
            //billboardPrimitiveObject.ObjectController = new Video3DController(this, "tv1", billboardPrimitiveObject, sampleVideo, "uniqueNameForVideo", 0.05f, 0.1f);
            //this.objectManager.Add(billboardPrimitiveObject);
        }

        private void InitializeNonCollidableBillboards()
        {
            /*
            BillboardPrimitiveObject billboardArchetypeObject = null, cloneBillboardObject = null;

            //archetype - clone from this
            billboardArchetypeObject = new BillboardPrimitiveObject("billboard", ObjectType.Billboard, 
                Transform3D.Zero, //transform reset in clones
                this.vertexDictionary["texturedquad"], 
                this.billboardEffect, Color.White, 1, 
                this.textureDictionary["white"],
                BillboardType.Normal); //texture reset in clones

            #region Normal
            cloneBillboardObject = (BillboardPrimitiveObject)billboardArchetypeObject.Clone();
            cloneBillboardObject.BillboardType = BillboardType.Spherical;
            cloneBillboardObject.Transform3D = new Transform3D(new Vector3(0, 5, -10), Vector3.Zero, 4 * Vector3.One, Vector3.UnitZ, Vector3.UnitY);
            cloneBillboardObject.Texture = this.textureDictionary["chevron1"];
            this.objectManager.Add(cloneBillboardObject);
            #endregion

            
            #region Normal Scrolling
            cloneBillboardObject = (BillboardPrimitiveObject)billboardArchetypeObject.Clone();
            cloneBillboardObject.Transform3D = new Transform3D(new Vector3(-15, 5, -10), new Vector3(45, 0, 0), new Vector3(16, 10, 1), Vector3.UnitX, Vector3.UnitY);
            cloneBillboardObject.Alpha = 0.4f; //remember we can set alpha
            cloneBillboardObject.Texture = this.textureDictionary["ml"];
            cloneBillboardObject.BillboardParameters.SetScrolling(true);
            cloneBillboardObject.BillboardParameters.SetScrollRate(new Vector2(0, -50));
            this.objectManager.Add(cloneBillboardObject);
            #endregion

            #region Normal Animated
            cloneBillboardObject = (BillboardPrimitiveObject)billboardArchetypeObject.Clone();
            cloneBillboardObject.Transform3D = new Transform3D(new Vector3(15, 5, -10), new Vector3(0, 0, 0), new Vector3(4, 4, 1), Vector3.UnitX, Vector3.UnitY);
            cloneBillboardObject.Texture = this.textureDictionary["alarm2"];
            cloneBillboardObject.Color = Color.Red;
            cloneBillboardObject.BillboardParameters.SetAnimated(true);
            cloneBillboardObject.BillboardParameters.SetAnimationRate(4, 1, 0);
            this.objectManager.Add(cloneBillboardObject);
            #endregion

            #region Normal Scrolling - Snow
            cloneBillboardObject = (BillboardPrimitiveObject)billboardArchetypeObject.Clone();
            cloneBillboardObject.Transform3D = new Transform3D(new Vector3(20, -25, -10), new Vector3(0, 0, 0), new Vector3(25, 25, 1), Vector3.UnitX, Vector3.UnitY);
            cloneBillboardObject.Texture = this.textureDictionary["snow1"];
            cloneBillboardObject.BillboardParameters.SetScrolling(true);
            cloneBillboardObject.BillboardParameters.SetScrollRate(new Vector2(5, -15));
            this.objectManager.Add(cloneBillboardObject);

            cloneBillboardObject = (BillboardPrimitiveObject)billboardArchetypeObject.Clone();
            cloneBillboardObject.Transform3D = new Transform3D(new Vector3(20, -25, -15), new Vector3(0, 30, 0), new Vector3(25, 25, 1), Vector3.UnitX, Vector3.UnitY);
            cloneBillboardObject.Texture = this.textureDictionary["snow1"];
            cloneBillboardObject.BillboardParameters.SetScrolling(true);
            cloneBillboardObject.BillboardParameters.SetScrollRate(new Vector2(3, -10));
            this.objectManager.Add(cloneBillboardObject);

            cloneBillboardObject = (BillboardPrimitiveObject)billboardArchetypeObject.Clone();
            cloneBillboardObject.Transform3D = new Transform3D(new Vector3(20, -25, -20), new Vector3(0, -15, 0), new Vector3(25, 25, 1), Vector3.UnitX, Vector3.UnitY);
            cloneBillboardObject.Texture = this.textureDictionary["snow1"];
            cloneBillboardObject.BillboardParameters.SetScrolling(true);
            cloneBillboardObject.BillboardParameters.SetScrollRate(new Vector2(1, -6));
            this.objectManager.Add(cloneBillboardObject);
            #endregion

            #region Normal Text To Texture
            cloneBillboardObject = (BillboardPrimitiveObject)billboardArchetypeObject.Clone();
            cloneBillboardObject.Transform3D = new Transform3D(new Vector3(-30, 5f, 20), new Vector3(0, 0, 0), 
                new Vector3(20, 5, 1), Vector3.UnitX, Vector3.UnitY);
            cloneBillboardObject.Texture = this.textureDictionary["white"];
            cloneBillboardObject.AddController(new TextRendererController("trc1", cloneBillboardObject, 
                this.fontDictionary["ui"], "Press V to play video", Color.Black, 
                new Color(255, 0, 255, 0)));
            this.objectManager.Add(cloneBillboardObject);
            #endregion

            #region Normal Video
            cloneBillboardObject = (BillboardPrimitiveObject)billboardArchetypeObject.Clone();
            cloneBillboardObject.Transform3D = new Transform3D(new Vector3(-30, -5, 20), new Vector3(0, 0, 0),
                new Vector3(8, 8, 1), Vector3.UnitX, Vector3.UnitY);
            cloneBillboardObject.Texture = this.textureDictionary["white"];
            cloneBillboardObject.AddController(new Video3DController("vc1", cloneBillboardObject,
                  this.textureDictionary["white"], this.videoDictionary["sample"], "sample.wmv", 0.1f, 0.5f));
            this.objectManager.Add(cloneBillboardObject);
            #endregion
            
            */
        }

        #endregion

        #region Demos
        //nmcg - 18.3.16
        private void demoPathFinding()
        {
            /*
            PathFindingEngine pathEngine = new PathFindingEngine("enemy path system");

            pathEngine.AddNode(new Node("a", new Vector3(-10, 2, 0)));
            pathEngine.AddNode(new Node("b", new Vector3(-5, 2, 0)));
            pathEngine.AddNode(new Node("c", new Vector3(0, 2, 0)));
            pathEngine.AddNode(new Node("d", new Vector3(5, 2, 0)));
            pathEngine.AddNode(new Node("e", new Vector3(10, 2, 0)));

            //define connections
            pathEngine.AddEdge("a", "b", 1);
            pathEngine.AddEdge("b", "c", 1);
            pathEngine.AddEdge("c", "d", 1);
            pathEngine.AddEdge("d", "e", 1);

            //obviously a could connect directly to e also but with a high cost e.g. 10
            pathEngine.AddEdge("a", "e", 10);


            //demo how to find a path between two points
            List<Node> nodeList = pathEngine.CalculatePath("a", "e");

            //show in debug output
            PathFindingEngine.Print(nodeList);
            */
        }

        ModelObject lastPickedModelObject;
        private void demoMousePicking()
        {
            mouseManager.IsVisible = true;
            if ((this.cameraManager.ActiveCamera != null)
                && (this.mouseManager.IsLeftButtonClicked()))
            {
                Vector3 pos, normal;

                Actor pickedActor = this.mouseManager.GetPickedObject(
                   this.cameraManager.ActiveCamera, 5 /*5 == how far from 1st Person collidable to start testing for collisions - should always exceed capsule collision skin radius*/,
                   1000, out pos, out normal);

                if (pickedActor != null)
                {
                    ModelObject nextPickedModelObject = pickedActor as ModelObject;
                    nextPickedModelObject.OriginalAlpha = 1;
                    nextPickedModelObject.OriginalColor = Color.White;

                    if (nextPickedModelObject != lastPickedModelObject)
                    {
                        //set the last back to original color
                        if (this.lastPickedModelObject != null)
                        {
                            this.lastPickedModelObject.Color =
                                this.lastPickedModelObject.OriginalColor;
                            this.lastPickedModelObject.Alpha =
                                this.lastPickedModelObject.OriginalAlpha;
                        }

                        //set next to picked color
                        nextPickedModelObject.Color = Color.Red;
                        nextPickedModelObject.Alpha = 0.5f;
                    }

                    this.lastPickedModelObject = nextPickedModelObject;
                }
            }
        }


        //nmcg - 18.3.16

        private void demoSoundManager()
        {
            if (this.keyboardManager.IsFirstKeyPress(Keys.B))
            {
                //Notice that the cue name is taken from inside SoundBank1
                //To see the sound bank contents open the file Demo2DSound.xap using XACT3 found through the start menu on Windows
                this.soundManager.PlayCue("boing");
            }
        }

        private void demoCameraTrack(GameTime gameTime)
        {
            /*
            Vector3 translation, look, up; //not used, just to show we can access
            Camera3DTrack cameraTrack = this.trackDictionary["puke"];
            cameraTrack.Evalulate((float)gameTime.TotalGameTime.TotalMilliseconds, 1, out translation, out look, out up);
            */
        }

        private void demoCameraLayout()
        {
            if (this.keyboardManager.IsFirstKeyPress(Keys.F1))
                this.cameraManager.CycleCamera();      
        }


        Vector2 positionOffset = new Vector2(0, 25);
        Color debugColor = Color.Red;
        SpriteFont debugFont = null;
        private Effect animatedModelEffect;

        private void drawDebugInfo()
        {
            //draw debug text after base.Draw() otherwise it will be behind the scene!
            if (debugFont == null)
                debugFont = this.fontDictionary["debug"];

            Vector2 debugPosition = new Vector2(20, 20);

            this.spriteBatch.Begin();
            this.spriteBatch.DrawString(debugFont, "ID:         " + this.cameraManager.ActiveCamera.ID, debugPosition, debugColor);
            debugPosition += positionOffset;

            this.spriteBatch.DrawString(debugFont, "Object Type:" + this.cameraManager.ActiveCamera.ObjectType, debugPosition, debugColor);
            debugPosition += positionOffset;

            this.spriteBatch.DrawString(debugFont, "Translation:" + MathUtility.Round(this.cameraManager.ActiveCamera.Transform3D.Translation, 1), debugPosition, debugColor);
            debugPosition += positionOffset;

            this.spriteBatch.DrawString(debugFont, "Look:       " + MathUtility.Round(this.cameraManager.ActiveCamera.Transform3D.Look, 1), debugPosition, debugColor);
            debugPosition += positionOffset;

            this.spriteBatch.DrawString(debugFont, "Up:         " + MathUtility.Round(this.cameraManager.ActiveCamera.Transform3D.Up, 1), debugPosition, debugColor);
            debugPosition += positionOffset;

            this.spriteBatch.DrawString(debugFont, "F1 - cycle cameras, WASD - move pawn camera, Space - Jump(1st person pawn only)", debugPosition, debugColor);
            this.spriteBatch.End();

        }
        
        private void demoTextToTextureAndVideo()
        {
            /*
            if (this.keyboardManager.IsFirstKeyPress(Keys.V))
            {
                //tell associated text renderer to change text
                EventDispatcher.Publish(new TextEventData("trc1", 
                    this, EventType.OnTextRender, EventCategoryType.TextRender, 
                    this.fontDictionary["ui"], "Video is now playing", Color.Red, Color.Black));

                //now play video
                EventDispatcher.Publish(new VideoEventData("vc1", this, 
                    EventType.OnPlay, EventCategoryType.Video, "sample.wmv"));
            }
            */
        }

        #endregion

        #region Networking
        private void startClient()
        {
            AsynchronousClient guider = new AsynchronousClient();
            AsynchronousClient.start();
        }

        private void startServer()
        {
            AsynchronousSocketListener runner = new AsynchronousSocketListener();
            AsynchronousSocketListener.start();
        }
        #endregion  

        #region Update & Draw
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            #region Demos
            /*
            demoTextToTextureAndVideo();
            demoCameraTrack(gameTime);
            */
            demoCameraLayout();
            demoSoundManager();
            demoMousePicking();
            
            #endregion

            base.Update(gameTime);
        }

 
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            this.graphics.GraphicsDevice.Viewport = this.cameraManager.ActiveCamera.Viewport;
            base.Draw(gameTime);

            if(this.menuManager.Pause)
                drawDebugInfo();
        }
        #endregion
    }
}
