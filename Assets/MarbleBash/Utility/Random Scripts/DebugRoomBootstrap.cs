using KahuInteractive.HassleFreeAudio;
using KahuInteractive.HassleFreeConfig;
using KahuInteractive.VisualFX;
using MarbleBash;
using UnityEngine;

public class DebugRoomBootstrap : MonoBehaviour
{
    private void Awake()
    {
        Configuration.Initialise();
        AudioEngine.Initialise();
        VFX.Initialise();
    }

    private void Start()
    {
        GameController.Initialise();
    }
}
