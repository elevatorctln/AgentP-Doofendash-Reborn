public class UIBackgroundLayout : UIAbstractContainer
{
	public UISprite background;

	public UIBackgroundLayout(string filename)
		: this(UI.firstToolkit, filename)
	{
	}

	public UIBackgroundLayout(UIToolkit manager, string filename)
		: base(UILayoutType.BackgroundLayout)
	{
		background = manager.addSprite(filename, 0, 0, 2);
		addChild(background);
		_width = background.width;
		_height = background.height;
	}
}
