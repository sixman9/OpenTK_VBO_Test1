using System;
using System.Drawing;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace Test1 {
    class RenderingThread : GameWindow {
        //Variables-----\
        private bool fullscreen;
        private Cube cube = new Cube();
        //Variables-----/
		
        public RenderingThread()
            : base(800, 600, GraphicsMode.Default, "Window") {
            VSync = VSyncMode.On;
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            GL.ShadeModel(ShadingModel.Smooth);
            GL.ClearColor(0, .3f, 0, 0);

            GL.ClearDepth(1.0);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.ColorMaterial);

            cube.initialize();
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, .1f, 1000f);
            GL.LoadMatrix(ref projection);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        protected override void OnUpdateFrame(FrameEventArgs e) {
            if (Keyboard[Key.Escape])
                Exit();

            if (Keyboard[Key.F1]) {
                fullscreen = !fullscreen;
                if (fullscreen)
                    WindowState = WindowState.Fullscreen;
                else
                    WindowState = WindowState.Normal;
            }
        }

        float rot = 0;
        protected override void OnRenderFrame(FrameEventArgs e) {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.LoadIdentity();
            GL.Translate(2, 0, -8);
            GL.Rotate(rot, 1f, 1, .1);
            cube.drawVBO();

            GL.LoadIdentity();
            GL.Translate(-2, 0, -8);
            GL.Rotate(rot, 1f, 1, .1);
            cube.drawOther();

            rot += 1;
            updateFPS();
            SwapBuffers();
        }

        long time = DateTime.Now.Ticks;
        private void updateFPS() {
            if (DateTime.Now.Ticks - time > 10000000) {
                Console.WriteLine(time);
                time = DateTime.Now.Ticks;
                Title = "FPS: " + RenderFrequency;
            }
        }

        public static void Main(){
            new RenderingThread().Run();
        }
    }
}
