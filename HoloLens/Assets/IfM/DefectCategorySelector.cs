using Assets.IfM;
using Assets.IfM.Models;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class DefectCategorySelector : MonoBehaviour
{
    private static readonly HttpClient client = new HttpClient();


    public Part part;
    public GameObject parent;
    public ScrollingObjectCollection scrollView;
    private List<GameObject> objects = new List<GameObject>();
    public GameObject defectPrefab;

    private Action<DefectCategory> callback;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPart(Part p, Action<DefectCategory> handler)
    {
        part = p;
        callback = handler;
        LoadParts();
    }

    public async void LoadParts()
    {
        var res = await client.GetAsync(WebConfig.BaseURL + "parts/" + part.Id);
        if (!res.IsSuccessStatusCode) { return;  }

        var jsonString = await res.Content.ReadAsStringAsync();
        Debug.Log(jsonString);
        var partDetails = JsonConvert.DeserializeObject<Part>(jsonString);

        foreach (GameObject box in objects)
        {
            Destroy(box);
        }
        scrollView.UpdateContent();

        int y = 0;
        foreach (DefectCategory category in partDetails.DefectCategories)
        {
            var box = Instantiate(defectPrefab);
            var buttonProps = (ButtonConfigHelper)box.GetComponent("ButtonConfigHelper");
            buttonProps.MainLabelText = category.Title;
            buttonProps.OnClick.AddListener(delegate { SelectCategory(category); });
            box.transform.SetParent(parent.transform, false);
            box.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            box.transform.localPosition = new Vector3(0, 0, 0);
            y++;
            objects.Add(box);
        }

        var grid = (GridObjectCollection)parent.GetComponent("GridObjectCollection");
        grid.UpdateCollection();

        scrollView.UpdateContent();
    }


    void SelectCategory(DefectCategory category)
    {
        //Output message to the console
        Debug.Log(category.Title + " was selected");
        callback(category);

    }

    public void ScrollUp()
    {
        scrollView.MoveByTiers(-1);
    }
    public void ScrollDown()
    {
        scrollView.MoveByTiers(1);
    }
}
