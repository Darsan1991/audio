using DGames.ObjectEssentials;
using DGames.ObjectEssentials.Scriptable;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DGames.Audio.AddOns
{
    [RequireComponent(typeof(Button))]
    public class SoundButton:MonoBehaviour,IPointerClickHandler
    {
        [SerializeField] private Sprite[] _soundEnableAndDisableSprites;
        [SerializeField] private ValueField<bool> _soundEffectEnable = new("SOUND_EFFECT");
    
        private Button _button;

        public IValue<bool> SoundEnable => _soundEffectEnable.Item;

        private void Awake()
        {
            _button = GetComponent<Button>();
            ValueChanged(SoundEnable.Get());
            SoundEnable.Binder.Bind(ValueChanged,this);
        }


        private void OnDestroy()
        {
            SoundEnable.Binder.UnBind(this);
        }


        private void ValueChanged(bool b)
        {
            _button.image.sprite = _soundEnableAndDisableSprites[b ? 0 : 1];
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            SoundEnable.Set(!SoundEnable.Get());
        }
    }
}