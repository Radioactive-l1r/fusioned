using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiniMapController : MonoBehaviour, IPointerClickHandler
{

    public Transform Player;
    public Transform MapAnchor;
    public Camera miniMapCam;
    public float cameraMoveSpeed = 10f;
    Vector3 targerPosition;
   public bool isMoving=false;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RawImage>().rectTransform, eventData.pressPosition, eventData.pressEventCamera, out Vector2 localCursorPoint))
        {
            Rect imageRectSize = GetComponent<RawImage>().rectTransform.rect;

            
            localCursorPoint.x = (localCursorPoint.x - imageRectSize.x) / imageRectSize.width;
            localCursorPoint.y = (localCursorPoint.y - imageRectSize.y) / imageRectSize.height;

            CastMiniMapRayToWorld(localCursorPoint);
        }
    }

    private void CastMiniMapRayToWorld(Vector2 localCursor)
    {
        Ray miniMapRay = miniMapCam.ScreenPointToRay(new Vector2(localCursor.x * miniMapCam.pixelWidth, localCursor.y * miniMapCam.pixelHeight));

        if (Physics.Raycast(miniMapRay, out RaycastHit miniMapHit, Mathf.Infinity))
        {
            Debug.DrawRay(miniMapHit.point, miniMapHit.normal * 0.5f, Color.blue);
           // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
          //  cube.transform.position = miniMapHit.point;
            targerPosition = new Vector3(miniMapHit.point.x,miniMapCam.transform.position.y,miniMapHit.point.z);

        

        }
    }

    private void Update()
    {
        MapAnchor.transform.position = Player.transform.position;
        if (Input.GetMouseButtonDown(0))
        {

           isMoving = true;

        }

        if(isMoving)
        {
            miniMapCam.transform.position = Vector3.Lerp(miniMapCam.transform.position, targerPosition, Time.deltaTime * cameraMoveSpeed);
            if (Vector3.Distance(miniMapCam.transform.position, targerPosition) < 0.01f) // Adjust the threshold as needed
            {
                isMoving = false;
            }
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            miniMapCam.transform.localPosition = new Vector3(0,25,0);
        }

    }
}