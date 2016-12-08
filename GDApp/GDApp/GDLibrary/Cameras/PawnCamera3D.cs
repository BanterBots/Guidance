using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDLibrary
{
    //Modifies a basic static camera to allow us to add multiple controllers e.g. rail, track, 1st or 3rd person
    public class PawnCamera3D : Camera3D
    {
        #region Fields

        #endregion

        #region Properties
     
        #endregion

        public PawnCamera3D(string id, ObjectType objectType,
            Transform3D transform, ProjectionParameters projectionParameters, Viewport viewPort)
                : base(id, objectType, transform, projectionParameters, viewPort)
        {

        }

        //add clone...
        public override object Clone()
        {
            PawnCamera3D clone
                = new PawnCamera3D("clone - " + this.ID,
                    this.ObjectType,
                    (Transform3D)this.Transform3D.Clone(), //deep copy
                    (ProjectionParameters)this.ProjectionParameters.Clone(), //deep copy - contains "copy by value" types
                    this.Viewport); //shallow

            if(this.ControllerList != null)  //if it has controllers then clone
            {
                for (int i = 0; i < this.ControllerList.Count; i++)
                {
                    IController cloneController = (IController)this.ControllerList[i].Clone();
                    //set cloned camera as new parent NOT original camera
                    cloneController.SetParentActor(clone);
                    clone.AddController(cloneController);
                }
            }

            return clone;
        }

    }
}
