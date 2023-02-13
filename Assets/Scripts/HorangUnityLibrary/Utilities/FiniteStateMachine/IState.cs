namespace Horang.HorangUnityLibrary.Utilities.FiniteStateMachine
{
	public interface IState
	{
		public string Name { get; set; }

		public void Initialize();
		public void Enter();
		public void Update();
		public void Exit();
	}
}