using Microsoft.Xna.Framework;
using System;

namespace GDLibrary
{
    public class DoorController : Controller
    {
        private bool isDoorOpen = true;

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


            //DOOR OPENED =>    z=30f 
            //DOOR CLOSED =>    z=12.5f 
            if (this.ParentActor != null)
            {
                if(this.isDoorOpen == true)
                    this.ParentActor.Transform3D.Translation = new Vector3(this.ParentActor.Transform3D.Translation.X, this.ParentActor.Transform3D.Translation.Y, 30);
                else
                {
                    if(this.ParentActor.Transform3D.Translation.Y > 12.5f)
                    {
                        float sinTime = (float)Math.Sin(MathHelper.ToRadians(300 * (float)gameTime.TotalGameTime.TotalSeconds));

                        sinTime *= 0.5f; //-0.5f -> + 0.5f
                        sinTime += 0.5f; //0 -> 1

                        this.ParentActor.Transform3D.Translation = this.ParentActor.Transform3D.OriginalTranslation - sinTime * 10 * Vector3.UnitY;
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