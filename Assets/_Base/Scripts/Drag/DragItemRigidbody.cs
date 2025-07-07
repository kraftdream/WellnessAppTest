using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace WellnessApp
{
    [RequireComponent(typeof(Rigidbody))]
    public class DragItemRigidbody : DragItemBase
    {
        [Header("Options")]
        [SerializeField]
        private bool _ignoreAnyUI;

        [SerializeField]
        private Vector3 _draggableOffset;

        [SerializeField]
        private float _forceAmount = 500f;

        [SerializeField]
        [Range(10f, 30f)]
        private float _maxHeightLimitation;

        [SerializeField]
        private InputActionProperty _clickProperty;

        [SerializeField]
        private InputActionProperty _pointerProperty;

        private Camera _camera;
        private Rigidbody _targetRigidbody;
        private float _selectionDistance;
        private Vector3 _offset;

        private void Awake()
        {
            _camera = Camera.main;
            _targetRigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            // Drag item start logic
            if (_clickProperty != null && _pointerProperty != null && _clickProperty.action.triggered && !IsDragging)
            {
                if (!_ignoreAnyUI && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                    return;

                Vector2 pointerPos = _pointerProperty.action.ReadValue<Vector2>();
                Ray ray = _camera.ScreenPointToRay(pointerPos);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Rigidbody selectedRigidbody = hit.collider.GetComponent<Rigidbody>();

                    if (selectedRigidbody != null && _targetRigidbody.Equals(selectedRigidbody))
                    {
                        _selectionDistance = Vector3.Distance(_camera.transform.position, hit.point);
                        _offset = _targetRigidbody.transform.position - hit.point;
                        StartDrag();
                    }
                }
            }

            // Drag item stop logic
            if (!_clickProperty.action.IsPressed() && IsDragging)
            {
                StopDrag();
            }
        }

        private void FixedUpdate()
        {
            // Drag item logic
            if (_pointerProperty != null && IsDragging)
            {
                Vector2 pointerPos = _pointerProperty.action.ReadValue<Vector2>();
                Vector3 worldPos = _camera.ScreenToWorldPoint(new Vector3(pointerPos.x, pointerPos.y, _selectionDistance));
                Vector3 targetPos = worldPos + _offset + _draggableOffset;

                Vector3 force = (targetPos - _targetRigidbody.position) * _forceAmount * Time.deltaTime;
                _targetRigidbody.linearVelocity = force;

                Dragging();
            }

            // Clamp Y position
            Vector3 pos = _targetRigidbody.position;
            pos.y = Mathf.Clamp(pos.y, 0f, _maxHeightLimitation);
            _targetRigidbody.MovePosition(pos);
        }
    }
}