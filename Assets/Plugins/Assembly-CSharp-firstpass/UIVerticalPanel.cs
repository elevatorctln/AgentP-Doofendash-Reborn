using UnityEngine;

public class UIVerticalPanel : UIAbstractContainer
{
	private UISprite _topStrip;

	private UISprite _middleStrip;

	private UISprite _bottomStrip;

	private int _topStripHeight;

	private int _bottomStripHeight;

	protected bool _sizeToFit = true;

	public bool sizeToFit
	{
		get
		{
			return _sizeToFit;
		}
		set
		{
			_sizeToFit = value;
			layoutChildren();
		}
	}

	public override Vector3 position
	{
		get
		{
			return clientTransform.position;
		}
		set
		{
			base.position = value;
			layoutChildren();
		}
	}

	public override bool hidden
	{
		set
		{
			if (value != hidden)
			{
				base.hidden = value;
				UISprite topStrip = _topStrip;
				bool flag = value;
				_bottomStrip.hidden = flag;
				flag = flag;
				_middleStrip.hidden = flag;
				topStrip.hidden = flag;
			}
		}
	}

	public UIVerticalPanel(UIToolkit manager, string topFilename, string middleFilename, string bottomFilename)
		: base(UILayoutType.Vertical)
	{
		UITextureInfo uITextureInfo = manager.textureInfoForFilename(topFilename);
		_topStrip = manager.addSprite(topFilename, 0, 0, 5);
		_topStrip.parentUIObject = this;
		_topStripHeight = (int)uITextureInfo.frame.height;
		_width = uITextureInfo.frame.width;
		_middleStrip = manager.addSprite(middleFilename, 0, _topStripHeight, 5);
		_middleStrip.parentUIObject = this;
		uITextureInfo = manager.textureInfoForFilename(middleFilename);
		_bottomStrip = manager.addSprite(bottomFilename, 0, 0, 5);
		_bottomStrip.parentUIObject = this;
		_bottomStripHeight = (int)uITextureInfo.frame.height;
	}

	public static UIVerticalPanel create(string topFilename, string middleFilename, string bottomFilename)
	{
		return create(UI.firstToolkit, topFilename, middleFilename, bottomFilename);
	}

	public static UIVerticalPanel create(UIToolkit manager, string topFilename, string middleFilename, string bottomFilename)
	{
		return new UIVerticalPanel(manager, topFilename, middleFilename, bottomFilename);
	}

	public new void addChild(params UISprite[] children)
	{
		foreach (UISprite uISprite in children)
		{
			uISprite.zIndex = zIndex + 1f;
		}
		base.addChild(children);
	}

	protected override void layoutChildren()
	{
		base.layoutChildren();
		matchSizeToContentSize();
		int num = _topStripHeight + _bottomStripHeight + 1 + _edgeInsets.top + _edgeInsets.bottom;
		if (_contentHeight > (float)num)
		{
			num = (int)_contentHeight;
		}
		int num2 = num - _topStripHeight - _bottomStripHeight + 3;
		_middleStrip.setSize(_middleStrip.width, num2);
		_bottomStrip.localPosition = new Vector3(_bottomStrip.localPosition.x, 0f - _contentHeight - (float)_bottomStripHeight, _bottomStrip.localPosition.z);
	}
}
