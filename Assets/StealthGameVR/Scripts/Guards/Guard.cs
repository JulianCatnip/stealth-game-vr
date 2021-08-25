using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowDetect;

public class Guard : MonoBehaviour
{
	// event for player script to subcribe to
	public static event System.Action OnGuardHasSpottedPlayer;
	public static event System.Action OnGuardHeardDisturbance;

	public float speed = 5;
	public float waitTime = .3f;
	public float turnSpeed = 90;
	public float timeToSpotPlayer = .5f;

	public Light spotlight;
	public float viewDistance;

	// layer mask for line of sight obstacles
	public LayerMask viewMask;

	float viewAngle;
	float playerVisibleTimer;

	public Transform pathHolder;
	Transform player;
	Color originalSpotlightColour;



	public AudioSource[] noises;
	private AudioSource noiseScource;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		viewAngle = spotlight.spotAngle;
		originalSpotlightColour = spotlight.color;

		// move between waypoints
		Vector3[] waypoints = new Vector3[pathHolder.childCount];
		for (int i = 0; i < waypoints.Length; i++)
		{
			waypoints[i] = pathHolder.GetChild(i).position;
			waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
		}

		// coroutines allow you to execute game logic over a number of frames.
		StartCoroutine(FollowPath(waypoints));

	}

	void Update()
	{
		if (CanSeePlayer())
		{
			playerVisibleTimer += Time.deltaTime;
		}
		else
		{
			playerVisibleTimer -= Time.deltaTime;
		}
		playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
		spotlight.color = Color.Lerp(originalSpotlightColour, Color.red, playerVisibleTimer / timeToSpotPlayer);

		if (playerVisibleTimer >= timeToSpotPlayer)
		{
			if (OnGuardHasSpottedPlayer != null)
				OnGuardHasSpottedPlayer();
		}

		if (HeardDisturbance())
		{
			if (OnGuardHasSpottedPlayer != null)
				OnGuardHeardDisturbance();
		}
	}

	bool CanSeePlayer()
	{   // player in view distance
		if (Vector3.Distance(transform.position, player.position) < viewDistance)
		{
			Vector3 dirToPlayer = (player.position - transform.position).normalized;
			float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
			// player in view angle
			if (angleBetweenGuardAndPlayer < viewAngle / 2f)
			{
				// line of sight obscured
				if (!Physics.Linecast(transform.position, player.position, viewMask))
				{
					return true;
				}
			}
		}
		return false;
	}

	bool HeardDisturbance()
	{
		foreach (AudioSource i in noises)
		{
			if (i.isPlaying)
			{
				noiseScource = i;
				return true;
			}
		}
		return false;
	}

	IEnumerator FollowPath(Vector3[] waypoints)
	{
		transform.position = waypoints[0];

		int targetWaypointIndex = 1;
		Vector3 targetWaypoint = waypoints[targetWaypointIndex];
		transform.LookAt(targetWaypoint);

		while (true)
		{
			// (re)enable walk anim
			gameObject.GetComponent<Animator>().SetBool("isWalking", true);
			transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
			if (transform.position == targetWaypoint)
			{
				targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
				if (CanSeePlayer())
					targetWaypoint = player.position;
				else if (HeardDisturbance())
					targetWaypoint = noiseScource.transform.position;
				else
					targetWaypoint = waypoints[targetWaypointIndex];
				// stop walk anim
				gameObject.GetComponent<Animator>().SetBool("isWalking", false);
				// wait at waypoint
				yield return new WaitForSeconds(waitTime);
				// then turn
				yield return StartCoroutine(TurnToFace(targetWaypoint));
			}
			// wait for the next frame
			yield return null;
		}
	}

	IEnumerator TurnToFace(Vector3 lookTarget)
	{
		Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
		float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

		while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
		{
			float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
			transform.eulerAngles = Vector3.up * angle;
			yield return null;
		}
	}

	void OnDrawGizmos()
	{
		Vector3 startPosition = pathHolder.GetChild(0).position;
		Vector3 previousPosition = startPosition;

		// visualize path
		foreach (Transform waypoint in pathHolder)
		{
			Gizmos.DrawSphere(waypoint.position, .3f);
			Gizmos.DrawLine(previousPosition, waypoint.position);
			previousPosition = waypoint.position;
		}
		Gizmos.DrawLine(previousPosition, startPosition);

		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
	}

	public void halfViewDistance()
	{
		viewDistance = viewDistance / 2;
	}

	public void doubleViewDistance()
	{
		if (viewDistance < 5)
			viewDistance = viewDistance * 2;
	}

}