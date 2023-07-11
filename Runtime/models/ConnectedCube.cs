using UnityEngine;

namespace PuzzleCubes
{
    namespace Models
    {
        public class ConnectedCube
        {
            [SerializeField] private string id;
            [SerializeField] private CubeDirection edge;

            public string Id
            {
                get => id;
                set => id = value;
            }

            public CubeDirection Edge
            {
                get => edge;
                set => edge = value;
            }
        }
    }
}