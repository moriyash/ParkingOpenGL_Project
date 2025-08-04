using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace OpenGL
{
    [Serializable]
    public class SceneData
    {
        public bool LightsOn { get; set; }
        public float[] LightPosition { get; set; } = new float[3];
        public bool TexturesEnabled { get; set; }
        public bool GateOpen { get; set; }
        public int TrafficLightState { get; set; }
        public float ZoomDistance { get; set; }
        public string SceneName { get; set; }
        public DateTime SaveTime { get; set; }
    }

    public static class SceneManager
    {
        public static bool SaveScene(cOGL cgl, string filePath, string sceneName = "")
        {
            try
            {
                var data = new SceneData
                {
                    LightsOn = cgl.light1On,
                    LightPosition = (float[])cgl.pos.Clone(),
                    TexturesEnabled = cgl.texturesEnabled,
                    GateOpen = cgl.gateOpen,
                    TrafficLightState = cgl.trafficLightState,
                    ZoomDistance = cgl.zoomDistance,
                    SceneName = string.IsNullOrEmpty(sceneName) ? "Scene" : sceneName,
                    SaveTime = DateTime.Now
                };

                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving: " + ex.Message);
                return false;
            }
        }

        public static bool LoadScene(cOGL cgl, string filePath)
        {
            try
            {
                if (!File.Exists(filePath)) return false;

                string json = File.ReadAllText(filePath);
                SceneData data = JsonConvert.DeserializeObject<SceneData>(json);

                cgl.light1On = data.LightsOn;
                cgl.light2On = data.LightsOn;
                cgl.pos = (float[])data.LightPosition.Clone();
                cgl.texturesEnabled = data.TexturesEnabled;
                cgl.gateOpen = data.GateOpen;
                cgl.trafficLightState = data.TrafficLightState;
                cgl.zoomDistance = data.ZoomDistance;

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading: " + ex.Message);
                return false;
            }
        }
    }
}