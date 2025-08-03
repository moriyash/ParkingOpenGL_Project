using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
namespace OpenGL
{
    public partial class cOGL
    {
        public void LoadParkingTextures()
        {
            GenerateTextures();      
            LoadSkyboxTextures();   
        }
        void LoadSkyboxTextures()
        {
            string[] skyboxNames = {
        "pic1.jpg", "pic2.jpg", "pic3.jpg",
        "pic4.jpg", "pic5.jpg", "pic6.jpg"
    };

            GL.glGenTextures(6, SkyboxTextures);

            for (int i = 0; i < 6; i++)
            {
                try
                {
                    Bitmap image = new Bitmap(@"Textures/Skybox/" + skyboxNames[i]);
                    image.RotateFlip(RotateFlipType.RotateNoneFlipY);

                    Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                    BitmapData bitmapdata = image.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                    GL.glBindTexture(GL.GL_TEXTURE_2D, SkyboxTextures[i]);

                    GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_S, (int)GL.GL_REPEAT);
                    GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_T, (int)GL.GL_REPEAT);
                    GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);
                    GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);

                    GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int)GL.GL_RGB8, image.Width, image.Height,
                                    0, GL.GL_BGR_EXT, GL.GL_UNSIGNED_byte, bitmapdata.Scan0);

                    image.UnlockBits(bitmapdata);
                    image.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading skybox texture {skyboxNames[i]}: {ex.Message}");
                }
            }
        }
        void DrawSkybox(float size, float uOffset, float vOffset)
        {
            GL.glDisable(GL.GL_LIGHTING);
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glDepthMask((byte)GL.GL_FALSE);

            GL.glRotatef(textureRotation * 0.03f, 0, 0, 1); 

            GL.glColor3f(1.0f, 1.0f, 1.0f);

            float half = size / 2f;

            float t0 = 0.0f;  
            float t1 = 1.0f;  

            // bottom
            GL.glBindTexture(GL.GL_TEXTURE_2D, SkyboxTextures[0]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(t0, t0); GL.glVertex3f(-half, -half, -half);
            GL.glTexCoord2f(t1, t0); GL.glVertex3f(half, -half, -half);
            GL.glTexCoord2f(t1, t1); GL.glVertex3f(half, half, -half);
            GL.glTexCoord2f(t0, t1); GL.glVertex3f(-half, half, -half);
            GL.glEnd();

            // top 
            GL.glBindTexture(GL.GL_TEXTURE_2D, SkyboxTextures[1]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(t0, t0); GL.glVertex3f(-half, -half, half);
            GL.glTexCoord2f(t1, t0); GL.glVertex3f(half, -half, half);
            GL.glTexCoord2f(t1, t1); GL.glVertex3f(half, half, half);
            GL.glTexCoord2f(t0, t1); GL.glVertex3f(-half, half, half);
            GL.glEnd();

            // back
            GL.glBindTexture(GL.GL_TEXTURE_2D, SkyboxTextures[2]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(t0, t0); GL.glVertex3f(-half, -half, -half);
            GL.glTexCoord2f(t1, t0); GL.glVertex3f(-half, -half, half);
            GL.glTexCoord2f(t1, t1); GL.glVertex3f(half, -half, half);
            GL.glTexCoord2f(t0, t1); GL.glVertex3f(half, -half, -half);
            GL.glEnd();

            // front
            GL.glBindTexture(GL.GL_TEXTURE_2D, SkyboxTextures[3]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(t0, t0); GL.glVertex3f(-half, half, -half);
            GL.glTexCoord2f(t1, t0); GL.glVertex3f(-half, half, half);
            GL.glTexCoord2f(t1, t1); GL.glVertex3f(half, half, half);
            GL.glTexCoord2f(t0, t1); GL.glVertex3f(half, half, -half);
            GL.glEnd();

            //left
            GL.glBindTexture(GL.GL_TEXTURE_2D, SkyboxTextures[4]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(t0, t0); GL.glVertex3f(-half, -half, -half);
            GL.glTexCoord2f(t1, t0); GL.glVertex3f(-half, half, -half);
            GL.glTexCoord2f(t1, t1); GL.glVertex3f(-half, half, half);
            GL.glTexCoord2f(t0, t1); GL.glVertex3f(-half, -half, half);
            GL.glEnd();

            // right
            GL.glBindTexture(GL.GL_TEXTURE_2D, SkyboxTextures[5]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(t0, t0); GL.glVertex3f(half, -half, -half);
            GL.glTexCoord2f(t1, t0); GL.glVertex3f(half, half, -half);
            GL.glTexCoord2f(t1, t1); GL.glVertex3f(half, half, half);
            GL.glTexCoord2f(t0, t1); GL.glVertex3f(half, -half, half);
            GL.glEnd();

            GL.glDepthMask((byte)GL.GL_TRUE);
            GL.glEnable(GL.GL_LIGHTING);
        }
        protected void UpdateSkyboxAnimation()
        {
            
            textureRotation += 0.3f; 
        }
        void GenerateTextures()
        {
            if (!texturesEnabled) return;
            GL.glGenTextures(8, Textures);
            string[] imagesName = { "floor.jpg", "parking_floor.jpg", "road.jpg", "Road_Center.jpg", "Security.jpg", "Waiting_Corner.jpg", "Wall_Parking.jpg", "Road_out2.jpg" };
            string basePath = Application.StartupPath + @"\Textures\";

            for (int i = 0; i < 8; i++)
            {
                string fullPath = Path.Combine(basePath, imagesName[i]);
                if (!File.Exists(fullPath))
                {
                    MessageBox.Show("no file exists " + fullPath);
                    continue;
                }

                Bitmap image = new Bitmap(fullPath);
                image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                BitmapData bitmapdata = image.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[i]);

                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_S, (int)GL.GL_REPEAT);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_T, (int)GL.GL_REPEAT);

                GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int)GL.GL_RGB8, image.Width, image.Height, 0, GL.GL_BGR_EXT, GL.GL_UNSIGNED_byte, bitmapdata.Scan0);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);

                GL.glTexEnvf(GL.GL_TEXTURE_ENV, GL.GL_TEXTURE_ENV_MODE, GL.GL_MODULATE);

                image.UnlockBits(bitmapdata);
                image.Dispose();
            }
        }
        void ResetTextureMatrix()
        {
            GL.glMatrixMode(GL.GL_TEXTURE);
            GL.glLoadIdentity();
            GL.glMatrixMode(GL.GL_MODELVIEW);
        }

        public struct MaterialProperties
        {
            public float[] ambient;
            public float[] diffuse;
            public float[] specular;
            public float shininess;

            public MaterialProperties(float[] amb, float[] diff, float[] spec, float shine)
            {
                ambient = amb;
                diffuse = diff;
                specular = spec;
                shininess = shine;
            }
        }
        private string currentMaterialType = "gold";
        private Dictionary<string, MaterialProperties> materials = new Dictionary<string, MaterialProperties>()
        {
            ["gold"] = new MaterialProperties(
          new float[] { 0.24725f, 0.2245f, 0.0645f, 1.0f },
          new float[] { 0.34615f, 0.3143f, 0.0903f, 1.0f },
          new float[] { 0.797357f, 0.723991f, 0.208006f, 1.0f },
          83.2f
      ),

            ["emerald"] = new MaterialProperties(
          new float[] { 0.0215f, 0.1745f, 0.0215f, 0.55f },
          new float[] { 0.07568f, 0.61424f, 0.07568f, 0.55f },
          new float[] { 0.633f, 0.727811f, 0.633f, 0.55f },
          76.8f
      ),

            ["chrome"] = new MaterialProperties(
          new float[] { 0.25f, 0.25f, 0.25f, 1.0f },
          new float[] { 0.4f, 0.4f, 0.4f, 1.0f },
          new float[] { 0.774597f, 0.774597f, 0.774597f, 1.0f },
          76.8f
      ),
            ["white"] = new MaterialProperties(
        new float[] { 0.2f, 0.2f, 0.2f, 1.0f },
        new float[] { 1.0f, 1.0f, 1.0f, 1.0f },
        new float[] { 0.3f, 0.3f, 0.3f, 1.0f },
        16.0f
    )
        };
        private void SetMaterial(MaterialProperties material)
        {
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT, material.ambient);
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_DIFFUSE, material.diffuse);
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_SPECULAR, material.specular);
            GL.glMaterialf(GL.GL_FRONT_AND_BACK, GL.GL_SHININESS, material.shininess);
        }
        public void SetCurrentMaterial(string materialType)
        {
            if (materials.ContainsKey(materialType))
            {
                currentMaterialType = materialType;
                SetMaterial(materials[materialType]);
            }
        }
        private void ApplyMaterialForObject(string objectType)
        {
            switch (objectType)
            {
                case "plant":
                case "flower":
                case "flowerpot":
                    SetMaterial(materials[currentMaterialType]);
                    break;
                default:
                    break;


            }
        }
    }
}
