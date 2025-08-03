using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
namespace OpenGL
{
    public partial class cOGL
    {
        void MakeShadowMatrix(float[,] ground)
        {
            float[] planeCoeff = new float[4];
            float dot;
            float[] v1 = new float[3];
            float[] v2 = new float[3];
            v1[0] = ground[0, 0] - ground[1, 0];
            v1[1] = ground[0, 1] - ground[1, 1];
            v1[2] = ground[0, 2] - ground[1, 2];
            v2[0] = ground[1, 0] - ground[2, 0];
            v2[1] = ground[1, 1] - ground[2, 1];
            v2[2] = ground[1, 2] - ground[2, 2];
            planeCoeff[0] = v1[1] * v2[2] - v1[2] * v2[1];
            planeCoeff[1] = v1[2] * v2[0] - v1[0] * v2[2];
            planeCoeff[2] = v1[0] * v2[1] - v1[1] * v2[0];
            float length = (float)Math.Sqrt(planeCoeff[0] * planeCoeff[0] +
                                           planeCoeff[1] * planeCoeff[1] +
                                           planeCoeff[2] * planeCoeff[2]);
            if (length != 0)
            {
                planeCoeff[0] /= length;
                planeCoeff[1] /= length;
                planeCoeff[2] /= length;
            }
            planeCoeff[3] = -(planeCoeff[0] * ground[2, 0] +
                              planeCoeff[1] * ground[2, 1] +
                              planeCoeff[2] * ground[2, 2]);
            float[] lightPos = { pos[0], pos[1], pos[2], pos[3] };
            dot = planeCoeff[0] * lightPos[0] +
                  planeCoeff[1] * lightPos[1] +
                  planeCoeff[2] * lightPos[2] +
                  planeCoeff[3] * lightPos[3];
            cubeXform[0] = dot - lightPos[0] * planeCoeff[0];
            cubeXform[4] = 0.0f - lightPos[0] * planeCoeff[1];
            cubeXform[8] = 0.0f - lightPos[0] * planeCoeff[2];
            cubeXform[12] = 0.0f - lightPos[0] * planeCoeff[3];
            cubeXform[1] = 0.0f - lightPos[1] * planeCoeff[0];
            cubeXform[5] = dot - lightPos[1] * planeCoeff[1];
            cubeXform[9] = 0.0f - lightPos[1] * planeCoeff[2];
            cubeXform[13] = 0.0f - lightPos[1] * planeCoeff[3];
            cubeXform[2] = 0.0f - lightPos[2] * planeCoeff[0];
            cubeXform[6] = 0.0f - lightPos[2] * planeCoeff[1];
            cubeXform[10] = dot - lightPos[2] * planeCoeff[2];
            cubeXform[14] = 0.0f - lightPos[2] * planeCoeff[3];
            cubeXform[3] = 0.0f - lightPos[3] * planeCoeff[0];
            cubeXform[7] = 0.0f - lightPos[3] * planeCoeff[1];
            cubeXform[11] = 0.0f - lightPos[3] * planeCoeff[2];
            cubeXform[15] = dot - lightPos[3] * planeCoeff[3];
        }
        
    }
}
