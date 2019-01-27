using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider), typeof(CableComponent))]
public class HookShot : MonoBehaviour
{
	[SerializeField] float LaunchSpeed = 5;
	[SerializeField] float PullSpeed = 5;
	[SerializeField] float MaxCableLength = 10;
	[SerializeField] string[] CollisionIgnoreTags;

	bool mIsLatched = false;
	bool mReel = false;
	CableComponent mCable;
	Rigidbody mOtherBody;
	HingeJoint mHingeJoint;

	Vector3 Velocity;

	RigidBodyPlayerController mOwningPlayer;
	SphereCollider mSphere;
	CapsuleCollider mPlayerCapsule;
	Rigidbody mHookRigidbody;

	public bool IsLatched => mIsLatched;
	public bool IsReeling => mReel;

	// Start is called before the first frame update
	void Awake()
	{
		mCable = FindObjectOfType<CableComponent>();
		mSphere = FindObjectOfType<SphereCollider>();
		mSphere.isTrigger = true;

		mHookRigidbody = FindObjectOfType<Rigidbody>();
		mHookRigidbody.isKinematic = true;
	}

	// Update is called once per frame
	void Update()
	{
		if (!mIsLatched)
		{
			float SquareLength = (mOwningPlayer.transform.position - transform.position).sqrMagnitude;
			if (SquareLength > (MaxCableLength*MaxCableLength))
			{
				Physics.IgnoreCollision(mSphere, mPlayerCapsule, false);
				mReel = true;
			}

			if (mReel)
			{
				Velocity = (mOwningPlayer.transform.position - transform.position).normalized * LaunchSpeed;
			}
		}

		transform.position += Velocity * Time.deltaTime;
	}

	public bool IsMobile()
	{
		return Velocity != Vector3.zero;
	}

	public void RegisterOwningPlayer(RigidBodyPlayerController InPlayer)
	{
		transform.position = InPlayer.transform.position;
		mOwningPlayer = InPlayer;
		mCable.SetEndPoint(InPlayer.transform);
		Velocity = Vector3.zero;
		mPlayerCapsule = InPlayer.GetComponent<CapsuleCollider>();

		Physics.IgnoreCollision(mSphere, mPlayerCapsule, true);
	}

	public void Launch(Vector3 startLocation, Vector3 Direction)
	{
		gameObject.SetActive(true);

		//gameObject.SetActive(true);
		transform.position = startLocation;
		Velocity = Direction * LaunchSpeed;
	}

	public void ForceReel()
	{
		mIsLatched = false;
		mReel = true;
	}

	public Vector3 GetPullVelocity()
	{
		if (mIsLatched)
		{
			Vector3 PlayerToHookDirection = (transform.position - mPlayerCapsule.transform.position).normalized;
			return PlayerToHookDirection * PullSpeed;
		}

		return Vector3.zero;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "NoGrapple")
			return;

		if (!IsReeling && other != mPlayerCapsule)
		{
			Physics.IgnoreCollision(mSphere, mPlayerCapsule, false);

			// latch the hook onto the hit object.
			Velocity = Vector3.zero;

			mIsLatched = true;
			mReel = false;
		}

		if (other == mPlayerCapsule)
		{
			Physics.IgnoreCollision(mSphere, mPlayerCapsule, true);
			Velocity = Vector3.zero;
			transform.position = mPlayerCapsule.transform.position;
			mReel = false;
			mIsLatched = false;
			gameObject.SetActive(false);
		}
	}
}
