using UnityEngine;

public class BaseAdProvider : MonoBehaviour
{
	protected int m_probability = 50;

	protected bool m_enabled;

	public string m_id;

	protected string PATH = "Game/Ads";

	public int probability
	{
		get
		{
			return m_probability;
		}
	}

	public virtual bool ShowAd()
	{
		return false;
	}

	public virtual bool ShowAd(string id)
	{
		return false;
	}

	public virtual bool HasAd()
	{
		return false;
	}

	protected virtual void FromXML()
	{
	}
}
