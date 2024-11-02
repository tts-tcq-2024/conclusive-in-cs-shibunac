public class TypewiseAlert
{
  public enum BreachType
  {
    NORMAL,
    TOO_LOW,
    TOO_HIGH
  };

  public static BreachType InferBreach(double value, double lowerLimit, double upperLimit)
  {
    if (value < lowerLimit)
      return BreachType.TOO_LOW;
    if (value > upperLimit)
      return BreachType.TOO_HIGH;
    return BreachType.NORMAL;
  }

  public enum CoolingType
  {
    PASSIVE_COOLING,
    HI_ACTIVE_COOLING,
    MED_ACTIVE_COOLING
  };

  public static (double lowerLimit, double upperLimit) GetCoolingLimits(CoolingType coolingType)
  {
    return coolingType switch
    {
      CoolingType.PASSIVE_COOLING => (0, 35),
      CoolingType.HI_ACTIVE_COOLING => (0, 45),
      CoolingType.MED_ACTIVE_COOLING => (0, 40),
      _ => throw new ArgumentOutOfRangeException(nameof(coolingType), "Invalid cooling type")
    };
  }

  private static BreachType ClassifyTemperatureBreach(CoolingType coolingType, double temperatureInC)
  {
    var (lowerLimit, upperLimit) = GetCoolingLimits(coolingType);
    return InferBreach(temperatureInC, lowerLimit, upperLimit);
  }

  public enum AlertTarget
  {
    TO_CONTROLLER,
    TO_EMAIL
  };

  public struct BatteryCharacter
  {
    public CoolingType coolingType;
    public string brand;
  }

  private static void CheckAndAlert(AlertTarget alertTarget, BatteryCharacter batteryChar, double temperatureInC)
  {
    BreachType breachType = ClassifyTemperatureBreach(batteryChar.coolingType, temperatureInC);
    NotifyAlert(alertTarget, breachType);
  }

  private static void NotifyAlert(AlertTarget alertTarget, BreachType breachType)
  {
    switch (alertTarget)
    {
      case AlertTarget.TO_CONTROLLER:
        SendToController(breachType);
        break;
      case AlertTarget.TO_EMAIL:
        SendToEmail(breachType);
        break;
    }
  }

  private static void SendToController(BreachType breachType)
  {
    const ushort header = 0xfeed;
    Console.WriteLine($"{header} : {breachType}\n");
  }

  private static void SendToEmail(BreachType breachType)
  {
    string recipient = "a.b@c.com";
    if (breachType == BreachType.TOO_LOW)
    {
      Console.WriteLine($"To: {recipient}\nHi, the temperature is too low\n");
    }
    else if (breachType == BreachType.TOO_HIGH)
    {
      Console.WriteLine($"To: {recipient}\nHi, the temperature is too high\n");
    }
  }
}
