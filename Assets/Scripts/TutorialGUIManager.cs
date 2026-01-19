using UnityEngine;

public class TutorialGUIManager : MonoBehaviour
{
	public enum TutorialSwipeDirection
	{
		LEFT = 0,
		RIGHT = 1,
		UP = 2,
		DOWN = 3
	}

	private UISprite m_handTouch;

	private UISpriteAnimation m_handTouchAnim;

	private UITextInstance m_instructionText;

	private int m_tutorialDepth = 4;

	public bool m_isSwipingAnim;

	private bool m_ShouldLoopSwipeUntilCancelled;

	private bool m_ShouldShowHandTouchUntilCancelled;

	private Timer m_InstructionTextTimer;

	private bool m_IsInstructionTextTimed;

	private static TutorialGUIManager m_the;

	private bool m_IsInited;

	public static TutorialGUIManager The
	{
		get
		{
			if (m_the == null)
			{
				GameObject gameObject = new GameObject("TutorialGUIManager");
				m_the = (TutorialGUIManager)gameObject.AddComponent<TutorialGUIManager>();
				m_the.Init();
			}
			return m_the;
		}
	}

	private void Awake()
	{
		GameEventManager.GameRestartMenu += GameRestartMenuListener;
	}

	private void OnDestroy()
	{
		GameEventManager.GameRestartMenu -= GameRestartMenuListener;
	}

	private void Start()
	{
		m_InstructionTextTimer = TimerManager.The().SpawnTimer();
	}

	public void Init()
	{
		if (!m_IsInited)
		{
			m_IsInited = true;
			Object.DontDestroyOnLoad(base.gameObject);
			InitHandTouch();
			InitTutText();
			HideAll();
		}
	}

	private void ResetVars()
	{
		m_isSwipingAnim = false;
		m_ShouldLoopSwipeUntilCancelled = false;
		m_ShouldShowHandTouchUntilCancelled = false;
		m_IsInstructionTextTimed = false;
		if (m_handTouchAnim.isPlaying)
		{
			m_handTouchAnim.stop();
		}
	}

	private void InitHandTouch()
	{
		m_handTouch = GlobalGUIManager.The.m_hudToolkit.addSprite("PerryHand001.png", 0, 0, m_tutorialDepth);
		m_handTouchAnim = m_handTouch.addSpriteAnimation("PerryHandAnim", 0.1f, "PerryHand001.png", "PerryHand002.png", "PerryHand003.png", "PerryHand004.png", "PerryHand005.png", "PerryHand006.png");
		m_handTouch.positionFromCenter(0f, 0f);
	}

	private void InitTutText()
	{
		m_instructionText = GlobalGUIManager.The.defaultText.addTextInstance("Default Instruction Text!", 0f, 0f, 1f, m_tutorialDepth);
		m_instructionText.alignMode = UITextAlignMode.Center;
		m_instructionText.textScale = 0.6f;
		m_instructionText.positionFromTop(0.2f);
	}

	public void HideAll()
	{
		if (m_handTouchAnim.isPlaying)
		{
			m_handTouchAnim.stop();
		}
		m_handTouch.hidden = true;
		m_instructionText.hidden = true;
	}

	private void HideAfterTapOrSwipe()
	{
		if (m_handTouchAnim.isPlaying)
		{
			m_handTouchAnim.stop();
		}
		m_handTouch.hidden = true;
		if (!m_IsInstructionTextTimed)
		{
			m_instructionText.hidden = true;
		}
	}

	public void ShowAllAfterPauseResume()
	{
		if (m_ShouldShowHandTouchUntilCancelled)
		{
			m_handTouch.hidden = false;
		}
		if (m_ShouldLoopSwipeUntilCancelled)
		{
			m_instructionText.hidden = false;
			m_handTouch.hidden = false;
		}
		if (m_IsInstructionTextTimed)
		{
			m_instructionText.hidden = false;
		}
	}

