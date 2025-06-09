using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(IntIncrementAttribute))]
public class IntIncrementAttributeDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        VisualElement root = new();

        /*
        PropertyField propertyField = new(property){style = { alignSelf = Align.Stretch }};
        propertyField.Bind(property.serializedObject);
        */
        VisualElement outerContainer = new()
        {
            style =
            {
                flexDirection = FlexDirection.Row,
                alignItems = Align.Stretch,
            }
        };

        Label label = new Label(property.displayName)
        {
            style =
            {
                width = EditorGUIUtility.labelWidth,
                unityTextAlign = TextAnchor.MiddleLeft,
                flexShrink = .8f,
                flexGrow = .88f,
                left = 2f
            }
        };
        outerContainer.Add(label);

        VisualElement contentContainer = new()
        {
            style =
            {
                flexDirection = FlexDirection.Row,
                alignItems = Align.Stretch,
                flexGrow = 1
            }
        };

        contentContainer.Add(new Button(() => ApplyIntChange(property, -1)) { text = "-1" });
        contentContainer.Add(new Button(() => ApplyIntChange(property, -10)) { text = "-10" });
        contentContainer.Add(new Button(() => ApplyIntChange(property, -100)) { text = "-100", style = { left = 0 } });

        IntegerField intField = new()
        {
            value = property.intValue,
            style =
            {
                width = 50,
                marginLeft = 5,
                flexGrow = 1
            }
        };
        intField.BindProperty(property);
        intField.RegisterValueChangedCallback(evt =>
        {
            property.intValue = evt.newValue;
            property.serializedObject.ApplyModifiedProperties();
            intField.value = property.intValue;
        });
        contentContainer.Add(intField);

        contentContainer.Add(new Button(() => ApplyIntChange(property, 100)) { text = "100" });
        contentContainer.Add(new Button(() => ApplyIntChange(property, 10)) { text = "10" });
        contentContainer.Add(new Button(() => ApplyIntChange(property, 1)) { text = "1" });

        outerContainer.Add(contentContainer);
        root.Add(outerContainer);
        
        return root;
    }
    
    void ApplyIntChange(SerializedProperty property, int delta)
    {
        property.intValue += delta;
        property.serializedObject.ApplyModifiedProperties();
    }
}