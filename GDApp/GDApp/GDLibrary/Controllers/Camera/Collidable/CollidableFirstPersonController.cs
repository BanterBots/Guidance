using GDApp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GDLibrary
{
    /// <summary>
    /// A collidable camera has a body and collision skin from a player object but it has no modeldata or texture
    /// </summary>
    public class CollidableFirstPersonController : FirstPersonController
    {
        #region Variables
        private PlayerObject playerObject;
        #endregion

        #region Properties
        #endregion

        public CollidableFirstPersonController(string id, Actor parentActor, 
                Keys[] moveKeys, float moveSpeed, float strafeSpeed, float rotationSpeed, 
            float radius, float height, float accelerationRate, float decelerationRate, float mass, Vector3 translationOffset)
            : base(id, parentActor, moveKeys, moveSpeed, strafeSpeed, rotationSpeed)
        {
            //to make a collidable camera we make a (collidable) player object and set model and texture to null
            //effectively we attach the camera to an invisible, but collidable, model
            this.playerObject = new PlayerObject("camera player object", ObjectType.CollidableCamera, this.ParentActor.Transform3D,
                null, null, null, Color.White, 1, moveKeys, radius, height, accelerationRate, decelerationRate, translationOffset);
            playerObject.Enable(false, mass);
        }

        public override void HandleMouseInput(GameTime gameTime)
        {
            /*
            Camera3D camera = this.ParentActor as Camera3D;
            Vector2 mouseDelta = game.MouseManager.GetDeltaFromPosition(camera.ViewportCentre);
            this.ParentActor.Transform3D.RotateBy(new Vector3(
                -mouseDelta * gameTime.ElapsedGameTime.Milliseconds * 0.01f, 0));
                */
        }

        public override void HandleKeyboardInput(GameTime gameTime)
        {
            //jump
            if (game.KeyboardManager.IsKeyDown(this.MoveKeys[KeyData.KeysIndexMoveJump]))
            {
                this.playerObject.CharacterBody.DoJump(GameData.CameraJumpHeight);
            }
            //crouch
            else if (game.KeyboardManager.IsKeyDown(this.MoveKeys[KeyData.KeysIndexMoveCrouch]))
            {
                this.playerObject.CharacterBody.IsCrouching = !this.playerObject.CharacterBody.IsCrouching;
            }

            //forward/backward
            if (game.KeyboardManager.IsKeyDown(this.MoveKeys[KeyData.KeysIndexMoveForward]))
            {
                Vector3 restrictedLook = this.ParentActor.Transform3D.Look;
                restrictedLook.Y = 0;
                this.playerObject.CharacterBody.Velocity += restrictedLook * GameData.PlayerMoveSpeed * gameTime.ElapsedGameTime.Milliseconds;
            }
            else if (game.KeyboardManager.IsKeyDown(this.MoveKeys[KeyData.KeysIndexMoveBackward]))
            {
                Vector3 restrictedLook = this.ParentActor.Transform3D.Look;
                restrictedLook.Y = 0;
                this.playerObject.CharacterBody.Velocity -= restrictedLook * GameData.PlayerMoveSpeed * gameTime.ElapsedGameTime.Milliseconds;
            }
            else //decelerate to zero when not pressed
            {
                this.playerObject.CharacterBody.DesiredVelocity = Vector3.Zero;
            }



            //rotate left/right
            if (game.KeyboardManager.IsKeyDown(this.MoveKeys[KeyData.KeysIndexRotateLeft]))
            {
                this.ParentActor.Transform3D.RotateAroundYBy(this.RotationSpeed * gameTime.ElapsedGameTime.Milliseconds);
            }
            else if (game.KeyboardManager.IsKeyDown(this.MoveKeys[KeyData.KeysIndexRotateRight]))
            {
                this.ParentActor.Transform3D.RotateAroundYBy(-this.RotationSpeed * gameTime.ElapsedGameTime.Milliseconds);
            }

            /*
            //strafe left/right
            if (game.KeyboardManager.IsKeyDown(this.MoveKeys[KeyData.KeysIndexRotateLeft]))
            {
                this.ParentActor.Transform3D.RotateAroundYBy(this.RotationSpeed * gameTime.ElapsedGameTime.Milliseconds);
            }
            else if (game.KeyboardManager.IsKeyDown(this.MoveKeys[KeyData.KeysIndexRotateRight]))
            {
                this.ParentActor.Transform3D.RotateAroundYBy(-this.RotationSpeed * gameTime.ElapsedGameTime.Milliseconds);
            }
            else //decelerate to zero when not pressed
            {
                this.playerObject.CharacterBody.DesiredVelocity = Vector3.Zero;
            }
            */
            //update the camera position to reflect the collision skin position
            this.ParentActor.Transform3D.Translation = this.playerObject.CharacterBody.Position;
        }


        //to do - clone, dispose

    }
}
