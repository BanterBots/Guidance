using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class ThirdPersonController : TargetController
    {
        #region Fields
        private float elevationAngle;
        private float distance, distanceScrollMultiplier;

        //used to dampen camera movement
        private Vector3 oldTranslation;
        private Vector3 oldLook;
        private Vector3 oldUp;
        private float lerpSpeed = 0.2f;
        #endregion

        #region Properties
        public float LerpSpeed
        {
            get
            {
                return lerpSpeed;
            }
            set
            {

                //lerp speed should be in the range >0 and <=1
                lerpSpeed = (value > 0) && (lerpSpeed <= 1) ? value : 0.1f;
            }
        }

        public float DistanceScrollMultiplier
        {
            get
            {
                return distanceScrollMultiplier;
            }
            set
            {
                //distanceScrollMultiplier should not be lower than 0
                distanceScrollMultiplier = (value > 0) ? value : 1;
            }
        }

        public float Distance
        {
            get
            {
                return distance;
            }
            set
            {
                //distance should not be lower than 0
                distance = (value > 0) ? value : 1;
            }
        }
        public float ElevationAngle
        {
            get
            {
                return elevationAngle;
            }
            set
            {
                elevationAngle = value % 360;

                elevationAngle = MathHelper.ToRadians(elevationAngle);
            }
        }
        #endregion

        public ThirdPersonController(string id, Actor parentActor, Actor targetActor,
            float distance, float distanceScrollMultiplier, float elevationAngle, float lerpSpeed)
            : base(id, parentActor, targetActor)
        {
            //call properties to set validation on distance and radian conversion
            this.Distance = distance;
            this.DistanceScrollMultiplier = distanceScrollMultiplier;
            this.ElevationAngle = elevationAngle;

            //dampen camera movement
            this.lerpSpeed = lerpSpeed; //slow = 0.05f, med = 0.1f, fast = 0.2f
            this.oldTranslation = this.ParentActor.Transform3D.Translation;
            this.oldLook = this.ParentActor.Transform3D.Look;
            this.oldUp = this.ParentActor.Transform3D.Up;
        }

        public override void Update(GameTime gameTime)
        {
            this.distance += distanceScrollMultiplier * gameTime.ElapsedGameTime.Milliseconds * -game.MouseManager.GetDeltaFromScrollWheel();
            
            //rotate the target look around the target right to get a vector pointing away from the target at a specified elevation
            Vector3 cameraToTarget 
                = Vector3.Transform(this.TargetActor.Transform3D.Look,
                Matrix.CreateFromAxisAngle(this.TargetActor.Transform3D.Right, elevationAngle));

            //normalize to give unit length, otherwise distance from camera to target will vary over time
            cameraToTarget.Normalize();

            //set the position of the camera to be a set distance from target and at certain elevation angle
            this.ParentActor.Transform3D.Translation = Vector3.Lerp(this.oldTranslation,
                cameraToTarget * distance + this.TargetActor.Transform3D.Translation, lerpSpeed);

            //set the camera to have the same orientation as the target object
            this.ParentActor.Transform3D.Look = Vector3.Lerp(this.oldLook,
                this.TargetActor.Transform3D.Look, lerpSpeed);
            this.ParentActor.Transform3D.Up = Vector3.Lerp(this.oldUp,
                this.TargetActor.Transform3D.Up, lerpSpeed);

            this.oldTranslation = this.ParentActor.Transform3D.Translation;
            this.oldLook = this.ParentActor.Transform3D.Look;
            this.oldUp = this.ParentActor.Transform3D.Up;
        }


        public override object Clone()
        {
            return new ThirdPersonController("clone - " + this.ID,
                this.ParentActor, //shallow - reset normally
                this.TargetActor, //shallow - cloned rail should have same target
                this.distance, this.distanceScrollMultiplier, this.elevationAngle, this.lerpSpeed); 
        }
    }
}
