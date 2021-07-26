
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SecondsController : UdonSharpBehaviour
{
    [Header("Internal. Do not modify")]
    public Animator Top;
    public Animator Bottom;
    
    [FieldChangeCallback(nameof(NumberChanged))]
    private int Number;

    private int NumberChanged
    {
        get => Number;
        set
        {
            Number = value;
            UpdateDisplay();
        }
    }
    
    private int On;

    private void Start()
    {
        On = Animator.StringToHash("On");
    }

    private void UpdateDisplay()
    {
        var value = Number % 2 == 0;
        Top.SetBool(On, value);
        Bottom.SetBool(On, value);
    }
}
