using System;

namespace TrackedRiderUtility
{
	public class BaseCar : Car
	{
		public void Decorate(bool isFront)
		{
			if (isFront)
			{
				frontAxisArray = new []{transform.Find("frontAxis")};
			}

            backAxisArray = new[] {transform.Find("backAxis")};
        }
	}
}
