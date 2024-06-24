using System;
using UnityEngine;

namespace HoakleEngine.Core.UI.Utils
{
    public class SafeAreaResize : MonoBehaviour
    {
        [SerializeField] private RectTransform _TopSafeArea = null;
        [SerializeField] private RectTransform _Body = null;
        [SerializeField] private RectTransform _BotSafeArea = null;

        private Rect _SafeArea = new Rect(0, 0, 0, 0);

        private void Start()
        {
            Refresh();
        }

        private void Refresh()
        {
            if (_SafeArea != Screen.safeArea)
            {
                _SafeArea =Screen.safeArea;
                SetLayoutVertical();
            }
        }

        public void SetLayoutVertical()
        {
            _Body.sizeDelta = new Vector2(_Body.sizeDelta.x, _SafeArea.height);
            _TopSafeArea.sizeDelta = new Vector2(_TopSafeArea.sizeDelta.x, Screen.height - _SafeArea.size.y - _SafeArea.position.y);
            _BotSafeArea.sizeDelta = new Vector2(_BotSafeArea.sizeDelta.x, _SafeArea.position.y);
        }
    }
}
