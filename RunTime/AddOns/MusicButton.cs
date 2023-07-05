using DGames.ObjectEssentials.Scriptable;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DGames.Audio.AddOns
{
    [RequireComponent(typeof(Button))]

    public class MusicButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Sprite[] _musicEnableAndDisableSprites;
        [SerializeField] private ValueField<bool> _musicValue = new("MUSIC");
        
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            
            ValueChanged(_musicValue.Item.Get());
            _musicValue.Item.Binder.Bind(ValueChanged,this);
        }

        private void OnDestroy()
        {
            _musicValue.Item.Binder.UnBind(this);
        }

        private void ValueChanged(bool enable)
        {
            _button.image.sprite = _musicEnableAndDisableSprites[enable ? 0 : 1];
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            _musicValue.Item.Set(!_musicValue.Item.Get());
        }
    }
}