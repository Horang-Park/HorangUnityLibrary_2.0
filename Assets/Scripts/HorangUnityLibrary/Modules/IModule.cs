namespace HorangUnityLibrary.Modules
{
	public interface IModule
	{
		/// <summary>
		/// Bind module to module manager by action subscriber. (If not call this method, the module is not working.)
		/// </summary>
		public void UseModule();

		/// <summary>
		/// Initialize only once when application turn on. (Working like Unity's Awake method.)
		/// </summary>
		public void InitializeOnce();

		/// <summary>
		/// Initialize only once when other initialize completed. (Working like Unity's Start method.)
		/// </summary>
		public void InitializeLate();
		
		/// <summary>
		/// Frame update.
		/// </summary>
		public void Update();
		
		/// <summary>
		/// Fixed frame update.
		/// </summary>
		public void FixedUpdate();
		
		/// <summary>
		/// Late update.
		/// </summary>
		public void LateUpdate();
	}
}