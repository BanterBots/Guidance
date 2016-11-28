

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class PotionObject : ModelObject
    {
        private PotionType potionType;
        public PotionType PotionType
        {
            get
            {
                return potionType;
            }
            set
            {
                potionType = value;
            }
        }

        public PotionObject(string id, ObjectType objectType, Transform3D transform, Effect effect, Texture2D texture, Model model, Color color, float alpha, PotionType potionType) : base(id,objectType,transform,effect,texture,model,color,alpha)
        {
            this.potionType = potionType;
        }
        
    }
}
