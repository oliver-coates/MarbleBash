#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

public class FpsBenchmark : MonoBehaviour
{
    [Header("Settings:")]
    [SerializeField, Min(0.1f)] private float _fpsRecordingTime;
    
    private List<float> _averageFpsList;
    private float _averageFps;

    private float _framesRecorded;
    private float _framesRecordingTimer;
    private float _currentFps;

    private void Start()
    {
        _averageFpsList = new List<float>();
    }

    private void Update()
    {
        _framesRecorded += 1;
        _framesRecordingTimer += Time.deltaTime;

        if (_framesRecordingTimer > _fpsRecordingTime)
        {
            _currentFps = (_framesRecorded / _framesRecordingTimer);
            
            _averageFpsList.Add(_currentFps);
            RecalculateAverageFPS();

            _framesRecordingTimer = 0f;
            _framesRecorded = 0;
        }
    }

    private void RecalculateAverageFPS()
    {
        float total = 0f;
        foreach(float a in _averageFpsList)
        {
            total += a;
        }

        _averageFps = total / _averageFpsList.Count;
    }

    public void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 350, 60),
                $"FPS: {_currentFps:0.0} across {_fpsRecordingTime}s\n" +
                $"Average: {_averageFps}");
    }
}
#endif