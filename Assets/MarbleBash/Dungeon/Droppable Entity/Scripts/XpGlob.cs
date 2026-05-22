using MarbleBash;
using UnityEngine;

public class XpGlob : DroppedEntity
{
    private int _xp;

    

    public void Initialise(Marble marble, int xp)
    {
        float radius = marble.transform.localScale.x / 2f;
        
        this.Initialise(xp, transform.position, radius);
    }

    public void Initialise(int xp, Vector3 position, float spawnRadius)
    {
        GetComponents();

        _hoverHeight = 0.5f;

        _xp = xp;

        float size = Mathf.Sqrt(xp * 0.001f)+0.05f; 
        SetSize(size);
        PositionWithinMarble(position, spawnRadius, size);

        Vector3 direction = GetRandomThrowDirection();
        float force = GetRandomThrowForce();

        Throw(force, direction);
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
