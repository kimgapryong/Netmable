using System.Collections;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public static CameraMove Instance {  get; private set; }
    private Transform player;  
    public float smoothSpeed = 0.125f;
    public Vector3 offset;  

    public float lookAheadDistanceX = 2f;  
    public float lookAheadSmoothTime = 0.5f;  
    private Vector3 currentVelocity;
    private Vector3 smoothedPosition;

    public float zoomSpeed = 6f;
    public float minZoom = 3f;
    public float maxZoom = 25f;

    private Camera cam;


    private Vector3 initialPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        player = GameObject.Find("Player").transform;
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");

       
        cam.orthographicSize -= scrollData * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        if(player != null )
        {
            initialPosition = smoothedPosition;
        }
        
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

            smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothSpeed);
            transform.position = smoothedPosition;
        }
       
    }

    //흔들림 설정
    public IEnumerator Shake(float shakeD, float shakeM, float damping)
    {
        float elapsed = 0.0f;
        float currentMagnitude = shakeM;  // 진폭을 별도의 변수로 관리

        while (elapsed < shakeD)
        {
            float x = Random.Range(-1f, 1f) * currentMagnitude;
            float y = Random.Range(-1f, 1f) * currentMagnitude;

            transform.localPosition = new Vector3(initialPosition.x + x, initialPosition.y + y, -10);

            elapsed += Time.deltaTime;

            // 진폭을 감쇠시키는 부분에서 시간을 고려하여 자연스럽게 줄어들도록 변경
            currentMagnitude = Mathf.Lerp(shakeM, 0f, elapsed / shakeD);

            yield return null;
        }

        // 쉐이크가 끝나면 원래 위치로 복귀
        transform.localPosition = new Vector3(initialPosition.x, initialPosition.y, -10);
    }


    public IEnumerator CamMover(bool right)
    {
        Vector3 camPos = cam.transform.position;
        if(right)
        {
            while(camPos.x < camPos.x - 3)
            {
                cam.transform.position += new Vector3(-0.2f, 0);
                yield return null;  
            }
        }
        else
        {
            while (camPos.x > camPos.x + 3)
            {
                cam.transform.position += new Vector3(0.2f, 0);
                yield return null;
            }
        }
    }

    public IEnumerator Tilt(float angle, float duration)
    {
        Quaternion originalRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = originalRotation;
    }
}
