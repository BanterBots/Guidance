﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GDApp;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    //Represents the base camera class to which controllers can be attached (to do...)
    public class Camera3D : Actor3D
    {
        public static Main game;

        #region Fields
        private ProjectionParameters projectionParameters;
        private Viewport viewPort;
        #endregion

        #region Properties
        public Matrix View
        {
            get
            {
                return Matrix.CreateLookAt(this.Transform3D.Translation,
                    this.Transform3D.Translation + this.Transform3D.Look,
                    this.Transform3D.Up);
            }
        }
        public Matrix Projection
        {
            get
            {
                return this.projectionParameters.Projection;
            }
        }
        public ProjectionParameters ProjectionParameters
        {
            get
            {
                return this.projectionParameters;
            }
            set
            {
                this.projectionParameters = value;
            }
        }
        #endregion

        //creates a default camera3D - we can use this for a fixed camera archetype i.e. one we will clone - see MainApp::InitialiseCameras()
        public Camera3D(string id, ActorType actorType, Viewport viewPort)
            : this(id, actorType, Transform3D.Zero, 
            ProjectionParameters.StandardMediumFourThree)
        {
            this.viewPort = viewPort;
        }
        
        public Camera3D(string id, ActorType actorType,
            Transform3D transform, ProjectionParameters projectionParameters)
            : base(id, actorType, transform)
        {
            this.projectionParameters = projectionParameters;
        }

        public override bool Equals(object obj)
        {
            Camera3D other = obj as Camera3D;

            return Vector3.Equals(this.Transform3D.Translation, other.Transform3D.Translation)
                && Vector3.Equals(this.Transform3D.Look, other.Transform3D.Look)
                    && Vector3.Equals(this.Transform3D.Up, other.Transform3D.Up)
                        && this.ProjectionParameters.Equals(other.ProjectionParameters);
        }
        public override int GetHashCode() //a simple hash code method 
        {
            int hash = 1;
            hash = hash * 31 + this.Transform3D.Translation.GetHashCode();
            hash = hash * 17 + this.Transform3D.Look.GetHashCode();
            hash = hash * 13 + this.Transform3D.Up.GetHashCode();
            hash = hash * 51 + this.ProjectionParameters.GetHashCode();
            return hash;
        }
        public object Clone()
        {
            return new Camera3D(this.ID,
                this.ActorType, (Transform3D)this.Transform3D.Clone(), (ProjectionParameters)this.projectionParameters.Clone());
        }
        public override string ToString()
        {
            return this.ID
                + ", Translation: " + MathUtility.Round(this.Transform3D.Translation, 0)
                + ", Look: " + MathUtility.Round(this.Transform3D.Look, 0)
                + ", Up: " + MathUtility.Round(this.Transform3D.Up, 0);

        }
    }
}

