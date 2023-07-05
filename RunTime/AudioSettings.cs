using System;
using System.Collections.Generic;
using DGames.ObjectEssentials;
using DGames.ObjectEssentials.Scriptable;
using UnityEngine;

namespace DGames.Audio
{
    public partial class AudioSettings : ScriptableObject
    {
        public static Action<int, bool> EnableStateChanged;

        [SerializeField] private List<AudioTypeAndValue> _audioTypeAndValues = new();
        
        
        private readonly Dictionary<int, IValue<bool>> _typeVsValues = new();
        
        [field:NonSerialized]public bool Initialized { get; private set; }

        

        protected void InitIfNeeded()
        {
            if (Initialized)
            {
                return;
            }
            _typeVsValues.Clear();
            foreach (var pair in _audioTypeAndValues)
            {
                _typeVsValues.Add(pair.type, pair.value.Item);
            }
            
            _audioTypeAndValues.ForEach(p=>
            {
                p.value.Item.Binder.UnBind(this);
                p.value.Item.Binder.Bind((enable) => OnAudioStateChanged(p.type, enable), this);
            });
            Initialized = true;

        }
        
        private void OnAudioStateChanged(int type, bool enable)
        {
            EnableStateChanged?.Invoke(type,enable);
        }
        
        // ReSharper disable once FlagArgument
        public static bool IsEnable(int type)
        {
            if (!Default._typeVsValues.ContainsKey(type))
            {
                Debug.LogWarning($"Audio Type:{type} Not Found");
                return false;
            }

            return Default._typeVsValues[type].Get();
        }
        
        // ReSharper disable once FlagArgument
        public static void SetEnable(int type,bool enable)
        {
            if (!Default._typeVsValues.ContainsKey(type))
            {
                Debug.LogWarning($"Audio Type:{type} Not Found");
                return;
            }

            Default._typeVsValues[type].Set(enable);
        }
        
        public static void ToggleEnable(int type)
        {
            if (!Default._typeVsValues.ContainsKey(type))
            {
                Debug.LogWarning($"Audio Type:{type} Not Found");
                return;
            }

            Default._typeVsValues[type].Set(!IsEnable(type));
        }

        
        

        [Serializable]
        public struct AudioTypeAndValue
        {
            public int type;
            public ValueField<bool> value;
        }

    }

    public partial class AudioSettings
    {
        public static AudioSettings Default => Resources.Load<AudioSettings>(nameof(AudioSettings));

        
            
#if UNITY_EDITOR
        [UnityEditor.MenuItem("MyGames/Others/AudioSettings")]
        public static void Open()
        {
            ScriptableEditorUtils.OpenOrCreateDefault<AudioSettings>();
        }
#endif
    }


    public enum AudioType
    {
        SoundEffect,
        Music
    }
}