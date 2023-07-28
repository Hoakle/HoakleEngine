using UnityEngine;

namespace HoakleEngine.Core.Graphics
{
    public interface IUserInterface
    {
        public GameObject GetFirstSelected { get; }
    }

    public abstract class GraphicalUserInterface : MonoBehaviour, IUserInterface
    {
        [SerializeField] private Canvas _Canvas = null;
        public Canvas Canvas => _Canvas;

        public GameObject GetFirstSelected { get; }

        protected GUIEngine _GuiEngine;
        
        public void LinkEngine(GUIEngine guiEngine)
        {
            _GuiEngine = guiEngine;
        }

        protected void Destroy()
        {
            Destroy(gameObject);
        }
    }

    public abstract class DataGUI<DataHandler> : GraphicalUserInterface, GraphicalObjectRepresentation<DataHandler>
    {
        public DataHandler Data { get; set; }
    }
}