using UnityEngine;

namespace WellnessApp
{
    [RequireComponent(typeof (DragItemBase))]
    public class DragItemEffects : MonoBehaviour
    {
        [Header("Audio Options")]
        [SerializeField]
        private AudioClip _startDrag;

        [SerializeField]
        private AudioClip _stopDrag;

        [Header("Particles")]
        [SerializeField]
        private ParticleSystem _releaseParticles;

        private AudioSource _audioSource;
        private DragItemBase _dragItem;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            if (_audioSource == null)
                _audioSource = gameObject.AddComponent<AudioSource>();

            _audioSource.playOnAwake = false;
            _dragItem = GetComponent<DragItemBase>();
            _dragItem.DraggingEvent += OnDragHandler;
        }

        private void OnDragHandler(DragActionType dragAction)
        {
            switch (dragAction)
            {
                case DragActionType.Start:
                    _audioSource.Stop();
                    _audioSource.clip = _startDrag;
                    _audioSource.Play();
                    break;

                case DragActionType.Stop:
                    _audioSource.Stop();
                    _audioSource.clip = _stopDrag;
                    _audioSource.Play();

                    if (_releaseParticles != null)
                        _releaseParticles.Play();
                    break;
            }
        }
    }
}
