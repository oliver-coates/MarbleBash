using KahuInteractive.HassleFreeAudio;
using KahuInteractive.HassleFreeConfig;
using UnityEngine;

public class DebugRoomBootstrap : MonoBehaviour
{
    private void Awake()
    {
        Configuration.Initialise();
        AudioEngine.Initialise();
    }
}
