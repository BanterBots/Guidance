using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GDLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDApp._3DTileEngine.Objects.Items
{
    class Potion : CollidableObject
    {
        public Potion(string id, ObjectType objectType, Transform3D transform, BasicEffect effect, Color color, float alpha, Texture2D texture, Model model) : base(id, objectType, transform, effect, texture, model, color, alpha)
        {
        }
    }
}
