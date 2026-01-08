using UnityEngine.InputSystem;

public static class Inputs
{
    public static bool GetKeyDown(Key key) => Keyboard.current[key].wasPressedThisFrame;
    public static bool GetKey(Key key) => Keyboard.current[key].isPressed;
    public static bool GetKeyUp(Key key) => Keyboard.current[key].wasReleasedThisFrame;
    
    public static bool anyKeyDown => Keyboard.current.anyKey.wasPressedThisFrame;
    private static bool rightPressed => Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed;
    private static bool leftPressed => Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed;
    private static bool upPressed => Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed;
    private static bool downPressed => Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed;
    
    public static float GetAxis(string axis)
    {
        return axis.ToLower() switch
        {
            "horizontal" => rightPressed.CompareTo(leftPressed),
            "vertical" => upPressed.CompareTo(downPressed),
            _ => 0f
        };
    }
}