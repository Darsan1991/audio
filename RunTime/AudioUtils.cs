using UnityEngine;

namespace DGames.Audio
{
    public class AudioUtils
    {
        // ReSharper disable once FlagArgument
        public static AudioSource PlayClipAtPoint(AudioClip clip, Vector3 position,bool autoDestroy=true)
        {
            var clipGo = new GameObject("Clip")
            {
                transform =
                {
                    position = position
                }
            };
            var audioSource = clipGo.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();
            if (clipGo != null && autoDestroy)
            {
                Object.Destroy(clipGo,clip.length);
            }
        
            return audioSource;
        }
    }
}