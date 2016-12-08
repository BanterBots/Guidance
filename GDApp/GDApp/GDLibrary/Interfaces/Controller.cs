using Microsoft.Xna.Framework;
using GDApp;

namespace GDLibrary
{
    public class Controller : IController
    {
        #region Fields
        public static Main game;

        private string id;
        private Actor parentActor;
        #endregion

        #region Properties
        public Actor ParentActor
        {
            get
            {
                return this.parentActor;
            }
            set
            {
                this.parentActor = value;
            }
        }
        public string ID
        {
            get
            {
                return this.id;
            }
        }
        #endregion

        public Controller(string id, Actor parentActor)
        {
            this.id = id;
            this.parentActor = parentActor;
        }

        public string GetName()
        {
            return this.id;
        }
        public Actor GetParentActor()
        {
            return this.parentActor;
        }

        public void SetParentActor(Actor parentActor)
        {
            this.parentActor = parentActor;
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual object Clone()
        {
            return new Controller("clone - " + this.id,
                //we will generally reset parent actor 
                //since new controller should have new parent
                this.parentActor);
        }
    }
}