	public void ShowHandTouchAnim(int tapTimes, bool loopUntilCancelled = false)
	{
		m_handTouch.hidden = false;
		m_ShouldShowHandTouchUntilCancelled = loopUntilCancelled;
		InvokeRepeating("UpdateTapAnim", Time.deltaTime, Time.deltaTime);
		if (m_ShouldShowHandTouchUntilCancelled)
		{
			m_handTouch.playSpriteAnimation("PerryHandAnim", 1);
		}
		else
		{
			m_handTouch.playSpriteAnimation("PerryHandAnim", tapTimes);
		}
	}

	public void CancelHandTouchAnim()
	{
		m_ShouldShowHandTouchUntilCancelled = false;
	}

	public void ShowInstructionText(string locKey, float duration, bool shouldAnimate = true)
	{
		m_instructionText.text = UIHelper.LineBreakString(LocalTextManager.GetUIText(locKey), 20);
		m_instructionText.parentUIObject = null;
		m_instructionText.positionFromCenter(0f, 0f);
		m_instructionText.hidden = false;
		if (m_InstructionTextTimer != null)
		{
			m_InstructionTextTimer.Start(duration);
		}
		m_IsInstructionTextTimed = true;
		if (shouldAnimate)
		{
			AnimateInstructionTextZoom();
		}
	}

	public void CacheInstructionText(string locKey)
	{
		m_instructionText.text = UIHelper.LineBreakString(LocalTextManager.GetUIText(locKey), 27);
		m_instructionText.parentUIObject = null;
		m_instructionText.positionFromCenter(0f, 0f);
		m_instructionText.hidden = true;
	}

	public void ShowCachedInstructionText(float duration, bool shouldAnimate = true)
	{
		m_instructionText.hidden = false;
		if (m_InstructionTextTimer != null)
		{
			m_InstructionTextTimer.Start(duration);
		}
		m_IsInstructionTextTimed = true;
		if (shouldAnimate)
		{
			AnimateInstructionTextZoom();
		}
	}

	public void HideInstructionText()
	{
		m_instructionText.hidden = true;
	}

	public void ShowSwipeAnim(int swipeTimes, TutorialSwipeDirection direction, bool loopUntilCancelled = false)
	{
		m_handTouch.hidden = false;
		m_isSwipingAnim = true;
		m_handTouch.positionFromCenter(0f, 0f);
		m_instructionText.parentUIObject = m_handTouch;
		InvokeRepeating("UpdateSwipeAnim", Time.deltaTime, Time.deltaTime);
		switch (direction)
		{
		case TutorialSwipeDirection.LEFT:
			m_instructionText.text = UIHelper.LineBreakString(LocalTextManager.GetUIText("_SWIPE_LEFT_"), 25);
			m_instructionText.positionFromTop(-1f);
			break;
		case TutorialSwipeDirection.RIGHT:
			m_instructionText.text = UIHelper.LineBreakString(LocalTextManager.GetUIText("_SWIPE_RIGHT_"), 25);
			m_instructionText.positionFromTop(-1f);
			break;
		case TutorialSwipeDirection.UP:
			m_instructionText.text = UIHelper.LineBreakString(LocalTextManager.GetUIText("_SWIPE_UP_"), 25);
			m_instructionText.positionFromTop(2f);
			break;
		case TutorialSwipeDirection.DOWN:
			m_instructionText.text = UIHelper.LineBreakString(LocalTextManager.GetUIText("_SWIPE_DOWN_"), 25);
			m_instructionText.positionFromTop(-1f);
			break;
		}
		m_instructionText.parentUIObject = null;
		m_instructionText.hidden = false;
		AnimateText();
		if (loopUntilCancelled)
		{
			swipeTimes = 0;
		}
		m_ShouldLoopSwipeUntilCancelled = loopUntilCancelled;
		AnimateSwipe(swipeTimes, direction);
	}

	public void CancelAnimSwipe()
	{
		m_ShouldLoopSwipeUntilCancelled = false;
	}

	private void GameRestartMenuListener()
	{
		ResetVars();
	}

