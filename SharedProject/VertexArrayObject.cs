using SharedProject.Base;
using SharedProject.Interface.Atomic;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedResProject
{
    public class VertexArrayObject<TVertexType, TIndexType> : BaseGLClass, IHandlerAble
        where TVertexType : unmanaged
        where TIndexType : unmanaged
    {
        public uint Handle { get; set; }

        public VertexArrayObject(GL gl, BufferObject<TVertexType> vbo, BufferObject<TIndexType> ebo) : base(gl)
        {
            //Setting out Handle and binding the VBO and EBO to this VAO.
            Handle = Gl.GenVertexArray();
            Bind();
            vbo.Bind();
            ebo.Bind();
        }

        public unsafe void VertexAttributePointer(uint index, int count, VertexAttribPointerType type, uint vertexSize, int offSet)
        {
            //Setting up a vertex attribute pointer
            Gl.VertexAttribPointer(index, count, type, false, vertexSize * (uint)sizeof(TVertexType), (void*)(offSet * sizeof(TVertexType)));
            Gl.EnableVertexAttribArray(index);
        }

        public void Bind()
        {
            //Binding the vertex array.
            Gl.BindVertexArray(Handle);
        }

        public override void Dispose()
        {
            //Remember to dispose this object so the data GPU side is cleared.
            //We dont delete the VBO and EBO here, as you can have one VBO stored under multiple VAO's.
            Gl.DeleteVertexArray(Handle);

            base.Dispose();
        }
    }
}
