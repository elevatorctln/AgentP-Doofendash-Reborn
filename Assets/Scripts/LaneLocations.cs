public class LaneLocations
{
	public static int None = 0;

	public static int L = 1;

	public static int M = 2;

	public static int R = 4;

	public static int LMR = L | M | R;

	public static int LM = L | M;

	public static int LR = L | R;

	public static int MR = M | R;

	public static int ToLaneLocations(Waypoint.Lane lane)
	{
		int num = 0;
		if (lane == Waypoint.Lane.Left)
		{
			num |= L;
		}
		if (lane == Waypoint.Lane.Middle)
		{
			num |= M;
		}
		if (lane == Waypoint.Lane.Right)
		{
			num |= R;
		}
		return num;
	}

	public static int ToLaneLocations(int lane)
	{
		switch (lane)
		{
		case -1:
			return L;
		case 0:
			return M;
		case 1:
			return R;
		default:
			return None;
		}
	}

	public static int ToLaneLocations(ObstacleSelector.LaneType laneType)
	{
		switch (laneType)
		{
		case ObstacleSelector.LaneType.ThreeLane:
			return LMR;
		case ObstacleSelector.LaneType.LM:
			return LM;
		case ObstacleSelector.LaneType.MR:
			return MR;
		case ObstacleSelector.LaneType.LR:
			return LR;
		case ObstacleSelector.LaneType.L:
			return L;
		case ObstacleSelector.LaneType.M:
			return M;
		case ObstacleSelector.LaneType.R:
			return R;
		default:
			return 0;
		}
	}
}
