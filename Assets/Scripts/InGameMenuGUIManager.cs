using System;
using UnityEngine;

public class InGameMenuGUIManager : MonoBehaviour
{
	private static UITextInstance m_scoreLabel;

	private static UISprite m_multiplierCircle;

	private static UITextInstance m_scoreMultiplier;

	private static UITextInstance m_finalScore;

	private static UISprite m_tokenSprite;

	private static UITextInstance m_tokenMultiplier;

	private static UITextInstance m_tokenScore;

	private static UITextInstance m_totalMeters;

	private static int scoreUIStartDepth = 6;

	private static InGameMenuGUIManager m_the;

	public static InGameMenuGUIManager The
	{
		get
		{
			if (m_the == null)
			{
				GameObject gameObject = new GameObject("InGameMenuGUIManager");
				((InGameMenuGUIManager)gameObject.AddComponent<InGameMenuGUIManager>()).Init();
			}
			return m_the;
		}
	}

	public void Init()
	{
		if (!(m_the != null))
		{
			m_the = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			InitScoreElements();
			HideAll();
		}
	}

	private void InitScoreElements()
	{
		m_scoreLabel = GlobalGUIManager.The.defaultTextAlt.addTextInstance(LocalTextManager.GetUIText("_IN_GAME_MENU_SCORE_"), 0f, 0f, 1f, scoreUIStartDepth);
		m_scoreLabel.alignMode = UITextAlignMode.Left;
		m_scoreLabel.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_scoreLabel.textScale = 0.7f;
		Color colorForAllLetters = new Color32(byte.MaxValue, 128, 0, byte.MaxValue);
		m_scoreLabel.setColorForAllLetters(colorForAllLetters);
		m_scoreLabel.text = LocalTextManager.GetUIText("_IN_GAME_MENU_SCORE_");
		if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{
			m_scoreLabel.textScale = 0.6f;
		}
		m_multiplierCircle = GlobalGUIManager.The.m_menuToolkit.addSprite("ScoreBonusCounter.png", 0, 0, scoreUIStartDepth + 1, true);
		m_multiplierCircle.parentUIObject = m_scoreLabel;
		m_multiplierCircle.positionFromTopLeft(-2.2f, 0.9f);
		if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{
			m_multiplierCircle.parentUIObject = m_finalScore;
			m_multiplierCircle.positionFromRight(-0.2f, 0.51f);
		}
		string text = AllMissionData.TotalScoreMultiplier.ToString("N0");
		m_scoreMultiplier = GlobalGUIManager.The.defaultTextAlt.addTextInstance("x" + text, 0f, 0f, 1f, 1);
		m_scoreMultiplier.alignMode = UITextAlignMode.Left;
		m_scoreMultiplier.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_scoreMultiplier.textScale = 0.45f;
		m_scoreMultiplier.setColorForAllLetters(Color.white);
		m_scoreMultiplier.parentUIObject = m_multiplierCircle;
		m_scoreMultiplier.positionFromCenter(0f, 0.01f);
		m_scoreMultiplier.text = "x" + text;
		m_finalScore = GlobalGUIManager.The.defaultTextAlt.addTextInstance("0", 0f, 0f, 1f, scoreUIStartDepth);
		m_finalScore.alignMode = UITextAlignMode.Left;
		m_finalScore.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_finalScore.textScale = 0.65f;
		m_finalScore.setColorForAllLetters(Color.blue);
		m_finalScore.parentUIObject = m_scoreLabel;
		m_finalScore.positionFromTopLeft(1.25f, 0f);
		m_finalScore.text = "0";
		m_tokenSprite = GlobalGUIManager.The.m_menuToolkit.addSprite("ScoreToken.png", 0, 0, scoreUIStartDepth + 1, true);
		m_tokenSprite.scale = new Vector3(0.75f, 0.75f, 0.75f);
		m_tokenSprite.positionFromTop(0.1f, -0.45f);
		m_tokenSprite.parentUIObject = m_finalScore;
		if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{
			m_tokenSprite.positionFromTop(0.97f, 0.2f);
		}
		string text2 = PlayerData.TotalTokenMultiplier.ToString("N0");
		m_tokenMultiplier = GlobalGUIManager.The.defaultText.addTextInstance("x" + text2, 0f, 0f, 1f, scoreUIStartDepth);
		m_tokenMultiplier.alignMode = UITextAlignMode.Left;
		m_tokenMultiplier.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_tokenMultiplier.textScale = 0.45f;
		m_tokenMultiplier.setColorForAllLetters(Color.red);
		m_tokenMultiplier.parentUIObject = m_tokenSprite;
		m_tokenMultiplier.positionCenter();
		m_tokenMultiplier.eulerAngles = new Vector3(0f, 0f, -8f);
		m_tokenMultiplier.text = "x" + text2;
		m_tokenScore = GlobalGUIManager.The.defaultTextAlt.addTextInstance("0", 0f, 0f, 1f, scoreUIStartDepth);
		m_tokenScore.alignMode = UITextAlignMode.Left;
		m_tokenScore.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_tokenScore.textScale = 0.6f;
		colorForAllLetters = new Color32(38, 168, 38, byte.MaxValue);
		m_tokenScore.setColorForAllLetters(colorForAllLetters);
		m_tokenScore.parentUIObject = m_tokenSprite;
		m_tokenScore.positionFromTopLeft(0.23f, 1.25f);
		m_tokenScore.text = "0";
		m_totalMeters = GlobalGUIManager.The.defaultTextAlt.addTextInstance("m_totalMeters.text", 0f, 0f, 1f, scoreUIStartDepth);
		m_totalMeters.alignMode = UITextAlignMode.Right;
		m_totalMeters.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_totalMeters.textScale = 0.45f;
		m_totalMeters.setColorForAllLetters(Color.black);
		m_totalMeters.parentUIObject = m_scoreLabel;
		m_totalMeters.positionFromTopLeft(-1f, -0.1f);
		if (LocalTextManager.CurrentLanguageType != LocalTextManager.PerryLanguages.English)
		{
			m_totalMeters.positionFromTopLeft(-1.15f, -0.1f);
		}
	}

