using System;
using UnityEngine;

public static class UIObjectAnimationExtensions
{
	public static UIAnimation alphaTo(this UIObject sprite, float duration, float target, Func<float, float> ease)
	{
		return sprite.to(duration, UIAnimationProperty.Alpha, target, ease);
	}

	public static UIAnimation alphaFrom(this UIObject sprite, float duration, float target, Func<float, float> ease)
	{
		return sprite.from(duration, UIAnimationProperty.Alpha, target, ease);
	}

	public static UIAnimation alphaFromTo(this UIObject sprite, float duration, float start, float target, Func<float, float> ease)
	{
		return sprite.fromTo(duration, UIAnimationProperty.Alpha, start, target, ease);
	}

	public static UIAnimation colorTo(this UIObject sprite, float duration, Color target, Func<float, float> ease)
	{
		return sprite.to(duration, UIAnimationProperty.Color, target, ease);
	}

	public static UIAnimation colorFrom(this UIObject sprite, float duration, Color target, Func<float, float> ease)
	{
		return sprite.from(duration, UIAnimationProperty.Color, target, ease);
	}

	public static UIAnimation colorFromTo(this UIObject sprite, float duration, Color start, Color target, Func<float, float> ease)
	{
		return sprite.fromTo(duration, UIAnimationProperty.Color, start, target, ease);
	}

	public static UIAnimation eulerAnglesTo(this UIObject sprite, float duration, Vector3 target, Func<float, float> ease)
	{
		return sprite.to(duration, UIAnimationProperty.EulerAngles, target, ease);
	}

	public static UIAnimation eulerAnglesFrom(this UIObject sprite, float duration, Vector3 target, Func<float, float> ease)
	{
		return sprite.from(duration, UIAnimationProperty.EulerAngles, target, ease);
	}

	public static UIAnimation eulerAnglesFromTo(this UIObject sprite, float duration, Vector3 start, Vector3 target, Func<float, float> ease)
	{
		return sprite.fromTo(duration, UIAnimationProperty.EulerAngles, start, target, ease);
	}

	public static UIAnimation scaleTo(this UIObject sprite, float duration, Vector3 target, Func<float, float> ease)
	{
		return sprite.to(duration, UIAnimationProperty.Scale, target, ease);
	}

	public static UIAnimation scaleFrom(this UIObject sprite, float duration, Vector3 target, Func<float, float> ease)
	{
		return sprite.from(duration, UIAnimationProperty.Scale, target, ease);
	}

	public static UIAnimation scaleFromTo(this UIObject sprite, float duration, Vector3 start, Vector3 target, Func<float, float> ease)
	{
		return sprite.fromTo(duration, UIAnimationProperty.Scale, start, target, ease);
	}

	public static UIAnimation positionTo(this UIObject sprite, float duration, Vector3 target, Func<float, float> ease)
	{
		return sprite.to(duration, UIAnimationProperty.Position, target, ease);
	}

	public static UIAnimation positionFrom(this UIObject sprite, float duration, Vector3 target, Func<float, float> ease)
	{
		return sprite.from(duration, UIAnimationProperty.Position, target, ease);
	}

	public static UIAnimation positionFromTo(this UIObject sprite, float duration, Vector3 start, Vector3 target, Func<float, float> ease)
	{
		return sprite.fromTo(duration, UIAnimationProperty.Position, start, target, ease);
	}

	public static UIAnimation to(this UIObject sprite, float duration, UIAnimationProperty aniProperty, float target, Func<float, float> ease)
	{
		return animate(sprite, true, duration, aniProperty, target, ease);
	}

	public static UIAnimation to(this UIObject sprite, float duration, UIAnimationProperty aniProperty, Color target, Func<float, float> ease)
	{
		return animate(sprite, true, duration, aniProperty, target, ease);
	}

	public static UIAnimation to(this UIObject sprite, float duration, UIAnimationProperty aniProperty, Vector3 target, Func<float, float> ease)
	{
		return animate(sprite, true, duration, aniProperty, target, ease);
	}

