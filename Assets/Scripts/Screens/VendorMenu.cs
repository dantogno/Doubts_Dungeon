using System;
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

    //[SerializeField] TextMeshProUGUI[] CheckMarks;
    public TextMeshProUGUI[] checkMarks;

    //public bool IsAnyChecked;
    public bool isAnyChecked;


    public class ItemInfo
    {
        public TextMeshProUGUI checkMark;
        public string titleText;

        public ItemInfo(TextMeshProUGUI checkMark, string titleText)
        {
            this.checkMark = checkMark;
            this.titleText = titleText;
        }

        public ItemInfo(string titleText)
        {
            this.titleText = titleText;
        }
    }

    //[SerializeField] List<ItemInfo> itemInfo = new List<ItemInfo>();
    //public string selectedItemName;

    public List<ItemInfo> itemInformation = new List<ItemInfo>();
    public string SelectedItemName;

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

            ItemInfo PulledItem = new ItemInfo(checkMarks[i], VendorManager.VendorItems[i].Name);
            itemInformation.Add(PulledItem);
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

    public void ExcangeSignature(Transform excangeTransform)
    {
        excangeTransform.gameObject.SetActive(true);

        Debug.Log(SelectedItemName);
        VendorManager.BuyItemByName(SelectedItemName);

        //Trying to have the signature only work when one of the items has been selected
        //if (IsAnyChecked)
        //    excangeTransform.gameObject.SetActive(true);
        //else
        //    excangeTransform.gameObject.SetActive(false);
    }

    public void SelectItem(Transform selectTransform)
    {
        isAnyChecked = !IsAnyCheckActive();
        selectTransform.gameObject.SetActive(isAnyChecked);

        if (isAnyChecked)
        {
            SelectedItemName = GetSelectedItemName(selectTransform);
            Debug.Log(SelectedItemName);
        } 
        else
            SelectedItemName = "";
    }

    private string GetSelectedItemName(Transform selectTransform)
    {
        // Iterate through the itemInfo list
        foreach (ItemInfo info in itemInformation)
        {
            // Compare the checkMark reference with selectTransform
            if (info.checkMark.transform == selectTransform)
            {
                // Return the associated titleText (item name)
                return info.titleText;
            }
        }

        // If no matching item found, return an empty string or handle the case as needed
        return string.Empty;
    }

    public bool IsAnyCheckActive()
    {
        foreach(TextMeshProUGUI checkMark in checkMarks)
        {
            if(checkMark.gameObject.activeSelf) 
                return true;
        }
        return false;
    }

    public int GetActiveCheckIndex()
    {
        for (int i = 0; i < checkMarks.Length; i++)
        {
            if (checkMarks[i].gameObject.activeSelf)
                return i;
        }
        return -1;
    }

}
