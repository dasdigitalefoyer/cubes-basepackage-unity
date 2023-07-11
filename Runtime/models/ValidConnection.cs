using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleCubes
{
    namespace Models
    {
        public class ValidConnection
        {
            [SerializeField] 
            private Edge connectedEdge;
            [SerializeField] 
            private bool valid;

            [SerializeField] 
            private ConnectedCube connectedCubeData;

            public Edge ConnectedEdge
            {
                get => connectedEdge;
                set => connectedEdge = value;
            }

            public bool Valid
            {
                get => valid;
                set => valid = value;
            }

            public ConnectedCube ConnectedCubeData
            {
                get => connectedCubeData;
                set => connectedCubeData = value;
            }
        }
    }
}