using System.Collections;
using UnityEngine;

public class ActionCamera : MonoBehaviour
{
    public Transform player;  
    public float smoothSpeed = 0.125f;
    public Vector3 offset;  

    public float lookAheadDistanceX = 2f;  
    public float lookAheadSmoothTime = 0.5f;  
    private Vector3 currentVelocity;

    public float zoomSpeed = 6f;
    public float minZoom = 3f;
    public float maxZoom = 25f;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");

       
        cam.orthographicSize -= scrollData * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
    }
    private void LateUpdate()
    {
        if(player != null)
        {
            Vector3 targetPosition = player.position + offset;

            Vector3 lookAheadPos = Vector3.zero;
            if (Input.GetAxis("Horizontal") != 0)
            {
                lookAheadPos = new Vector3(lookAheadDistanceX * Mathf.Sign(Input.GetAxis("Horizontal")), 0, 0);
            }

            targetPosition += lookAheadPos;

            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothSpeed);
            transform.position = smoothedPosition;
        }
       
    }

    //Èçµé¸² ¼³Á¤
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
