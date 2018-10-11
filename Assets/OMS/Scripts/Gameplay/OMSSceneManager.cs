using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OMSSceneManager : MonoBehaviour
{
	[SerializeField] ScenarioSceneInfo[] scenarios;

	ScenarioSceneInfo currentScenario;

	public System.Action<ScenarioScene> onCreateScene;

	bool scenarioCreated = false;

	void Start()
	{
		CreateScenario(PlayerPrefs.GetString("SceneName"));
	}

	void CreateScenario(string scenarioName)
	{
		if (PhotonNetwork.isMasterClient) 
		{
			if (scenarioCreated) 
			{
				return;
			}
			scenarioCreated = true;
			Debug.Log ("CreateScenario " + scenarioName);
			ScenarioSceneInfo scenarioToCreate = null;
			foreach (ScenarioSceneInfo scenario in scenarios) 
			{
				if (scenario.name == scenarioName) 
				{
					scenarioToCreate = scenario;
					break;
				}
			}

			if (scenarioToCreate != null) 
			{
				GameObject scenarioGO = PhotonNetwork.InstantiateSceneObject(scenarioToCreate.scenarioScene.name, 
					Vector3.zero, Quaternion.identity, 0, null);
				ScenarioScene scene = scenarioGO.GetComponent<ScenarioScene>();
				currentScenario = new ScenarioSceneInfo(scenarioToCreate.name, scene);


				if (onCreateScene != null) 
				{
					onCreateScene (currentScenario.scenarioScene);
				}
			}
		}
	}
}

[System.Serializable]
public class ScenarioSceneInfo
{
	public string name;
	public ScenarioScene scenarioScene;

	public ScenarioSceneInfo(string name, ScenarioScene prefab)
	{
		this.name = name;
		this.scenarioScene = prefab;
	}
}
