
using NUnit.Framework;

public class TypeWiseAlertTests
{
  public enum BreachType
  {
    TOO_LOW,
    TOO_HIGH,
    NORMAL
  }

  public enum CoolingType
  {
    PASSIVE_COOLING,
    HI_ACTIVE_COOLING
  }

  public class BatteryParameters
  {
    public CoolingType CoolingType { get; set; }
  }

  public static class TypeWiseAlert
  {
    // Mocked method to classify temperature breach
    public static BreachType ClassifyTemperatureBreach(CoolingType coolingType, double temperatureInC)
    {
      // Simplified logic for example
      if (temperatureInC < 0)
        return BreachType.TOO_LOW;
      if (temperatureInC > (coolingType == CoolingType.PASSIVE_COOLING ? 35 : 45))
        return BreachType.TOO_HIGH;
      return BreachType.NORMAL;
    }

    public static void CheckAndAlert(string recipient, BatteryParameters batteryParams, double temperature)
    {
      BreachType breach = ClassifyTemperatureBreach(batteryParams.CoolingType, temperature);
      // Alert logic here (not implemented for this example)
    }
  }

  public class TypeWiseAlertTestsSuite
  {
    private Func<CoolingType, double, BreachType> _classifyTemperatureBreachFunc;

    [SetUp]
    public void Setup()
    {
      _classifyTemperatureBreachFunc = TypeWiseAlert.ClassifyTemperatureBreach;
    }

    [Test]
    public void CheckAndAlert_ToController_LowBreach()
    {
      var temperature = -10;
      var expectedBreach = BreachType.TOO_LOW;
      var batteryParams = new BatteryParameters { CoolingType = CoolingType.PASSIVE_COOLING };

      TypeWiseAlert.CheckAndAlert("TO_CONTROLLER", batteryParams, temperature);
      Assert.Equals(expectedBreach, _classifyTemperatureBreachFunc(batteryParams.CoolingType, temperature));
    }

    [Test]
    public void CheckAndAlert_ToController_NormalBreach()
    {
      var temperature = 5;
      var expectedBreach = BreachType.NORMAL;
      var batteryParams = new BatteryParameters { CoolingType = CoolingType.PASSIVE_COOLING };

      TypeWiseAlert.CheckAndAlert("TO_CONTROLLER", batteryParams, temperature);
      Assert.Equals(expectedBreach, _classifyTemperatureBreachFunc(batteryParams.CoolingType, temperature));
    }

    [Test]
    public void CheckAndAlert_ToController_HighBreach()
    {
      var temperature = 50;
      var expectedBreach = BreachType.TOO_HIGH;
      var batteryParams = new BatteryParameters { CoolingType = CoolingType.PASSIVE_COOLING };

      TypeWiseAlert.CheckAndAlert("TO_CONTROLLER", batteryParams, temperature);
      Assert.Equals(expectedBreach, _classifyTemperatureBreachFunc(batteryParams.CoolingType, temperature));
    }

    [Test]
    public void CheckAndAlert_ToEmail_LowBreach()
    {
      var temperature = -2;
      var expectedBreach = BreachType.TOO_LOW;
      var batteryParams = new BatteryParameters { CoolingType = CoolingType.HI_ACTIVE_COOLING };

      TypeWiseAlert.CheckAndAlert("TO_EMAIL", batteryParams, temperature);
      Assert.Equals(expectedBreach, _classifyTemperatureBreachFunc(batteryParams.CoolingType, temperature));
    }

    [Test]
    public void CheckAndAlert_ToEmail_NormalBreach()
    {
      var temperature = 5;
      var expectedBreach = BreachType.NORMAL;
      var batteryParams = new BatteryParameters { CoolingType = CoolingType.PASSIVE_COOLING };

      TypeWiseAlert.CheckAndAlert("TO_EMAIL", batteryParams, temperature);
      Assert.Equals(expectedBreach, _classifyTemperatureBreachFunc(batteryParams.CoolingType, temperature));
    }

    [Test]
    public void CheckAndAlert_ToEmail_HighBreach()
    {
      var temperature = 50;
      var expectedBreach = BreachType.TOO_HIGH;
      var batteryParams = new BatteryParameters { CoolingType = CoolingType.HI_ACTIVE_COOLING };

      TypeWiseAlert.CheckAndAlert("TO_EMAIL", batteryParams, temperature);
      Assert.Equals(expectedBreach, _classifyTemperatureBreachFunc(batteryParams.CoolingType, temperature));
    }
  }
}
