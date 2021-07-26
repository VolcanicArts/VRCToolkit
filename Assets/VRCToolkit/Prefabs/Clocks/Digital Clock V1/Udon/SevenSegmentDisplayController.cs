using UdonSharp;
using UnityEngine;

public class SevenSegmentDisplayController : UdonSharpBehaviour
{
    
    [Header("Internal. Do not modify")]
    public Animator Center;
    public Animator Top;
    public Animator TopLeft;
    public Animator TopRight;
    public Animator Bottom;
    public Animator BottomLeft;
    public Animator BottomRight;
    
    private int Number;

    private readonly bool[][] Segments =
    {
        new[] {false, true, true, true, true, true, true},
        new[] {false, false, false, true, false, false, true},
        new[] {true, true, false, true, true, true, false},
        new[] {true, true, false, true, true, false, true},
        new[] {true, false, true, true, false, false, true},
        new[] {true, true, true, false, true, false, true},
        new[] {true, true, true, false, true, true, true},
        new[] {false, true, false, true, false, false, true},
        new[] {true, true, true, true, true, true, true},
        new[] {true, true, true, true, true, false, true}
    };

    private int On;

    private void Start()
    {
        On = Animator.StringToHash("On");
    }

    private void Update()
    {
        var stateArray = Segments[Number];
        Center.SetBool(On, stateArray[0]);
        Top.SetBool(On, stateArray[1]);
        TopLeft.SetBool(On, stateArray[2]);
        TopRight.SetBool(On, stateArray[3]);
        Bottom.SetBool(On, stateArray[4]);
        BottomLeft.SetBool(On, stateArray[5]);
        BottomRight.SetBool(On, stateArray[6]);
    }
}