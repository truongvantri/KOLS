using System;
using DG.Tweening;
using ECM2.Components;
using UnityEngine;
using ECM2.Examples;
namespace KOLS
{
    public class CustomeSimpleCameraController : MonoBehaviour
    {
        #region PUBLIC FIELDS

        [SerializeField]
        private Transform _target;

        [SerializeField] private Transform body;
        
        [SerializeField]
        private float _distanceToTarget = 10.0f;

        [SerializeField]
        private float _smoothTime = 0.1f;

        [SerializeField] private float speedRotate;

        #endregion

        #region FIELDS

        private Vector3 _followVelocity;

        #endregion

        #region PROPERTIES

        public Transform target
        {
            get => _target;
            set => _target = value;
        }

        public float distanceToTarget
        {
            get => _distanceToTarget;
            set => _distanceToTarget = Mathf.Max(0.0f, value);
        }

        #endregion

        #region MONOBEHAVIOUR

        private Camera _myCamera;

        public void OnValidate()
        {
            distanceToTarget = _distanceToTarget;
        }

        private void Awake()
        {
            _myCamera = GetComponent<Camera>();
        }

        public void Start()
        {
            if (target != null)
            {
                Vector3 targetPosition = target.position - transform.forward * distanceToTarget;
                transform.position = targetPosition;
            }
        }
        
        public void LateUpdate()
        {
            Movement();
            UpdateRotation();
        }

        void Movement()
        {
            if(_target == null)
                return;
            
            Vector3 targetPosition = target.position - transform.forward * distanceToTarget;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _followVelocity, _smoothTime);
        }

        public void UpdateRotation()
        {
            if(body == null)
                return;
            
            Vector3 euler = transform.eulerAngles;
            euler.y = body.eulerAngles.y;
            euler.z = 0;
            Quaternion newQuaternion = Quaternion.Euler(euler);
            Quaternion newRot = Quaternion.Slerp(transform.rotation, newQuaternion, speedRotate * Time.deltaTime);
            transform.rotation = newRot;
        }

        #endregion

        public void SetTargetFollow(Transform obj)
        {
            _target = obj;
        }

        public void SetPlayer(Transform player)
        {
            body = player;
        }

        public Camera GetCamera()
        {
            return _myCamera;
        }
    }
}