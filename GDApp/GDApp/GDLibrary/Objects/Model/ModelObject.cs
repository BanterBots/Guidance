using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class ModelObject : DrawnActor
    {
        #region Fields
        private Texture2D texture;
        private Model model;
        //each mesh in the model has a bone transform which represent the transformation necessary to position it in 3D design program e.g. 3DS Max
        private Matrix[] boneTransforms;
        #endregion

        #region Properties 
        public Matrix[] BoneTransforms
        {
            get
            {
                return this.boneTransforms;
            }
            set
            {
                this.boneTransforms = value;
            }
        }
        public Model Model
        {
            get
            {
                return this.model;
            }
            set
            {
                this.model = value;
            }
        }
        public Texture2D Texture
        {
            get
            {
                return this.texture;
            }
            set
            {
                this.texture = value;
            }
        }
        #endregion

        public ModelObject(string id, ObjectType objectType, Transform3D transform, Effect effect, Texture2D texture, Model model, Color color, float alpha)
            : base(id, objectType, transform, effect, color, alpha)
        {
            this.texture = texture;
            this.model = model;

            //load bone transforms and copy transfroms to transform array (transforms)
            if (this.model != null)
            {
                this.boneTransforms = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(this.boneTransforms);
            }
        }
    }
}
