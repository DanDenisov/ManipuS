﻿using System.Numerics;

using OpenTK.Graphics.OpenGL4;

using Graphics;

namespace Logic
{
    public enum ObstacleShape
    {
        Box,
        Sphere
    }

    public class Obstacle
    {
        private Vector3[] Data;
        public ImpDualQuat State;

        public Model Model;
        public Collider Collider;

        public Obstacle(Vector3[] data, ImpDualQuat state, ColliderShape shape)
        {
            Data = data;
            State = state;
            
            switch (shape)
            {
                case ColliderShape.Box:
                    Collider = new BoxCollider(Data);
                    break;
                case ColliderShape.Sphere:
                    Collider = new SphereCollider(Data);
                    break;
            }
        }

        public Obstacle(Model model, Collider collider)
        {

        }

        public bool Contains(Vector3 point)
        {
            return Collider.Contains(point);
        }

        public Vector3 Extrude(Vector3 point)
        {
            return Collider.Extrude(point);
        }

        public void Move(Vector3 offset)
        {
            State *= new ImpDualQuat(offset);
            Collider.Center = State.Translation;
        }

        public void Render(Shader shader, bool showCollider = false)
        {
            if (Model == default)
                Model = new Model(MeshVertex.Convert(Data), material: MeshMaterial.White);

            var stateMatrix = State.ToMatrix();
            Model.State = stateMatrix;
            Model.Render(shader, MeshMode.Solid, () =>
            {
                GL.DrawArrays(PrimitiveType.Points, 0, Data.Length);
            });

            if (showCollider)
            {
                Collider.Render(shader, ref stateMatrix);
            }
        }
    }
}
