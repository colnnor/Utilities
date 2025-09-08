using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Cinemachine;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

[Overlay(typeof(SceneView), "Cinemachine Camera Switcher")]
public class CinemachineCameraSwitcher : Overlay
{
    private List<CinemachineCamera> cameras = new();
    private Vector3 cameraEulerAngles;
    private Vector3 cameraEulerAnglesRounded;
    public static CinemachineCamera activeCamera { get; private set; }
    public static bool rotationActive { get; private set; }
    public override VisualElement CreatePanelContent()
    {
        Initialize();
        Undo.undoRedoPerformed += OnUndoRedoPerformed;
        Selection.selectionChanged += OnSelectionChanged;
        
        var container = new VisualElement()
        {
            style =
            {
                flexDirection = FlexDirection.Column,
                paddingTop = 10,
                paddingLeft = 10,
                paddingRight = 10,
                paddingBottom = 10,
            }
        };
        var horizontal = new VisualElement()
        {
            style =
            {
                flexDirection = FlexDirection.Row,
                justifyContent = Justify.SpaceEvenly,
                alignItems = Align.Center,
                marginBottom = 10
            }
        };
        var titleLabel = new Label("Cinemachine Cameras")
        {
            style =
            {
                unityFontStyleAndWeight = FontStyle.Bold,
                fontSize = 12
            }
        };
        
        var refreshButton = new Button(() =>
        {
            Initialize();
            Refresh();
        })
        {
            text = "Refresh",
            tooltip = "Refresh the list of Cinemachine Cameras in the scene",
            style =
            {
                marginBottom = 10,
                marginRight = 5
            }
        };
        
        horizontal.Add(refreshButton);

        var AddCameraButton = new Button(CreateNewCamera)
        {
            text = "Add Cinemachine Camera",
            tooltip = "Adds a new Cinemachine Camera to the scene at the current Scene view position",
            style =
            {
                marginBottom = 10,
                marginRight = 5,
                //stretch to fit
                alignSelf = Align.FlexEnd
            }
        };
        horizontal.Add(AddCameraButton);
        container.Add(horizontal);
        container.Add(titleLabel);
        var separator = new VisualElement()
        {
            style =
            {
                height = 2f,
                backgroundColor = new StyleColor(Color.gray),
                marginTop = 5f,
                marginBottom = 5f,
                width = Length.Percent(100)
            }
        };
        container.Add(separator);
        foreach (var camera in cameras.Where(c => c != null))
        {
            var button = new Button(() =>
            {
                SelectCamera(camera);
            })
            {
                text = camera.name,
                style =
                {
                    marginBottom = 5,
                    unityTextAlign = TextAnchor.MiddleLeft
                }
            };
            container.Add(button);
        }
        
        var camEulers = new Vector3Field()
        {
            value = cameraEulerAnglesRounded,
            tooltip = "Edit the rotation of the selected camera",
            style =
            {
                marginTop = 10
            }
        };
        camEulers.RegisterValueChangedCallback(evt =>
        {
            
            cameraEulerAngles = evt.newValue;
            cameraEulerAnglesRounded = GetRoundedEulers();
            if (activeCamera != null)
            {
                Undo.RecordObject(activeCamera.transform, "Change Camera Rotation");
                activeCamera.transform.eulerAngles = cameraEulerAngles;
                EditorUtility.SetDirty(activeCamera);
            }
        });
        
        container.Add(camEulers);
        return container;
    }
    
    private void OnUndoRedoPerformed()
    {
        if(Selection.activeGameObject != null)
        {
            var selectedCamera = Selection.activeGameObject.GetComponent<CinemachineCamera>();
            if (selectedCamera != null)
                SelectCamera(selectedCamera);
        }
    }

    private Vector3 GetRoundedEulers()
    {
        return new Vector3(
            (float)Math.Round(cameraEulerAngles.x, 0),
            (float)Math.Round(cameraEulerAngles.y, 0),
            (float)Math.Round(cameraEulerAngles.z, 0));
    }

    public override void OnWillBeDestroyed()
    {
        Unsubscribe();
    }

    private void Unsubscribe()
    {
        Undo.undoRedoPerformed -= OnUndoRedoPerformed;
        Selection.selectionChanged -= OnSelectionChanged;
    }

    private void OnSelectionChanged()
    {
        Refresh();
        if(Selection.activeGameObject != null)
        {
            var selectedCamera = Selection.activeGameObject.GetComponent<CinemachineCamera>();
            if (selectedCamera != null)
                SelectCamera(selectedCamera);
        }
    }
    void SelectCamera(CinemachineCamera camera)
    {
        if (camera == null) return;
        foreach (var cam in cameras)
        {
            if (cam == null) continue;
            cam.Priority = cam == camera ? 10 : 0;
        }
        
        activeCamera = camera;
        Selection.activeGameObject = camera.gameObject;
        
        cameraEulerAngles = camera.transform.eulerAngles;
        cameraEulerAnglesRounded = GetRoundedEulers();
        
        Refresh();
    }
     
    void CreateNewCamera()
    {
        var sceneView = SceneView.lastActiveSceneView;
        if (sceneView == null) return;
        
        var cinemachineCameraGO = new GameObject("Cinemachine Camera", typeof(CinemachineCamera));
        var cinemachineCamera = cinemachineCameraGO.GetComponent<CinemachineCamera>();
        
        cinemachineCameraGO.transform.position = sceneView.camera.transform.position;
        cinemachineCameraGO.transform.rotation = sceneView.camera.transform.rotation;
        cinemachineCamera.Priority = 10;
        cameras.Add(cinemachineCamera);
        SelectCamera(cinemachineCamera);
        
        Undo.RegisterCreatedObjectUndo(cinemachineCameraGO, "Create Cinemachine Camera");
        Selection.activeGameObject = cinemachineCameraGO;

        Refresh();
    }

    private void Refresh()
    {
        Unsubscribe();
        var rebuildContentMethod = typeof(Overlay).GetMethod("RebuildContent", BindingFlags.NonPublic | BindingFlags.Instance);
        if (rebuildContentMethod != null)
        {
            rebuildContentMethod.Invoke(this, null);
        }
    }


    private void Initialize()
    {
        cameras = Object.FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None).ToList();
        
    }
}
