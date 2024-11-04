using Xunit;
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
        public static BreachType ClassifyTemperatureBreach(CoolingType coolingType, double temperatureInC)
        {
            if (temperatureInC < GetLowerLimit(coolingType))
                return BreachType.TOO_LOW;
            if (temperatureInC > GetUpperLimit(coolingType))
                return BreachType.TOO_HIGH;
            return BreachType.NORMAL;
        }

        private static double GetLowerLimit(CoolingType coolingType) => 0;
        private static double GetUpperLimit(CoolingType coolingType) =>
            coolingType == CoolingType.PASSIVE_COOLING ? 35 : 45;

        public static void CheckAndAlert(string recipient, BatteryParameters batteryParams, double temperature)
        {
            var breachType = ClassifyTemperatureBreach(batteryParams.CoolingType, temperature);
            Alert(recipient, breachType);
        }

        private static void Alert(string recipient, BreachType breachType)
        {
            // Alert logic based on recipient and breachType
            // This method can be expanded with actual alerting logic
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
            AssertBreachType("TO_CONTROLLER", CoolingType.PASSIVE_COOLING, -10, BreachType.TOO_LOW);
        }

        [Test]
        public void CheckAndAlert_ToController_NormalBreach()
        {
            AssertBreachType("TO_CONTROLLER", CoolingType.PASSIVE_COOLING, 5, BreachType.NORMAL);
        }

        [Test]
        public void CheckAndAlert_ToController_HighBreach()
        {
            AssertBreachType("TO_CONTROLLER", CoolingType.PASSIVE_COOLING, 50, BreachType.TOO_HIGH);
        }

        [Test]
        public void CheckAndAlert_ToEmail_LowBreach()
        {
            AssertBreachType("TO_EMAIL", CoolingType.HI_ACTIVE_COOLING, -2, BreachType.TOO_LOW);
        }

        [Test]
        public void CheckAndAlert_ToEmail_NormalBreach()
        {
            AssertBreachType("TO_EMAIL", CoolingType.PASSIVE_COOLING, 5, BreachType.NORMAL);
        }

        [Test]
        public void CheckAndAlert_ToEmail_HighBreach()
        {
            AssertBreachType("TO_EMAIL", CoolingType.HI_ACTIVE_COOLING, 50, BreachType.TOO_HIGH);
        }

        private void AssertBreachType(string recipient, CoolingType coolingType, double temperature, BreachType expectedBreach)
        {
            var batteryParams = new BatteryParameters { CoolingType = coolingType };
            TypeWiseAlert.CheckAndAlert(recipient, batteryParams, temperature);
            Assert.AreEqual(expectedBreach, _classifyTemperatureBreachFunc(coolingType, temperature));
        }
    }
}
