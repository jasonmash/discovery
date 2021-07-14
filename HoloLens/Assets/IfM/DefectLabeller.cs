using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefectLabeller : MonoBehaviour, IMixedRealityGestureHandler, IMixedRealityHandJointHandler
{
    public GameObject partSelector;
    public GameObject partList;
    public GameObject defectCategorySelector;

    public GameObject arrowPrefab;
    public GameObject labelPrefab;
    public Dictionary<int, GameObject> labels = new Dictionary<int, GameObject>();
    public List<GameObject> boxes = new List<GameObject>();


    [SerializeField]
    private MixedRealityInputAction selectAction;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenPartSelector()
    {
        partSelector.SetActive(!partSelector.activeSelf);
        var parts = (PartList)partList.GetComponent("PartList");
        if (parts != null)
        {
            parts.LoadParts();
        }
    }

    private bool canLabel = false;
    public void ToggleLabelling()
    {
        canLabel = true;
    }

    private void OnEnable()
    {
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityGestureHandler>(this);
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityHandJointHandler>(this);
    }

    private void OnDisable()
    {
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityGestureHandler>(this);
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityHandJointHandler>(this);
    }

    MixedRealityPose pose;
    int i = 0;
    public void OnGestureCompleted(InputEventData eventData)
    {
        if (canLabel && HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, eventData.Handedness, out pose))
        {
            AddLabel();
            canLabel = false;
        }
    }

    public void OnGestureStarted(InputEventData eventData)
    {
    }

    public void OnGestureUpdated(InputEventData eventData)
    {
    }

    public void OnGestureCanceled(InputEventData eventData)
    {
    }

    public void AddLabel()
    {
        i++;


        var parts = (PartList)partList.GetComponent("PartList");
        var part = parts.SelectedPart;
        var labelTitle = part == null ? "Label " + i : part.Title;

        Vector3 posR = pose.Position;
        Quaternion rot = Quaternion.Euler(0, 0, 0);
        var label = Instantiate(labelPrefab, posR, rot);
        var labelProps = (ToolTip)label.GetComponent("ToolTip");
        labelProps.ToolTipText = labelTitle;
        var defectProps = (DefectLabel)label.GetComponent("DefectLabel");
        defectProps.labelId = i;
        defectProps.defectCategorySelector = this.defectCategorySelector;
        defectProps.part = part == null ? new Assets.IfM.Models.Part { Id = 1, Title = "Test" } : part;
        defectProps.deleteCallback = DeleteLabel;
        labels.Add(i, label);
        defectProps.defect = new Assets.IfM.Models.Defect { PartId = defectProps.part.Id, Title = labelTitle };

    }

    public void DeleteLabel(int labelId)
    {
        if (labels.ContainsKey(labelId))
        {
            var label = labels[labelId];
            Destroy(label);
            labels.Remove(labelId);
        }
    }

    public void ClearAll()
    {
        foreach (GameObject box in boxes)
        {
            Destroy(box);
        }
        foreach (GameObject label in labels.Values)
        {
            Destroy(label);
        }
    }



    GameObject jointObject;
    public void OnHandJointsUpdated(InputEventData<IDictionary<TrackedHandJoint, MixedRealityPose>> eventData)
    {
        var inputSystem = CoreServices.InputSystem;

        MixedRealityHandTrackingProfile handTrackingProfile = inputSystem?.InputSystemProfile.HandTrackingProfile;

        if (!canLabel)
        {
            if (jointObject != null)
            {
                jointObject.SetActive(false);
            }
            return;
        }

        foreach (TrackedHandJoint handJoint in eventData.InputData.Keys)
        {
            if (handJoint == TrackedHandJoint.ThumbTip)
            {
                if (jointObject == null)
                {
                    jointObject = Instantiate(arrowPrefab);
                    jointObject.name = handJoint.ToString() + " Proxy Transform";
                }
                jointObject.SetActive(true);
                jointObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                jointObject.transform.position = eventData.InputData[handJoint].Position + new Vector3(0f, 0.05f, 0f);
                jointObject.transform.rotation = new Quaternion(180, eventData.InputData[handJoint].Rotation.y, eventData.InputData[handJoint].Rotation.z, eventData.InputData[handJoint].Rotation.w);
                jointObject.transform.parent = transform;
            }
        }
    }
}
