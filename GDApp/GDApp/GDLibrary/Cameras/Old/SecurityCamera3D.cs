using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class SecurityCamera3D : Camera3D
    {
        private int maxSweepAngle;
        private int sweepSpeed;
        //constructor
        public SecurityCamera3D(string id, ObjectType objectType,
            Transform3D transform, ProjectionParameters projectionParameters, 
            Viewport viewPort, int maxSweepAngle, int sweepSpeed)
            : base(id, objectType, transform, projectionParameters, viewPort)
        {
            this.maxSweepAngle = maxSweepAngle;
            this.sweepSpeed = sweepSpeed;
        }

        //update
        public override void Update(GameTime gameTime)
        {
            float sinOfTime = (float)Math.Sin(
                MathHelper.ToRadians(
                    this.sweepSpeed*(float)gameTime.TotalGameTime.TotalSeconds));

            float rotationAngle = sinOfTime * this.maxSweepAngle;

            this.Transform3D.RotateBy(Vector3.UnitX * rotationAngle);

            base.Update(gameTime);
        }
    }
}
