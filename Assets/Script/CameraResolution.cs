using UnityEngine;

namespace Camera
{
    public class CameraResolution : MonoBehaviour
    {
        private void Start()
        {
            var thisCamera = GetComponent<UnityEngine.Camera>();
            var rect = thisCamera.rect;
            var scaleHeight = (float)Screen.width / Screen.height / ((float)9 / 16);// (가로 / 세로)
            var scaleWidth = 1f / scaleHeight;
            if (scaleHeight < 1)
            {
                rect.height = scaleHeight;
                rect.y = (1f - scaleHeight) / 2f;
            }
            else
            {
                rect.width = scaleWidth;
                rect.x = (1f - scaleWidth) / 2f;
            }

            thisCamera.rect = rect;
        }

        private void OnPreCull()
        {
            GL.Clear(true, true, Color.black);
        }
    }
}