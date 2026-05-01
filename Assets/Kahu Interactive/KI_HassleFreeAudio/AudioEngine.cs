using UnityEngine;


namespace KahuInteractive.HassleFreeAudio
{

    public static class AudioEngine
    {
        private static AudioPlayer _AudioPlayer;
        private static bool _Initialised;

        public static void Initialise()
        {
            if (_Initialised)
            {
                Debug.LogError($"Do not initiliase the audio engine twice.");
                return;
            }

            _Initialised = true;

            _AudioPlayer = new GameObject("Hassle Free Audio Engine").AddComponent<AudioPlayer>();

            Object.DontDestroyOnLoad(_AudioPlayer.gameObject);
        }


        #region Playing Sound
        public static byte PlaySound(ClipSet clipSet)
        {
            return PlaySound(clipSet, 1f);
        }

        public static byte PlaySound(ClipSet clipSet, float volume)
        {
            if (clipSet == null)
            {
                Debug.LogError("Recieved null clip set");
                return 0;
            }

            return PlaySound(new AudioPlayData(clipSet, volume));
        }

        public static byte PlaySound(AudioPlayData data)
        {
            // Ensure we are initialised
            if (!_Initialised)
            {
                Initialise();
            }

            // Play sound:
            return _AudioPlayer.PlaySound(data);
        }
        #endregion


        #region Halting Sound


        public static void StopAudio(byte eventID)
        {
            _AudioPlayer.StopSound(eventID);
        }


        #endregion

    }

    public struct AudioPlayData
    {
        public ClipSet clipSet;
        public float volume;

        private Vector3 _worldLocation;
        private bool _hasWorldLocation;
        public bool doPlayInWorld
        {
            get
            {
                return _hasWorldLocation;
            }
        }

        public Vector3 worldLocation
        {
            set
            {
                _worldLocation = value;
                _hasWorldLocation = true;
            }
            get
            {
                return _worldLocation;
            }
        }


        public Transform attachedTransform;


        public AudioPlayData(ClipSet clipSet, float volume)
        {
            this.clipSet = clipSet;
            this.volume = volume;

            _hasWorldLocation = false;
            _worldLocation = Vector3.zero;

            attachedTransform = null;
        }
    }

}

