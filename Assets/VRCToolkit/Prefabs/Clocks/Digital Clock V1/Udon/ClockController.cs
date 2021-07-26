using System;
using UdonSharp;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class ClockController : UdonSharpBehaviour
{
    [Header("Settings")]
    public bool TwentyFourHour = true;
    
    [Header("Internal. Do not modify")]
    public TensAndUnitsController HoursController;
    public TensAndUnitsController MinutesController;
    public SecondsController SecondsController;

    private void Update()
    {
        var dateTime = DateTime.Now;
        HoursController.SetProgramVariable("Number", TwentyFourHour ? dateTime.Hour : dateTime.Hour % 12);
        MinutesController.SetProgramVariable("Number", dateTime.Minute);
        SecondsController.SetProgramVariable("Number", dateTime.Second);
    }
}
