using Assets.IfM;
using Assets.IfM.Models;
using Assets.IfM.Services;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using UnityEngine;

public class DefectLabel : MonoBehaviour
{
    private static readonly HttpClient client = new HttpClient();

    public GameObject defectCategorySelector;
    public GameObject defectContextMenu;
    public GameObject label;
    public Part part;
    public Defect defect;
    public int labelId;

    public Action<int> deleteCallback;

    public bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnLabelMenuPress()
    {
        Debug.Log("Pressed: " + labelId);
        defectContextMenu.SetActive(!defectContextMenu.activeSelf);
    }


    public void OnImageCapture()
    {
        Debug.Log("Pressed: " + labelId);
        ImageCaptureService ics = new ImageCaptureService();
        ics.completionCallback = OnImageCaptured;
        ics.CapturePhoto(this.transform);
    }


    public void OnCategoryChange()
    {
        Debug.Log("Pressed: " + labelId);
        defectCategorySelector.SetActive(!defectCategorySelector.activeSelf);

        if (defectCategorySelector.activeSelf)
        {
            var selector = (DefectCategorySelector)defectCategorySelector.GetComponent("DefectCategorySelector");
            selector.SetPart(part, CategoryChanged);

        }
    }


    public void CategoryChanged(DefectCategory category)
    {
        var labelProps = (ToolTip)label.GetComponent("ToolTip");
        labelProps.ToolTipText = category.Title;
        defect.Title = category.Title;
        defect.CategoryId = category.Id;

        if (defect.Id == 0) { CreateDefect(); }
        else { UpdateDefect(); }
    }



    public void OnMove()
    {
        Debug.Log("Pressed: " + labelId);
        this.canMove = !this.canMove;

        var objectManipulator = (ObjectManipulator)label.GetComponent("ObjectManipulator");
        objectManipulator.enabled = this.canMove;
        var nearInteractionGrabbable = (NearInteractionGrabbable)label.GetComponent("NearInteractionGrabbable");
        nearInteractionGrabbable.enabled = this.canMove;
        var boxCollider = (BoxCollider)label.GetComponent("BoxCollider");
        boxCollider.enabled = this.canMove;
    }



    public void OnDelete()
    {
        Debug.Log("Pressed: " + labelId);
        if (deleteCallback != null)
        {
            DeleteDefect();
            deleteCallback(labelId);
        }
    }

    public async void CreateDefect()
    {
        var json = JsonConvert.SerializeObject(defect);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var res = await client.PostAsync(WebConfig.BaseURL + "defects", content);

        var jsonString = await res.Content.ReadAsStringAsync();
        defect = JsonConvert.DeserializeObject<Defect>(jsonString);
    }

    public async void UpdateDefect()
    {
        var json = JsonConvert.SerializeObject(defect);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        await client.PutAsync(WebConfig.BaseURL + "defects/" + defect.Id, content);
    }

    public async void DeleteDefect()
    {
        await client.DeleteAsync(WebConfig.BaseURL + "defects/" + defect.Id);
    }

    public async void OnImageCaptured(string base64Image)
    {
        var labelProps = (ToolTip)label.GetComponent("ToolTip");

        try
        {
            var image = new Image
            {
                Timestamp = DateTime.UtcNow,
                ImageData = base64Image
            };

            if (defect.Id != 0)
            {
                image.DefectId = defect.Id;
                image.CategoryId = defect.CategoryId;
                image.PartId = defect.PartId;
            }

            var json = JsonConvert.SerializeObject(image);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await client.PostAsync(WebConfig.BaseURL + "images", content);
            
            if (!res.IsSuccessStatusCode)
            {
                var c = await res.Content.ReadAsStringAsync();
                labelProps.ToolTipText = res.StatusCode.ToString() + c + res.ReasonPhrase;
            }
        }
        catch (Exception e)
        {
            labelProps.ToolTipText = e.ToString();
        }
    }


    public class Image
    {
        public int Id { get; set; }
        public Part Part { get; set; }
        public int? PartId { get; set; }
        public DefectCategory Category { get; set; }
        public int? CategoryId { get; set; }
        public Defect Defect { get; set; }
        public int? DefectId { get; set; }

        public DateTime Timestamp { get; set; }
        public string ImageData { get; set; }
    }
}
