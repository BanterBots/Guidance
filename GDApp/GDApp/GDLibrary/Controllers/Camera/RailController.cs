using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class RailController : TargetController
    {
        #region Fields
        private RailParameters railParameters;
        #endregion

        #region Properties
        #endregion

        public RailController(string id, Actor parentActor, Actor targetActor, 
                                RailParameters railParameters)
            : base(id, parentActor, targetActor)
        {
            this.railParameters = railParameters;

            //set the initial position of the camera
            this.ParentActor.Transform3D.Translation = railParameters.MidPoint;
        }

        public override void Update(GameTime gameTime)
        {
            //get look vector to target
            Vector3 cameraToTarget = CameraUtility.GetCameraToTarget(this.TargetActor.Transform3D, this.ParentActor.Transform3D);

            //new position for camera if it is positioned between start and the end points of the rail
            Vector3 projectedCameraPosition = this.ParentActor.Transform3D.Translation
                + Vector3.Dot(cameraToTarget, railParameters.Look) * railParameters.Look * gameTime.ElapsedGameTime.Milliseconds;

            //do not allow the camera to move outside the rail
            if (railParameters.InsideRail(projectedCameraPosition))
                this.ParentActor.Transform3D.Translation = projectedCameraPosition;

            //set the camera to look at the object
            this.ParentActor.Transform3D.Look = cameraToTarget;

            //bug - set the up vector???
        }


        public override object Clone()
        {
            return new RailController("clone - " + this.ID,
                this.ParentActor, //shallow - reset normally
                this.TargetActor,//shallow - cloned rail should have same target
                this.railParameters); //shallow - change to deep - add Clone()
        }
    }
}
