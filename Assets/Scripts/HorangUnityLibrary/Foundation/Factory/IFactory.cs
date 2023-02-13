namespace Horang.HorangUnityLibrary.Foundation.Factory
{
	public interface IFactory<out T>
	{
		public T Create();
	}
}