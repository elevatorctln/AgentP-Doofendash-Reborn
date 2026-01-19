using System;
using System.Collections.Generic;
using UnityEngine;

public class CopaGUIManager : MonoBehaviour
{
	private enum COPAFieldType
	{
		none = 0,
		day = 1,
		month = 2,
		year = 3
	}

	private static float BUTTON_PERCENTAGE_FROM_TOP = 0.15f;

	private DateTime m_DateOfBirth;

	private COPAFieldType m_currentInput;

	private UISprite m_COPAFrame;

	private UITextInstance m_InstructionsLabel;

	private UIButton m_MonthButton;

	private UIButton m_DayButton;

	private UIButton m_YearButton;

	private UITextInstance m_Month;

	private UITextInstance m_Day;

	private UITextInstance m_Year;

	private UITextInstance m_MonthLabel;

	private UITextInstance m_DayLabel;

	private UITextInstance m_YearLabel;

	private UIButton m_OkButton;

	private TouchScreenKeyboard m_Keyboard;

	public void DrawCopaScreen()
	{
		
	}

	private void Update()
	{
		
	}

}
