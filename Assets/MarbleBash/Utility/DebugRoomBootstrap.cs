using KahuInteractive.HassleFreeAudio;
using KahuInteractive.HassleFreeConfig;
using MarbleBash;
using UnityEngine;

public class DebugRoomBootstrap : MonoBehaviour
{
    private void Awake()
    {
        Configuration.Initialise();
        AudioEngine.Initialise();
    }

    private void Start()
    {
        GameController.Initialise();
    }
}
