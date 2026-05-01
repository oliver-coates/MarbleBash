using System;
using UnityEngine;
using UnityEngine.Audio;

namespace KahuInteractive.HassleFreeAudio
{

    public class KahuAudioSource : MonoBehaviour
    {
        // public static event Action<KahuAudioSource> OnAudioSourceCreated;
        public static event Action<KahuAudioSource> OnAudioSourceDestroyed;

        private AudioSource _source;


        public bool isPlaying
        {
            get
            {
                return _source.isPlaying;
            }
        }



        #region Initialisation & Destruction

        private void Awake()
        {
            // OnAudioSourceCreated?.Invoke(this);

            _source = gameObject.AddComponent<AudioSource>();
        }

        private void OnDisable()
        {
            OnAudioSourceDestroyed?.Invoke(this);
            Destroy(_source);
            Destroy(this);
        }

        private void OnDestroy()
        {
            OnAudioSourceDestroyed?.Invoke(this);
        }

        #endregion

        public void Play(AudioPlayData data)
        {
            AudioClip audioClip = data.clipSet.GetRandomClip(out AudioMixerGroup mixer, out float pitch, out float volume);

            _source.dopplerLevel = 0;
            _source.volume = volume * data.volume;
            _source.pitch = pitch;

            // Setup mixer group
            if (mixer != null)
            {
                _source.outputAudioMixerGroup = mixer;
            }
            else
            {
                // No mixer group set
                _source.outputAudioMixerGroup = null;
            }

            // Setup spatialisation
            if (data.doPlayInWorld)
            {
                _source.transform.position = data.worldLocation;

                _source.spatialBlend = 1;
                _source.spatialize = true;

                _source.rolloffMode = AudioRolloffMode.Custom;
                _source.maxDistance = data.clipSet.maxDistance;
                _source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, data.clipSet.spatialisationCurve);
            }
            else
            {
                _source.transform.position = Vector3.zero;
                _source.spatialize = false;
                _source.spatialBlend = 0;
            }

            // Setup parenting
            if (data.attachedTransform != null)
            {
                _source.transform.SetParent(data.attachedTransform);
                _source.transform.localPosition = Vector3.zero;
            }
            else
            {
                _source.transform.SetParent(transform);
                _source.transform.localPosition = Vector3.zero;
            }

            _source.PlayOneShot(audioClip);
        }

        public void Stop()
        {
            _source.Stop();
        }
    }
}