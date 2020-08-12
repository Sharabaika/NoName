using TMPro;
using UnityEngine;

namespace Character.DamageSystem
{
    [RequireComponent(typeof(TextMeshPro))]
    public class DamagePopUp : MonoBehaviour
    {
        private static Transform CameraT => Camera.main.transform;
        
        
        // TODO add ScriptableObject with styles
        private Vector3 _velocity;
        private Vector3 _acceleration;

        private float _startingSpeed = 0.3f;
        private float _lifeSpan = 1.5f;
        
        public static void Instantiate(DamagePopUp prefab, Vector3 pos, int damage)
        {
            
            var dmgPopUp = Instantiate(prefab, pos, Quaternion.identity);
            dmgPopUp.transform.LookAt(CameraT);
            dmgPopUp.Display(damage);
        }

        private TextMeshPro _textMesh;
        private void Awake()
        {
            Destroy(gameObject,_lifeSpan);
            _textMesh = GetComponent<TextMeshPro>();
            
            _velocity = new Vector3(Random.Range(-1f,1f),0f,Random.Range(-1f,1f)).normalized * _startingSpeed;
            _acceleration =Vector3.down*0.3f;
        }

        private void Update()
        {
            transform.LookAt(CameraT);
            transform.Translate(_velocity*Time.deltaTime);
            _velocity += _acceleration * Time.deltaTime;
        }

        public void Display(int damage)
        {
            _textMesh.text = damage.ToString();
        }
    }
}