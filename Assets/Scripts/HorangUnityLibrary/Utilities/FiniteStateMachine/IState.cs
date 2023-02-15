namespace Horang.HorangUnityLibrary.Utilities.FiniteStateMachine
{
	internal interface IState
	{
		public string Name { get; }

		public void Enter();
		public void Update();
		public void Exit();
	}
}