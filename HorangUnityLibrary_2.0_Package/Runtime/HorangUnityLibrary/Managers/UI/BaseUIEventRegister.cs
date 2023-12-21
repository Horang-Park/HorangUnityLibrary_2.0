namespace Horang.HorangUnityLibrary.Managers.UI
{
	public abstract class BaseUIEventRegister
	{
		protected UIManager uiManager;
		
		protected abstract void BindEvents();

		public T Bind<T>(UIManager instance) where T : BaseUIEventRegister
		{
			uiManager = instance;
			
			BindEvents();
		
			return this as T;
		}
	}
}