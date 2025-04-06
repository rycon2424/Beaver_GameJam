using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance { get; private set; }

    public float shakeDuration = 1.5f;
    public float shakeAmount = 0.7f;

    private Transform _cameraTransform;
    private Vector3 _originalPosition;
    private Coroutine _shakeCoroutine;

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Shake()
    {
        // Als er al een shake bezig is, stoppen we 'm eerst
        if (_shakeCoroutine != null)
            StopCoroutine(_shakeCoroutine);

        _shakeCoroutine = StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        _cameraTransform = Camera.main.transform;
        _originalPosition = _cameraTransform.localPosition;

        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeAmount;
            _cameraTransform.localPosition = _originalPosition + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }


        //Laatste grote shake
        Vector3 finalKick = Random.insideUnitSphere * (shakeAmount * 2f); // of 1.5f, net wat je wilt
        _cameraTransform.localPosition = _originalPosition + finalKick;

        yield return new WaitForSeconds(0.05f); // Eventjes tonen (kan je tweaken)

        _cameraTransform.localPosition = _originalPosition;
        _shakeCoroutine = null;
    }
}
