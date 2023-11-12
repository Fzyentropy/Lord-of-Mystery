using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerPerspective : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject infoPanelPrefab;
    private GameObject selectedObject;
    private Vector3 initialMousePosition;
    private Plane dragPlane;
    private bool isDragging;
    
    public float CameraMoveOffset = 3f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            initialMousePosition = Input.mousePosition;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                selectedObject = hit.collider.gameObject;
                dragPlane = new Plane(mainCamera.transform.forward, hit.point);
            }
            else
            {
                selectedObject = null;
            }

            isDragging = false;
        }
        else if (Input.GetMouseButton(0))
        {
            float dragDistance = Vector3.Distance(initialMousePosition, Input.mousePosition);

            if (dragDistance >= 10) // Adjust the threshold value as needed
            {
                isDragging = true;
            }

            if (selectedObject != null && isDragging)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                float distance;
                if (dragPlane.Raycast(ray, out distance))
                {
                    Vector3 hitPoint = ray.GetPoint(distance);
                    selectedObject.transform.position = new Vector3(hitPoint.x, hitPoint.y, selectedObject.transform.position.z);
                }
            }
            else if (isDragging)
            {
                Vector3 delta = mainCamera.ScreenToViewportPoint(initialMousePosition - Input.mousePosition);
                Vector3 move = new Vector3(delta.x, delta.y, 0) * CameraMoveOffset;

                mainCamera.transform.Translate(move, Space.World);
                initialMousePosition = Input.mousePosition;
            }
        }
        else if (Input.GetMouseButtonUp(0) && !isDragging)
        {
            if (selectedObject != null)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    ShowInfoPanel(hit.point);
                }
            }
            selectedObject = null;
        }
    }

    private void ShowInfoPanel(Vector3 position)
    {
        GameObject panel = Instantiate(infoPanelPrefab, position, Quaternion.identity);
        panel.transform.localScale = Vector3.zero;
        StartCoroutine(ScaleUp(panel.transform));
    }

    private IEnumerator ScaleUp(Transform transform)
    {
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            yield return null;
        }

        transform.localScale = Vector3.one;
    }
}


