using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MapController : MonoBehaviour
{
    public static MapController instance;

    public GameObject[] mapButtons;
    public GameObject[] mapImages;
    public GameObject[] mapPanels;
    
    public GameObject[] HanoiButtons;

    public GameObject map;

    private bool _isInitialized;
    private Vector3 _originalPos;
    

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < mapImages.Length; i++)
        {
            GameObject currentMap = mapImages[i]; // Store reference
            EventTrigger trigger = currentMap.GetComponent<EventTrigger>();
            if (trigger == null) trigger = currentMap.AddComponent<EventTrigger>();
        
            EventTrigger.Entry enterEntry = new EventTrigger.Entry();
            enterEntry.eventID = EventTriggerType.PointerEnter;
            // Pass currentMap directly here
            enterEntry.callback.AddListener((data) => { OnPointerEnterMap(currentMap); });
            trigger.triggers.Add(enterEntry);
        
            EventTrigger.Entry exitEntry = new EventTrigger.Entry();
            exitEntry.eventID = EventTriggerType.PointerExit;
            // Pass currentMap directly here
            exitEntry.callback.AddListener((data) => { OnPointerExitMap(currentMap); });
            trigger.triggers.Add(exitEntry);
        }
    }

    public void OnPointerEnterMap(GameObject enteredObject)
    {
        StartCoroutine(MapPopUpAnimation(enteredObject));

        // Debug.Log("Detected entry on: " + enteredObject.name);
    }

    public void OnPointerExitMap(GameObject enteredObject)
    {
        StartCoroutine(MapDissapearAnimation(enteredObject));

        Debug.Log("Detected entry on: " + enteredObject.name);
    }
    
    IEnumerator MapPopUpAnimation(GameObject go)
    {
        GameObject obj = go.GetComponent<Map>() != null ? go : go.transform.parent.gameObject;
        RectTransform rect = obj.GetComponent<RectTransform>();
        
        rect.DOKill();
    
        // Capture the starting position once so we know where to return to
        _originalPos = rect.localPosition;

        // Logic for the Map model
        Map map = obj.GetComponent<Map>();
        if (map != null && !map.isMultiple) {
            HidePanels();
            map.ShowModel();
        }

        // Animation: Move to target and Fade In
        obj.GetComponent<Image>().enabled = true;
    
        // Move to original + offset
        rect.DOLocalMove(_originalPos + new Vector3(5f, 5f, 0f), 0.1f).SetEase(Ease.OutQuad);
    
        // Optional: If you have a CanvasGroup, you can fade it
        // obj.GetComponent<CanvasGroup>().DOFade(1, 0.2f);

        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator MapDissapearAnimation(GameObject go)
    {
        GameObject obj = go.GetComponent<Map>() != null ? go : go.transform.parent.gameObject;
        RectTransform rect = obj.GetComponent<RectTransform>();
        
        rect.DOKill();

        // Animation: Return to EXACT original position
        rect.DOLocalMove(_originalPos, 0.1f).SetEase(Ease.InQuad);
    
        // Optional: Fade out
        // obj.GetComponent<CanvasGroup>().DOFade(0, 0.2f);

        yield return new WaitForSeconds(0.1f);
    
        // Now actually hide it
        obj.GetComponent<Image>().enabled = true;
    }

    
    

    public void HidePanels()
    {
        for (int i = 0; i < mapPanels.Length; i++)
        {
            GameObject panel = mapPanels[i];
            panel.SetActive(false);
        }
    }

    public void Zoom()
    {
        StartCoroutine(ZoomInAnimation());
    }

    IEnumerator ZoomInAnimation()
    {
        map.transform.DOScale(Vector3.one * 16, 0.2f);
        
        for (int i = 0; i < mapImages.Length; i++)
        {
            mapImages[i].SetActive(false);
        }
        yield return new WaitForSeconds(0.2f);
        foreach (var button in HanoiButtons)
        {
            button.SetActive(true);
        }
    }

    public void ZoomOut()
    {
        StartCoroutine(ZoomOutAnimation());
    }
    
    IEnumerator ZoomOutAnimation()
    {
        map.transform.DOScale(Vector3.one , 0.2f);
        yield return new WaitForSecondsRealtime(0.2f);
        for (int i = 0; i < mapImages.Length; i++)
        {
            mapImages[i].SetActive(true);
        }
        foreach (var button in HanoiButtons)
        {
            button.SetActive(false);
        }
    }
}
