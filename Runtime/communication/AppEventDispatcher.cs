namespace PuzzleCubes
{
    namespace Applications
    {


        using PuzzleCubes.Models;
        using UnityEngine;

        public abstract class AppEventDispatcher : MonoBehaviour
        {
            public abstract void HandleDatagram(string data);
        }


    }
}