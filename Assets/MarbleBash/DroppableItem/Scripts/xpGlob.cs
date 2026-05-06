using MarbleBash;
using UnityEngine;

public class XpGlob : DroppedEntity
{
    protected override void OnHitGround()
    {
        
    }

    protected override void Setup()
    {
        _hoverHeight = 0.5f;

        SetSize(UnityEngine.Random.Range(0.1f, 0.25f));

        float throwForceVariance = 0.66f;

        float throwForce = 10f * (1 + UnityEngine.Random.Range(-throwForceVariance, throwForceVariance));
        float throwVerticality = UnityEngine.Random.Range(0.35f, 0.85f);
        
        Throw(throwForce, throwVerticality);
    }

    protected override void Update()
    {
        base.Update();

        if (_isOnGround)
        {
            Vector3 targetPos = Player.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Mathf.Pow(_timeAlive, 2) * 5f * Time.deltaTime);
            
            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

}
