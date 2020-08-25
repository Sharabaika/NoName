using System;
using Character.DamageSystem;
using UnityEngine;

namespace Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] protected LayerMask collisionMask;
        [SerializeField] protected float lifeSpan = 5f;

        private ProjectileData _projectileData;
        public ProjectileData ProjectileData
        {
            get=>_projectileData;
            set
            {
                _projectileData = value;
                speed = value.startingSpeed;
                velocity = transform.forward * speed;
                isActive = true;
            }
        }

        protected bool isActive = false;
        protected Vector3 velocity = Vector3.zero;
        protected float speed = 0f;
        protected float distTraveled = 0f;
        
        private void Awake()
        {
            Destroy(gameObject,lifeSpan);
        }


        private void Update()
        {
            if(isActive == false) return;

            if (CheckCollisionsInfront(speed*Time.deltaTime, out var hit))
            {
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeHit(ProjectileData.CalculateDamage(distTraveled),hit.point,transform.forward);
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
            velocity += Vector3.down * (ProjectileData.gravityAcceleration * deltaTime);
            speed = velocity.magnitude;
        }

        protected void LookInDirectionOfMovement() =>
            transform.rotation = Quaternion.FromToRotation(transform.forward, velocity);
        

        protected bool CheckCollisionsInfront(float dist, out RaycastHit hit)
        {
            var ray = new Ray(transform.position,velocity);
            return Physics.Raycast(ray, out hit, dist);
        }
    }
}