using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using Assets.Scripts.ScriptableObjects;

public enum RoomType { Battle, Chest, Vendor }

//Change whe then next room is figured
public class TransitionManager : MonoBehaviour
{
    public List<SceneAsset> BattleScenes = new List<SceneAsset>();
    public List<SceneAsset> ChestScenes = new List<SceneAsset>();
    public List<SceneAsset> VendorScenes = new List<SceneAsset>();

    [SerializeField] List<Room> Rooms;
    [SerializeField] List<Room> EditableRooms;

    public Room ChosenRoom;

    [SerializeField]
    private Room StartRoomScene;

    public SceneAsset sceneToLoad; // The scene to transition to

    public bool SeenChest = false;
    public bool SeenVendor = false;
    public bool StartRoom = false;

    //Don't like
    PlayerSaveManager PS;

    RoomType chosenRoomType;

    // Start is called before the first frame update
    void Start()
    {
        GameObject PlayerSaveManager = GameObject.Find("PlayerSaveManager");
        PS = PlayerSaveManager.GetComponent<PlayerSaveManager>();

        EditableRooms = new List<Room>(Rooms);
        Randomize<Room>(EditableRooms);//Randomize list on creation
        WhichRoom();
    }

    // Update is called once per frame
    void Update()
    {
    }

    [SerializeField] int RandomRoomNum;
    public void WhichRoom()
    {

        if(EditableRooms.Count == 0 )
        {
            StartRoom = true;
            LoadScene();
            return;
        }

        //get a random number within rooms range
        RandomRoomNum = UnityEngine.Random.Range(0, EditableRooms.Count);
        ChosenRoom = EditableRooms[RandomRoomNum];//Set choosen room
        
        //(This is to ensure that each playthrough has 3 battle rooms, 1 chest room, and 1 vendor room)

        chosenRoomType = ChosenRoom.roomType;//Set choosen room type
        ChooseScene();

        EditableRooms.RemoveAt(RandomRoomNum);//Remove that one from the list

    }

    public void ChooseScene()
    {
        switch (chosenRoomType)
        {
            case RoomType.Battle:
                sceneToLoad = PickFromList(BattleScenes);

                break;
            case RoomType.Chest:
                SeenChest = true;
                sceneToLoad = PickFromList(ChestScenes);

                break; 
            case RoomType.Vendor:
                SeenVendor = true;
                sceneToLoad = PickFromList(VendorScenes);

                break;
            
        }
    }

    public SceneAsset PickFromList(List<SceneAsset> SA)
    {
        if (SA.Count > 1) //If count is greater then one
        {
            int num = UnityEngine.Random.Range(0, SA.Count);//random number betwen list range
            return SA[num];//return random pick
        }
       
        else { return SA[0]; }// else return first and only Scene
    }

    public void LoadScene()
    {
        if (sceneToLoad != null)
        {
            if(StartRoom == true)
            {
                EditableRooms = new List<Room>(Rooms);
                Randomize<Room>(EditableRooms);//Randomize list on creation
                SceneManager.LoadScene("HubRoom");

                PS.ClearSaveValues();
            }
            else 
            SceneManager.LoadScene(sceneToLoad.name);
        }
        else
        {
            Debug.LogWarning("Scene to load is not set!");
        }
    }

    public static void Randomize<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

}

