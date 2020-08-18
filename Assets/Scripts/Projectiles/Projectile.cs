using System;
using Character.DamageSystem;
using UnityEngine;

namespace Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] protected ProjectileData data;
        [SerializeField] protected LayerMask collisionMask;
        [SerializeField] protected float lifeSpan = 5f;

        public ProjectileData ProjectileData
        {
            set => data = value;
            get => data;
        }
        
        protected virtual int Damage => 1;
        
        protected Vector3 velocity;
        protected float speed;
        protected float distTraveled = 0f;
        
        private void Awake()
        {
            speed = data.startingSpeed;
            velocity = transform.forward*speed;
            Destroy(gameObject,lifeSpan);
        }


        private void Update()
        {
            if (CheckCollisionsInfront(speed*Time.deltaTime, out var hit))
            {
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeHit(Damage,hit.point,transform.forward);
                }
                Destroy(gameObject);
                return;
            }

            Move(Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(new Ray(transform.position,velocity));
        }

        protected virtual void Move(float deltaTime)
        {
            distTraveled += speed * deltaTime;
            transform.Translate(velocity*deltaTime, Space.World);
            velocity += Vector3.down * (data.gravityAcceleration * deltaTime);
            speed = velocity.magnitude;
        }

        protected void LookInDirectionOfMovement() =>
            transform.rotation = Quaternion.FromToRotation(transform.forward, velocity);
        

        protected bool CheckCollisionsInfront(float dist, out RaycastHit hit)
        {
            Ray ray = new Ray(transform.position,velocity);
            return Physics.Raycast(ray, out hit, dist);
        }
    }
}