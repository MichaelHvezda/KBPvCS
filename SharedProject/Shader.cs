using SharedProject.Base;
using SharedProject.Interface.Atomic;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedResProject
{
    public class Shader : BaseGLClass, IHandlerAble
    {
        public uint Handle { get; set; }

        public Shader(GL gl, string vertexPath, string fragmentPath) : base(gl)
        {

            //Load the individual shaders.
            uint vertex = LoadShader(ShaderType.VertexShader, vertexPath);
            uint fragment = LoadShader(ShaderType.FragmentShader, fragmentPath);
            //Create the shader program.
            Handle = Gl.CreateProgram();
            //Attach the individual shaders.
            Gl.AttachShader(Handle, vertex);
            Gl.AttachShader(Handle, fragment);
            Gl.LinkProgram(Handle);
            //Check for linking errors.
            gl.GetProgram(Handle, GLEnum.LinkStatus, out var status);
            if (status == 0)
            {
                throw new Exception($"Program failed to link with error: {gl.GetProgramInfoLog(Handle)}");
            }
            //Detach and delete the shaders
            gl.DetachShader(Handle, vertex);
            gl.DetachShader(Handle, fragment);
            gl.DeleteShader(vertex);
            gl.DeleteShader(fragment);
        }

        public Shader(GL gl, string shaderName) : base(gl)
        {
            var staticPath = Path.GetFullPath("../../../shaders/");
            var vertexPath = Path.Combine(staticPath, shaderName + ".vert");
            var fragmentPath = Path.Combine(staticPath, shaderName + ".frag");

            //Load the individual shaders.
            uint vertex = LoadShader(ShaderType.VertexShader, vertexPath);
            uint fragment = LoadShader(ShaderType.FragmentShader, fragmentPath);
            //Create the shader program.
            Handle = Gl.CreateProgram();
            //Attach the individual shaders.
            gl.AttachShader(Handle, vertex);
            gl.AttachShader(Handle, fragment);
            gl.LinkProgram(Handle);
            //Check for linking errors.
            gl.GetProgram(Handle, GLEnum.LinkStatus, out var status);
            if (status == 0)
            {
                throw new Exception($"Program failed to link with error: {gl.GetProgramInfoLog(Handle)}");
            }
            //Detach and delete the shaders
            gl.DetachShader(Handle, vertex);
            gl.DetachShader(Handle, fragment);
            gl.DeleteShader(vertex);
            gl.DeleteShader(fragment);
        }


        public void Use()
        {
            //Using the program
            Gl.UseProgram(Handle);
        }

        //Uniforms are properties that applies to the entire geometry
        public void SetUniform(string name, int value)
        {
            //Setting a uniform on a shader using a name.
            int location = Gl.GetUniformLocation(Handle, name);
            if (location == -1) //If GetUniformLocation returns -1 the uniform is not found.
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            Gl.Uniform1(location, value);
        }

        public void SetUniform(string name, float value)
        {
            int location = Gl.GetUniformLocation(Handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            Gl.Uniform1(location, value);
        }
        public void SetUniform3(string name, float val1, float val2, float val3)
        {
            int location = Gl.GetUniformLocation(Handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            Gl.Uniform3(location, val1, val2, val3);
        }
        public void SetUniform4(string name, float val1, float val2, float val3, float val4)
        {
            int location = Gl.GetUniformLocation(Handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            Gl.Uniform4(location, val1, val2, val3, val4);
        }
        public void SetUniformVec3(string name, Vector3D<float> vector3D)
        {
            int location = Gl.GetUniformLocation(Handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            Gl.Uniform3(location, vector3D.X, vector3D.Y, vector3D.Z);
        }

        public override void Dispose()
        {
            //Remember to delete the program when we are done.
            Gl.DeleteProgram(Handle);

            base.Dispose();
        }

        private uint LoadShader(ShaderType type, string path)
        {
            //To load a single shader we need to:
            //1) Load the shader from a file.
            //2) Create the Handle.
            //3) Upload the source to opengl.
            //4) Compile the shader.
            //5) Check for errors.
            string src = File.ReadAllText(path);
            uint handle = Gl.CreateShader(type);
            Gl.ShaderSource(handle, src);
            Gl.CompileShader(handle);
            string infoLog = Gl.GetShaderInfoLog(handle);
            if (!string.IsNullOrWhiteSpace(infoLog))
            {
                throw new Exception($"Error compiling shader of type {type}, failed with error {infoLog}");
            }

            return handle;
        }
    }
}
