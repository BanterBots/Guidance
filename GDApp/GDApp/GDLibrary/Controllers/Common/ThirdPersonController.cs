using Microsoft.Xna.Framework;
namespace GDLibrary
{
    public class ThirdPersonController : Controller
    {
        private Actor3D targetActor;
        private float elevationAngle;
        private float distance;
        private Vector3 oldLook;
        private float lerpFactor = 0.01f;
        private Vector3 oldTranslation;

        public ThirdPersonController(string id, ControllerType controllerType,
            Actor3D targetActor, float distance, float elevationAngle)
            :base(id, controllerType)
        {
            this.targetActor = targetActor;
            this.distance = distance;
            this.elevationAngle = MathHelper.ToRadians(elevationAngle);
        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            Actor3D parentActor = actor as Actor3D; //camera

            UpdateParentElevation(gameTime);

            UpdateCamera(gameTime, parentActor);
            
            base.Update(gameTime, actor);
        }

        private void UpdateCamera(GameTime gameTime, Actor3D parentActor)
        {

            //get rotated look vector == cameraLook
            Vector3 cameraLook = Vector3.Transform(this.targetActor.Transform3D.Look,Matrix.CreateFromAxisAngle(this.targetActor.Transform3D.Right,this.elevationAngle));

            //setting camera position
            cameraLook.Normalize(); //set to unit length

            Vector3 newTranslation = this.distance * cameraLook + this.targetActor.Transform3D.Translation;
            parentActor.Transform3D.Translation = MathUtility.Lerp(this.oldTranslation, newTranslation, this.lerpFactor);

            Vector3 newLook = -cameraLook;
            parentActor.Transform3D.Look = MathUtility.Lerp(this.oldLook, newLook, this.lerpFactor);


            this.oldLook = newLook;
            this.oldTranslation = newTranslation;

        }

        private void UpdateParentElevation(GameTime gameTime)
        {
            int scrollDelta = game.MouseManager.GetDeltaFromScrollWheel();
            if (scrollDelta < 0)
            {
                this.distance += 10f;
                this.elevationAngle += 0.05f;
            }
            else if (scrollDelta > 0)
            {
                this.distance -= 10f;
                this.elevationAngle -= 0.05f;
            }
        }

    }
}
