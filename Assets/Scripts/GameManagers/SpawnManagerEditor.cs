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

            SpawnManager myTarget = (SpawnManager)target;


            PopulateAllTroops("_friendlyTroopsPrefabs");
            PopulateAllTroops("_enemyTroopsPrefabs");

            Debug.Log("All troops populated");
        }
    }


    private void PopulateAllTroops(string propertyName)
    {
        SerializedProperty friendlyTroopObject = GetSerializedProperty(propertyName);
        var troopToSpawn = friendlyTroopObject.GetValue<SpawnManager.TroopToSpawn[]>();


        var data = PrefabLoader.LoadAllPrefabsOfType("Assets/Prefabs/Troops/Friendly/");

        SpawnManager.TroopToSpawn[] newTroopToSpawn = new SpawnManager.TroopToSpawn[data.Count];

        for (int i = 0; i < data.Count; i++)
        {
            newTroopToSpawn[i] = new SpawnManager.TroopToSpawn();
            newTroopToSpawn[i].troopType = data[i].GetComponent<Troop>().TroopType;
            newTroopToSpawn[i].prefabConfig = PrefabConfig.CreatePrefabConfig(data[i]);
            Debug.Log("Updated troop to spawn: " + newTroopToSpawn[i].troopType);
        }

        friendlyTroopObject.SetValue<SpawnManager.TroopToSpawn[]>(newTroopToSpawn);
    }




    private SerializedProperty GetSerializedProperty(string propertyName)
    {
        return new UnityEditor.SerializedObject(target).FindProperty(propertyName);
    }
}
