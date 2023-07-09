using UnityEngine;

namespace HoakleEngine.Graphics
{
    public interface IUserInterface
    {
        public GameObject GetFirstSelected { get; }
    }

    public abstract class GraphicalUserInterface : MonoBehaviour, IUserInterface
    {
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

    public abstract class ObjectRepresentation<TData> : GraphicalUserInterface
    {
        protected TData _Data;

        public void SetData(TData data)
        {
            _Data = data;
        }

        public TData GetData()
        {
            return _Data;
        }
    }
}