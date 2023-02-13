

namespace PuzzleCubes.Models
{

    [System.Serializable]

    public class AppState : BaseData
    {
        
        [UnityEngine.SerializeField] private string appName = "<APP>";

        [UnityEngine.SerializeField] private string cubeId;
        [UnityEngine.SerializeField] private bool isRunning;

        public string AppName { get => appName; set => appName = value; }
        public string CubeId { get => cubeId; set => cubeId = value; }
        public bool IsRunning { get => isRunning; set => isRunning = value; }
    }

}