

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
            sb.AppendLine("<style=DebugHeader>AppState");
			sb.Append("</style><style=DebugContent>"); // End style in new line to use line-height change
            sb.Append("CubeId:\t\t").Append(CubeId).AppendLine();
			sb.Append("MqttConnected:\t").Append(MqttConnected).AppendLine();
			sb.Append("Volume:\t\t").Append(Volume).AppendLine();
			sb.Append("</style>");
			return sb.ToString();
		}
    }

}