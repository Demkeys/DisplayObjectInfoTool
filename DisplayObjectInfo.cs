// Put this script in the Editor folder of your project.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DisplayObjectInfo : EditorWindow {

	const string dataKeyName = "DisplayObjectInfoToolData"; // Name of the PlayerPrefs key to be used. 
	bool showInfo = false;
	bool showName = false; bool showTag = false; bool showLayer = false;
	bool showPosition = false; int LocalOrGlobalPos = 0;
	bool showRotation = false; int LocalOrGlobalRot = 0;
	bool showScale = false; int LocalOrGlobalScale = 0;
	bool showMagnitude = false;
	bool showParentConnection = false;
	Object[] allObjectsInSceneArr; GUIStyle sceneViewGUIStyle; GUIStyle headerGUIStyle; GUIStyle groupHeaderGUIStyle;
	float labelPositionYOffset = 0.5f;
	
	Color nameColor = Color.black; Color tagColor = Color.black; Color layerColor = Color.black; 
	Color positionColor = Color.black; Color rotationColor = Color.black; Color scaleColor = Color.black; 
	Color magnitudeColor = Color.magenta; Color parentConnectionColor = Color.green;

	Vector2 objectInfoOptionsVerticalScrollBarPos = new Vector2(0,0);
	Vector2 toolOptionsVerticalScrollBarPos = new Vector2(0,0);
	Vector3 labelPositionOffset = new Vector3(0,0,0);
	int LocalOrGlobalLabelPos = 0;
	int sceneViewGUIStylefontSize = 12;

	[MenuItem("My Tools/Display Object Info")]
	static void ShowWindow()
	{
		EditorWindow window = EditorWindow.GetWindow(typeof(DisplayObjectInfo));
		Rect windowRect = new Rect(350, 150, 350, 500);
		window.position = windowRect;
		window.minSize = new Vector3(window.position.width/1.2f, window.position.height/2); // Minimum size.
		window.maxSize = new Vector3(window.position.width, window.position.height); // Maximum size.
	}

	// Initialization and setup
	void OnEnable()
	{
		// Hook the custom OnSceneGUI method into the onSceneGUIDelegate so it gets called every time the event is raised.
		SceneView.duringSceneGui += OnSceneGUI; 

		LoadData();

		Handles.color = Color.black;
		sceneViewGUIStyle = new GUIStyle();
		sceneViewGUIStyle.fontStyle = FontStyle.Bold;	
		sceneViewGUIStyle.fontSize = sceneViewGUIStylefontSize;
		sceneViewGUIStyle.alignment = TextAnchor.MiddleCenter;

		headerGUIStyle = new GUIStyle();
		headerGUIStyle.fontSize = 20;
		headerGUIStyle.fontStyle = FontStyle.Bold;
		headerGUIStyle.alignment = TextAnchor.MiddleCenter;

		groupHeaderGUIStyle = new GUIStyle();
		groupHeaderGUIStyle.fontSize = 14;
		groupHeaderGUIStyle.fontStyle = FontStyle.Bold;
		groupHeaderGUIStyle.alignment = TextAnchor.MiddleLeft;

	}

	// Drawing all the GUI controls
	void OnGUI()
	{
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Display Object Info", headerGUIStyle); EditorGUILayout.Space();
		EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
		EditorGUILayout.LabelField("Object Info Options", groupHeaderGUIStyle);
		
		showInfo = EditorGUILayout.BeginToggleGroup("Show Info", showInfo); // Opening ShowInfo Toggle group

		// To accommodate for small window size, because this editor window can be resized vectically to a certain extent.
		objectInfoOptionsVerticalScrollBarPos = EditorGUILayout.BeginScrollView(objectInfoOptionsVerticalScrollBarPos, false, false);
		
		EditorGUILayout.BeginHorizontal();
		showName = EditorGUILayout.BeginToggleGroup("Name", showName);
		nameColor = EditorGUILayout.ColorField(nameColor);
		EditorGUILayout.EndToggleGroup();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		showTag = EditorGUILayout.BeginToggleGroup("Tag", showTag);
		tagColor = EditorGUILayout.ColorField(tagColor);
		EditorGUILayout.EndToggleGroup();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		showLayer = EditorGUILayout.BeginToggleGroup("Layer", showLayer);
		layerColor = EditorGUILayout.ColorField(layerColor);
		EditorGUILayout.EndToggleGroup();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		showPosition = EditorGUILayout.BeginToggleGroup("Position", showPosition);
		positionColor = EditorGUILayout.ColorField(positionColor);
		LocalOrGlobalPos = GUILayout.SelectionGrid(LocalOrGlobalPos, new string[] {"Local","Global"}, 2);
		EditorGUILayout.EndToggleGroup();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		showRotation = EditorGUILayout.BeginToggleGroup("Rotation", showRotation);
		rotationColor = EditorGUILayout.ColorField(rotationColor);
		LocalOrGlobalRot = GUILayout.SelectionGrid(LocalOrGlobalRot, new string[] {"Local","Global"}, 2);
		EditorGUILayout.EndToggleGroup();
		EditorGUILayout.EndHorizontal();
	
		EditorGUILayout.BeginHorizontal();
		showScale = EditorGUILayout.BeginToggleGroup("Scale", showScale);
		scaleColor = EditorGUILayout.ColorField(scaleColor);
		LocalOrGlobalScale = GUILayout.SelectionGrid(LocalOrGlobalScale, new string[] {"Local","Global"}, 2);
		EditorGUILayout.EndToggleGroup();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		showMagnitude = EditorGUILayout.BeginToggleGroup("Magnitude", showMagnitude);
		magnitudeColor = EditorGUILayout.ColorField(magnitudeColor);
		EditorGUILayout.EndToggleGroup();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		showParentConnection = EditorGUILayout.BeginToggleGroup("Parent Connection", showParentConnection);
		parentConnectionColor = EditorGUILayout.ColorField(parentConnectionColor);
		EditorGUILayout.EndToggleGroup();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndToggleGroup(); // Closing ShowInfo Toggle group
		
		// To accommodate for small window size, because this editor window can be resized vectically to a certain extent.
		toolOptionsVerticalScrollBarPos = EditorGUILayout.BeginScrollView(toolOptionsVerticalScrollBarPos);
		EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
		EditorGUILayout.LabelField("Tool Options", groupHeaderGUIStyle);
		sceneViewGUIStylefontSize = EditorGUILayout.IntSlider("Text Size", sceneViewGUIStylefontSize, 1, 50);
		sceneViewGUIStyle.fontSize = sceneViewGUIStylefontSize;
		labelPositionYOffset = EditorGUILayout.Slider("Text Space", labelPositionYOffset, 0, 5);
		labelPositionOffset = EditorGUILayout.Vector3Field("Text Position Offset", labelPositionOffset);

		EditorGUILayout.BeginHorizontal();
		LocalOrGlobalLabelPos = GUILayout.SelectionGrid(LocalOrGlobalLabelPos, new string[] {"Local","Global"}, 2);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space(); EditorGUILayout.Space(); EditorGUILayout.Space();
		EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
		if(PlayerPrefs.HasKey(dataKeyName))
		{
			if(GUILayout.Button("Restore Tool To Default")) ResetToDefault();
			
		}

		EditorGUILayout.EndScrollView();

		// HandleUtility.Repaint();
		SceneView.RepaintAll();
	}

	// Cleanup
	void OnDisable()
	{
		SceneView.duringSceneGui -= OnSceneGUI;
		allObjectsInSceneArr = null;
		SaveData();
	}

	// Drawing Handles in Scene. This method will be hooked into the SceneView.onSceneGUIDelegate so it's called 
	// whenever the event is raised.
	void OnSceneGUI(SceneView sv)
	{
		if(!showInfo) return; // If showInfo is false, don't proceed further.

		allObjectsInSceneArr = GameObject.FindObjectsOfType(typeof(GameObject)); // Get all GameObjects in scene.

		// Perform Frustrum Culling to determine which GameObjects are seen by the Scene Camera, and then store
		// those gameobjects in the allFrustrumCulledObjectsWithinSceneView. This way, only the GameObjects that
		// are seem by the Scene camera have handles drawn on them.
		GameObject[] allFrustrumCulledObjectsWithinSceneView = FrustrumCullObjectsWithinSceneView(allObjectsInSceneArr);
		
		// List to store positions so we can check for duplicate positions and calculate an offset.
		List<Vector3> allFrustrumCulledObjectsWithinSceneViewPositionsList = new List<Vector3>();

		if(showInfo)
		{
			foreach(GameObject g in allFrustrumCulledObjectsWithinSceneView)
			{
				Vector3 posOffset = labelPositionOffset;
				Vector3 newPosition = LocalOrGlobalLabelPos == 0 ? g.transform.TransformPoint(posOffset): g.transform.position + posOffset;
				
				// Add the position to the allFrustrumCulledObjectsWithinSceneViewPositionsList.
				allFrustrumCulledObjectsWithinSceneViewPositionsList.Add(newPosition);
				
				// Find out if how many positions in allFrustrumCulledObjectsWithinSceneViewPositionsList are the same as newPosition
				int totalNumberOfSimilarPositions = 
					TotalNumberOfSamePositionsInPositionsList(newPosition, allFrustrumCulledObjectsWithinSceneViewPositionsList);

				// If there's more than position, that means there are duplicates. So offset the position. Since the value is bound to
				// be more than 1, subtract the value by 1. So for example if the value is 2, 1 will be used. This way the offset 
				// starts with 1.
				if(totalNumberOfSimilarPositions > 1) newPosition += new Vector3((float)totalNumberOfSimilarPositions-1,0,0);

				if(showName){
					sceneViewGUIStyle.normal.textColor = nameColor;
					Handles.Label(newPosition, g.name, sceneViewGUIStyle);
					newPosition.y -= labelPositionYOffset; }
				if(showTag){ 
					sceneViewGUIStyle.normal.textColor = tagColor;
					Handles.Label(newPosition, "Tag:" + g.tag, sceneViewGUIStyle);
				newPosition.y -= labelPositionYOffset; }
				if(showLayer){ 
					sceneViewGUIStyle.normal.textColor = layerColor;
					Handles.Label(newPosition, "Layer:" + LayerMask.LayerToName(g.layer), sceneViewGUIStyle);
				newPosition.y -= labelPositionYOffset; }
				if(showPosition){ 
					sceneViewGUIStyle.normal.textColor = positionColor;
					Handles.Label(newPosition, 
					LocalOrGlobalPos == 0 ? g.transform.localPosition.ToString() : g.transform.position.ToString(), sceneViewGUIStyle);
				newPosition.y -= labelPositionYOffset; }
				if(showRotation){ 
					sceneViewGUIStyle.normal.textColor = rotationColor;
					Handles.Label(newPosition, 
					LocalOrGlobalRot == 0 ? g.transform.localEulerAngles.ToString() : g.transform.eulerAngles.ToString(), sceneViewGUIStyle);
				newPosition.y -= labelPositionYOffset; }
				if(showScale){ 
					sceneViewGUIStyle.normal.textColor = scaleColor;
					Handles.Label(newPosition, 
					LocalOrGlobalScale == 0 ? g.transform.localScale.ToString() : g.transform.lossyScale.ToString(), sceneViewGUIStyle);
				newPosition.y -= labelPositionYOffset; }
				if(showMagnitude)
				{
					Handles.color = magnitudeColor;
					Handles.DrawLine(Vector3.zero, g.transform.position);
					sceneViewGUIStyle.normal.textColor = Color.black;
					Handles.Label(newPosition, "Magnitude:" + g.transform.position.magnitude.ToString(), sceneViewGUIStyle);
					newPosition.y -= labelPositionYOffset;
				}
				if(showParentConnection && g.transform.parent != null)
				{
					Handles.color = parentConnectionColor;
					Handles.DrawLine(g.transform.position, g.transform.parent.position);
				}
			}
		}
	}

	// This method takes in a Vector3 value PositionToCheck and compares it to all positions in PositionsList to 
	//find out how many such positions exist in the list.
	int TotalNumberOfSamePositionsInPositionsList(Vector3 PositionToCheck, List<Vector3> PositionsList)
	{
		int result = 0;
	
		for (int i = 0; i < PositionsList.Count; i++)
		{ if(PositionsList[i] == PositionToCheck) { result++; } }

		return result;
	}

	// This method takes in an Object array, performs Frustrum Culling calculations to determine which of those 
	// Objects are seen by the Scene camera, and then return an Object array containing all the Objects seen
	// by the scene camera.
	GameObject[] FrustrumCullObjectsWithinSceneView(Object[] allObjectsInSceneArr)
	{
		List<GameObject> allFrustrumCulledGameObjectsWithinSceneViewList = new List<GameObject>();
		GameObject[] allFrustrumCulledObjectsWithinSceneViewArr;

		for(int i = 0; i < allObjectsInSceneArr.Length; i++)
		{
			GameObject gameObjectInScene = (GameObject)allObjectsInSceneArr[i];
			
			// Get the viewport position of the gameobject using the Scene camera (not MainCamera).
			Vector3 viewportPos = SceneView.currentDrawingSceneView.camera.WorldToViewportPoint(gameObjectInScene.transform.position);
			
			// Check whether gameobject is within the Scene camera's viewport or not. If it is...
			if(viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1 && viewportPos.z > 0)
			{
				// ...add it to the list.
				allFrustrumCulledGameObjectsWithinSceneViewList.Add((GameObject)allObjectsInSceneArr[i]);
			}
		}

		allFrustrumCulledObjectsWithinSceneViewArr = allFrustrumCulledGameObjectsWithinSceneViewList.ToArray();
		return allFrustrumCulledObjectsWithinSceneViewArr;
	}

	void ResetToDefault()
	{
		showInfo = false; showName = false; showTag = false; showLayer = false;
		showPosition = false; LocalOrGlobalPos = 0; showRotation = false; LocalOrGlobalRot = 0; showScale = false; LocalOrGlobalScale = 0;
		showMagnitude = false; showParentConnection = false; labelPositionYOffset = 0.5f;
		
		nameColor = Color.black; tagColor = Color.black; layerColor = Color.black; 
		positionColor = Color.black; rotationColor = Color.black; scaleColor = Color.black; 
		magnitudeColor = Color.magenta; parentConnectionColor = Color.green;

		objectInfoOptionsVerticalScrollBarPos = new Vector2(0,0);
		toolOptionsVerticalScrollBarPos = new Vector2(0,0);
		labelPositionOffset = new Vector3(0,0,0);
		LocalOrGlobalLabelPos = 0;
		sceneViewGUIStylefontSize = 12;
	}

	void SaveData()
	{
		DisplayObjectInfoSaveData dataToSave = new DisplayObjectInfoSaveData
			{_showInfo = showInfo, _showName = showName, _showTag = showTag, _showLayer = showLayer, 
			 _showPosition = showPosition, _LocalOrGlobalPos = LocalOrGlobalPos,
			 _showRotation = showRotation, _LocalOrGlobalRot = LocalOrGlobalRot,
			 _showScale = showScale, _LocalOrGlobalScale = LocalOrGlobalScale,
			 _showMagnitude = showMagnitude, _showParentConnection = showParentConnection,
			 _labelPositionYOffset = labelPositionYOffset, 
			 _nameColor = nameColor, _tagColor = tagColor, _layerColor = layerColor,
			 _positionColor = positionColor, _rotationColor = rotationColor, _scaleColor = scaleColor,
			 _magnitudeColor = magnitudeColor, _parentConnectionColor = parentConnectionColor,
			 _objectInfoOptionsVerticalScrollBarPos = objectInfoOptionsVerticalScrollBarPos,
			 _toolOptionsVerticalScrollBarPos = toolOptionsVerticalScrollBarPos,
			 _labelPositionOffset = labelPositionOffset, _LocalOrGlobalLabelPos = LocalOrGlobalLabelPos,
			 _sceneViewGUIStylefontSize = sceneViewGUIStylefontSize};
		string saveDataText = JsonUtility.ToJson(dataToSave);
		PlayerPrefs.SetString(dataKeyName, saveDataText); 
	}

	void LoadData()
	{
		if(PlayerPrefs.HasKey(dataKeyName))
		{
			DisplayObjectInfoSaveData dataToLoad = 
				(DisplayObjectInfoSaveData) JsonUtility.FromJson(PlayerPrefs.GetString(dataKeyName), typeof(DisplayObjectInfoSaveData));
			showInfo = dataToLoad._showInfo; showName = dataToLoad._showInfo; showTag = dataToLoad._showTag; showLayer = dataToLoad._showLayer;
			showPosition = dataToLoad._showPosition; LocalOrGlobalPos = dataToLoad._LocalOrGlobalPos;
			showRotation = dataToLoad._showRotation; LocalOrGlobalRot = dataToLoad._LocalOrGlobalRot;
			showScale = dataToLoad._showScale; LocalOrGlobalScale = dataToLoad._LocalOrGlobalScale;
			showMagnitude = dataToLoad._showMagnitude; showParentConnection = dataToLoad._showParentConnection;
			labelPositionYOffset = dataToLoad._labelPositionYOffset;
			nameColor = dataToLoad._nameColor; tagColor = dataToLoad._tagColor; layerColor = dataToLoad._layerColor;
			positionColor = dataToLoad._positionColor; rotationColor = dataToLoad._rotationColor; scaleColor = dataToLoad._scaleColor;
			magnitudeColor = dataToLoad._magnitudeColor; parentConnectionColor = dataToLoad._parentConnectionColor;
			objectInfoOptionsVerticalScrollBarPos = dataToLoad._objectInfoOptionsVerticalScrollBarPos;
			toolOptionsVerticalScrollBarPos = dataToLoad._toolOptionsVerticalScrollBarPos;
			labelPositionOffset = dataToLoad._labelPositionOffset; LocalOrGlobalLabelPos = dataToLoad._LocalOrGlobalLabelPos;
			sceneViewGUIStylefontSize = dataToLoad._sceneViewGUIStylefontSize;
		}
	}
}

[System.Serializable]
public class DisplayObjectInfoSaveData
{
	public bool _showInfo = false;
	public bool _showName = false; public bool _showTag = false;  public bool _showLayer = false;
	public bool _showPosition = false; public int _LocalOrGlobalPos = 0;
	public bool _showRotation = false; public int _LocalOrGlobalRot = 0;
	public bool _showScale = false; public int _LocalOrGlobalScale = 0;
	public bool _showMagnitude = false;
	public bool _showParentConnection = false;
	public float _labelPositionYOffset = 0.5f;
	public Color _nameColor = Color.black; public Color _tagColor = Color.black; public Color _layerColor = Color.black; 
	public Color _positionColor = Color.black; public Color _rotationColor = Color.black; public Color _scaleColor = Color.black; 
	public Color _magnitudeColor = Color.magenta; public Color _parentConnectionColor = Color.green;
	public Vector2 _objectInfoOptionsVerticalScrollBarPos = new Vector2(0,0);
	public Vector2 _toolOptionsVerticalScrollBarPos = new Vector2(0,0);
	public Vector3 _labelPositionOffset = new Vector3(0,0,0);
	public int _LocalOrGlobalLabelPos = 0;

	public int _sceneViewGUIStylefontSize = 12;
}
