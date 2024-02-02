using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public enum RoomType { Battle, Chest, Vendor }

public class TransitionManager : MonoBehaviour
{
    public List<SceneAsset> BattleScenes = new List<SceneAsset>();
    public List<SceneAsset> ChestScenes = new List<SceneAsset>();
    public List<SceneAsset> VendorScenes = new List<SceneAsset>();

    private Room Battle1 = new Room(RoomType.Battle);
    private Room Battle2 = new Room(RoomType.Battle);
    private Room Battle3 = new Room(RoomType.Battle);
    private Room Chest = new Room(RoomType.Chest);
    private Room Vendor = new Room(RoomType.Vendor);
    private List<Room> Rooms;

    public Room ChosenRoom;

    public SceneAsset sceneToLoad; // The scene to transition to

    public bool SeenChest = false;
    public bool SeenVendor = false;

    

    RoomType chosenRoomType;

    // Start is called before the first frame update
    void Start()
    {
        Rooms = new List<Room>() { Battle1, Battle2, Battle3, Chest, Vendor};
        Randomize<Room>(Rooms);//Randomize list on creation

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void WhichRoom()
    {
        //get a random number within rooms range
        int range = UnityEngine.Random.Range(0, Rooms.Count);
        ChosenRoom = Rooms[range];//Set choosen room

        Rooms.RemoveAt(range);//Remove that one from the list
        //(This is to ensure that each playthrough has 3 battle rooms, 1 chest room, and 1 vendor room)

        chosenRoomType = ChosenRoom.roomType;//Set choosen room type
        ChooseScene();

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

public class Room
{
    public RoomType roomType;

    public Room(RoomType rt)
    {
        roomType = rt;
    }
}