	public void HideAll()
	{
		HideScoreElements();
	}

	public void HideScoreElements()
	{
		m_scoreLabel.hidden = true;
		m_multiplierCircle.hidden = true;
		m_scoreMultiplier.hidden = true;
		m_finalScore.hidden = true;
		m_tokenSprite.hidden = true;
		m_tokenMultiplier.hidden = true;
		m_tokenScore.hidden = true;
		m_totalMeters.hidden = true;
	}

	public void ShowScoreTokenCountAndMultiplier()
	{
		m_tokenMultiplier.text = "x" + PlayerData.TotalTokenMultiplier.ToString("N0");
		m_tokenScore.text = PlayerData.RoundTokens.ToString("N0");
		m_tokenMultiplier.hidden = false;
		m_tokenScore.hidden = false;
		m_finalScore.text = PlayerData.GetTotalRoundScore().ToString("N0");
		m_finalScore.hidden = false;
	}

	public void ShowScores()
	{
		UpdateScorePositions();
		UpdateScoreElements();
		m_scoreLabel.hidden = false;
		if (AllMissionData.TotalScoreMultiplier > 1)
		{
			m_multiplierCircle.hidden = false;
			m_scoreMultiplier.hidden = true;
		}
		m_finalScore.hidden = false;
		m_tokenSprite.hidden = false;
		m_tokenMultiplier.hidden = true;
		m_tokenScore.hidden = true;
		m_totalMeters.hidden = false;
	}

	private void UpdateScoreElements()
	{
		m_finalScore.text = "0";
		m_scoreMultiplier.text = "x" + AllMissionData.TotalScoreMultiplier.ToString("N0");
		m_tokenMultiplier.text = "x" + PlayerData.TotalTokenMultiplier.ToString("N0");
		m_tokenScore.text = "0";
		m_totalMeters.text = PlayerData.RoundMeters.ToString("N0") + LocalTextManager.GetUIText("_METERS_");
	}

	public void UpdateScorePositions()
	{
		m_scoreLabel.positionFromTopLeft(0.35f, 0.15f);
	}

	public void AnimateScoreText(Action<bool> onDone)
	{
		m_scoreLabel.hidden = false;
		MainMenuEventManager.TriggerStartAnimation();
		UIAnimation uIAnimation = m_scoreLabel.positionFrom(0.5f, new Vector3(-Screen.width, m_scoreLabel.position.y, m_scoreLabel.position.z), Easing.Exponential.easeInOut);
		uIAnimation.onComplete = delegate
		{
			if (AllMissionData.TotalScoreMultiplier > 1)
			{
				m_multiplierCircle.hidden = false;
				m_scoreMultiplier.hidden = false;
				m_scoreMultiplier.scaleFrom(0.5f, new Vector3(5f, 5f, 1f), Easing.Exponential.easeOut);
			}
			m_finalScore.text = "0";
			m_finalScore.hidden = false;
			m_finalScore.alphaFromTo(0.25f, 0f, 1f, Easing.Sinusoidal.easeIn);
			UIHelper.SetTextForWriteNumberAnim(ref m_finalScore);
			StartCoroutine(UIHelper.TextNumberGrowAnimAction(0, PlayerData.GetTotalRoundScore(), delegate
			{
				AnimateTokenText(onDone);
			}));
		};
	}

	private void AnimateTokenText(Action<bool> onDone)
	{
		m_tokenSprite.hidden = false;
		m_tokenMultiplier.hidden = false;
		UIAnimation uIAnimation = m_tokenMultiplier.scaleFrom(0.5f, new Vector3(5f, 5f, 1f), Easing.Exponential.easeOut);
		uIAnimation.onComplete = delegate
		{
			UIHelper.SetTextForWriteNumberAnim(ref m_tokenScore);
			m_tokenScore.hidden = false;
			StartCoroutine(UIHelper.TextNumberGrowAnimAction(0, PlayerData.RoundTokens, delegate
			{
				MainMenuEventManager.TriggerEndAnimation();
				onDone(true);
			}));
		};
	}

	public void MoveScores(float offset, bool bAnimate = false, Action<bool> onDone = null)
	{
		Vector3 position = m_scoreLabel.position;
		Vector3 target = m_scoreLabel.position + new Vector3(offset, 0f, 0f);
		if (bAnimate)
		{
			MainMenuEventManager.TriggerStartAnimation();
			UIAnimation uIAnimation = m_scoreLabel.positionFromTo(1f, position, target, Easing.Sinusoidal.easeInOut);
			uIAnimation.onComplete = delegate
			{
				MainMenuEventManager.TriggerEndAnimation();
				if (onDone != null)
				{
					onDone(true);
				}
			};
		}
		else
		{
			m_scoreLabel.position = new Vector3(target.x, target.y, target.z);
		}
	}

	public void ReloadStaticText()
	{
		m_scoreLabel.text = LocalTextManager.GetUIText("_IN_GAME_MENU_SCORE_");
	}
}
