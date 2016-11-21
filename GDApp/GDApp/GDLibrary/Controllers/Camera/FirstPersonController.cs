using GDApp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GDLibrary
{
    public class FirstPersonController : Controller
    {
        private float moveSpeed, strafeSpeed, rotationSpeed;
        private Keys[] moveKeys;
        #region Fields
        #endregion

        #region Properties
        public Keys[] MoveKeys
        {
            get
            {
                return this.moveKeys;
            }
            set
            {
                this.moveKeys = value;
            }
        }
        public float RotationSpeed
        {
            get
            {
                return this.rotationSpeed;
            }
            set
            {
                this.rotationSpeed = (value > 0) ? value : 0.1f;
            }
        }
        public float MoveSpeed
        {
            get
            {
                return this.moveSpeed;
            }
            set
            {
                this.moveSpeed = (value > 0) ? value : 0.1f;
            }
        }
        public float StrafeSpeed
        {
            get
            {
                return this.strafeSpeed;
            }
            set
            {
                this.strafeSpeed = (value > 0) ? value : 0.1f;
            }
        }
        #endregion

        public FirstPersonController(string id, Actor parentActor,
            Keys[] moveKeys, float moveSpeed, float strafeSpeed, float rotationSpeed)
            : base(id, parentActor)
        {
            this.MoveKeys = moveKeys;
            this.MoveSpeed = moveSpeed;
            this.StrafeSpeed = strafeSpeed;
            this.RotationSpeed = rotationSpeed;
        }

        public override void Update(GameTime gameTime)
        {
            HandleKeyboardInput(gameTime);
            HandleMouseInput(gameTime);
        }

        public virtual void HandleMouseInput(GameTime gameTime)
        {
            Vector2 mouseDelta = game.MouseManager.GetDeltaFromPosition(game.ScreenCentre);
            mouseDelta *= gameTime.ElapsedGameTime.Milliseconds;
            mouseDelta *= 0.01f;
            this.ParentActor.Transform3D.RotateBy(new Vector3(-mouseDelta, 0));

        }

        public virtual void HandleKeyboardInput(GameTime gameTime)
        {
            if (game.KeyboardManager.IsKeyDown(this.moveKeys[KeyData.KeysIndexRotateLeft]))
            {
                float speedMultiplier = this.strafeSpeed * gameTime.ElapsedGameTime.Milliseconds;
                this.ParentActor.Transform3D.TranslateBy(-this.ParentActor.Transform3D.Right, speedMultiplier);
            }
            else if (game.KeyboardManager.IsKeyDown(this.moveKeys[KeyData.KeysIndexRotateRight]))
            {
                float speedMultiplier = this.strafeSpeed * gameTime.ElapsedGameTime.Milliseconds;
                this.ParentActor.Transform3D.TranslateBy(this.ParentActor.Transform3D.Right, speedMultiplier);
            }
            
            if (game.KeyboardManager.IsKeyDown(this.moveKeys[KeyData.KeysIndexMoveForward]))
            {
                Vector3 temp = this.ParentActor.Transform3D.Look;
                temp.Y = 0;
                float speedMultiplier = this.moveSpeed * gameTime.ElapsedGameTime.Milliseconds;
                this.ParentActor.Transform3D.TranslateBy(temp, speedMultiplier);
            }
            else if (game.KeyboardManager.IsKeyDown(this.moveKeys[KeyData.KeysIndexMoveBackward]))
            {
                Vector3 temp = this.ParentActor.Transform3D.Look;
                temp.Y = 0;
                float speedMultiplier = this.moveSpeed * gameTime.ElapsedGameTime.Milliseconds;
                this.ParentActor.Transform3D.TranslateBy(-temp, speedMultiplier);
            }
        }


        public override object Clone()
        {
            return new FirstPersonController("clone - " + this.ID,
                this.ParentActor, //shallow - reference
                this.moveKeys, //shallow - reference
                this.moveSpeed, //primitive so a simple copy
                this.strafeSpeed, //primitive so a simple copy
                this.rotationSpeed); //primitive so a simple copy
        }
    }
}
