using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
namespace OpenGL
{
    public partial class cOGL
    {
        void DrawParkingLot()
        {
            GL.glPushMatrix();

            //GL.glEnable(GL.GL_LIGHTING);
            //GL.glEnable(GL.GL_COLOR_MATERIAL);
            //GL.glColorMaterial(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE);

            GL.glTranslatef(-4.0f, -3.5f, 0);

            float lotWidth = 8.5f;
            float lotHeight = 6.0f;
            float spaceWidth = 1.08f;
            float roadGap = 2.0f;

            DrawParkingBackground(lotWidth, lotHeight);
            DrawParkingSurfaces(lotWidth, lotHeight, roadGap);
            DrawRoads(lotWidth, lotHeight, roadGap);
            DrawParkingSpaces(lotWidth, lotHeight, spaceWidth, roadGap);
            DrawParkingInfrastructure(lotWidth, lotHeight);


            GL.glPopMatrix();
        }
        private void DrawParkingInfrastructure(float lotWidth, float lotHeight)
        {
            DrawSecurityBooth(lotWidth + 3.3f, lotHeight / 2 + 0.49f);
            DrawGate(lotWidth + 1.8f, lotHeight / 2 -0.1f);
            DrawParkingWall();
            DrawLighting(lotWidth, lotHeight);
            DrawTrafficLights();
        }
    }
}
