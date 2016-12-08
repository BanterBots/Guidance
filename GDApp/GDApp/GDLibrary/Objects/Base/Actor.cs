using GDApp;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GDLibrary
{
    public class Actor : IActor
    {
        public static Main game;

        #region Fields
        private string id;
        private ObjectType objectType;
        private Transform3D transform;
        private List<IController> controllerList;
        #endregion

        #region Properties
        public List<IController> ControllerList
        {
            get
            {
                return this.controllerList;
            }
            set
            {
                controllerList = value;
            }
        }
        public string ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }
        public ObjectType ObjectType
        {
            get
            {
                return this.objectType;
            }
            set
            {
                this.objectType = value;
            }
        }
        public Transform3D Transform3D
        {
            get
            {
                return this.transform;
            }
            set
            {
                this.transform = value;
            }
        }
        public Matrix World
        {
            get
            {
                return this.transform.World;
            }
        }

        #endregion

        public Actor(string id, ObjectType objectType, Transform3D transform)
        {
            this.id = id;
            this.objectType = objectType;
            this.transform = transform;
        }

        public void AddController(IController controller)
        {
            if (this.controllerList == null)
                this.controllerList = new List<IController>();

            //contains? hash code and equals?
            this.controllerList.Add(controller);
        }

        public void RemoveController(string name)
        {
            for (int i = 0; i < this.controllerList.Count; i++)
            {
                if (this.controllerList[i].GetName().Equals(name))
                {
                    this.controllerList.RemoveAt(i);
                }
            }
        }

        public void RemoveAllControllers()
        {
            if (this.controllerList != null)
                this.controllerList.Clear();
        }



        public virtual void Update(GameTime gameTime)
        {
            if (this.controllerList != null)
            {
                foreach (IController controller in this.controllerList)
                    controller.Update(gameTime);
            }
        }

        public virtual Matrix GetWorldMatrix()
        {
            return this.transform.World;
        }

        public virtual void Remove()
        {
            //tag for garbage collection
            this.transform = null;
        }
    }
}
