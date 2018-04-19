using System;

namespace TrackedRiderUtility
{
	public class BaseCar : Car
	{
		public void Decorate(bool isFront)
		{
			if (isFront)
			{
				frontAxis = transform.Find("frontAxis");
			}
			backAxis = transform.Find("backAxis");
		}
	}
}
