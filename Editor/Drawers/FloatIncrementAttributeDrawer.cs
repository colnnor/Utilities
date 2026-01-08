using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(FloatIncrementAttribute))]
public class FloatIncrementAttributeDrawer : PropertyDrawer
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
        
        contentContainer.Add(new Button(() => ApplyFloatChange(property, -0.001f)) { text = "<" });
        contentContainer.Add(new Button(() => ApplyFloatChange(property, -0.01f)) { text = "<<" });
        contentContainer.Add(new Button(() => ApplyFloatChange(property, -0.1f)) { text = "<<<" ,style = { left = 0}});

        FloatField floatField = new()
        {
            value = property.floatValue,
            style =
            {
                width = 50,
                marginLeft = 5,
                flexGrow = 1
            }
        };
        floatField.BindProperty(property);
        floatField.RegisterValueChangedCallback(evt =>
        {
            property.floatValue = evt.newValue;
            property.serializedObject.ApplyModifiedProperties();
        });
        contentContainer.Add(floatField);
         
        contentContainer.Add(new Button(() => ApplyFloatChange(property, 0.1f)) { text = ">>>" });
        contentContainer.Add(new Button(() => ApplyFloatChange(property, 0.01f)) { text = ">>" });
        contentContainer.Add(new Button(() => ApplyFloatChange(property, 0.001f)) { text = ">" });
        

//        root.Add(propertyField);    
        outerContainer.Add(contentContainer);
        root.Add(outerContainer);

        return root;
    }

    private void ApplyFloatChange(SerializedProperty property, float delta)
    {
        property.floatValue += delta;
        property.serializedObject.ApplyModifiedProperties();
    }
}