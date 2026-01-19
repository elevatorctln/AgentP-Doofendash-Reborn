public class UISpacer : UISprite
{
	public override bool hidden
	{
		get
		{
			return ___hidden;
		}
		set
		{
		}
	}

	public UISpacer(int width, int height)
	{
		manager = UI.firstToolkit;
		_width = width;
		_height = height;
	}

	public override void updateTransform()
	{
	}
}
