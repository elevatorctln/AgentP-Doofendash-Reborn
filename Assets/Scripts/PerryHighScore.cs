public class PerryHighScore
{
	private enum SocialNetwork
	{
		IOS = 0,
		GOOGLE = 1,
		AMAZON = 2,
		FACEBOOK = 3,
		XBLA = 4
	}

	private string m_faceUrl = string.Empty;

	private string m_fbName;

	private string m_fbId;

	private int m_fbScore;

	private bool m_isMe;

	private SocialNetwork m_social;

	public bool IsMe
	{
		get
		{
			return m_isMe;
		}
		set
		{
			m_isMe = value;
		}
	}

	public long ScoreVal
	{
		get
		{
			if (m_social == SocialNetwork.FACEBOOK)
			{
				return 0;
			}
			return 0;
		}
	}

	public string FormattedValue
	{
		get
		{
			return "service removed";
		}
	}

	public string PlayerID
	{
		get
		{	
			return "service removed";
		}
	}

	public int Rank
	{
		get
		{
			return 0;
		}
	}

	public bool IsFriend
	{
		get
		{
			return false;
		}
	}

	public string Name
	{
		get
		{
			return "service removed";
		}
	}

	public string FaceUrl
	{
		get
		{
			return m_faceUrl;
		}
		set
		{
			m_faceUrl = value;
		}
	}

	public PerryHighScore(GPGScore gpgs)
	{
		
	}

	public void SetFacebook(string faceUrl, string id, int score, string name)
	{
		m_social = SocialNetwork.FACEBOOK;
		m_faceUrl = faceUrl;
		m_fbScore = score;
		m_fbName = name;
		m_fbId = id;
	}

	public override string ToString()
	{
		return "service removed";
	}
}
