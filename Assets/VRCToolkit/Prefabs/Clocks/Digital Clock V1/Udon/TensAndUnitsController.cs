
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TensAndUnitsController : UdonSharpBehaviour
{
    [Header("Internal. Do not modify")]
    public SevenSegmentDisplayController TensController;
    public SevenSegmentDisplayController UnitsController;
    
    [FieldChangeCallback(nameof(NumberChanged))]
    private int Number;

    private int NumberChanged
    {
        get => Number;
        set
        {
            Number = value;
            TensController.SetProgramVariable("Number", (int) Mathf.Floor((float) Number / 10));
            UnitsController.SetProgramVariable("Number", Number % 10);
        }
    }
}
