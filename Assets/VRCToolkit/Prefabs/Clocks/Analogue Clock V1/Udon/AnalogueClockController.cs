using System;
using UdonSharp;
using UnityEngine;

public class AnalogueClockController : UdonSharpBehaviour
{
    [Header("Settings")]
    public bool SmoothHands;
    
    [Header("Internal. Do not touch")]
    public Transform HourHand;
    public Transform MinuteHand;
    public Transform SecondHand;

    public void Update()
    {
        var currentTime = DateTime.Now;
        
        var currentSeconds = (float) currentTime.Second;
        if (SmoothHands) currentSeconds += (float) currentTime.Millisecond / 1000;
        var secondRotation = GetSecondHandRotation(currentSeconds);

        var currentMinutes = (float) currentTime.Minute;
        if (SmoothHands) currentMinutes += currentSeconds / 60;
        var minuteRotation = GetMinuteHandRotation(currentMinutes);
        
        var currentHours = (float) currentTime.Hour % 12;
        if (SmoothHands) currentHours += currentMinutes / 60;
        var hourRotation = GetHourHandRotation(currentHours);

        HourHand.SetPositionAndRotation(HourHand.position, Quaternion.Euler(new Vector3(hourRotation, 0, 0)));
        MinuteHand.SetPositionAndRotation(MinuteHand.position, Quaternion.Euler(new Vector3(minuteRotation, 0, 0)));
        SecondHand.SetPositionAndRotation(SecondHand.position, Quaternion.Euler(new Vector3(secondRotation, 0, 0)));
    }

    private float GetHourHandRotation(float hour)
    {
        return hour * 30 * -1 - 90;
    }

    private float GetMinuteHandRotation(float minute)
    {
        return minute * 6 * -1 - 90;
    }

    private float GetSecondHandRotation(float second)
    {
        return second * 6 * -1 - 90;
    }
}