	private void Update()
	{
		if (m_IsInstructionTextTimed && m_InstructionTextTimer != null && m_InstructionTextTimer.IsFinished())
		{
			m_IsInstructionTextTimed = false;
			HideInstructionText();
		}
	}

	private void UpdateTapAnim()
	{
		if (!m_handTouchAnim.isPlaying)
		{
			if (m_ShouldShowHandTouchUntilCancelled)
			{
				m_handTouch.playSpriteAnimation("PerryHandAnim", 1);
				return;
			}
			CancelInvoke("UpdateTapAnim");
			HideAfterTapOrSwipe();
		}
	}

	private void UpdateSwipeAnim()
	{
		if (!m_isSwipingAnim)
		{
			CancelInvoke("UpdateSwipeAnim");
			HideAfterTapOrSwipe();
		}
	}

	public void AnimateSwipe(int swipeTimes, TutorialSwipeDirection direction)
	{
		InvokeRepeating("UpdateSwipeAnim", Time.deltaTime, Time.deltaTime);
		Vector3 ogPos = new Vector3(m_handTouch.position.x, m_handTouch.position.y, m_handTouch.position.z);
		Vector3 target = new Vector3(1f, 1f, 1f);
		if (direction == TutorialSwipeDirection.LEFT)
		{
			target = new Vector3(0f + (float)Screen.width / 30f, m_handTouch.position.y, m_handTouch.position.z);
		}
		else if (direction == TutorialSwipeDirection.RIGHT)
		{
			target = new Vector3((float)Screen.width - (float)Screen.width / 8f, m_handTouch.position.y, m_handTouch.position.z);
		}
		else if (direction == TutorialSwipeDirection.UP)
		{
			target = new Vector3(m_handTouch.position.x, 0f - (float)Screen.height / 40f, m_handTouch.position.z);
		}
		else if (direction == TutorialSwipeDirection.DOWN)
		{
			target = new Vector3(m_handTouch.position.x, (float)(-Screen.height) + (float)Screen.height / 8f, m_handTouch.position.z);
		}
		UIAnimation uIAnimation = m_handTouch.positionTo(1.2f, target, Easing.Exponential.easeInOut);
		uIAnimation.onComplete = delegate
		{
			if (swipeTimes > 0)
			{
				swipeTimes--;
			}
			if (swipeTimes == 0 && !m_ShouldLoopSwipeUntilCancelled)
			{
				m_isSwipingAnim = false;
			}
			else
			{
				m_handTouch.position = ogPos;
				AnimateSwipe(swipeTimes, direction);
			}
		};
	}

	private void AnimateText()
	{
		AnimateTextZoom();
		AnimateTextRotate(true);
	}

	private void AnimateInstructionTextZoom()
	{
		UIAnimation uIAnimation = m_instructionText.scaleTo(0.3f, new Vector3(1.4f, 1.4f, 1f), Easing.Linear.easeInOut);
		uIAnimation.autoreverse = true;
		uIAnimation.onComplete = delegate
		{
			if (m_IsInstructionTextTimed)
			{
				AnimateInstructionTextZoom();
			}
		};
	}

	private void AnimateTextZoom()
	{
		UIAnimation uIAnimation = m_instructionText.scaleTo(0.6f, new Vector3(1.2f, 1.2f, 1f), Easing.Linear.easeInOut);
		uIAnimation.autoreverse = true;
		uIAnimation.onComplete = delegate
		{
			if (m_handTouchAnim.isPlaying || m_isSwipingAnim)
			{
				AnimateTextZoom();
			}
		};
	}

	private void AnimateTextRotate(bool rotateRight)
	{
		float num = 1.5f;
		if (rotateRight)
		{
			num = 0f - num;
		}
		UIAnimation uIAnimation = m_instructionText.eulerAnglesTo(0.62f, new Vector3(0f, 0f, num), Easing.Linear.easeInOut);
		uIAnimation.autoreverse = true;
		uIAnimation.onComplete = delegate
		{
			if (m_handTouchAnim.isPlaying || m_isSwipingAnim)
			{
				AnimateTextRotate(!rotateRight);
			}
		};
	}
}
