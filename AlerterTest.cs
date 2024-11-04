


using System;
using System.IO;
using Xunit;

public class AlerterTest
{
  [Fact]
  public void TestInferBreach_Normal()
  {
    var result = TemperatureBreachAnalyzer.InferBreach(25, 0, 35);
    Assert.Equal(TemperatureBreachAnalyzer.BreachType.NORMAL, result);
  }

  [Fact]
  public void TestInferBreach_TooLow()
  {
    var result = TemperatureBreachAnalyzer.InferBreach(-5, 0, 35);
    Assert.Equal(TemperatureBreachAnalyzer.BreachType.TOO_LOW, result);
  }

  [Fact]
  public void TestInferBreach_TooHigh()
  {
    var result = TemperatureBreachAnalyzer.InferBreach(50, 0, 35);
    Assert.Equal(TemperatureBreachAnalyzer.BreachType.TOO_HIGH, result);
  }

  [Fact]
  public void TestGetCoolingLimits_PassiveCooling()
  {
    var (lower, upper) = TemperatureBreachAnalyzer.GetCoolingLimits(TemperatureBreachAnalyzer.CoolingType.PASSIVE_COOLING);
    Assert.Equal(0, lower);
    Assert.Equal(35, upper);
  }

  [Fact]
  public void TestGetCoolingLimits_HighActiveCooling()
  {
    var (lower, upper) = TemperatureBreachAnalyzer.GetCoolingLimits(TemperatureBreachAnalyzer.CoolingType.HI_ACTIVE_COOLING);
    Assert.Equal(0, lower);
    Assert.Equal(45, upper);
  }

  [Fact]
  public void TestGetCoolingLimits_MedActiveCooling()
  {
    var (lower, upper) = TemperatureBreachAnalyzer.GetCoolingLimits(TemperatureBreachAnalyzer.CoolingType.MED_ACTIVE_COOLING);
    Assert.Equal(0, lower);
    Assert.Equal(40, upper);
  }

  [Fact]
  public void TestClassifyTemperatureBreach_Normal()
  {
    var breachType = TemperatureBreachAnalyzer.ClassifyTemperatureBreach(TemperatureBreachAnalyzer.CoolingType.PASSIVE_COOLING, 25);
    Assert.Equal(TemperatureBreachAnalyzer.BreachType.NORMAL, breachType);
  }

  [Fact]
  public void TestClassifyTemperatureBreach_TooLow()
  {
    var breachType = TemperatureBreachAnalyzer.ClassifyTemperatureBreach(TemperatureBreachAnalyzer.CoolingType.HI_ACTIVE_COOLING, -1);
    Assert.Equal(TemperatureBreachAnalyzer.BreachType.TOO_LOW, breachType);
  }

  [Fact]
  public void TestClassifyTemperatureBreach_TooHigh()
  {
    var breachType = TemperatureBreachAnalyzer.ClassifyTemperatureBreach(TemperatureBreachAnalyzer.CoolingType.MED_ACTIVE_COOLING, 45);
    Assert.Equal(TemperatureBreachAnalyzer.BreachType.TOO_HIGH, breachType);
  }

  [Fact]
  public void TestCheckAndAlert_ToController()
  {
    // Redirect console output for testing
    using var sw = new StringWriter();
    Console.SetOut(sw);

    var batteryChar = new TypewiseAlert.BatteryCharacter { coolingType = TemperatureBreachAnalyzer.CoolingType.PASSIVE_COOLING, brand = "BrandX" };
    TypewiseAlert.CheckAndAlert(TypewiseAlert.AlertTarget.TO_CONTROLLER, batteryChar, 30);

    var output = sw.ToString();
    Assert.Contains("0xfeed : NORMAL", output);
  }

  [Fact]
  public void TestSendToEmail_TooLow()
  {
    // Redirect console output for testing
    using var sw = new StringWriter();
    Console.SetOut(sw);

    TypewiseAlert.SendToEmail(TemperatureBreachAnalyzer.BreachType.TOO_LOW);

    var output = sw.ToString();
    Assert.Contains("Hi, the temperature is too low", output);
  }

  [Fact]
  public void TestSendToEmail_TooHigh()
  {
    // Redirect console output for testing
    using var sw = new StringWriter();
    Console.SetOut(sw);

    TypewiseAlert.SendToEmail(TemperatureBreachAnalyzer.BreachType.TOO_HIGH);

    var output = sw.ToString();
    Assert.Contains("Hi, the temperature is too high", output);
  }
}

