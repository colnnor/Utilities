using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MoveComponentTool
{
    const string MOVE_TO_TOP = "CONTEXT/Component/Move To Top";
    const string MOVE_TO_BOTTOM = "CONTEXT/Component/Move To Bottom";

    [MenuItem(MOVE_TO_TOP, priority = 501)]
    public static void MoveComponentToTopMenuItem(MenuCommand command)
    {
        while (UnityEditorInternal.ComponentUtility.MoveComponentUp((Component)command.context))
        {
        }
    }

    [MenuItem(MOVE_TO_TOP, validate = true)]
    public static bool MoveComponentToTopValidate(MenuCommand command)
    {
        Component[] otherComponents = ((Component)command.context).GetComponents<Component>();
        int index = otherComponents.ToList().FindIndex(c => c == command.context);
        return index != 1;
    }

    [MenuItem(MOVE_TO_BOTTOM, priority = 502)]
    public static void MoveComponentToBottomMenuItem(MenuCommand command)
    {
        while (UnityEditorInternal.ComponentUtility.MoveComponentDown((Component)command.context))
        {
        }
    }
    
    [MenuItem(MOVE_TO_BOTTOM, validate = true)]
    public static bool MoveComponentToBottomValidate(MenuCommand command)
    {
        Component[] otherComponents = ((Component)command.context).GetComponents<Component>();
        int index = otherComponents.ToList().FindIndex(c => c == command.context);
        return index != otherComponents.Length - 1;
    }
    
    
}