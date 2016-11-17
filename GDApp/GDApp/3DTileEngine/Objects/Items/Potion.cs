using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GDLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDApp._3DTileEngine.Objects.Items
{
    class Potion : GDLibrary.CollidableObject
    {
        public Potion(string id, ActorType actorType, Transform3D transform, BasicEffect effect, Color color, float alpha, Texture2D texture, Model model) : base(id, actorType, transform, effect, color, alpha, texture, model)
        {
        }
    }
}
