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

            // override Equals
            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }

                ConnectedCube cc = (ConnectedCube)obj;
                return (id == cc.id) && (edge == cc.edge);
            }
        }
    }
}