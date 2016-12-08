using JigLibX.Math;
using JigLibX.Physics;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using JigLibX.Collision;

namespace GDLibrary
{
    public class PotionZoneObject : ZoneObject
    {
        
        #region Variables
        private DrawnActor potion;
        
        //temp vars
        private CollidableObject currentCollidableObject;
        private bool collisionStart = false;
        #endregion

        #region Properties
        public DrawnActor Potion
        {
            get
            {
                return this.potion;
            }
            set
            {
                this.potion = value;
            }
        }
        #endregion

        public PotionZoneObject(string id, ObjectType objectType, Transform3D transform,
            Effect effect, Color color, float alpha,
            CollidableObject targetObject, bool isImpenetrable, DrawnActor potion)
            : base(id, objectType, transform, effect, color, alpha, targetObject, isImpenetrable)
        {
            this.potion = potion;
        }

        public override bool HandleCollision(CollisionSkin collider, CollisionSkin collidee)
        {
            if ((this.currentCollidableObject == null) && (collidee.Owner.ExternalData is PlayerObject))
            {
                PlayerObject playerObject = collidee.Owner.ExternalData as PlayerObject;
                EventDispatcher.Publish(new EventData("potion", this, EventType.OnZoneEnter, EventCategoryType.Zone, potion));
                this.currentCollidableObject = playerObject;
                this.collisionStart = true;
            }
            else
            {
                this.currentCollidableObject = null;
            }

            if (this.currentCollidableObject == null && this.collisionStart == true)
            {
                EventDispatcher.Publish(new EventData("potion", this, EventType.OnZoneExit, EventCategoryType.Zone, potion));
                this.collisionStart = false;
            }
            
               
            
            return base.HandleCollision(collider, collidee);
        }
    }
}
