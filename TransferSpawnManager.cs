using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Location
{
    public string name;
    public Transform tf_Spawn;
}
public class TransferSpawnManager : MonoBehaviour
{
    [SerializeField] Location[] locations;
    Dictionary<string, Transform> locationDic = new Dictionary<string, Transform>();

    public static bool isSpawnTiming = false;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < locations.Length; i++)
        {
            locationDic.Add(locations[i].name, locations[i].tf_Spawn);

        }

        if (isSpawnTiming)
        {
            TransferManager theTM = FindObjectOfType<TransferManager>();
            string t_LocationName = theTM.GetLocationName();
            Transform t_Spawn = locationDic[t_LocationName];
            PlayerController.instance.transform.position = t_Spawn.position;
            PlayerController.instance.transform.rotation = t_Spawn.rotation;
            Camera.main.transform.localPosition = new Vector3(0, 0, 0);
            Camera.main.transform.localEulerAngles = Vector3.zero;
            PlayerController.instance.Reset();

            isSpawnTiming = false;

            StartCoroutine(theTM.Done());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
