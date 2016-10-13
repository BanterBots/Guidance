using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace _3DTileEngine
{
    public class Camera3D : GameComponent
    {
        private Matrix view, projection;
        private Vector3 position, look, up, right;
        private float fieldOfView, aspectRatio;
        private float nearClipPlane, farClipPlane;

        //temp - remove later - just for movement
        private float moveSpeed = 0.05f;

        public Matrix View
        {
            get
            {
                return this.view;
            }
        }
        public Matrix Projection
        {
            get
            {
                return this.projection;
            }
        }

        public Camera3D(Main game, Vector3 position,
            Vector3 look, Vector3 up,
            float fieldOfView, float aspectRatio,
            float nearClipPlane, float farClipPlane)
            : base(game)
        {
            this.position = position;
            this.look = Vector3.Normalize(look);
            this.up = Vector3.Normalize(up);
            this.right = Vector3.Normalize(Vector3.Cross(this.look, this.up));

            this.fieldOfView = fieldOfView;
            this.aspectRatio = aspectRatio;
            this.nearClipPlane = nearClipPlane;
            this.farClipPlane = farClipPlane;

            this.view = Matrix.CreateLookAt(position,
                position + look, up);

            this.projection
           = Matrix.CreatePerspectiveFieldOfView(fieldOfView,
          aspectRatio, nearClipPlane, farClipPlane);

        }

        public override void Update(GameTime gameTime)
        {
            HandleInput(gameTime);
            Updateview();
            base.Update(gameTime);
        }

        private void Updateview()
        {
            this.view = Matrix.CreateLookAt(this.position,
                this.position + this.look, this.up);
        }

        private void HandleInput(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            if(ks.IsKeyDown(Keys.W))
            {
                this.position += this.look * gameTime.ElapsedGameTime.Milliseconds * this.moveSpeed;
            }
            else if (ks.IsKeyDown(Keys.S))
            {
                this.position -= this.look * gameTime.ElapsedGameTime.Milliseconds * this.moveSpeed;
            }

            if (ks.IsKeyDown(Keys.A))
            {
                //remember left == -right
                this.position += -this.right * gameTime.ElapsedGameTime.Milliseconds * this.moveSpeed;
            }
            else if (ks.IsKeyDown(Keys.D))
            {
                this.position += this.right * gameTime.ElapsedGameTime.Milliseconds * this.moveSpeed;
            }

        }



    }
}
