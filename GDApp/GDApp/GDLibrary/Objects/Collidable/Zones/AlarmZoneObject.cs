using GDApp;
using JigLibX.Collision;
using JigLibX.Geometry;
using JigLibX.Math;
using JigLibX.Physics;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class AlarmZoneObject : ZoneObject
    {
        #region 
        //add whatever is specific to this class...
        #endregion

        #region Properties
        //add whatever is specific to this class...
        #endregion

        //no target specified e.g. we detect by object type not specific target address
        public AlarmZoneObject(string id, ObjectType objectType, Transform3D transform, 
            Effect effect, Color color, float alpha, bool isImpenetrable)
            : this(id, objectType, transform,  effect, color, alpha, null, isImpenetrable)
        {

        }

        //we know address of the target
        public AlarmZoneObject(string id, ObjectType objectType, Transform3D transform, 
            Effect effect, Color color, float alpha, 
            CollidableObject targetObject, bool isImpenetrable)
            : base(id, objectType, transform, effect, color, alpha, targetObject, isImpenetrable)
        {
          
        }

        public override bool HandleCollision(CollisionSkin collider, CollisionSkin collidee)
        {
            if (collidee.Owner.ExternalData is PlayerObject)
            {
                PlayerObject playerObject = collidee.Owner.ExternalData as PlayerObject;

                //Event, Sound, Pickup
                //Event, ID, Remove
                //Even, UI, Bullet/Health, Increment
            }

            return base.HandleCollision(collider, collidee);
        }
    }
}
