using UnityEngine;
using UnityEngine.UI;

public class CorrectRatio : MonoBehaviour {

    public CanvasScaler[] canvasScaler;
    public Transition transition;

    private void Start() {
        float targetAspect = 9.0f / 16.0f;
        float windowAspect = (float)Screen.width / Screen.height;

        if (!Mathf.Approximately(targetAspect, windowAspect)) {

            float height = 640 / (float)Screen.height;
            int heightPartCount = Screen.height / 640;

            float width = 360 / (float)Screen.width;
            int widthPartCount = Screen.width / 360;

            int minPartCount = Mathf.Min(heightPartCount, widthPartCount);
            if (minPartCount == 0) {
                minPartCount = 1;
            }
            float newHeight;
            if ((float)(heightPartCount * 640) / Screen.width < 1.25f) {
                newHeight = 1;
            }
            else {
                newHeight = height * minPartCount;
            }
            float newWidth = width * minPartCount;

            GetComponent<Camera>().rect = new Rect(0, (1.0f - newHeight) / 2.0f, 1, newHeight);

            float scaleHeight = windowAspect / targetAspect;

            transition.UpdatePartsSize((Screen.height * newHeight / Screen.width) * 160 / (1 / targetAspect));

            for (int i = 0; i < canvasScaler.Length; i++) {
                canvasScaler[i].referenceResolution = new Vector2(720, 1280 / newHeight);
                if (canvasScaler != null) {
                    canvasScaler[i].matchWidthOrHeight = 1;
                }
            }
        }
    }
}
