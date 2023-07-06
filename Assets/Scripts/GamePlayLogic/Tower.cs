using UnityEngine;

public class Tower : PooledObject
{
    public Tower_SO towerData;
    public Projectile projectilePrefab;
    private Enemy currentTarget;
    float elapsedTimeSinceLastAttack;
    public int Cost { get => towerData.cost; }

    private float speedCalc;
    private float projectileLife = 0;
    private void Start()
    {
        speedCalc = 100 / towerData.speed;
        switch (towerData.Type)
        {
            case Tower_SO.TowerType.Arrow:
                projectileLife = 1;
                break;
            case Tower_SO.TowerType.Bomb:
                projectileLife = .5f;

                break;
            case Tower_SO.TowerType.Magic:
                projectileLife = .35f;
                break;
            default:
                break;
        }
    }
    public void Update()
    {
        if (currentTarget != null)
        {
            if (TargetInBounds() && currentTarget.IsAlive())
            {
                elapsedTimeSinceLastAttack += Time.deltaTime;
                if (elapsedTimeSinceLastAttack >= speedCalc)
                {

                    currentTarget.ReceiveDamage(towerData.damage);
                    elapsedTimeSinceLastAttack = 0;
                    projectilePrefab.GetPooledInstance<Projectile>().Shoot(transform.position, currentTarget.transform.position, speedCalc*5,projectileLife);
                    //Debug.Log("Deal Damage " + towerData.damage);
                }
            }
            else
            {
                elapsedTimeSinceLastAttack = 0;
                currentTarget = null;
            }
        }
    }
    private void FixedUpdate()
    {
        if (currentTarget == null)
        {
            GetClosestTarget();
        }
    }
    // Find the closest enemy within the tower attack radius
    private void GetClosestTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, towerData.attackRadius, towerData.targetLayer);
        float nearestDistanceSqr = Mathf.Infinity;
        if (colliders.Length != 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag(towerData.targetTag))
                {
                    float distanceSqr = (collider.transform.position - transform.position).sqrMagnitude;
                    if (distanceSqr < nearestDistanceSqr)
                    {
                        nearestDistanceSqr = distanceSqr;
                        currentTarget = collider.GetComponent<Enemy>();
                    }
                }
            }
        }
    }
    // Check if the current target is within the tower's attack range
    private bool TargetInBounds()
    {
        if (currentTarget != null)
        {
            return (Vector3.Distance(transform.position, currentTarget.transform.position) <= towerData.attackRadius);
        }
        return false;
    }
    public override void ReturnToPool()
    {
        base.ReturnToPool();
        currentTarget = null;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (currentTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, currentTarget.transform.position);
        }
    }
#endif
}
