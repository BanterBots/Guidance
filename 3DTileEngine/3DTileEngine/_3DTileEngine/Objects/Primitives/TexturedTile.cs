using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3DTileEngine
{
    public class TexturedTile
        : DrawableGameComponent
    {
        private Vector3 position, rotation, scale;
        private BasicEffect effect;
        private Texture2D texture;
        private Main game;
        private VertexPositionColorTexture[] verts;

        public TexturedTile(Main game,
            Vector3 position, Vector3 rotation,
            Vector3 scale, BasicEffect effect,
            Texture2D texture)
            : base(game)
        {
            this.game = game;
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.effect = effect;
            this.texture = texture;
        }

        public override void Initialize()
        {
            IntialiseVertices();
            base.Initialize();
        }

        private void IntialiseVertices()
        {
            this.verts = new VertexPositionColorTexture[4];
            float halfLength = this.scale.X / 2;

            //BL
            this.verts[0] = new VertexPositionColorTexture(
                new Vector3(-halfLength, -halfLength, 0), Color.White,
                new Vector2(0, 1));

            //TL
            this.verts[1] = new VertexPositionColorTexture(
               new Vector3(-halfLength, halfLength, 0), Color.White,
               new Vector2(0, 0));

            //BR
            this.verts[2] = new VertexPositionColorTexture(
               new Vector3(halfLength, -halfLength, 0), Color.White,
               new Vector2(1, 1));

            //TR
            this.verts[3] = new VertexPositionColorTexture(
               new Vector3(halfLength, halfLength, 0), Color.White,
               new Vector2(1, 0));

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.effect.View = this.game.Camera.View;
            this.effect.Projection = this.game.Camera.Projection;
            this.effect.World = Matrix.Identity
                * Matrix.CreateRotationX(
                MathHelper.ToRadians(this.rotation.X))
                 * Matrix.CreateRotationY(
                MathHelper.ToRadians(this.rotation.Y))
                 * Matrix.CreateRotationZ(
                MathHelper.ToRadians(this.rotation.Z))
                * Matrix.CreateTranslation(this.position * this.scale);

            this.effect.Texture = this.texture;
            this.effect.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            this.effect.CurrentTechnique.Passes[0].Apply();
            this.effect.GraphicsDevice.DrawUserPrimitives
            <VertexPositionColorTexture>(PrimitiveType.TriangleStrip, this.verts, 0, 2);

            base.Draw(gameTime);
        }



    }
}
