
using Microsoft.Xna.Framework;
using System;

namespace GDLibrary
{
    public class PlayerArrowController : Controller
    {
        private Actor target;

        public PlayerArrowController(string id, Actor parentActor, Actor target)
            : base(id, parentActor)
        {
            this.target = target;
        }

        public override void Update(GameTime gameTime)
        {

            //float size = this.ParentActor.Transform3D.Scale.X;
            //float height = this.ParentActor.Transform3D.Translation.Y;

            //Vector3 rotation = this.target.Transform3D.Rotation;
            //rotation.X = -90;
            //this.ParentActor.Transform3D.Rotation = rotation;

            //float xOffset = (float)Math.Sin(MathHelper.ToRadians(rotation.X));
            //float yOffset = (float)Math.Sin(MathHelper.ToRadians(rotation.Y));
            //float zOffset = (float)Math.Sin(MathHelper.ToRadians(rotation.Z));

            //Vector3 translation = this.target.Transform3D.Translation;
            ////translation.X -= xOffset * size;
            //translation.Y = height;
            //translation.Z -= yOffset * size;
            //this.ParentActor.Transform3D.Translation = translation;

            float size = this.ParentActor.Transform3D.Scale.X;

            Vector3 rotation = this.target.Transform3D.Rotation;
            rotation.X = -90;
            this.ParentActor.Transform3D.Rotation = rotation;


            float xOffset = 0, yOffset = 0;
            float newRotation = rotation.Y;

            int dir = 0;
            if (newRotation < 0)
            {
                dir = -1;
            }
            else
            {
                dir = 1;
            }

            newRotation = newRotation * dir; // make it positive
            newRotation = newRotation % 360;
            if (newRotation >= 0 && newRotation <= 90)        // between 0-90
            {
                xOffset = (size) * (newRotation / 90);
                yOffset = (size) * (newRotation / 90);
            }

            else if (newRotation > 90 && newRotation <= 180)   // between 90-180
            {
                xOffset = (size) * (newRotation / 180);
                yOffset = (size) * (newRotation / 180);
            }
            
            else if (newRotation > 180 && newRotation <= 270)   // between 180-270
            {
                xOffset = (size) * (newRotation / 270);
                yOffset = (size) * (newRotation / 270);
            }

            else if (newRotation > 270 && newRotation <= 360) // between 270-360
            {
                xOffset = (size) * (newRotation / 360);
                yOffset = (size) * (newRotation / 360);
            }

            xOffset *= dir;
            yOffset *= dir;


            //this.ParentActor.Transform3D.Translation = new Vector3(translation.X + offsetX * size, translation.Y, translation.Z + offsetY * size);

            Vector3 translation = this.target.Transform3D.Translation;

            translation.X += xOffset;
            translation.Z += yOffset;
            translation.Y = 100;


            this.ParentActor.Transform3D.Translation = translation;

        }
    }
}
