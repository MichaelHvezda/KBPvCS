using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedResProject
{
    public class Shader : IDisposable
    {
        //Our Handle and the GL instance this class will use, these are private because they have no reason to be public.
        //Most of the time you would want to abstract items to make things like this invisible.
        private uint handle;
        private GL gl;

        public Shader(GL _gl, string vertexPath, string fragmentPath)
        {
            gl = _gl;

            //Load the individual shaders.
            uint vertex = LoadShader(ShaderType.VertexShader, vertexPath);
            uint fragment = LoadShader(ShaderType.FragmentShader, fragmentPath);
            //Create the shader program.
            handle = gl.CreateProgram();
            //Attach the individual shaders.
            gl.AttachShader(handle, vertex);
            gl.AttachShader(handle, fragment);
            gl.LinkProgram(handle);
            //Check for linking errors.
            gl.GetProgram(handle, GLEnum.LinkStatus, out var status);
            if (status == 0)
            {
                throw new Exception($"Program failed to link with error: {gl.GetProgramInfoLog(handle)}");
            }
            //Detach and delete the shaders
            gl.DetachShader(handle, vertex);
            gl.DetachShader(handle, fragment);
            gl.DeleteShader(vertex);
            gl.DeleteShader(fragment);
        }

        public Shader(GL _gl, string shaderName)
        {
            gl = _gl;

            //var staticPath = "C:\\Users\\Hvězdič\\source\\repos\\KBPvCS\\KBPvCS\\shaders\\";
            var staticPath = Path.GetFullPath("../../../shaders/");
            var vertexPath = Path.Combine(staticPath, shaderName + ".vert");
            var fragmentPath = Path.Combine(staticPath, shaderName + ".frag");

            //Load the individual shaders.
            uint vertex = LoadShader(ShaderType.VertexShader, vertexPath);
            uint fragment = LoadShader(ShaderType.FragmentShader, fragmentPath);
            //Create the shader program.
            handle = gl.CreateProgram();
            //Attach the individual shaders.
            gl.AttachShader(handle, vertex);
            gl.AttachShader(handle, fragment);
            gl.LinkProgram(handle);
            //Check for linking errors.
            gl.GetProgram(handle, GLEnum.LinkStatus, out var status);
            if (status == 0)
            {
                throw new Exception($"Program failed to link with error: {gl.GetProgramInfoLog(handle)}");
            }
            //Detach and delete the shaders
            gl.DetachShader(handle, vertex);
            gl.DetachShader(handle, fragment);
            gl.DeleteShader(vertex);
            gl.DeleteShader(fragment);
        }


        public void Use()
        {
            //Using the program
            gl.UseProgram(handle);
        }

        //Uniforms are properties that applies to the entire geometry
        public void SetUniform(string name, int value)
        {
            //Setting a uniform on a shader using a name.
            int location = gl.GetUniformLocation(handle, name);
            if (location == -1) //If GetUniformLocation returns -1 the uniform is not found.
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            gl.Uniform1(location, value);
        }

        public void SetUniform(string name, float value)
        {
            int location = gl.GetUniformLocation(handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            gl.Uniform1(location, value);
        }
        public void SetUniform3(string name, float val1, float val2, float val3)
        {
            int location = gl.GetUniformLocation(handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            gl.Uniform3(location, val1, val2, val3);
        }
        public void SetUniform4(string name, float val1, float val2, float val3, float val4)
        {
            int location = gl.GetUniformLocation(handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            gl.Uniform4(location, val1, val2, val3, val4);
        }

        public void Dispose()
        {
            //Remember to delete the program when we are done.
            gl.DeleteProgram(handle);
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
            uint handle = gl.CreateShader(type);
            gl.ShaderSource(handle, src);
            gl.CompileShader(handle);
            string infoLog = gl.GetShaderInfoLog(handle);
            if (!string.IsNullOrWhiteSpace(infoLog))
            {
                throw new Exception($"Error compiling shader of type {type}, failed with error {infoLog}");
            }

            return handle;
        }
    }
}
