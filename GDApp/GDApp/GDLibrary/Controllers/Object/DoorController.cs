using Microsoft.Xna.Framework;
using System;

namespace GDLibrary
{
    public class DoorController : Controller
    {
        private bool isDoorOpen = true;
        private bool lockDoor = false;

        public bool IsDoorOpen
        {
            get
            {
                return this.isDoorOpen;
            }
            set
            {
                this.isDoorOpen = value;
            }
        }

        public DoorController(string id, Actor parentActor, bool isDoorOpen)
            : base(id, parentActor)
        {
            this.isDoorOpen = isDoorOpen;
        }

        
        public override void Update(GameTime gameTime)
        {
            game.EventDispatcher.ZoneChanged += new EventDispatcher.ZoneEventHandler(eventDispatcher_ZoneChanged);

            //DOOR OPENED =>    y = 30f
            //DOOR CLOSED =>    y = 12.5f
            if (this.ParentActor != null)
            {
                if (this.lockDoor == false)
                {
                    if (this.isDoorOpen != true)
                    {
                        if (this.ParentActor.Transform3D.Translation.Y > 12.5f)
                        {
                            this.ParentActor.Transform3D.Translation -= 0.2f * Vector3.UnitY;
                        }
                        else
                        {
                            CollidableObject actor = this.ParentActor as CollidableObject;
                            this.lockDoor = true;
                            actor.Enable(true, 1);
                        }
                    }
                    else
                    {
                        this.ParentActor.Transform3D.Translation = new Vector3(this.ParentActor.Transform3D.Translation.X, 30, this.ParentActor.Transform3D.Translation.Z);
                    }
                }
            }
        }

        private void eventDispatcher_ZoneChanged(EventData eventData)
        {
            if (eventData.ID == "start")
            {
                if (eventData.EventType == EventType.OnZoneExit)
                {
                    this.isDoorOpen = false;
                }
            }
        }
    }
}