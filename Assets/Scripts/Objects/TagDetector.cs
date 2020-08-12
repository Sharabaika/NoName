using System;
using UnityEngine;

namespace Objects
{
    [RequireComponent(typeof(Collider))]
    public class TagDetector : MonoBehaviour
    {
        [SerializeField] private string tagToDetect;

        public delegate void DetectorHandler(GameObject obj);
        
        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }
        
        public event DetectorHandler OnEnter;
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(tagToDetect))
            {
                OnEnter?.Invoke(other.gameObject);
            }
        }
        
        public event DetectorHandler OnLeave;
        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag(tagToDetect))
            {
                OnLeave?.Invoke(other.gameObject);
            }
        }
    }
}