using System;

namespace TrackedRiderUtility
{
	public class BaseCar : Car
	{
		public void Decorate(bool isFront)
		{
			if (isFront)
			{
				this.frontAxis = base.transform.Find("frontAxis");
			}
		}
	}
}
