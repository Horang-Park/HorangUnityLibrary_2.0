using Horang.HorangUnityLibrary.Utilities;

namespace Horang.HorangUnityLibrary.Managers.UI
{
	public abstract class BaseUIEventRegister
	{
		protected UIManager uiManager;

		private bool bind;
		
		protected abstract void BindEvents();

		public T Bind<T>(UIManager instance) where T : BaseUIEventRegister
		{
			if (bind)
			{
				Log.Print($"[{typeof(T)}] is already bound.", LogPriority.Warning);

				return this as T;
			}
			
			uiManager = instance;
			
			BindEvents();

			bind = true;
		
			return this as T;
		}
	}
}