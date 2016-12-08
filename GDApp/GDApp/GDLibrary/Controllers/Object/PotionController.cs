using Microsoft.Xna.Framework;
using System;

namespace GDLibrary
{
    public class PotionController : Controller
    {
        private Vector3 rotation;
        private int count = 0;

        public PotionController(string id, Actor parentActor, Vector3 rotation)
            : base(id, parentActor)
        {
            this.rotation = rotation;
        }

        public override void Update(GameTime gameTime)
        {

            if (this.ParentActor != null)
            {
                float sinTime = (float)Math.Sin(MathHelper.ToRadians(300 * (float)gameTime.TotalGameTime.TotalSeconds));

                sinTime *= 0.5f; //-0.5f -> + 0.5f
                sinTime += 0.5f; //0 -> 1

                //calculate the new translation by adding to the original translation
                this.ParentActor.Transform3D.Translation =
                   this.ParentActor.Transform3D.OriginalTranslation
                           + sinTime * Vector3.UnitY * 5;

                //rotate
                this.ParentActor.Transform3D.RotateBy(this.rotation
                    * count * gameTime.ElapsedGameTime.Milliseconds);
                count++;
            }
        }
    }
}
