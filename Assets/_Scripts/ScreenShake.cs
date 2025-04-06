using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance { get; private set; }

    public float shakeDuration = 1.5f;
    public float shakeAmount = 0.7f;

    private Vector3 _originalPosition;
    private Coroutine _shakeCoroutine;

    private void Awake()
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
        if (_shakeCoroutine != null)
            StopCoroutine(_shakeCoroutine);

        _shakeCoroutine = StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        _originalPosition = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeAmount;
            transform.localPosition = _originalPosition + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }
        
        Vector3 finalKick = Random.insideUnitSphere * (shakeAmount * 2f);
        transform.localPosition = _originalPosition + finalKick;

        yield return new WaitForSeconds(0.05f);

        transform.localPosition = _originalPosition;
        _shakeCoroutine = null;
    }
}
