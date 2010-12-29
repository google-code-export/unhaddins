namespace YourPrjDomain.Core
{
	public class Suricato : Animal
	{
		private bool leader;

		public virtual bool Leader
		{
			get { return leader; }
			set { leader = value; }
		} 
		
	}
}
