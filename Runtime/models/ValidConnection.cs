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
            private CubeDirection edge;
            [SerializeField] 
            private bool valid;

            [SerializeField] 
            private ConnectedCube connectedTo;

            public CubeDirection Edge
            {
                get => edge;
                set => edge = value;
            }

            public bool Valid
            {
                get => valid;
                set => valid = value;
            }

            public ConnectedCube ConnectedTo
            {
                get => connectedTo;
                set => connectedTo = value;
            }
        }
    }
}