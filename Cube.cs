using System;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Resources;
using System.Reflection;

namespace Test1 {
    class Cube {
        private Vertex[] vertex = new Vertex[24];
        private ushort[] index = new ushort[36];
        private uint 
            vertexVertexBufferObjectID,
            indexVertexBufferObjectID,
            textureStorage;

        public void drawVBO() {
            GL.BindTexture(TextureTarget.Texture2D, textureStorage);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexVertexBufferObjectID);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexVertexBufferObjectID);

            GL.EnableClientState(ArrayCap.TextureCoordArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.NormalArray);
            GL.EnableClientState(ArrayCap.VertexArray);

            GL.InterleavedArrays(InterleavedArrayFormat.T2fC4fN3fV3f, 0, IntPtr.Zero);

            GL.DrawElements(BeginMode.Triangles, 36, DrawElementsType.UnsignedShort, 0);

            GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.DisableClientState(ArrayCap.ColorArray);
            GL.DisableClientState(ArrayCap.NormalArray);
            GL.DisableClientState(ArrayCap.VertexArray);
        }

        public void drawOther() {
            GL.Begin(BeginMode.Quads);
            for (int i = 0; i < vertex.Length; i += 4) {
                GL.Normal3(vertex[i].normal);

                GL.Color4(vertex[i].color);
                GL.TexCoord2(vertex[i].texture);
                GL.Vertex3(vertex[i].position);

                GL.Color4(vertex[i+1].color);
                GL.TexCoord2(vertex[i + 1].texture);
                GL.Vertex3(vertex[i + 1].position);

                GL.Color4(vertex[i+2].color);
                GL.TexCoord2(vertex[i + 2].texture);
                GL.Vertex3(vertex[i + 2].position);

                GL.Color4(vertex[i+3].color);
                GL.TexCoord2(vertex[i + 3].texture);
                GL.Vertex3(vertex[i + 3].position);
            }
            GL.End();
        }

        public void initialize() {
            buildCube();

            loadTextures();

            int stride = BlittableValueType.StrideOf(vertex);
            
            //Vertexes
            GL.GenBuffers(1, out vertexVertexBufferObjectID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexVertexBufferObjectID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(24 * stride), vertex, BufferUsageHint.StaticDraw);

            GL.InterleavedArrays(InterleavedArrayFormat.T2fC4fN3fV3f, 0, IntPtr.Zero);

            //Indexes
            GL.GenBuffers(1, out indexVertexBufferObjectID);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexVertexBufferObjectID);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(36 * sizeof(ushort)), IntPtr.Zero, BufferUsageHint.StaticDraw);
            GL.BufferSubData(BufferTarget.ElementArrayBuffer, IntPtr.Zero, (IntPtr)(36 * sizeof(ushort)), index);
        }

        private void loadTextures() {
            Bitmap texture = (Bitmap)new ResourceManager("Test1.Properties.Resources", Assembly.GetExecutingAssembly()).GetObject("grid");

            texture.RotateFlip(RotateFlipType.RotateNoneFlipY);

            Rectangle rect = new Rectangle(0, 0, texture.Width, texture.Height);

            System.Drawing.Imaging.BitmapData bitmapdata = texture.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            GL.GenTextures(1, out textureStorage);
            GL.BindTexture(TextureTarget.Texture2D, textureStorage);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, bitmapdata.Width, bitmapdata.Height, 0, PixelFormat.Bgr, PixelType.UnsignedByte, bitmapdata.Scan0);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);		// Linear Filtering
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);		// Linear Filtering

            texture.UnlockBits(bitmapdata);
            texture.Dispose();
        }

        private void buildCube() {
            assignVertexes();
            assignColors();
            assignIndexes();
        }

        private void assignColors() {
            Random ran = new Random();
            for (int i = 0; i < vertex.Length; i+=4) {
                vertex[i].color = new Vector4((float)ran.NextDouble(), (float)ran.NextDouble(), (float)ran.NextDouble(), 1);
                vertex[i + 1].color = vertex[i].color;
                vertex[i + 2].color = vertex[i].color;
                vertex[i + 3].color = vertex[i].color;
            }
        }

        private void assignVertexes() {
            for (int i = 0; i < vertex.Length; i++)
                vertex[i] = new Vertex();

            //Front
            int index = 0; //0
            vertex[index].position = new Vector3(1, -1, 1);
            vertex[index].normal = new Vector3(0, 0, 1);
            vertex[index].texture = new Vector2(1, 0);

            index += 1; //1
            vertex[index].position = new Vector3(1, 1, 1);
            vertex[index].normal = new Vector3(0, 0, 1);
            vertex[index].texture = new Vector2(1, 1);

            index += 1; //2
            vertex[index].position = new Vector3(-1, 1, 1);
            vertex[index].normal = new Vector3(0, 0, 1);
            vertex[index].texture = new Vector2(0, 1);

            index += 1; //3
            vertex[index].position = new Vector3(-1, -1, 1);
            vertex[index].normal = new Vector3(0, 0, 1);
            vertex[index].texture = new Vector2(0, 0);

            //Back
            index += 1; //4
            vertex[index].position = new Vector3(-1, -1, -1);
            vertex[index].normal = new Vector3(0, 0, -1);
            vertex[index].texture = new Vector2(1, 0);

            index += 1; //5
            vertex[index].position = new Vector3(-1, 1, -1);
            vertex[index].normal = new Vector3(0, 0, -1);
            vertex[index].texture = new Vector2(1, 1);

            index += 1;//6
            vertex[index].position = new Vector3(1, 1, -1);
            vertex[index].normal = new Vector3(0, 0, -1);
            vertex[index].texture = new Vector2(0, 1);

            index += 1;//7
            vertex[index].position = new Vector3(1, -1, -1);
            vertex[index].normal = new Vector3(0, 0, -1);
            vertex[index].texture = new Vector2(0, 0);

            //Top
            index += 1;//8
            vertex[index].position = new Vector3(-1, 1, -1);
            vertex[index].normal = new Vector3(0, 1, 0);
            vertex[index].texture = new Vector2(0, 1);

            index += 1;//9
            vertex[index].position = new Vector3(-1, 1, 1);
            vertex[index].normal = new Vector3(0, 1, 0);
            vertex[index].texture = new Vector2(0, 0);

            index += 1;//10
            vertex[index].position = new Vector3(1, 1, 1);
            vertex[index].normal = new Vector3(0, 1, 0);
            vertex[index].texture = new Vector2(1, 0);

            index += 1; //11
            vertex[index].position = new Vector3(1, 1, -1);
            vertex[index].normal = new Vector3(0, 1, 0);
            vertex[index].texture = new Vector2(1, 1);

            // Bottom Face
            index += 1;//12
            vertex[index].position = new Vector3(-1, -1, -1);
            vertex[index].normal = new Vector3(0, -1, 0);
            vertex[index].texture = new Vector2(1, 1);

            index += 1;//13
            vertex[index].position = new Vector3(1, -1, -1);
            vertex[index].normal = new Vector3(0, -1, 0);
            vertex[index].texture = new Vector2(0, 1);

            index += 1;//14
            vertex[index].position = new Vector3(1, -1, 1);
            vertex[index].normal = new Vector3(0, -1, 0);
            vertex[index].texture = new Vector2(0, 0);

            index += 1; //15
            vertex[index].position = new Vector3(-1, -1, 1);
            vertex[index].normal = new Vector3(0, -1, 0);
            vertex[index].texture = new Vector2(1, 0);

            // Right Face
            index += 1;//16
            vertex[index].position = new Vector3(1, -1, -1);
            vertex[index].normal = new Vector3(1, 0, 0);
            vertex[index].texture = new Vector2(1, 0);

            index += 1;//17
            vertex[index].position = new Vector3(1, 1, -1);
            vertex[index].normal = new Vector3(1, 0, 0);
            vertex[index].texture = new Vector2(1, 1);

            index += 1;//18
            vertex[index].position = new Vector3(1, 1, 1);
            vertex[index].normal = new Vector3(1, 0, 0);
            vertex[index].texture = new Vector2(0, 1);

            index += 1; //19
            vertex[index].position = new Vector3(1, -1, 1);
            vertex[index].normal = new Vector3(1, 0, 0);
            vertex[index].texture = new Vector2(0, 0);

            // Left Face
            index += 1;//20
            vertex[index].position = new Vector3(-1, -1, -1);
            vertex[index].normal = new Vector3(-1, 0, 0);
            vertex[index].texture = new Vector2(0, 0);

            index += 1;//21
            vertex[index].position = new Vector3(-1, -1, 1);
            vertex[index].normal = new Vector3(-1, 0, 0);
            vertex[index].texture = new Vector2(1, 0);

            index += 1;//22
            vertex[index].position = new Vector3(-1, 1, 1);
            vertex[index].normal = new Vector3(-1, 0, 0);
            vertex[index].texture = new Vector2(1, 1);

            index += 1; //23
            vertex[index].position = new Vector3(-1, 1, -1);
            vertex[index].normal = new Vector3(-1, 0, 0);
            vertex[index].texture = new Vector2(0, 1);
        }

        private void assignIndexes() {
            //Front face
            index[0] = 0; index[1] = 1; index[2] = 2;
            index[3] = 2; index[4] = 3; index[5] = 0;

            //Back face
            index[6] = 4; index[7] = 5; index[8] = 6;
            index[9] = 6; index[10] = 7; index[11] = 4;

            //Top
            index[12] = 8; index[13] = 9; index[14] = 10;
            index[15] = 10; index[16] = 11; index[17] = 8;

            //Bottom
            index[18] = 12; index[19] = 13; index[20] = 14;
            index[21] = 14; index[22] = 15; index[23] = 12;

            //Right
            index[24] = 16; index[25] = 17; index[26] = 18;
            index[27] = 18; index[28] = 19; index[29] = 16;

            //Left
            index[30] = 20; index[31] = 21; index[32] = 22;
            index[33] = 22; index[34] = 23; index[35] = 20;
        }
    }
}
