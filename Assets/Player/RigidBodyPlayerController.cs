using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(HookShot))]
public class RigidBodyPlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float xSensitivity = 3.0f;
    public float jumpVelocity = 5.0f;
    public float sneakModifier = 0.5f;

	[SerializeField] GameObject HookshotClass;

    private float xMove = 0.0f;
    private float zMove = 0.0f;
    private Vector3 rotation = Vector3.zero;

    private bool jumpPressed = false;
    private bool sneakPressed = false;

    private Rigidbody mRigidBody;
    private Collider mCollider;
	private HookShot mHookshot;

    public bool IsGrounded { get; private set;}

    void Start()
    {
        mRigidBody = GetComponent<Rigidbody>();
        mCollider = GetComponent<Collider>();

		GameObject HookObject = Instantiate(HookshotClass);
		mHookshot = HookObject.GetComponent<HookShot>();
		mHookshot.RegisterOwningPlayer(this);

        // Subscribe to mouse move events
        InputManager.Singleton.OnMouseMoveEvent += (Vector3 pos, Vector3 delta) => Rotate(delta.x);
		InputManager.Singleton.OnLeftMouseButtonDown += OnUseHookshot;
    }

	private void OnDisable()
	{
		InputManager.Singleton.OnMouseMoveEvent -= (Vector3 pos, Vector3 delta) => Rotate(delta.x);
		InputManager.Singleton.OnLeftMouseButtonDown -= OnUseHookshot;
	}

	void FixedUpdate()
    {
		mRigidBody.angularVelocity = Vector3.zero;

        UpdateState();
        GetInput();
        ApplyForces();
    }

    void ApplyForces()
    {
		Vector3 velocity = GetVelocity();
		
		mRigidBody.MoveRotation(mRigidBody.rotation * Quaternion.Euler(rotation));
        mRigidBody.MovePosition(mRigidBody.position + velocity * Time.fixedDeltaTime);

        if (jumpPressed && IsGrounded)
        {
            float finalVelocity = jumpVelocity * (sneakPressed ? sneakModifier : 1);
            mRigidBody.AddForce(0, finalVelocity, 0, ForceMode.Impulse);
            NoiseManager.Instance.IncreaseNoise(15.0f);
        }

		mRigidBody.AddForce(mHookshot.GetPullVelocity());
    }

    private void UpdateState()
    {
        // Update grounded state
        float distanceToGround = mCollider.bounds.extents.y;
        IsGrounded = Physics.Raycast(transform.position, Vector3.down, distanceToGround + 0.1f);
    }

    private void GetInput()
    {
        var inputManager = InputManager.Singleton;

        xMove = Input.GetAxis("Horizontal");
        zMove = Input.GetAxis("Vertical");

        jumpPressed = Input.GetKeyDown(KeyCode.Space);
        sneakPressed = Input.GetAxis("Sneak") > 0;
    }

	private void OnUseHookshot(Vector3 MousePosition)
	{
		if (!mHookshot.IsMobile() && !mHookshot.IsLatched)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
			Vector3 ShootDirection = ray.direction;

			RaycastHit Hit;
			if (Physics.Raycast(ray, out Hit, 2000))
			{
				ShootDirection = (Hit.point - ray.origin).normalized;
			}

			mHookshot.Launch(transform.position, ray.direction);
		}
		else if (mHookshot.IsLatched)
		{
			mHookshot.ForceReel();
		}
		else
		{
			Debug.Log("Hookshot not launched, already mobile.");
		}
	}

    private void Rotate(float x)
    {
        // Rotate around the y axis
        rotation = new Vector3(0f, x, 0f) * xSensitivity;
    }

    private Vector3 GetVelocity()
    {
        Vector3 moveHorizontal = transform.right * xMove;
        Vector3 moveVertical = transform.forward * zMove;

        float finalMoveSpeed = moveSpeed * (sneakPressed ? sneakModifier : 1);
        return (moveHorizontal + moveVertical) * finalMoveSpeed;
    }
}