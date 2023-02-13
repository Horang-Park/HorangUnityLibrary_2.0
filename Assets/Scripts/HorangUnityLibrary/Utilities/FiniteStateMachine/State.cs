namespace Horang.HorangUnityLibrary.Utilities.FiniteStateMachine
{
	public abstract class State : IState
	{
		public string Name { get; }
		
		private State() {}
		protected State(string name)
		{
			Name = name;
		}

		public abstract void Enter();
		public abstract void Update();
		public abstract void Exit();
	}
}