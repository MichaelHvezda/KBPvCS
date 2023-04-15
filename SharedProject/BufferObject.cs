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
    public class BufferObject<TDataType> : BaseGLClass, IHandlerAble, IDisposable
        where TDataType : unmanaged
    {
        //Our Handle, buffertype and the GL instance this class will use, these are private because they have no reason to be public.
        //Most of the time you would want to abstract items to make things like this invisible.
        public uint Handle { get; set; }
        private readonly BufferTargetARB bufferType;

        public unsafe BufferObject(GL gl, Span<TDataType> data, BufferTargetARB buf) : base(gl)
        {
            //Setting the gl instance and storing our buffer type.
            bufferType = buf;

            //Getting the Handle, and then uploading the data to said Handle.
            Handle = Gl.GenBuffer();
            Bind();
            fixed (void* d = data)
            {
                Gl.BufferData(bufferType, (nuint)(data.Length * sizeof(TDataType)), d, BufferUsageARB.StaticDraw);
            }
        }

        public void Bind()
        {
            //Binding the buffer object, with the correct buffer type.
            Gl.BindBuffer(bufferType, Handle);
        }

        public override void Dispose()
        {
            //Remember to delete our buffer.
            Gl.DeleteBuffer(Handle);

            base.Dispose();
        }
    }
}
