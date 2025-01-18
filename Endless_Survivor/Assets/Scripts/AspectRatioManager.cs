using UnityEngine;

public class AspectRatioManager : MonoBehaviour
{
    void Start()
    {
        // Obtiene la resolución nativa del dispositivo
        int nativeWidth = Screen.currentResolution.width;
        int nativeHeight = Screen.currentResolution.height;

        // Establece la relación de aspecto objetivo (por ejemplo, 16:9)
        float targetAspect = 16f / 9f;

        // Calcula la relación de aspecto del dispositivo
        float deviceAspect = (float)nativeWidth / nativeHeight;

        // Configura el rectángulo de la cámara para mantener la relación de aspecto
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("No se encontró una cámara principal en la escena.");
            return;
        }

        if (deviceAspect > targetAspect)
        {
            // El dispositivo es más ancho que el objetivo, ajusta el ancho
            float scaleWidth = targetAspect / deviceAspect;
            mainCamera.rect = new Rect((1f - scaleWidth) / 2f, 0f, scaleWidth, 1f);
        }
        else if (deviceAspect < targetAspect)
        {
            // El dispositivo es más alto que el objetivo, ajusta la altura
            float scaleHeight = deviceAspect / targetAspect;
            mainCamera.rect = new Rect(0f, (1f - scaleHeight) / 2f, 1f, scaleHeight);
        }
        else
        {
            // La relación de aspecto ya coincide, usa pantalla completa
            mainCamera.rect = new Rect(0f, 0f, 1f, 1f);
        }

        // Opcional: forzar la resolución lógica del juego
        Screen.SetResolution(nativeWidth, nativeHeight, true);

        Debug.Log($"Resolución configurada: {nativeWidth}x{nativeHeight}, Aspect Ratio: {targetAspect}");

    }
}
