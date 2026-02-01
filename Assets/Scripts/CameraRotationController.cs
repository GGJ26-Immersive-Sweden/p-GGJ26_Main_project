using System.Collections;
using UnityEngine;

//TODO Study this implementation because I Cluade coded it
public class CameraRotationController : MonoBehaviour
{
    // Rotation configuration
    private const float ROTATION_STEP = -90f;
    private const float ROTATION_DURATION = 0.5f; // 500ms

    // Track if rotation is in progress to prevent overlapping calls
    private bool _isRotating = false;

    public void RotateCameraToNextRoom()
    {
        // Prevent starting a new rotation while one is in progress
        if (_isRotating) return;

        StartCoroutine(RotateCameraAroundScene());
    }

    private IEnumerator RotateCameraAroundScene()
    {
        _isRotating = true;

        // Store initial and target rotations
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0f, ROTATION_STEP, 0f);

        // Track elapsed time
        float elapsedTime = 0f;

        while (elapsedTime < ROTATION_DURATION)
        {
            elapsedTime += Time.deltaTime;

            // Calculate normalized progress (0 to 1)
            float t = elapsedTime / ROTATION_DURATION;

            // Optional: Apply easing for smoother feel (ease-in-out)
            float easedT = t * t * (3f - 2f * t);

            // Interpolate between start and target rotation
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, easedT);

            yield return null;
        }

        // Ensure we land exactly on target rotation
        transform.rotation = targetRotation;

        _isRotating = false;
    }

    public void RotateCameraToNextRoom(Vector3 point, float angle, float duration)
    {
        // Prevent starting a new rotation while one is in progress
        if (!_isRotating)
            StartCoroutine(RotateCameraAroundPoint(point, angle, duration));
    }

    private IEnumerator RotateCameraAroundPoint(Vector3 point, float angle, float duration) {
        _isRotating = true;

        Quaternion targetRotation = Quaternion.Euler(0f, angle, 0f) * transform.rotation;

        // Track elapsed time
        float elapsedTime = 0f;
        float lastTheta = 0f;
        while ((elapsedTime += Time.deltaTime) < duration) {

            // Calculate normalized progress (0 to 1)
            float t = elapsedTime / duration;

            // Optional: Apply easing for smoother feel (ease-in-out)
            float easedT = t * t * (3f - 2f * t);
            transform.RotateAround(point, Vector3.up, (easedT - lastTheta) * angle);
            lastTheta = easedT;
            
            yield return null;
        }

        // Ensure we land exactly on target rotation
        transform.rotation = targetRotation;

        _isRotating = false;
    }
}
