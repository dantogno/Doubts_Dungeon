using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VendorMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject VendorScreen;

    [SerializeField] VendorManager VendorManager;

    [SerializeField]
    GameObject[] Items;

    [SerializeField] 
    TextMeshProUGUI[] CheckMarks;


    private class ItemInfo
    {
        public TextMeshProUGUI checkMark;
        public TextMeshProUGUI titleText;

        public ItemInfo(TextMeshProUGUI checkMark, TextMeshProUGUI titleText)
        {
            this.checkMark = checkMark;
            this.titleText = titleText;
        }

        public ItemInfo(TextMeshProUGUI titleText)
        {
            this.titleText = titleText;
        }
    }

    private List<ItemInfo> itemInfo = new List<ItemInfo>();

    // Start is called before the first frame update
    void Start()
    {
        GameObject VM = GameObject.Find("VendorManager");
        VendorManager = VM.GetComponent<VendorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        TestWindow();
    }

    public void PopulateVendor()
    {
        for(int i = 0; i < VendorManager.VendorTotalItems; i++)
        {
            TextMeshProUGUI titleText = Items[i].transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI descriptionText = Items[i].transform.Find("Description").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI costText = Items[i].transform.Find("Cost").GetComponent<TextMeshProUGUI>();

            titleText.text = VendorManager.VendorItems[i].Name;
            descriptionText.text = VendorManager.VendorItems[i].Description;
            costText.text = VendorManager.VendorItems[i].Cost.ToString();

            itemInfo.Add(new ItemInfo(CheckMarks[i],titleText));
        }
    }

    public void OpenVendor()
    {
        PopulateVendor();
        VendorScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void ExitVendor()
    {
        VendorScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void TestWindow()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("V key pressed");
            OpenVendor();
        }
    }

    
    public void SelectItem(Transform selectTransform)
    {
        selectTransform.gameObject.SetActive(!IsAnyCheckActive());
    }

    public bool IsAnyCheckActive()
    {
        foreach(TextMeshProUGUI checkMark in CheckMarks)
        {
            if(checkMark.gameObject.activeSelf) 
                return true;
        }
        return false;
    }

    public int GetActiveCheckIndex()
    {
        for (int i = 0; i < CheckMarks.Length; i++)
        {
            if (CheckMarks[i].gameObject.activeSelf)
                return i;
        }
        return -1;
    }

}
