using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GDLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JigLibX.Collision;

namespace GDApp._3DTileEngine
{
    public class ModelTileObject :  TriangleMeshObject, ICloneable
    {

        #region Variables
        private Texture2D texture;
        public Model collisionModel;
        private Model model;
        private Matrix[] boneTransforms;
        public int modelNo;
        public int x, y, tileSize, rotation;
        #endregion

        #region Properties
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
        #endregion

        public ModelTileObject(string id, ObjectType objectType, Transform3D transform, BasicEffect effect, Color color, float alpha, Texture2D texture, Model model, int modelNo,
                    int x, int y) : base(id, objectType, transform, effect, texture, model, color, alpha, new MaterialProperties(0.2f, 0.8f, 0.7f))

        {
            this.texture = texture;
            this.model = model;
            this.x = x;
            this.y = y;
            this.modelNo = modelNo;

            InitializeBoneTransforms();
        }

        public ModelTileObject(string id, ObjectType objectType, Transform3D transform, BasicEffect effect, Color color, float alpha, Texture2D texture, Model model, Model collisionModel, int modelNo,
             int x, int y) : base(id, objectType, transform, effect, texture, model, collisionModel, color, alpha, new MaterialProperties(0.2f, 0.8f, 0.7f))

        {
            this.texture = texture;
            this.model = model;
            this.collisionModel = collisionModel;
            this.x = x;
            this.y = y;
            this.modelNo = modelNo;

            InitializeBoneTransforms();
        }

        public void InitializeCollision()
        {
            this.Enable(true, 1);
            this.ObjectType = ObjectType.CollidableProp;
        }

        private void InitializeBoneTransforms()
        {
            //load bone transforms and copy transfroms to transform array (transforms)
            if(this.collisionModel != null)
            {
                this.boneTransforms = new Matrix[this.collisionModel.Bones.Count];
                collisionModel.CopyAbsoluteBoneTransformsTo(this.boneTransforms);
            }
            else if (this.model != null)
            {
                this.boneTransforms = new Matrix[this.model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(this.boneTransforms);
            }
        }

        /*
        public override void Draw(GameTime gameTime)
        {
            this.Effect.View = game.CameraManager.ActiveCamera.View;
            this.Effect.Projection = game.CameraManager.ActiveCamera.Projection;
            this.Effect.World = this.Transform3D.World;
            this.Effect.DiffuseColor = this.Color.ToVector3();
            this.Effect.Alpha = this.Alpha;
            this.Effect.Texture = this.texture;
            this.Effect.CurrentTechnique.Passes[0].Apply();

            foreach (ModelMesh mesh in this.Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = this.Effect;
                }
                this.Effect.World
                    = this.BoneTransforms[mesh.ParentBone.Index]
                                    * this.Transform3D.World;
                mesh.Draw();
            }

            base.Draw(gameTime);
        }*/

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
