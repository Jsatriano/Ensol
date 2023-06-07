using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using Ink.Runtime;


public class DataPersistanceManager : MonoBehaviour
{
  [Header("File Storage Config")]
  [SerializeField] private string fileName;
  [SerializeField] private string skippingFileName;


  private PData playerData;
  // private string SF = "";
  public TextAsset globals;
  private List<IDataPersistance> dataPersistenceObjects;
  private Story story;

  private FileDataHandler dataHandler;
  private FileDataHandler dataSkipHandler;

  public static DataPersistanceManager instance { get; private set; }

  private void Awake()
  {
    if (instance != null)
    {
        Debug.LogError("Found more than one Data Persistance Manager in the scene");
    }
    instance = this;
    // SceneManager.activeSceneChanged += sceneChange;
  }

  private void Start()
  {
    this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, globals);
    this.dataSkipHandler = new FileDataHandler(Application.dataPath+"/TestingSaveData", skippingFileName, globals);
    this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    LoadGame();
  }

  // private void Update()
  // {
  //   SceneManager.activeSceneChanged += sceneChange;
  // }

  // private void sceneChange(Scene current, Scene next)
  // {
  //   print("scene has changed !!!!!!!!!!!!!!!!");
  //   SaveGame();
  // }

  public void NewGame()
  {
    this.playerData = new PData();
    // this.SF = "";
  }

  public void ClearGame()
  {
    story = new Story(globals.text);
    DialogueVariables.saveFile = story.state.ToJson();
    dataHandler.Delete();
    LoadGame();
  }

  public void LoadGame()
  {
    this.playerData = dataHandler.Load();
    Debug.Log(playerData);
    dataHandler.LoadStory();
    if (this.playerData == null)
    {
        Debug.Log("No Data was found. Initializing data to defaults");
        NewGame();
    }

    foreach (IDataPersistance dataPersistenceObj in dataPersistenceObjects)
    {
        dataPersistenceObj.LoadData(playerData);
        // dataPersistenceObj.LoadStory(globals);
    }
    // Debug.Log("Is loading working because ngworked is now: " + PlayerData.NGworked);
  }

   public void LoadSkippingGame()
  {
    this.playerData = dataSkipHandler.Load();
    Debug.Log(playerData);
    dataSkipHandler.LoadSkippedStory();
    if (this.playerData == null)
    {
        Debug.Log("No Data was found. Initializing data to defaults");
        NewGame();
    }

    foreach (IDataPersistance dataPersistenceObj in dataPersistenceObjects)
    {
        dataPersistenceObj.LoadData(playerData);
        // dataPersistenceObj.LoadStory(globals);
    }
    // Debug.Log("Is loading working because ngworked is now: " + PlayerData.NGworked);
  }

  public void SaveGame()
  {
    foreach (IDataPersistance dataPersistenceObj in dataPersistenceObjects)
    {
        dataPersistenceObj.SaveData(ref playerData);
        dataPersistenceObj.SaveStory(ref globals);
    }
    // Debug.Log("Is saving working because ngworked is now: " + playerData.NGworked);
    // Debug.Log("Is string there?:" + globals.text);

    dataHandler.Save(playerData);
    dataHandler.SaveStory(globals);
  }

  private void OnApplicationQuit()
  {
    SaveGame();
  }

  private List<IDataPersistance> FindAllDataPersistenceObjects()
  {
    IEnumerable<IDataPersistance> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();

    return new List<IDataPersistance>(dataPersistenceObjects);
  }
}
