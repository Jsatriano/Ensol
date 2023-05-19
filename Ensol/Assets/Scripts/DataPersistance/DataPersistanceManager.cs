using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private PData playerData;
    private List<IDataPersistance> dataPersistenceObjects;

    private FileDataHandler dataHandler;

  public static DataPersistanceManager instance { get; private set; }

  private void Awake()
  {
    if (instance != null)
    {
        Debug.LogError("Found more than one Data Persistance Manager in the scene");
    }
    instance = this;
  }

  private void Start()
  {
    this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    LoadGame();
  }

  public void NewGame()
  {
    this.playerData = new PData();
  }

  public void LoadGame()
  {
    this.playerData = dataHandler.Load();
    if (this.playerData == null)
    {
        Debug.Log("No Data was found. Initializing data to defaults");
        NewGame();
    }

    foreach (IDataPersistance dataPersistenceObj in dataPersistenceObjects)
    {
        dataPersistenceObj.LoadData(playerData);
    }
    Debug.Log("Is loading working because NGworked is now: " + playerData.NGworked);
  }

  public void SaveGame()
  {
    foreach (IDataPersistance dataPersistenceObj in dataPersistenceObjects)
    {
        dataPersistenceObj.SaveData(ref playerData);
    }
    Debug.Log("Is saving working because NGworked is now: " + playerData.NGworked);

    dataHandler.Save(playerData);
  }

//   private void OnApplicationQuit()
//   {
//     SaveGame();
//   }

  private List<IDataPersistance> FindAllDataPersistenceObjects()
  {
    IEnumerable<IDataPersistance> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();

    return new List<IDataPersistance>(dataPersistenceObjects);
  }
}
