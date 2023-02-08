using System;
using UniRx;

namespace Horang.HorangUnityLibrary.Utilities.FiniteStateMachine
{
	public struct FiniteStateMachine
	{
		private static State current;
		private static IDisposable updateRunner;

		public FiniteStateMachine(State startState, string name)
		{
			current = startState;

			Log.Print($"Start finite state machine named [{name}] with [{current.Name}] state.", LogPriority.Verbose);
			
			current.Enter();

			updateRunner = Observable
				.EveryUpdate()
				.Subscribe(_ =>
				{
					current.Update();
				});
		}

		public void ChangeState(State state)
		{
			if (state.Name.Equals(current.Name))
			{
				Log.Print("Requested state name is same with current state. Not going to change state.", LogPriority.Warning);

				return;
			}
			
			current.Exit();
			updateRunner.Dispose();
			
			Log.Print($"State changed from [{current.Name}] to [{state.Name}]");

			current = state;
			current.Enter();

			updateRunner = Observable
				.EveryUpdate()
				.Subscribe(_ =>
				{
					current.Update();
				});
		}
	}
}