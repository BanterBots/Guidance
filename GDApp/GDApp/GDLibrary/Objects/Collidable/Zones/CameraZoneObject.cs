
using JigLibX.Collision;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace GDLibrary
{
    /// <summary>
    /// Represents an area for camera switching 
    /// </summary>
    public class CameraZoneObject : ZoneObject
    {

        #region Fields
        private string cameraID, cameraLayout;

        //temp vars
        private CollidableObject currentCollidableObject;
        #endregion

        #region Properties
        private string CameraID
        {
            get
            {
                return this.cameraID;
            }
            set
            {
                this.cameraID = value;
            }
        }
        private string CameraLayout
        {
            get
            {
                return this.cameraLayout;
            }
            set
            {
                this.cameraLayout = value;
            }
        }

        #endregion

        //no target - just responds to whoever walks through
        public CameraZoneObject(string id, ObjectType objectType, Transform3D transform, 
            Effect effect, Color color, float alpha, 
            bool isImpenetrable, string cameraLayout, string cameraID)
            : this(id, objectType, transform, effect, color, alpha, isImpenetrable, null, cameraLayout, cameraID)
        {

        }

        //specific target
        public CameraZoneObject(string id, ObjectType objectType, Transform3D transform, 
            Effect effect, Color color, float alpha, 
            bool isImpenetrable, CollidableObject targetObject, string cameraLayout, string cameraID)
            : base(id, objectType, transform, effect, color, alpha, isImpenetrable)
        {
            this.cameraLayout = cameraLayout;
            this.cameraID = cameraID;
        }

        public override bool HandleCollision(CollisionSkin collider, CollisionSkin collidee)
        {
            //if colliding with player object and its a new object then publish event
            if ((this.currentCollidableObject == null) && (collidee.Owner.ExternalData is PlayerObject))
            {
                PlayerObject playerObject = collidee.Owner.ExternalData as PlayerObject;
                EventDispatcher.Publish(new CameraEventData(this.ID, this, EventType.OnCameraChanged, EventCategoryType.Camera, this.cameraLayout, this.cameraID));
                this.currentCollidableObject = playerObject;
            }

            return base.HandleCollision(collider, collidee);

            /*
            if (collidee.Owner.ExternalData is PlayerObject)
            {
                PlayerObject playerObject = collidee.Owner.ExternalData as PlayerObject;

                //is this the player we are interested in and has it just entered this zone for the first time
                if (!this.InZone)
                {
                    EventDispatcher.Publish(new CameraEventData(this.ID, this, EventType.OnCameraChanged, this.cameraLayout, this.cameraID));
                    //set so we don't repeat if the player stays in the zone
                    this.InZone = true;
                }
            }
            else
            {
                //we were in it, so now reset
                if(this.InZone)
                    EventDispatcher.Publish(new CameraEventData(this.ID, this, EventType.OnCameraChanged, this.cameraLayout, "collidable 1st person front"));
                
                //reset so the target can re-enter the zone and re-set the camera again
                this.InZone = false;
            }
             * */

            // this.Body.DisableBody(); //remove from physics manager
            // this.Remove();           //remove from object manager

        }
    }
}
