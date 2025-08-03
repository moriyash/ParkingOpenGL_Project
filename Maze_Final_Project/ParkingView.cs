using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
namespace OpenGL
{
    public partial class cOGL
    {
        private void SetupParkingViewportAndMatrices()
        {
            GL.glViewport(0, 0, Width, Height);
            GL.glMatrixMode(GL.GL_PROJECTION);
            GL.glLoadIdentity();

            GL.glEnable(GL.GL_CULL_FACE);
            GL.glCullFace(GL.GL_BACK);        
            GL.glFrontFace(GL.GL_CCW);        

            float aspectRatio = (float)Width / Height;
            if (isOrthogonal)
            {
                float orthoSize = zoomDistance * 2.0f; 
                GL.glOrtho(-orthoSize * aspectRatio, orthoSize * aspectRatio,
                           -orthoSize, orthoSize,
                           1.0f, 5000.0f);
            }
            else
            {
                GLU.gluPerspective(45.0f, aspectRatio, 1.0f, 5000.0f);
            }
            GL.glMatrixMode(GL.GL_MODELVIEW);
            GL.glLoadIdentity();
        }
        public void ToggleProjectionMode()
        {
            isOrthogonal = !isOrthogonal;
            OnResize();
        }

    }
}
