using UnityEngine;

namespace PuzzleCubes
{
    namespace Models
    {
        public class ConnectedCube
        {
            [SerializeField] private string id;
            [SerializeField] private Edge connectedTo;

            public string Id
            {
                get => id;
                set => id = value;
            }

            public Edge ConnectedTo
            {
                get => connectedTo;
                set => connectedTo = value;
            }
        }
    }
}