using UnityEngine;

public class EvilInteriorFallingDoors : Obstacle
{
	private Animation m_Animation;

	public GameObject[] m_Doors;

	private void Awake()
	{
		m_Animation = GetComponent<Animation>();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider trigger)
	{
		TriggerFallAnim();
	}

	private void TriggerFallAnim()
	{
		Debug.Log("Triggered Animated Obstacle - " + base.name);
		m_Animation.Play("OneShot");
	}
}
