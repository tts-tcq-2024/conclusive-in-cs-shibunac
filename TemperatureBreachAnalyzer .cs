using System.Collections.Generic;

public partial class TemperatureBreachAnalyzer 
{
    public enum BreachType 
    { 
        NORMAL, 
        TOO_LOW, 
        TOO_HIGH 
    };
  
    public enum CoolingType 
    { 
        PASSIVE_COOLING,
        MED_ACTIVE_COOLING,
        HI_ACTIVE_COOLING
    };
  
    public static (double lowerLimit, double upperLimit) GetCoolingLimits(CoolingType coolingType)
    {
      var coolingLimits = new Dictionary<CoolingType, (double, double)>
      {
        { CoolingType.PASSIVE_COOLING, (0, 35) },
        { CoolingType.HI_ACTIVE_COOLING, (0, 45) },
        { CoolingType.MED_ACTIVE_COOLING, (0, 40) }
      };

      if (coolingLimits.TryGetValue(coolingType, out var limits))
      {
        return limits;
      }
      else
      {
        throw new ArgumentOutOfRangeException(nameof(coolingType), "Invalid cooling type");
      }
    }

    public static BreachType inferBreach(double value, double lowerLimit, double upperLimit)
    {
        if (value < lowerLimit) 
          return BreachType.TOO_LOW;
        if (value > upperLimit) 
          return BreachType.TOO_HIGH;
        return BreachType.NORMAL;
    }

  public static BreachType ClassifyTemperatureBreach(CoolingType coolingType, double temperatureInC)
  {
    var (lowerLimit, upperLimit) = GetCoolingLimits(coolingType);
    return InferBreach(temperatureInC, lowerLimit, upperLimit);
  }
}
