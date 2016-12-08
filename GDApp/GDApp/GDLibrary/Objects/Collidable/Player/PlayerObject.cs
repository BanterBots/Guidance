using GDApp;
using JigLibX.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GDLibrary
{
    /// <summary>
    /// Represents your MOVEABLE player in the game. 
    /// </summary>
    public class PlayerObject : CharacterObject
    {
        #region Variables
        private Keys[] moveKeys;
        private Vector3 translationOffset;
        #endregion

        #region Properties
        public Vector3 TranslationOffset
        {
            get
            {
                return translationOffset;
            }
            set
            {
                translationOffset = value;
            }
        }
        public Keys[] MoveKeys
        {
            get
            {
                return moveKeys;
            }
            set
            {
                moveKeys = value;
            }
        }
        #endregion

        public PlayerObject(string id, ObjectType objectType, Transform3D transform,
            Effect effect, Texture2D texture, Model model, Color color, float alpha,
            Keys[] moveKeys, float radius, float height, float accelerationRate, float decelerationRate, Vector3 translationOffset)
            : base(id, objectType, transform, effect, texture, model, color, alpha, radius, height, accelerationRate, decelerationRate)
        {
            this.moveKeys = moveKeys;
            this.translationOffset = translationOffset;
        }

        public override Matrix GetWorldMatrix()
        {
            return Matrix.CreateScale(this.Transform3D.Scale) *
                this.Collision.GetPrimitiveLocal(0).Transform.Orientation *
                this.Body.Orientation *
                this.Transform3D.Orientation *
                Matrix.CreateTranslation(this.Body.Position + translationOffset);
        }


        public override void Update(GameTime gameTime)
        {
            HandleKeyboardInput(gameTime);
            HandleMouseInput(gameTime);
            base.Update(gameTime);
        }

        protected virtual void HandleMouseInput(GameTime gameTime)
        {
          //perhaps rotate using mouse pointer distance from centre?
        }

        protected virtual void HandleKeyboardInput(GameTime gameTime)
        {
            //jump
            if (game.KeyboardManager.IsKeyDown(this.MoveKeys[KeyData.KeysIndexMoveJump]))
            {
                this.CharacterBody.DoJump(GameData.PlayerJumpHeight);
            }
            //crouch
            else if (game.KeyboardManager.IsKeyDown(this.MoveKeys[KeyData.KeysIndexMoveCrouch]))
            {
                this.CharacterBody.IsCrouching = !this.CharacterBody.IsCrouching;
            }

            //forward/backward
            if (game.KeyboardManager.IsKeyDown(this.MoveKeys[KeyData.KeysIndexMoveForward]))
            {
                this.CharacterBody.Velocity += this.Transform3D.Look * 1 * gameTime.ElapsedGameTime.Milliseconds;
            }
            else if (game.KeyboardManager.IsKeyDown(this.MoveKeys[KeyData.KeysIndexMoveBackward]))
            {
                this.CharacterBody.Velocity -= this.Transform3D.Look * 1 * gameTime.ElapsedGameTime.Milliseconds;
            }
            else //decelerate to zero when not pressed
            {
                this.CharacterBody.DesiredVelocity = Vector3.Zero;
            }

            //strafe left/right
            if (game.KeyboardManager.IsKeyDown(this.MoveKeys[KeyData.KeysIndexRotateLeft]))
            {
                this.Transform3D.RotateAroundYBy(0.1f * gameTime.ElapsedGameTime.Milliseconds);
            }
            else if (game.KeyboardManager.IsKeyDown(this.MoveKeys[KeyData.KeysIndexRotateRight]))
            {
                this.Transform3D.RotateAroundYBy(-0.1f * gameTime.ElapsedGameTime.Milliseconds);
            }
            else //decelerate to zero when not pressed
            {
                this.CharacterBody.DesiredVelocity = Vector3.Zero;
            }

            //update the camera position to reflect the collision skin position
            this.Transform3D.Translation = this.CharacterBody.Position;
        }

        //Do we want to detect if the player object collides with something?
        //in this case we need to add the CollisionSkin_callbackFn() method
        // - See CollidableObject::CollisionSkin_callbackFn
     
    }
}
