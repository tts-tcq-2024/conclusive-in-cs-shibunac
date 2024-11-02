using System;
using System.IO;
using Xunit;

public class TypewiseAlertTests
{
    [Fact]
    public void TestInferBreach_Normal()
    {
        var result = TypewiseAlert.InferBreach(25, 0, 35);
        Assert.Equal(TypewiseAlert.BreachType.NORMAL, result);
    }

    [Fact]
    public void TestInferBreach_TooLow()
    {
        var result = TypewiseAlert.InferBreach(-5, 0, 35);
        Assert.Equal(TypewiseAlert.BreachType.TOO_LOW, result);
    }

    [Fact]
    public void TestInferBreach_TooHigh()
    {
        var result = TypewiseAlert.InferBreach(50, 0, 35);
        Assert.Equal(TypewiseAlert.BreachType.TOO_HIGH, result);
    }

    [Fact]
    public void TestGetCoolingLimits_PassiveCooling()
    {
        var (lower, upper) = TypewiseAlert.GetCoolingLimits(TypewiseAlert.CoolingType.PASSIVE_COOLING);
        Assert.Equal(0, lower);
        Assert.Equal(35, upper);
    }

    [Fact]
    public void TestGetCoolingLimits_HighActiveCooling()
    {
        var (lower, upper) = TypewiseAlert.GetCoolingLimits(TypewiseAlert.CoolingType.HI_ACTIVE_COOLING);
        Assert.Equal(0, lower);
        Assert.Equal(45, upper);
    }

    [Fact]
    public void TestGetCoolingLimits_MedActiveCooling()
    {
        var (lower, upper) = TypewiseAlert.GetCoolingLimits(TypewiseAlert.CoolingType.MED_ACTIVE_COOLING);
        Assert.Equal(0, lower);
        Assert.Equal(40, upper);
    }

    [Fact]
    public void TestClassifyTemperatureBreach_Normal()
    {
        var breachType = TypewiseAlert.ClassifyTemperatureBreach(TypewiseAlert.CoolingType.PASSIVE_COOLING, 25);
        Assert.Equal(TypewiseAlert.BreachType.NORMAL, breachType);
    }

    [Fact]
    public void TestClassifyTemperatureBreach_TooLow()
    {
        var breachType = TypewiseAlert.ClassifyTemperatureBreach(TypewiseAlert.CoolingType.HI_ACTIVE_COOLING, -1);
        Assert.Equal(TypewiseAlert.BreachType.TOO_LOW, breachType);
    }

    [Fact]
    public void TestClassifyTemperatureBreach_TooHigh()
    {
        var breachType = TypewiseAlert.ClassifyTemp

