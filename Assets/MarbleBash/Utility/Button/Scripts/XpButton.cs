using System;
using UnityEngine;

public class XpButton : PhysicalButton
{
    private float _timer;

    [SerializeField] private GameObject _xpPrefab;
    protected override void Activate()
    {
        _timer += Time.deltaTime;
    }


    protected override void Update()
    {
        base.Update();

        if (_timer > 0.1f)
        {
            SpawnXp();
            _timer = 0f;
        }
    }

    private void SpawnXp()
    {
        XpGlob xp = Instantiate(_xpPrefab).GetComponent<XpGlob>();
        
        int xpAmount = UnityEngine.Random.Range(10, 26);
        
        xp.Initialise(xpAmount, transform.position + Vector3.up, 0.5f);
    }
}
