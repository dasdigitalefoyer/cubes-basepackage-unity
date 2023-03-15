

using System.Text;

namespace PuzzleCubes.Models
{

    [System.Serializable]

    public class AppState : BaseData
    {
        
        [UnityEngine.SerializeField] private string appName = "<APP>";

        [UnityEngine.SerializeField] private string cubeId;
        [UnityEngine.SerializeField] private bool isRunning;

        [UnityEngine.SerializeField] private float volume;

        [UnityEngine.SerializeField] private bool mqttConnected;

        public string AppName { get => appName; set => appName = value; }
        public string CubeId { get => cubeId; set => cubeId = value; }
        public bool IsRunning { get => isRunning; set => isRunning = value; }

        public float Volume { get => volume; set => volume = value; }
        public bool MqttConnected { get => mqttConnected; set => mqttConnected = value; }

        public override string ToString() {
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("CubeId:\t").Append(CubeId);
			sb.AppendLine("MqttConnected:\t").Append(MqttConnected);
			sb.AppendLine("Volume:\t").Append(Volume);
			return sb.ToString();
		}
    }

}