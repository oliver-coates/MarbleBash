using System.Collections.Generic;
using UnityEngine;

namespace KahuInteractive.HassleFreeAudio
{

    public class AudioPlayer : MonoBehaviour
    {
        private List<KahuAudioSource> _audioSources;

        private Dictionary<byte, KahuAudioSource> _sourceEventDictionary;
        private byte _eventIterator;

        #region Initialisation
        private void Awake()
        {
            KahuAudioSource.OnAudioSourceDestroyed += DeregisterAudioSource;

            _audioSources = new List<KahuAudioSource>();
            _sourceEventDictionary = new Dictionary<byte, KahuAudioSource>();

            for (byte eventIndex = byte.MinValue; eventIndex < byte.MaxValue; eventIndex++)
            {
                _sourceEventDictionary.Add(eventIndex, null);
            }

            _eventIterator = 0;
        }

        private void OnDestroy()
        {
            KahuAudioSource.OnAudioSourceDestroyed -= DeregisterAudioSource;
        }

        #endregion

        public byte PlaySound(AudioPlayData data)
        {
            _eventIterator += 1; // Byte (so maxed at 255), but auto-wraps around.

            KahuAudioSource source = GetNextAvailableAudioSource();

            _sourceEventDictionary[_eventIterator] = source;

            source.Play(data);

            return _eventIterator;
        }


        private KahuAudioSource GetNextAvailableAudioSource()
        {
            foreach (KahuAudioSource source in _audioSources)
            {
                if (source.isPlaying)
                {
                    continue;
                }
                else
                {
                    return source;
                }
            }

            // We haven't found a proper source
            // Create a new one
            GameObject newPlayer = new GameObject("Audio Source");
            newPlayer.transform.parent = transform;
            KahuAudioSource newSource = newPlayer.AddComponent<KahuAudioSource>();
            _audioSources.Add(newSource);

            return newSource;
        }


        private void DeregisterAudioSource(KahuAudioSource kahuAudioSource)
        {
            _audioSources.Remove(kahuAudioSource);
        }


        public void StopSound(byte eventID)
        {
            KahuAudioSource source = _sourceEventDictionary[eventID];

            if (source)
            {
                if (source.isPlaying)
                {
                    source.Stop();
                }
            }
        }
        
    }

}
