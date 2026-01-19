using UnityEngine;

public class BossWeapon : MonoBehaviour
{
	public ObstacleHitTrigger m_ObstacleHitTrigger;

	public float m_CollisionStartDelay = 1.25f;

	public float m_AttackLoopDuration = 2f;

	public float m_ParticleCoolOffDuration = 3f;

	public float m_ColliderCoolOffDuration = 1.05f;
}
