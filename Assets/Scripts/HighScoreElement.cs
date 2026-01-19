using UnityEngine;

public class HighScoreElement
{
	public UITouchableSprite m_btn;

	public UIButton m_fbBtn;

	public UISprite m_face;

	public UITextInstance m_number;

	public UITextInstance m_name;

	public UITextInstance m_score;

	public bool isSet;

	public bool isFbButton;

	public FaceTexture m_faceTexture;

	public void AddFaceTexture(string url)
	{
		if (!(GlobalGUIManager.The.faceTexture == null))
		{
			m_faceTexture = Object.Instantiate(GlobalGUIManager.The.faceTexture, Vector3.zero, Quaternion.Euler(new Vector3(90f, 180f, 0f))) as FaceTexture;
			m_faceTexture.parent = m_btn;
			m_faceTexture.LoadTexture(url);
			float num = m_btn.height / 18f;
			m_faceTexture.transform.localScale = Vector3.one * num;
		}
	}
}
