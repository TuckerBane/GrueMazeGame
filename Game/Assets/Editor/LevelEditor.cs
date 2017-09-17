using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class LevelEditor : EditorWindow
{

    [MenuItem("Tools/LevelEditor")]
    private static void InitLevelEditor()
    {
        // Get existing open window or if none, make a new one:
        LevelEditor window = (LevelEditor)EditorWindow.GetWindow(typeof(LevelEditor));
        window.Show();
    }

    private void OnEnable()
    {
        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }
    private void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        DrawGrid();

        Event cur = Event.current;

        //Debug.Log("OnSceneGUI: button" + cur.button);

        Vector3 mousePos = (Vector3)cur.mousePosition;
        mousePos.y = sceneView.camera.pixelHeight - mousePos.y;
        Vector3 worldPos = sceneView.camera.ScreenToWorldPoint(mousePos);

        int buttonPressed = cur.button;

        // left Mouse, Place Wall
        var isMouse =
            cur.type == EventType.MouseDown    ||
            cur.type == EventType.MouseDrag    ||
            cur.type == EventType.DragUpdated  ||
            cur.type == EventType.DragExited   ||
            cur.type == EventType.DragPerform;



        if (isMouse && buttonPressed == 0 && state == State.Draw)
        {
            //Debug.Log(sceneView.camera.transform.position);
            // Raycast, delete walls if its there
            Ray ray = sceneView.camera.ScreenPointToRay(mousePos);
            RaycastHit hit;

            Vector3 wallPos = Vector3.zero;
            bool hitSomething = false;
            if (Physics.Raycast(ray, out hit))
            {
                hitSomething = true;
                if (hit.collider.name.Contains("Wall"))
                {
                    Debug.Log("Hit Wall");
                    wallPos = hit.collider.transform.position;
                    Undo.DestroyObjectImmediate(hit.collider.gameObject);
                }
                else
                {
                    wallPos = new Vector3(
                        Mathf.Floor(hit.point.x + 0.5f),
                        0,
                        Mathf.Floor(hit.point.z + 0.5f));
                }
            }

            var floorRoot = GameObject.Find("WallsRoot");
            {// Get or Add FloorRoot
                if(floorRoot == null)
                {
                    floorRoot = new GameObject("WallsRoot");
                }
            }

            if (hitSomething)
            {
                var wallPrefab = FFResource.Load_Prefab("Wall");
                var wallObj = Instantiate(wallPrefab);
                wallObj.transform.position = wallPos;
                wallObj.transform.SetParent(floorRoot.transform);
                Undo.RegisterCreatedObjectUndo(wallObj, wallObj.transform.name);
            }
        }

        // right Mouse
        else if (isMouse && buttonPressed == 0 && state == State.Erase)
        {
            Ray ray = sceneView.camera.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.name.Contains("Wall"))
                {
                    Undo.DestroyObjectImmediate(hit.collider.gameObject);
                }
            }
        }

        //mousePos.y = Camera.current.pixelHeight - mousePos.y;
        //Vector3 worldPos = sceneView.camera.ScreenToWorldPoint(mousePos);
        //
        //UpdateMouse(mousePos);
        //
        //EditorMousePos editorMoseuPos = GameObject.FindGameObjectWithTag("EditorMousePos").GetComponent<EditorMousePos>();
        //editorMoseuPos.editorMousePos = mousePos;
        //editorMoseuPos.editorMouseWorldPos = worldPos;
    }
    
    public enum State
    {
        Draw,
        Erase,
    }
    State state;
    private void OnGUI()
    {
        GUILayout.Label("Draw State", EditorStyles.boldLabel);
        state = (State)EditorGUILayout.EnumPopup("Set Draw State", state);

    }

    void UpdateMouse(Vector3 mouseScreenPos)
    {
        var mousePos = Input.mousePosition;

        // Left mouse button is pressed
        if(Input.GetMouseButton(0))
        {
            PlaceWallAtMouse();
        }

        if(Input.GetMouseButton(1))
        {
            RemoveWallAtMouse();
        }
    }
    
    void DrawGrid()
    { // Grid Grid

        // Horizontal
        for (int i = 0; i < 100; ++i)
        {
            Debug.DrawLine(
                new Vector3(
                50.0f + (i * -1.0f),
                0.0f,
                -100.0f),

                new Vector3(
                50.0f + (i * -1.0f),
                0.0f,
                100.0f),
                Color.cyan * 0.6f);
        }

        // Vertical
        for (int j = 0; j < 100; ++j)
        {
            Debug.DrawLine(
                new Vector3(
                -100.0f,
                0,
                50.0f + (j * -1.0f)),

                new Vector3(
                100.0f,
                0.0f,
                50.0f + (j * -1.0f)),
                Color.cyan * 0.6f);
        }
    }

    void PlaceWallAtMouse()
    {
        // Raycast, delete walls if its there
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Destroy(hit.collider.gameObject);
        }

        var wallPos = new Vector3(
            ray.origin.x,
            0,
            ray.origin.z);


        var wallPrefab = FFResource.Load_Prefab("Wall");
        var wallObj = Instantiate(wallPrefab);
        wallObj.transform.position = wallPos;
    }
    void RemoveWallAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Destroy(hit.collider.gameObject);
        }
    }
}

