using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIAnimation
{
	private static Dictionary<UIObject, List<UIAnimation>> _spriteAnimations = new Dictionary<UIObject, List<UIAnimation>>();

	private bool _running;

	private UIAnimationProperty _aniProperty;

	public Action onComplete;

	public bool autoreverse;

	private float startTime;

	private UIObject sprite;

	private float duration;

	private bool affectedByTimeScale;

	private Func<float, float> ease;

	private Vector3 start;

	private Vector3 target;

	private float startFloat;

	private float targetFloat;

	private Color startColor;

	private Color targetColor;

	public bool running
	{
		get
		{
			return _running;
		}
	}

	public UIAnimationProperty aniProperty
	{
		get
		{
			return _aniProperty;
		}
	}

	public UIAnimation(UIObject sprite, float duration, UIAnimationProperty aniProperty, Vector3 start, Vector3 target, Func<float, float> ease, bool affectedByTimeScale = true)
	{
		this.sprite = sprite;
		this.duration = duration;
		_aniProperty = aniProperty;
		this.ease = ease;
		this.affectedByTimeScale = affectedByTimeScale;
		this.target = target;
		this.start = start;
		_running = true;
		startTime = ((!affectedByTimeScale) ? Time.realtimeSinceStartup : Time.time);
	}

	public UIAnimation(UIObject sprite, float duration, UIAnimationProperty aniProperty, float startFloat, float targetFloat, Func<float, float> ease, bool affectedByTimeScale = true)
	{
		this.sprite = sprite;
		this.duration = duration;
		_aniProperty = aniProperty;
		this.ease = ease;
		this.affectedByTimeScale = affectedByTimeScale;
		this.targetFloat = targetFloat;
		this.startFloat = startFloat;
		_running = true;
		startTime = ((!affectedByTimeScale) ? Time.realtimeSinceStartup : Time.time);
	}

	public UIAnimation(UIObject sprite, float duration, UIAnimationProperty aniProperty, Color startColor, Color targetColor, Func<float, float> ease, bool affectedByTimeScale = true)
	{
		this.sprite = sprite;
		this.duration = duration;
		_aniProperty = aniProperty;
		this.ease = ease;
		this.affectedByTimeScale = affectedByTimeScale;
		this.startColor = startColor;
		this.targetColor = targetColor;
		_running = true;
		startTime = ((!affectedByTimeScale) ? Time.realtimeSinceStartup : Time.time);
	}

	public IEnumerator animate()
	{
		_running = true;
		if (_spriteAnimations.ContainsKey(sprite))
		{
			UIAnimation sameAnimationType = _spriteAnimations[sprite].Where((UIAnimation i) => i.aniProperty == aniProperty).FirstOrDefault();
			if (sameAnimationType != null)
			{
				sameAnimationType.stop();
				_spriteAnimations[sprite].Remove(sameAnimationType);
			}
		}
		else
		{
			_spriteAnimations.Add(sprite, new List<UIAnimation>());
		}
		_spriteAnimations[sprite].Add(this);
		startTime = ((!affectedByTimeScale) ? Time.realtimeSinceStartup : Time.time);
		while (_running)
		{
			float currentTime = ((!affectedByTimeScale) ? Time.realtimeSinceStartup : Time.time);
			float easPos = Mathf.Clamp01((currentTime - startTime) / duration);
			easPos = ease(easPos);
			switch (_aniProperty)
			{
			case UIAnimationProperty.Position:
				sprite.localPosition = Vector3.Lerp(start, target, easPos);
				break;
			case UIAnimationProperty.Scale:
				sprite.scale = Vector3.Lerp(start, target, easPos);
				break;
			case UIAnimationProperty.EulerAngles:
				sprite.eulerAngles = Vector3.Lerp(start, target, easPos);
				break;
			case UIAnimationProperty.Alpha:
			{
				Color currentColor = sprite.color;
				currentColor.a = Mathf.Lerp(startFloat, targetFloat, easPos);
				sprite.color = currentColor;
				break;
			}
			case UIAnimationProperty.Color:
				sprite.color = Color.Lerp(startColor, targetColor, easPos);
				break;
			}
			if (startTime + duration <= currentTime)
			{
				if (autoreverse)
				{
					autoreverse = false;
					Vector3 temp = start;
					start = target;
					target = temp;
					float tempFloat = startFloat;
					startFloat = targetFloat;
					targetFloat = tempFloat;
					Color tempColor = startColor;
					startColor = targetColor;
					targetColor = tempColor;
					startTime = currentTime;
				}
				else
				{
					_running = false;
				}
			}
			else
			{
				yield return null;
			}
		}
		_spriteAnimations[sprite].Remove(this);
		if (_spriteAnimations[sprite].Count == 0)
		{
			_spriteAnimations.Remove(sprite);
		}
		if (onComplete != null)
		{
			onComplete();
		}
	}

	public WaitForSeconds chain()
	{
		int num = ((!autoreverse) ? 1 : 2);
		return new WaitForSeconds(startTime + duration * (float)num - Time.time);
	}

	public IEnumerator realChain()
	{
		int multiplier = ((!autoreverse) ? 1 : 2);
		float endTime = startTime + duration * (float)multiplier;
		while (endTime - Time.realtimeSinceStartup > 0f)
		{
			yield return null;
		}
	}

	public void stop()
	{
		_running = false;
	}

	public void restartStartToCurrent()
	{
		switch (_aniProperty)
		{
		case UIAnimationProperty.Position:
			start = sprite.localPosition;
			break;
		case UIAnimationProperty.Scale:
			start = sprite.scale;
			break;
		case UIAnimationProperty.EulerAngles:
			start = sprite.eulerAngles;
			break;
		case UIAnimationProperty.Alpha:
			startFloat = sprite.color.a;
			break;
		case UIAnimationProperty.Color:
			startColor = sprite.color;
			break;
		}
	}

	public void setDuration(float newDuration)
	{
		if (!_running)
		{
			duration = newDuration;
		}
	}

	public void setTarget(Vector3 newTarget)
	{
		if (!_running)
		{
			switch (_aniProperty)
			{
			case UIAnimationProperty.Position:
			case UIAnimationProperty.Scale:
			case UIAnimationProperty.EulerAngles:
				target = newTarget;
				break;
			}
		}
	}
}
