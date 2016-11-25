
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
            float size = this.ParentActor.Transform3D.Scale.X;
            float height = this.ParentActor.Transform3D.Translation.Y;

            Vector3 rotation = this.target.Transform3D.Rotation;
            rotation.X = -90;
            this.ParentActor.Transform3D.Rotation = rotation;

            float xOffset = (float)Math.Sin(MathHelper.ToRadians(rotation.X));
            float yOffset = (float)Math.Sin(MathHelper.ToRadians(rotation.Y));
            float zOffset = (float)Math.Sin(MathHelper.ToRadians(rotation.Z));

            Vector3 translation = this.target.Transform3D.Translation;
            //translation.X -= xOffset * size;
            translation.Y = height;
            translation.Z -= yOffset * size;
            this.ParentActor.Transform3D.Translation = translation;

            
        }
    }
}
