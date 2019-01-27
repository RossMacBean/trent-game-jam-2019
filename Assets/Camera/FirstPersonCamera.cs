using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    private static FirstPersonCamera _instance;
    public static FirstPersonCamera instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<FirstPersonCamera>();

                //Tell unity not to destroy this object when loading a new scene!
                //DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    #region inspector properties    

    private Transform AttatchTo;
    public bool IsAttatched => AttatchTo != null;
    public float XSensitivity = 3f;
    public float YMouseSensitivity = 3f;
    public float YMinLimit = -40f;
    public float YMaxLimit = 80f;
    #endregion

    #region hide properties    

    private Camera mCamera;
    private float lookX = 0f;
    private float lookY = 0f;
    private float xMinLimit = -360f;
    private float xMaxLimit = 360f;

    #endregion

    void Start()
    {
        Init();
    }

    public void Init()
    {
        mCamera = FindObjectOfType<Camera>();

        // Subscribe to mouse move events
        InputManager.Singleton.OnMouseMoveEvent += (Vector3 pos, Vector3 delta) => Rotate(delta.x, delta.y);

		AttatchTo = GetComponentInParent<Transform>();

		lookY = IsAttatched ? AttatchTo.eulerAngles.x : 0.0f;
        lookX = IsAttatched ? AttatchTo.eulerAngles.y : 0.0f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
		UpdateCamera();
    }

    public void Rotate(float x, float y)
    {
        lookX += x * XSensitivity;
        lookY -= y * YMouseSensitivity;

        lookY = CameraUtils.ClampAngle(lookY, YMinLimit, YMaxLimit);
        lookX = CameraUtils.ClampAngle(lookX, xMinLimit, xMaxLimit);
    }
  
    void UpdateCamera()
    {
        transform.rotation = IsAttatched ? Quaternion.Euler(lookY, AttatchTo.eulerAngles.y, 0) : Quaternion.Euler(lookY, lookX, 0); ;
    }
}
