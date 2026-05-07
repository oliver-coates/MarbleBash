using MarbleBash;
using UnityEngine;

public class XpGlob : DroppedEntity
{
    private int _xp;

    public void Initialise(Marble marble, int xp)
    {
        InitialiseInternal();

        transform.position = marble.transform.position + (UnityEngine.Random.insideUnitSphere * marble.transform.localScale.x * 0.5f);
        _hoverHeight = 0.5f;

        _xp = xp;
        SetSize(Mathf.Sqrt(xp * 0.001f)+0.05f);

        float throwForceVariance = 0.66f;
        float throwForce = 10f * (1 + UnityEngine.Random.Range(-throwForceVariance, throwForceVariance));
        float throwVerticality = UnityEngine.Random.Range(0.35f, 0.85f);
        
        Throw(throwForce, throwVerticality);
    }

    protected override void OnHitGround()
    {
        
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
                ReachedPlayer();
            }
        }
    }

    private void ReachedPlayer()
    {
        Player.instance.stats.AddXp(_xp);
        Destroy(gameObject);
    }
}
