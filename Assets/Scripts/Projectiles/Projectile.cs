using System;
using Character.DamageSystem;
using UnityEngine;

namespace Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] protected ProjectileData data;
        [SerializeField] protected LayerMask collisionMask;
        [SerializeField] protected float lifeSpan = 1f;
        
        protected Vector3 velocity;
        protected float speed;

        private void Awake()
        {
            speed = data.StartingSpeed;
            velocity = Vector3.forward*speed;
            Destroy(gameObject,lifeSpan);
        }


        private void FixedUpdate()
        {
            Move(Time.fixedDeltaTime);
        }

        private void Update()
        {
            if (CheckCollisionsInfront(speed*Time.deltaTime*5f, out var hit))
            {
                var damagable = hit.collider.gameObject.GetComponent<IDamageable>();
                if (damagable != null)
                {
                    damagable.TakeHit(data.Damage,hit.point,transform.forward);
                }
                Destroy(gameObject);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(new Ray(transform.position,transform.forward));
        }

        protected virtual void Move(float deltaTime)
        {
            transform.Translate(velocity*deltaTime);
            velocity += Vector3.down * (data.GravityAcceleration * deltaTime);
            speed = velocity.magnitude;
        }

        protected void LookInDirectionOfMovement() =>
            transform.rotation = Quaternion.FromToRotation(transform.forward, velocity);
        

        protected bool CheckCollisionsInfront(float dist, out RaycastHit hit)
        {
            Ray ray = new Ray(transform.position,transform.forward);
            return Physics.Raycast(ray, out hit, dist);
        }
    }
}