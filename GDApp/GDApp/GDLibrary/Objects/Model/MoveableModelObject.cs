using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GDLibrary
{
    public class MoveableModelObject : ModelObject
    {
        private Keys[] moveKeys;

        public MoveableModelObject(string id, ObjectType objectType, Transform3D transform, 
            Effect effect, Texture2D texture, Model model, Color color, float alpha,
            Keys[] moveKeys)
            : base(id, objectType, transform, effect, texture, model, color, alpha)
        {
            this.moveKeys = moveKeys;
        }

        public override void Update(GameTime gameTime)
        {
            if(game.KeyboardManager.IsKeyDown(Keys.Y))
            {
                this.Transform3D.TranslateBy(this.Transform3D.Look,
                    0.01f * gameTime.ElapsedGameTime.Milliseconds);
            }
            else if (game.KeyboardManager.IsKeyDown(Keys.H))
            {
                this.Transform3D.TranslateBy(-this.Transform3D.Look,
                    0.01f * gameTime.ElapsedGameTime.Milliseconds);
            }

            if (game.KeyboardManager.IsKeyDown(Keys.G))
            {
                float rot = gameTime.ElapsedGameTime.Milliseconds * 0.1f;
                this.Transform3D.RotateAroundYBy(rot);
            }
            else if (game.KeyboardManager.IsKeyDown(Keys.J))
            {
                float rot = gameTime.ElapsedGameTime.Milliseconds * 0.1f;
                this.Transform3D.RotateAroundYBy(-rot);
            }



            base.Update(gameTime);
        }

    }
}
