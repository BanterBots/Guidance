using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GDApp;
using System;
using System.Collections;

namespace GDLibrary
{
    public class CameraManager : GameComponent, IEnumerable<Camera3D>
    {
        #region Fields
        private Dictionary<string, List<Camera3D>> cameraDictionary;
        private List<Camera3D> activeCameraList;
        private string currentCameraLayout;
        private int activeCameraIndex;
        private bool bPaused;
        #endregion

        #region Properties
        public Camera3D ActiveCamera
        {
            get
            {
                return this.activeCameraList[this.activeCameraIndex];
            }
        }
        public int ActiveCameraIndex
        {
            set
            {
                this.activeCameraIndex = ((value >= 0)
                    && (value < this.activeCameraList.Count)) ? value : 0;
            }
            get
            {
                return this.activeCameraIndex;
            }
        }
        public string CurrentCameraLayout
        {
            get
            {
                return this.currentCameraLayout;
            }
        }
        public Camera3D this[int index]
        {
            get
            {
                return this.activeCameraList[index];
            }
        }
        public int Size
        {
            get
            {
                return this.activeCameraList.Count;
            }
        }
        public bool Paused
        {
            get
            {
                return bPaused;
            }
            set
            {
                bPaused = value;
            }
        }
        #endregion

        public CameraManager(Main game)
            : base(game)
        {
            this.cameraDictionary =new Dictionary<string, List<Camera3D>>();

            game.EventDispatcher.CameraChanged += new EventDispatcher.CameraEventHandler(EventDispatcher_CameraChanged);

            //register for the menu events
            game.EventDispatcher.MainMenuChanged += EventDispatcher_MainMenu;
        }

 

        #region Event Handling
        //handle the relevant menu events
        public virtual void EventDispatcher_MainMenu(EventData eventData)
        {
            if ((eventData.EventType == EventType.OnPlay) || (eventData.EventType == EventType.OnRestart))
                this.bPaused = false;
            else if (eventData.EventType == EventType.OnPause)
                this.bPaused = true;
        }
        public virtual void EventDispatcher_CameraChanged(EventData eventData)
        {
            CameraEventData cameraEventData = eventData as CameraEventData;
            SetCamera(cameraEventData.CameraLayout, cameraEventData.CameraID);
        }
        #endregion


        public void Add(string cameraLayout, Camera3D camera)
        {
            if (this.cameraDictionary.ContainsKey(cameraLayout))
            {
                List<Camera3D> list = this.cameraDictionary[cameraLayout];

                if(!list.Contains(camera))
                    list.Add(camera);
            }
            else
            {
                List<Camera3D> list = new List<Camera3D>();
                list.Add(camera);
                this.cameraDictionary.Add(cameraLayout, list);
            }
        }

        public void Remove(string cameraLayout, string id)
        {
            if (this.cameraDictionary.ContainsKey(cameraLayout))
            {
                List<Camera3D> list = this.cameraDictionary[cameraLayout];

                foreach (Camera3D camera in list)
                {
                    if(camera.ID.Equals(id))
                        list.Remove(camera);
                }
            }
        }

        /// <summary>
        /// Call to cycle through the camera in the current list
        /// </summary>
        public void CycleCamera()
        {
            this.ActiveCameraIndex += 1;
        }

        public bool SetCamera(string cameraLayout, string cameraID)
        {
            SetCameraLayout(cameraLayout); //set to the appropriate layout
            int index = 0;
            Camera3D camera = null;

            FindCameraBy(cameraLayout, cameraID, out camera, out index); //find the camera
            ActiveCameraIndex = index; //set to be active

            return (camera != null); //true if we found the camera
        }

        public void FindCameraBy(string cameraLayout, string cameraID, 
            out Camera3D camera, out int index)
        {
            camera = null;
            index = -1;

            List<Camera3D> list = this.cameraDictionary[cameraLayout];

            if(list != null)
            {
                for(int i = 0; i < list.Count; i++)
                {
                    if(list[i].ID.Equals(cameraID))
                    {
                        camera = list[i];
                        index = i;
                        break;
                    }
                }
            }
        }

        public void spinMap()
        {

        }

        public bool SetCameraLayout(string cameraLayout)
        {
            //if first time and NULL or not the same as current
            if((this.activeCameraList == null) || (!this.currentCameraLayout.Equals(cameraLayout)))
            {
                //if layout exists in the dictionary
                if(this.cameraDictionary.ContainsKey(cameraLayout))
                {
                    this.activeCameraList = this.cameraDictionary[cameraLayout];
                    this.ActiveCameraIndex = 0;
                    this.currentCameraLayout = cameraLayout;
                    return true;
                }
            }
            return false;
        }
        

        public override void Update(GameTime gameTime)
        {
            if (!this.bPaused)
            {
                for (int i = 0; i < this.activeCameraList.Count; i++)
                {
                    if (this.activeCameraList[i].ID.Equals("camRight"))
                    {
                        //Vector3 rotation = this.activeCameraList[i].Transform3D.Rotation;
                        //Vector3 look = this.activeCameraList[i].Transform3D.Look;
                        //this.activeCameraList[i].Transform3D.Look = (new Vector3(look.X, look.Y + 0.0001f, look.Z));
                        //this.activeCameraList[i].Transform3D.Rotation = (new Vector3(rotation.X, rotation.Y+1, rotation.Z));
                    }
                     this.activeCameraList[i].Update(gameTime);
                    
                }
            }
            base.Update(gameTime);
        }

        public IEnumerator<Camera3D> GetEnumerator()
        {
            return this.activeCameraList.GetEnumerator();

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
