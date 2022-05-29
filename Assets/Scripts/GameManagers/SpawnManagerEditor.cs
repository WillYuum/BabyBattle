using UnityEditor;
using SpawnManagerCore;
using EditorExtensions;
using EditorExtensions.Prefabs;
using UnityEngine;
using Troops;
using GameplayUtils.Prefabs;


//TODO: NEED TO MAKE EDITORS SCRIPTS SIT IN EDITOR FOLDER AND NOT IN PROJECT FOLDER AND SHOULD HANDLE ASSEMBLY REFERENCES
[CustomEditor(typeof(SpawnManager))]
public class SpawnManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Populate troops"))
        {
            PopulateAllTroops("_friendlyTroopsPrefabs", "Assets/Prefabs/Troops/Friendly/");
            PopulateAllTroops("_enemyTroopsPrefabs", "Assets/Prefabs/Troops/Enemy/");


            //FIXME: Setting data does work but doesn't seem to update scene serialized object, I need to do another change to tell the scene to update 
            // the serialized object, need to find a solution for this
            Debug.Log("All troops populated");
        }
    }


    private void PopulateAllTroops(string propertyName, string path)
    {
        SerializedProperty troopsObject = GetSerializedProperty(propertyName);
        var troopToSpawn = troopsObject.GetValue<SpawnManager.TroopToSpawn[]>();


        var data = PrefabLoader.LoadAllPrefabsOfType(path);

        SpawnManager.TroopToSpawn[] newTroopToSpawn = new SpawnManager.TroopToSpawn[data.Count];

        for (int i = 0; i < data.Count; i++)
        {
            newTroopToSpawn[i] = new SpawnManager.TroopToSpawn();
            newTroopToSpawn[i].troopType = data[i].GetComponent<Troop>().TroopType;
            newTroopToSpawn[i].prefabConfig = PrefabConfig.CreatePrefabConfig(data[i]);
            Debug.Log("Updated troop to spawn: " + newTroopToSpawn[i].troopType);
        }

        troopsObject.SetValue<SpawnManager.TroopToSpawn[]>(newTroopToSpawn);
    }




    private SerializedProperty GetSerializedProperty(string propertyName)
    {
        return GetSerializedObject().FindProperty(propertyName);
    }

    private SerializedObject GetSerializedObject()
    {
        return new UnityEditor.SerializedObject(target);
    }
}
