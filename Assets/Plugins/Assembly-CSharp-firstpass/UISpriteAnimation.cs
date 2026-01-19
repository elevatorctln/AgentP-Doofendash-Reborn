using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpriteAnimation
{
	public bool loopReverse;

	public float frameTime = 0.2f;

	private List<UIUVRect> uvRects = new List<UIUVRect>();

	private bool _isPlaying;

	public bool isPlaying
	{
		get
		{
			return _isPlaying;
		}
	}

	public UISpriteAnimation(float frameTime, List<UIUVRect> uvRects)
	{
		this.frameTime = frameTime;
		this.uvRects = uvRects;
	}

	public IEnumerator play(UISprite sprite, int loopCount)
	{
		UIUVRect originalUVFrame = sprite.uvFrame;
		int totalFrames = uvRects.Count;
		int currentFrame = 0;
		WaitForSeconds waiter = new WaitForSeconds(frameTime);
		bool loopingForward = true;
		_isPlaying = true;
		while (_isPlaying && (loopCount > 0 || loopCount == -1))
		{
			currentFrame = ((!loopingForward) ? (currentFrame - 1) : (currentFrame + 1));
			if (currentFrame < 0 || currentFrame == totalFrames)
			{
				if (loopCount > 0)
				{
					loopCount--;
				}
				if (loopReverse)
				{
					loopingForward = !loopingForward;
				}
				currentFrame = ((!loopingForward) ? (currentFrame - 1) : 0);
			}
			sprite.uvFrame = uvRects[currentFrame];
			yield return waiter;
		}
		_isPlaying = false;
		sprite.uvFrame = originalUVFrame;
	}

	public void stop()
	{
		_isPlaying = false;
	}
}
