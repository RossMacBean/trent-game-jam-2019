using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
	[SerializeField] float MaxInteractDistance = 5;
	[SerializeField] float InteractRadius = 5;
	[SerializeField] bool DrawDebug = true;

    // Start is called before the first frame update
    void Start()
    {
		//InputManager.Singleton.RegisterKeyDownEvent(KeyCode.E, InputInteract);
    }

	private void OnDisable()
	{
		//InputManager.Singleton.UnregisterKeyDownEvent(KeyCode.E, InputInteract);
	}

	// Update is called once per frame
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
		{
			bool bSuccess = AttemptInteract();
		}
    }


	public void InputInteract()
	{
		bool bSuccess = AttemptInteract();
	}

	public bool AttemptInteract()
	{
		RaycastHit Hit;
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));

		if (DrawDebug)
		{
			Debug.DrawRay(ray.origin, ray.direction, Color.red, 3);
		}		

		if (Physics.Raycast(ray, out Hit, MaxInteractDistance))
		{
			IInteractive InteractiveObject = Hit.collider.GetComponent<IInteractive>();
			if (InteractiveObject != null)
			{
				InteractiveObject.OnInteract();
				return true;
			}
		}

		return false;
	}

	private void OnDrawGizmos()
	{
		if (DrawDebug)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
			Vector3 OriginLocation = ray.origin;
			Vector3 EndLocation = ray.origin + (ray.direction * MaxInteractDistance);
			Gizmos.DrawWireSphere(OriginLocation, InteractRadius);
			Gizmos.DrawWireSphere(EndLocation, InteractRadius);
		}
	}
}
