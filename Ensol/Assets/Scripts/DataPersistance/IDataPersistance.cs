using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistance
{
    void LoadData(PData data);
    void SaveData(ref PData data);
    // void LoadStory(TextAsset globals);
    void SaveStory(ref TextAsset globals);
}
