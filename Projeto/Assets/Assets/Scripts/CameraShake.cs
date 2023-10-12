using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;


	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shake_intensity = 1f;


	Vector3 originalPos;

	void Awake()
	{
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	void OnEnable()
	{
		originalPos = camTransform.localPosition;

	}

	void Update()
	{
		camTransform.localPosition = new Vector3(originalPos.x + shake_intensity * Mathf.Sin(Time.time * .7f), originalPos.y + shake_intensity * Mathf.Sin(Time.time * 1.2f), originalPos.z + shake_intensity * Mathf.Sin(Time.time*2.3f));

	}
}