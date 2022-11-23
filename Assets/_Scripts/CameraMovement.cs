using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Camera mainCamera;

    float controlsMoveSpeed = 20f;
    float mouseMoveSpeed = 25f;
    float edgeSize = 10f;
    float xAxisLimit = 90f;
    float yAxisLimit = 70f;

    float zoom = 20f;
    float maxZoom = 50f;
    float minZoom = 3f;
    float zoomKeySpeed = 10f;
    float zoomScrollSpeed = 250f;

    void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Start()
    {
        mainCamera.orthographicSize = zoom;    
    }

    void Update()
    {
        CameraControls();
        //MouseControls();
        HandleZoom();
    }

    void CameraControls()
    {
        float speed = controlsMoveSpeed * Time.deltaTime;

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && mainCamera.transform.position.y < yAxisLimit)
        {
            mainCamera.transform.position += Vector3.up * speed;
        }
        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && mainCamera.transform.position.y > -yAxisLimit)
        {
            mainCamera.transform.position += Vector3.down * speed;
        }
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && mainCamera.transform.position.x > -xAxisLimit)
        {
            mainCamera.transform.position += Vector3.left * speed;
        }
        if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && mainCamera.transform.position.x < xAxisLimit)
        {
            mainCamera.transform.position += Vector3.right * speed;
        }
    }

    void MouseControls()
    {
        float speed = mouseMoveSpeed * Time.deltaTime;

        if (Input.mousePosition.y > Screen.height - edgeSize && mainCamera.transform.position.y < yAxisLimit)
        {
            mainCamera.transform.position += Vector3.up * speed;
        }
        if (Input.mousePosition.y < edgeSize && mainCamera.transform.position.y > -yAxisLimit)
        {
            mainCamera.transform.position += Vector3.down * speed;
        }
        if (Input.mousePosition.x > Screen.width - edgeSize && mainCamera.transform.position.x < xAxisLimit)
        {
            mainCamera.transform.position += Vector3.right * speed;
        }
        if (Input.mousePosition.x < edgeSize && mainCamera.transform.position.x > -xAxisLimit)
        {
            mainCamera.transform.position += Vector3.left * speed;
        }
    }

    void HandleZoom()
    {        
        if (zoom > minZoom)
        {
            if (Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.Equals))
            {
                zoom -= zoomKeySpeed * Time.deltaTime;
                mainCamera.orthographicSize = Mathf.Clamp(zoom, minZoom, maxZoom);
            }

            if (Input.mouseScrollDelta.y > 0)
            {
                zoom -= zoomScrollSpeed * Time.deltaTime;
                mainCamera.orthographicSize = Mathf.Clamp(zoom, minZoom, maxZoom);
            }
        }
        
        if (zoom < maxZoom)
        {
            if (Input.GetKey(KeyCode.KeypadMinus) || Input.GetKey(KeyCode.Minus))
            {
                zoom += zoomKeySpeed * Time.deltaTime;
                mainCamera.orthographicSize = Mathf.Clamp(zoom, minZoom, maxZoom);
            }

            if (Input.mouseScrollDelta.y < 0)
            {
                zoom += zoomScrollSpeed * Time.deltaTime;
                mainCamera.orthographicSize = Mathf.Clamp(zoom, minZoom, maxZoom);
            }
        }
    }
}
