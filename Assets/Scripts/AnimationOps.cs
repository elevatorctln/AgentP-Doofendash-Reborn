using UnityEngine;

public class AnimationOps
{
	public static void SetAnimationSpeed(Animation anim, float speed)
	{
		foreach (AnimationState item in anim)
		{
			item.speed = speed;
		}
	}

	public static void SetAnimationSpeed(Animation anim, string animName, float speed)
	{
		AnimationState animationState = anim[animName];
		if (animationState != null)
		{
			animationState.speed = speed;
		}
	}

	public static float GetAnimationSpeed(Animation anim, string animName)
	{
		AnimationState animationState = anim[animName];
		if (animationState != null)
		{
			return animationState.speed;
		}
		return 0f;
	}
}
