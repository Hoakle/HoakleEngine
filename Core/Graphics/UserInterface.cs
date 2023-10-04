using HoakleEngine.Core.Audio;
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
        [SerializeField] protected Animator _Animator = null;
        public Canvas Canvas => _Canvas;

        public GameObject GetFirstSelected { get; }

        protected GUIEngine _GuiEngine;
        
        public void LinkEngine(GUIEngine guiEngine)
        {
            _GuiEngine = guiEngine;
        }

        protected virtual void Close()
        {
            if(_Animator != null)
                _Animator.SetTrigger("Dispose");
            else
            {
                Dispose();
            }
        }
        private void Dispose()
        {
            _GuiEngine.Dispose(this);
            Destroy(gameObject);
        }

        public virtual void OnReady()
        {
            PlayDisplayAnimation();
        }

        private void PlayDisplayAnimation()
        {
            if (_Animator != null)
            {
                _Animator.SetTrigger("Display");
                AudioPlayer.Instance.Play(AudioKeys.Whoosh);
            }
                
        }
    }

    public abstract class DataGUI<TData> : GraphicalUserInterface
    {
        public TData Data { get; set; }
    }

    public abstract class GuiComponent : MonoBehaviour
    {
        protected GUIEngine _GuiEngine;
        public void LinkEngine(GUIEngine guiEngine)
        {
            _GuiEngine = guiEngine;
        }

        protected void Destroy()
        {
            Destroy(gameObject);
        }
        
        public virtual void OnReady()
        {

        }
    }

    public abstract class DataGuiComponent<TData> : GuiComponent
    {
        public TData Data { get; set; }
    }
}