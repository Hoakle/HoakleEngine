using UnityEngine;

namespace HoakleEngine.Core.Graphics
{
    public abstract class GraphicalObjectRepresentation<TData> : MonoBehaviour
    {
        protected GraphicsEngine _GraphicsEngine;
        public TData Data { get; set; }

        public Transform Parent;
        
        public void LinkEngine(GraphicsEngine graphicsEngine)
        {
            _GraphicsEngine = graphicsEngine;
        }
    }
}
