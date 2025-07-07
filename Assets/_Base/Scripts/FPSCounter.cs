using TMPro;
using UnityEngine;

namespace WellnessApp
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField]
        public TextMeshProUGUI _fpsText;

        private float _timer;
        private int _frameCount;

        private void Start ()
        {
            if (Application.platform == RuntimePlatform.Android)
                Application.targetFrameRate = 60;
        }

        private void Update()
        {
            if (_fpsText == null)
                return;

            _frameCount++;
            _timer += Time.unscaledDeltaTime;

            if (_timer >= 1f)
            {
                int fps = Mathf.RoundToInt(_frameCount / _timer);
                _fpsText.text = "FPS: " + fps;
                _frameCount = 0;
                _timer = 0f;
            }
        }
    }
}
