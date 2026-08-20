using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// One-click scene builder. Creates the entire Main Menu / Instrument
/// Selection / Settings hierarchy plus a virtual drum kit, adds all the
/// controller scripts, and wires every Inspector reference automatically.
///
/// Usage: Unity menu bar → Tools → HoloLens Menu System → Build Scene
/// </summary>
public static class HoloLensMenuSceneBuilder
{
    [MenuItem("Tools/HoloLens Menu System/Build Scene")]
    public static void BuildScene()
    {
        // 1. Fresh empty scene
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // 2. Main Camera
        var cameraGO = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
        cameraGO.tag = "MainCamera";
        cameraGO.transform.position = new Vector3(0, 0, -5);

        // 3. EventSystem (required for any UI to receive clicks)
        var eventSystemGO = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));

        // 4. GameManager
        var gameManagerGO = new GameObject("GameManager");
        gameManagerGO.AddComponent<GameManager>();

        // 5. Root Canvas (Screen Space for easy Editor testing — switch to
        //    World Space later for the HoloLens build, see README).
        var canvasGO = new GameObject("MainCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280, 720);

        // 6. Panels
        GameObject mainMenuPanel = CreatePanel(canvasGO.transform, "MainMenuPanel");
        GameObject instrumentPanel = CreatePanel(canvasGO.transform, "InstrumentSelectionPanel");
        GameObject settingsPanel = CreatePanel(canvasGO.transform, "SettingsPanel");

        // ---- Main Menu ----
        Button playBtn = CreateButton(mainMenuPanel.transform, "PlayButton", "Play", 120);
        Button instrumentBtn = CreateButton(mainMenuPanel.transform, "InstrumentSelectionButton", "Instrument Selection", 40);
        Button settingsBtn = CreateButton(mainMenuPanel.transform, "SettingsButton", "Settings", -40);
        Button exitBtn = CreateButton(mainMenuPanel.transform, "ExitButton", "Exit", -120);

        var mainMenuController = mainMenuPanel.AddComponent<MainMenuController>();
        SetPrivateField(mainMenuController, "playButton", playBtn);
        SetPrivateField(mainMenuController, "instrumentSelectionButton", instrumentBtn);
        SetPrivateField(mainMenuController, "settingsButton", settingsBtn);
        SetPrivateField(mainMenuController, "exitButton", exitBtn);

        // ---- Instrument Selection ----
        Button pianoBtn = CreateButton(instrumentPanel.transform, "PianoButton", "Piano", 120);
        Button guitarBtn = CreateButton(instrumentPanel.transform, "GuitarButton", "Guitar", 40);
        Button drumBtn = CreateButton(instrumentPanel.transform, "DrumButton", "Drum", -40);
        Button instrumentBackBtn = CreateButton(instrumentPanel.transform, "BackButton", "Back", -120);

        var instrumentController = instrumentPanel.AddComponent<InstrumentSelectionController>();
        SetPrivateField(instrumentController, "pianoButton", pianoBtn);
        SetPrivateField(instrumentController, "guitarButton", guitarBtn);
        SetPrivateField(instrumentController, "drumButton", drumBtn);
        SetPrivateField(instrumentController, "backButton", instrumentBackBtn);

        // ---- Settings ----
        Slider volumeSlider = CreateSlider(settingsPanel.transform, "VolumeSlider", 100);
        Slider sensitivitySlider = CreateSlider(settingsPanel.transform, "SensitivitySlider", 0);
        Button settingsBackBtn = CreateButton(settingsPanel.transform, "BackButton", "Back", -120);

        var sensitivityController = settingsPanel.AddComponent<InteractionSensitivityController>();

        var settingsController = settingsPanel.AddComponent<SettingsController>();
        SetPrivateField(settingsController, "volumeSlider", volumeSlider);
        SetPrivateField(settingsController, "sensitivitySlider", sensitivitySlider);
        SetPrivateField(settingsController, "sensitivityController", sensitivityController);
        SetPrivateField(settingsController, "backButton", settingsBackBtn);

        // 7. MenuManager wiring
        var menuManagerGO = new GameObject("MenuManager");
        var menuManager = menuManagerGO.AddComponent<MenuManager>();
        SetPrivateField(menuManager, "mainMenuPanel", mainMenuPanel);
        SetPrivateField(menuManager, "instrumentSelectionPanel", instrumentPanel);
        SetPrivateField(menuManager, "settingsPanel", settingsPanel);

        // 8. Virtual Drum Kit — floats in front of the camera, hidden by default.
        //    Play (with Drum selected) shows it; its own Back pad hides it.
        GameObject drumKitRoot = new GameObject("DrumKit");
        drumKitRoot.transform.position = new Vector3(0, -0.5f, 3f);

        CreateDrumPad(drumKitRoot.transform, "KickPad", new Vector3(-0.6f, 0f, 0f), new Color(0.85f, 0.35f, 0.3f));
        CreateDrumPad(drumKitRoot.transform, "SnarePad", new Vector3(0f, 0f, 0.3f), new Color(0.3f, 0.6f, 0.9f));
        CreateDrumPad(drumKitRoot.transform, "HiHatPad", new Vector3(0.6f, 0f, 0f), new Color(0.9f, 0.8f, 0.25f));

        GameObject backPad = GameObject.CreatePrimitive(PrimitiveType.Cube);
        backPad.name = "BackToMenuPad";
        backPad.transform.SetParent(drumKitRoot.transform, false);
        backPad.transform.localPosition = new Vector3(0f, 0.6f, 0f);
        backPad.transform.localScale = new Vector3(0.4f, 0.2f, 0.2f);
        SetPadColor(backPad, new Color(0.6f, 0.6f, 0.6f));
        backPad.AddComponent<DrumKitBackButton>();

        drumKitRoot.SetActive(false);

        var drumKitManagerGO = new GameObject("DrumKitManager");
        var drumKitManager = drumKitManagerGO.AddComponent<DrumKitManager>();
        SetPrivateField(drumKitManager, "drumKitRoot", drumKitRoot);
        SetPrivateField(drumKitManager, "menuCanvas", canvasGO);

        // 9. Deactivate sub-panels so only Main Menu shows first
        instrumentPanel.SetActive(false);
        settingsPanel.SetActive(false);

        // 10. Save the scene
        System.IO.Directory.CreateDirectory("Assets/Scenes");
        bool saved = EditorSceneManager.SaveScene(scene, "Assets/Scenes/MainScene.unity");

        AssetDatabase.SaveAssets();
        Debug.Log(saved
            ? "[HoloLensMenuSceneBuilder] Scene built and saved to Assets/Scenes/MainScene.unity. " +
              "Drag audio clips onto the three DrumPad objects, then press Play to test."
            : "[HoloLensMenuSceneBuilder] Scene built but could not be saved — save manually via File > Save As.");
    }

    // ---------- helpers ----------

    private static GameObject CreatePanel(Transform parent, string name)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        return go;
    }

    private static Button CreateButton(Transform parent, string name, string label, float yPos)
    {
        GameObject go = DefaultControls.CreateButton(new DefaultControls.Resources());
        go.name = name;
        go.transform.SetParent(parent, false);

        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(320, 60);
        rt.anchoredPosition = new Vector2(0, yPos);

        var text = go.GetComponentInChildren<Text>();
        if (text != null) text.text = label;

        return go.GetComponent<Button>();
    }

    private static Slider CreateSlider(Transform parent, string name, float yPos)
    {
        GameObject go = DefaultControls.CreateSlider(new DefaultControls.Resources());
        go.name = name;
        go.transform.SetParent(parent, false);

        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(320, 40);
        rt.anchoredPosition = new Vector2(0, yPos);

        var slider = go.GetComponent<Slider>();
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = 0.75f;
        return slider;
    }

    private static GameObject CreateDrumPad(Transform parent, string name, Vector3 localPos, Color shellColor)
    {
        // Empty parent holds the collider + scripts; two child primitives
        // give it an actual drum silhouette (shell + head) instead of
        // looking like a flat button.
        GameObject padRoot = new GameObject(name);
        padRoot.transform.SetParent(parent, false);
        padRoot.transform.localPosition = localPos;

        // Shell (drum body) — a squat cylinder, colored.
        GameObject shell = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        shell.name = "Shell";
        shell.transform.SetParent(padRoot.transform, false);
        shell.transform.localScale = new Vector3(0.4f, 0.18f, 0.4f);
        SetPadColor(shell, shellColor);
        Object.DestroyImmediate(shell.GetComponent<Collider>());

        // Head (drum skin) — thin, light cream cylinder sitting on top of the shell.
        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        head.name = "Head";
        head.transform.SetParent(padRoot.transform, false);
        head.transform.localScale = new Vector3(0.42f, 0.02f, 0.42f);
        head.transform.localPosition = new Vector3(0f, 0.18f, 0f);
        SetPadColor(head, new Color(0.93f, 0.9f, 0.82f));
        Object.DestroyImmediate(head.GetComponent<Collider>());

        // One collider covering the whole pad, on the root, so a single
        // Hit() fires regardless of whether the shell or head was clicked.
        var collider = padRoot.AddComponent<CapsuleCollider>();
        collider.center = new Vector3(0f, 0.1f, 0f);
        collider.height = 0.4f;
        collider.radius = 0.21f;

        padRoot.AddComponent<AudioSource>();
        padRoot.AddComponent<DrumPad>();
        return padRoot;
    }

    private static void SetPadColor(GameObject pad, Color color)
    {
        var renderer = pad.GetComponent<Renderer>();
        if (renderer == null) return;
        var mat = new Material(Shader.Find("Standard"));
        mat.color = color;
        renderer.sharedMaterial = mat;
    }

    /// <summary>Sets a private [SerializeField] via SerializedObject so it shows
    /// up correctly in the Inspector and survives the scene save.</summary>
    private static void SetPrivateField(Object target, string fieldName, Object value)
    {
        var so = new SerializedObject(target);
        var prop = so.FindProperty(fieldName);
        if (prop == null)
        {
            Debug.LogWarning($"[HoloLensMenuSceneBuilder] Field '{fieldName}' not found on {target.GetType().Name}.");
            return;
        }
        prop.objectReferenceValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
    }
}
