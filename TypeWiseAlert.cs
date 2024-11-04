

using static TemperatureBreachAnalyzer;

public class TypewiseAlert
{

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

  public static void CheckAndAlert(AlertTarget alertTarget, BatteryCharacter batteryChar, double temperatureInC)
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

  public static void SendToController(BreachType breachType)
  {
    const ushort header = 0xfeed;
    Console.WriteLine($"{header} : {breachType}\n");
  }

  public static void SendToEmail(BreachType breachType)
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


