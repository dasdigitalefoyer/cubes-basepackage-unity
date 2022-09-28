namespace PuzzleCubes
{
    namespace Applications
    {


        using PuzzleCubes.Models;
        using UnityEngine;
        using Newtonsoft.Json.Linq;

        public abstract class AppEventDispatcher : MonoBehaviour
        {
            public abstract void HandleDatagram(JRaw data);
        }


    }
}