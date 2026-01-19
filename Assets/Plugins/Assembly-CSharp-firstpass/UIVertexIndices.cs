public struct UIVertexIndices
{
	public UIVertexIndex mv;

	public UIVertexIndex uv;

	public UIVertexIndex cv;

	public void initializeVertsWithIndex(int i)
	{
		mv.one = i * 4;
		mv.two = i * 4 + 1;
		mv.three = i * 4 + 2;
		mv.four = i * 4 + 3;
		uv.one = i * 4;
		uv.two = i * 4 + 1;
		uv.three = i * 4 + 2;
		uv.four = i * 4 + 3;
		cv.one = i * 4;
		cv.two = i * 4 + 1;
		cv.three = i * 4 + 2;
		cv.four = i * 4 + 3;
	}
}
