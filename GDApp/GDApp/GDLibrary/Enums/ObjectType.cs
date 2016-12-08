using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDLibrary
{
    public enum ObjectType : sbyte
    {
        //non-collidable drawn objects
        Prop,               //a health pickup, an interactive object like a door
        Decorator,          //a chair, a table
        Player,             //you
        NonPlayerCharacter, //enemy
        Pickup,
        Helper,

        //cameras
        FirstPersonCamera,
        ThirdPersonCamera,
        RailCamera,
        TrackCamera,
        FixedCamera,
        SecurityCamera,
        CollidableCamera,
        PawnCamera,

        //collidable drawn objects
        CollidableGround,
        CollidableProp,
        CollidableTriggerZone,
        CollidableCameraTriggerZone,

        //ui objects
        UIText,
        UITexture2D,
        Billboard,
        AnimatedPlayer,
    }
}
