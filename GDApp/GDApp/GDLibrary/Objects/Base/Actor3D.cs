﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
namespace GDLibrary
{
    public class Actor3D : Actor, ICloneable
    {
        #region Variables
        public Transform3D transform;   //maybe revert this back to private at some stage
        private List<IController> controllerList;
        #endregion

        #region Properties
        public List<IController> ControllerList
        {
            get
            {
                return this.controllerList;
            }
        }
        public Transform3D Transform3D
        {
            get
            {
                return this.transform;
            }
            // no set
        }
        #endregion

        public Actor3D(string id, ActorType actorType,
                            Transform3D transform)
            : base(id, actorType)
        {

            this.transform = transform;
        }

        public void AttachController(IController controller)
        {
            if(this.controllerList == null)
                this.controllerList = new List<IController>();
            this.controllerList.Add(controller); //duplicates?
        }
        public bool DetachController(string id)
        {
            return false; //to do...
        }
        public bool DetachController(IController controller)
        {
            return false; //to do...
        }

        public override void Update(GameTime gameTime)
        {
            if (this.controllerList != null)
            {
                foreach (IController controller in this.controllerList)
                    controller.Update(gameTime, this); //you control me, update!
            }
            base.Update(gameTime);
        }



        public object Clone()
        {
            return new Actor3D("clone - " + ID, //deep
                this.ActorType, //deep
                (Transform3D)this.transform.Clone()); //deep
        }
    }
}
