using JigLibX.Math;
using JigLibX.Physics;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using JigLibX.Collision;

namespace GDLibrary
{
    public class EndZoneObject : ZoneObject
    {

        #region Variables
        #endregion

        #region Properties
        #endregion

        public EndZoneObject(string id, ObjectType objectType, Transform3D transform,
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
                EventDispatcher.Publish(new EventData("end", this, EventType.OnZoneExit, EventCategoryType.Zone));
            }

            return base.HandleCollision(collider, collidee);
        }
    }
}