﻿using SharedProject.Base;
using SharedProject.Interface.Atomic;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedResProject
{
    public class DrawBuffer : BaseGLClass
    {
        private readonly BufferObject<float> Vbo;
        private readonly BufferObject<uint> Ebo;
        private readonly VertexArrayObject<float, uint> Vao;

        public DrawBuffer(GL gl) : base(gl)
        {
            Ebo = new BufferObject<uint>(Gl, Indices, BufferTargetARB.ElementArrayBuffer);
            Vbo = new BufferObject<float>(Gl, Vertices, BufferTargetARB.ArrayBuffer);
            Vao = new VertexArrayObject<float, uint>(Gl, Vbo, Ebo);

            //Telling the VAO object how to lay out the attribute pointers
            Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 5, 0);
            Vao.VertexAttributePointer(1, 3, VertexAttribPointerType.Float, 5, 3);
        }
        public static readonly float[] Vertices =
{
            //X    Y      Z     U   V
             1.0f,  1.0f, 0.0f, 1f, 0f,
             1.0f, -1.0f, 0.0f, 1f, 1f,
            -1.0f, -1.0f, 0.0f, 0f, 1f,
            -1.0f,  1.0f, 0.0f, 0f, 0f
        };

        public static readonly uint[] Indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        public void Bind()
        {
            Vao.Bind();
        }

        public override void Dispose()
        {
            //Remember to dispose all the instances.
            Vbo.Dispose();
            Ebo.Dispose();
            Vao.Dispose();

            base.Dispose();
        }

    }
}
