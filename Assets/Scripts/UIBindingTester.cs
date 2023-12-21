using Horang.HorangUnityLibrary.Managers.UI;

public class UIBindingTester
{
	private UIManager uiManager;

	private void Test()
	{
		var b = new OneUIEventRegister().Bind<OneUIEventRegister>(uiManager);
	}
}

public class OneUIEventRegister : BaseUIEventRegister
{
	protected override void BindEvents()
	{
	}
}

public class TwoUIEventRegister : BaseUIEventRegister
{
	protected override void BindEvents()
	{
	}
}