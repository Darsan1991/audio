using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DGames.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public partial class BackgroundMusic : MonoBehaviour
    {
        private static readonly Dictionary<string, BackgroundMusic> _idVsMusics = new();

        [SerializeField] private bool _keepOnSceneLoad;
        [SerializeField] private List<int> _allowedScenes = new();
        [HideInInspector] [SerializeField] private string _id;

        private AudioSource _audio;

        public bool IsMusicEnable => AudioSettings.IsEnable((int)AudioType.Music);

        private void Awake()
        {
            _audio = GetComponent<AudioSource>();
            _audio.loop = true;
            UpdatePlayOrStop();
            UpdateInstance();
        }

        private void UpdatePlayOrStop()
        {
            if (_audio.isPlaying && !IsMusicEnable)
            {
                _audio.Stop();
            }
            else if (!_audio.isPlaying && IsMusicEnable)
            {
                _audio.Play();
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
            AudioSettings.EnableStateChanged += AudioManagerOnMusicStateChanged;
        }


        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
            AudioSettings.EnableStateChanged -= AudioManagerOnMusicStateChanged;
        }

        private void AudioManagerOnMusicStateChanged(int type, bool enable)
        {
            if (type != (int)AudioType.Music)
                return;

            UpdatePlayOrStop();
        }

        private void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            UpdateInstance();
        }

        private void UpdateInstance()
        {
            if (!_keepOnSceneLoad)
            {
                return;
            }
            
            if (RemoveIfNotAllowedInScene()) return;

            if (!_idVsMusics.ContainsKey(_id))
            {
                _idVsMusics.Add(_id, this);
                DontDestroyOnLoad(gameObject);
            }
            else if (_idVsMusics.ContainsKey(_id) && _idVsMusics[_id] != this)
            {
                Destroy(gameObject);
            }
        }

        private bool RemoveIfNotAllowedInScene()
        {
            if (!HasAllowedScene())
            {
                if (_idVsMusics.ContainsKey(_id))
                {
                    _idVsMusics.Remove(_id);
                }

                Destroy(gameObject);
                return true;
            }

            return false;
        }


        private bool HasAllowedScene()
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                if (_allowedScenes.Contains(SceneManager.GetSceneAt(i).buildIndex))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!string.IsNullOrEmpty(_id))
                return;
            _id = Guid.NewGuid().ToString();
        }
#endif
    }

    public partial class BackgroundMusic
    {
        public const string KEEP_SCENE_LOAD_PROPERTY = nameof(_keepOnSceneLoad);
        public const string ALLOWED_SCENES_PROPERTY = nameof(_allowedScenes);
    }
}