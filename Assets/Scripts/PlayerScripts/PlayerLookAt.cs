using UnityEngine;

namespace PlayerScripts
{
    public class PlayerLookAt : MonoBehaviour
    {
        [SerializeField] private float rotSpeed;
        [SerializeField] private int lookForwardBy;
        private float _timer;
        private Transform _target;
        public int LookForwardBy => lookForwardBy;

        private void Start()
        {
            _timer = 1;
        }

        public void SetNewTarget(Transform lookTarget)
        {
            _timer = 0f;
            _target = lookTarget;
        }

        private void LateUpdate()
        {
            if (_timer >= 1)
            {
                return;
            }
        
            _timer += Time.deltaTime * rotSpeed;
            var look = Quaternion.LookRotation(_target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, _timer);
        }
    }
}