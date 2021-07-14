using Assets.IfM;
using Assets.IfM.Models;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class PartList : MonoBehaviour
{

    private static readonly HttpClient client = new HttpClient();


    private List<Part> parts;
    private List<GameObject> objects = new List<GameObject>();

    public GameObject partPrefab;
    public GameObject parent;
    public ScrollingObjectCollection scrollView;
    public GameObject selectedPartLabel;

    private Part selectedPart;
    public Part SelectedPart
    {
        get { return selectedPart; }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadParts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void LoadParts()
    {
        var res = await client.GetAsync(WebConfig.BaseURL + "parts");
        var jsonString = await res.Content.ReadAsStringAsync();
        parts = JsonConvert.DeserializeObject<List<Part>>(jsonString);
        
        foreach (GameObject box in objects)
        {
            Destroy(box);
        }

        int y = 0;
        foreach (Part part in parts)
        {
            var box = Instantiate(partPrefab);
            var buttonProps = (ButtonConfigHelper)box.GetComponent("ButtonConfigHelper");
            buttonProps.MainLabelText = part.Title;
            buttonProps.OnClick.AddListener(delegate { SelectPart(part); });
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

    void SelectPart(Part part)
    {
        //Output message to the console
        selectedPart = part;
        Debug.Log(part.Title + " was selected");

        var label = (TextMeshPro)selectedPartLabel.GetComponent("TextMeshPro");
        label.SetText("Selected Part: " + part.Title + " (#" + part.Id + ")");

        Task.Run(async () =>
        {
            var res = await client.GetAsync(WebConfig.BaseURL + "parts/" + part.Id);
            var jsonString = await res.Content.ReadAsStringAsync();
            selectedPart = JsonConvert.DeserializeObject<Part>(jsonString);
        });

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