	public static UIAnimation from(this UIObject sprite, float duration, UIAnimationProperty aniProperty, float target, Func<float, float> ease)
	{
		return animate(sprite, false, duration, aniProperty, target, ease);
	}

	public static UIAnimation from(this UIObject sprite, float duration, UIAnimationProperty aniProperty, Color target, Func<float, float> ease)
	{
		return animate(sprite, false, duration, aniProperty, target, ease);
	}

	public static UIAnimation from(this UIObject sprite, float duration, UIAnimationProperty aniProperty, Vector3 target, Func<float, float> ease)
	{
		return animate(sprite, false, duration, aniProperty, target, ease);
	}

	public static UIAnimation fromTo(this UIObject sprite, float duration, UIAnimationProperty aniProperty, float start, float target, Func<float, float> ease)
	{
		return animate(sprite, duration, aniProperty, start, target, ease);
	}

	public static UIAnimation fromTo(this UIObject sprite, float duration, UIAnimationProperty aniProperty, Color start, Color target, Func<float, float> ease)
	{
		return animate(sprite, duration, aniProperty, start, target, ease);
	}

	public static UIAnimation fromTo(this UIObject sprite, float duration, UIAnimationProperty aniProperty, Vector3 start, Vector3 target, Func<float, float> ease)
	{
		return animate(sprite, duration, aniProperty, start, target, ease);
	}

	private static UIAnimation animate(UIObject sprite, bool animateTo, float duration, UIAnimationProperty aniProperty, float target, Func<float, float> ease)
	{
		float num = 0f;
		if (aniProperty == UIAnimationProperty.Alpha)
		{
			num = sprite.color.a;
		}
		float start = ((!animateTo) ? target : num);
		if (!animateTo)
		{
			target = num;
		}
		return animate(sprite, duration, aniProperty, start, target, ease);
	}

	private static UIAnimation animate(UIObject sprite, float duration, UIAnimationProperty aniProperty, float start, float target, Func<float, float> ease)
	{
		UIAnimation uIAnimation = new UIAnimation(sprite, duration, aniProperty, start, target, ease);
		UI.instance.StartCoroutine(uIAnimation.animate());
		return uIAnimation;
	}

	private static UIAnimation animate(UIObject sprite, bool animateTo, float duration, UIAnimationProperty aniProperty, Color target, Func<float, float> ease)
	{
		Color color = sprite.color;
		Color start = ((!animateTo) ? target : color);
		if (!animateTo)
		{
			target = color;
		}
		return animate(sprite, duration, aniProperty, start, target, ease);
	}

	private static UIAnimation animate(UIObject sprite, float duration, UIAnimationProperty aniProperty, Color start, Color target, Func<float, float> ease)
	{
		UIAnimation uIAnimation = new UIAnimation(sprite, duration, aniProperty, start, target, ease);
		UI.instance.StartCoroutine(uIAnimation.animate());
		return uIAnimation;
	}

	private static UIAnimation animate(UIObject sprite, bool animateTo, float duration, UIAnimationProperty aniProperty, Vector3 target, Func<float, float> ease)
	{
		Vector3 vector = Vector3.zero;
		switch (aniProperty)
		{
		case UIAnimationProperty.Position:
			vector = sprite.localPosition;
			break;
		case UIAnimationProperty.Scale:
			vector = sprite.scale;
			break;
		case UIAnimationProperty.EulerAngles:
			vector = sprite.eulerAngles;
			break;
		}
		Vector3 start = ((!animateTo) ? target : vector);
		if (!animateTo)
		{
			target = vector;
		}
		return animate(sprite, duration, aniProperty, start, target, ease);
	}

	private static UIAnimation animate(UIObject sprite, float duration, UIAnimationProperty aniProperty, Vector3 start, Vector3 target, Func<float, float> ease)
	{
		UIAnimation uIAnimation = new UIAnimation(sprite, duration, aniProperty, start, target, ease);
		UI.instance.StartCoroutine(uIAnimation.animate());
		return uIAnimation;
	}
}
