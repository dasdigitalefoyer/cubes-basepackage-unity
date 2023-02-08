namespace PuzzleCubes.Config
{
    using UnityEngine;
    public static class App
    {
       

        public static string cubeId = SystemInfo.deviceName;
        

        public static string GetGlobalAppStateTopic() { return "puzzleCubes/app";}
        public static string GetDedicatedAppStateTopic(string name ) { return $"puzzleCubes/{name}/app";}
    }
}
