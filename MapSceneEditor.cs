using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class MapSceneEditor : EditorWindow
{
    private string mapID = "";
    private string sceneName = "";

    [MenuItem("Load Map Tools/Mở Scene")]
    private static void ShowWindow()
    {
        var window = (MapSceneEditor)EditorWindow.GetWindow(typeof(MapSceneEditor), true);
        window.titleContent = new GUIContent("Load Map Scene");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Nhập Map ID", EditorStyles.boldLabel);
        mapID = EditorGUILayout.TextField("Map ID", mapID);

        if (GUILayout.Button("Nhấn vào đây để mở map"))
        {
            NavigateToScene();
        }
    }

    private void NavigateToScene()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            sceneName = "scene_map" + mapID;
            string[] guids = AssetDatabase.FindAssets("scene_map" + mapID + " t:Scene");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                EditorSceneManager.OpenScene(path);

                EditorApplication.delayCall += () =>
                {
                    GameObject transformParent = GameObject.Find("MapScene");
                    if (transformParent == null)
                    {
                        Debug.LogError("MapScene not found! Inbox lark");
                        return;
                    }

                    for (int childIndex = 0; childIndex < transformParent.transform.childCount; childIndex++)
                    {
                        Transform childTransform = transformParent.transform.GetChild(childIndex);
                        if (childTransform == null)
                        {
                            continue;
                        }

                        string sortingLayerName = MapConfig.MAP_SORT_NAMED_DEFAULT;

                        if (childTransform.name == MapConfig.PATH_REGION_LAYER)
                        {
                            sortingLayerName = MapConfig.MAP_SORT_NAMED_L;
                        }
                        else if (childTransform.name == MapConfig.PATH_REGION_OBJECT)
                        {
                            sortingLayerName = MapConfig.MAP_SORT_NAMED_O;
                        }
                        else if (childTransform.name == MapConfig.PATH_REGION_TOP_OBJECT)
                        {
                            sortingLayerName = MapConfig.MAP_SORT_NAMED_OBJECT;
                        }

                        for (int l = 0; l < childTransform.childCount; l++)
                        {
                            Transform childPathRegionLayer = childTransform.GetChild(l);
                            if (childPathRegionLayer == null)
                            {
                                continue;
                            }

                            SpriteRenderer spriteRenderer = childPathRegionLayer.GetComponent<SpriteRenderer>();

                            if (spriteRenderer != null)
                            {
                                spriteRenderer.sortingLayerName = sortingLayerName;
                            }
                        }
                    }
                };
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Mã id map copy sai -.- " + mapID +"\n\tCopy rồi nhập lại! ", "OK");
            }
        }
    }

    [MenuItem("Load Map Tools/Bấm vào đây nếu không thấy nhà")]
    private static void UpdateSortingLayers()
    {
        GameObject transformParent = GameObject.Find("MapScene");
        if (transformParent == null)
        {
            Debug.LogError("MapScene not found");
            return;
        }
        for (int childIndex = 0; childIndex < transformParent.transform.childCount; childIndex++)
        {
            Transform childTransform = transformParent.transform.GetChild(childIndex);
            if (childTransform == null)
            {
                continue;
            }

            string sortingLayerName = MapConfig.MAP_SORT_NAMED_DEFAULT;

            if (childTransform.name == MapConfig.PATH_REGION_LAYER)
            {
                sortingLayerName = MapConfig.MAP_SORT_NAMED_L;
            }
            else if (childTransform.name == MapConfig.PATH_REGION_OBJECT)
            {
                sortingLayerName = MapConfig.MAP_SORT_NAMED_O;
            }
            else if (childTransform.name == MapConfig.PATH_REGION_TOP_OBJECT)
            {
                sortingLayerName = MapConfig.MAP_SORT_NAMED_OBJECT;
            }

            for (int l = 0; l < childTransform.childCount; l++)
            {
                Transform childPathRegionLayer = childTransform.GetChild(l);
                if (childPathRegionLayer == null)
                {
                    continue;
                }

                SpriteRenderer spriteRenderer = childPathRegionLayer.GetComponent<SpriteRenderer>();

                if (spriteRenderer != null)
                {
                    spriteRenderer.sortingLayerName = sortingLayerName;
                }
            }
        }
    }
}

public class MapConfig
{
    // Layer của một object trong map có tên là LAYER
    public const string MAP_SORT_NAMED_DEFAULT = "Default";
    public const string MAP_SORT_NAMED_L = "MapTitleL";
    public const string MAP_SORT_NAMED_OBJECT = "MapObject";
    public const string MAP_SORT_NAMED_O = "MapTitleO";
    public const string PATH_REGION_LAYER = "MSTitleL";
    public const string PATH_REGION_OBJECT = "MSTitleO";
    public const string PATH_REGION_TOP_OBJECT = "MSObjectA";
}