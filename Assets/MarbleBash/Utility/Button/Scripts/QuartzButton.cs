using System;
using MarbleBash;
using UnityEngine;

public class QuartzButton : PhysicalButton
{
    private float _timer;

    [SerializeField] private GameObject _quartzButton;
    protected override void Activate()
    {
        _timer += Time.deltaTime;
    }


    protected override void Update()
    {
        base.Update();

        if (_timer > 0.5f)
        {
            SpawnQuartz();
            _timer = 0f;
        }
    }

    private void SpawnQuartz()
    {
        QuartzCrystal xp = Instantiate(_quartzButton).GetComponent<QuartzCrystal>();
        
        int amount = UnityEngine.Random.Range(50, 100);
        
        xp.Initialise(amount, transform.position + Vector3.up, 0.5f);
    }
}
