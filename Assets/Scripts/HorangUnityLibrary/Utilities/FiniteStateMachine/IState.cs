using Horang.HorangUnityLibrary.Foundation.Factory;

namespace Horang.HorangUnityLibrary.Utilities.FiniteStateMachine
{
	public interface IState : IFactory<IState>
	{
		public string Name { get; }

		public void Enter();
		public void Update();
		public void Exit();
	}
}