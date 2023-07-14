using HoakleEngine.Core.Communication;
using UnityEngine;

namespace HoakleEngine.Core.Graphics
{
    public interface IUserInterface
    {
        public GameObject GetFirstSelected { get; }
    }

    public abstract class GraphicalUserInterface : MonoBehaviour, IUserInterface
    {
        public GameObject GetFirstSelected { get; }

        protected GUIEngine _GuiEngine;
        protected EventBus _EventBus;
        
        public void LinkEngine(GUIEngine guiEngine, EventBus bus)
        {
            _GuiEngine = guiEngine;
            _EventBus = bus;
        }

        protected void Destroy()
        {
            Destroy(gameObject);
        }
    }

    public abstract class ObjectRepresentation : GraphicalUserInterface
    {
        protected Object _Data;

        public void SetData(Object data)
        {
            _Data = data;
        }

        public Object GetData()
        {
            return _Data;
        }
    }
}