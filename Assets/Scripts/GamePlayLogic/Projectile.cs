using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

//Only Visual effect of the tower projectiles
public class Projectile : PooledObject
{
    [SerializeField]
    Rigidbody _rigidbody;
    Vector3 target;
    float elapsedTime = 0, lifeTime = .5f;
    public void Shoot(Vector3 origin, Vector3 target, float projectileSpeed, float life = 1)
    {
        lifeTime = life;
        this.target = target;
        transform.position = origin;
        transform.rotation.SetLookRotation(target);
        Vector3 direction = (target - transform.position).normalized;
        _rigidbody.velocity = direction * projectileSpeed;

    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= lifeTime)
        {
            ReturnToPool();
        }

    }
    public override void ReturnToPool()
    {
        elapsedTime = 0;
        _rigidbody.velocity = Vector3.zero;
        base.ReturnToPool();
    }
}
