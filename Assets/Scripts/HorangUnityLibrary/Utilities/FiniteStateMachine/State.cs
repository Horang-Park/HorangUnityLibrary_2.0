namespace Horang.HorangUnityLibrary.Utilities.FiniteStateMachine
{
	public abstract class State
	{
		public string Name { get; }

		protected State(string name)
		{
			Name = name;
		}

		public abstract void Enter();
		public abstract void Update();
		public abstract void Exit();
	}
}