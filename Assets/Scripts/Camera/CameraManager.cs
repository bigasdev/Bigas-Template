using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
public class CameraManager : MonoBehaviour
{
    private static CameraManager instance;
    public static CameraManager Instance{
        get{
            if(instance == null){
                instance = FindObjectOfType<CameraManager>();
            }
            return instance;
        }
    }
    [SerializeField] float offset;
    [SerializeField] float damping;
    public void killShake() => OnCameraShake(.05f, .25f);
    public void critShake() => OnCameraShake(.025f, .125f);
    public static void CritShake() => CameraManager.Instance.critShake();
    public static void KillShake() => CameraManager.Instance.killShake();
    //Parameter to set an action to be thrown at the end of the zoom
    public event Action OnEndZoom = delegate{};
    public Camera mainCamera;
    private Vector2 p = Vector2.zero;
    Coroutine shakeRoutine, zoomRoutine;
    private void Start() {
        this.transform.parent = null;
    }
    private void Update() {
        if(Player.Instance == null)return;
        var mousePos = Engine.Mouse.MousePos;
        var player = Player.Instance.transform.position;
        if(Vector2.Distance(mousePos, player) > 14 && p == Vector2.zero){
            p = mousePos;
            SmoothFollowMouse(p, player);
            return;
        }
        SmoothFollowMouse(mousePos, player);
    }
    void SmoothFollowMouse(Vector2 a, Vector2 b){
        var c = (a+b)/2;
        var xx = (b.x + c.x)/2;
        var yy = (b.y + c.y)/2;
        this.transform.position = Vector2.Lerp(this.transform.position, new Vector2(xx,yy), damping * Time.deltaTime);
    }
    public void OnCameraShake(float duration, float magnitude) {
        if (shakeRoutine != null) {
            return;
        }
        shakeRoutine = StartCoroutine(Shake(duration, magnitude));
    }
    private IEnumerator Shake(float duration, float magnitude) {
        Vector3 originalPos3 = mainCamera.transform.localPosition;
        Vector2 originalPos = mainCamera.transform.localPosition;
        float elapsedTime = 0f;
        while (elapsedTime < duration) {
            mainCamera.transform.localPosition = new Vector3(originalPos.x + Random.insideUnitCircle.x * magnitude, originalPos.y + Random.insideUnitCircle.y * magnitude, -10f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.localPosition = originalPos3;
        //DefaultPosition();
        shakeRoutine = null;
    }
    //Methods to manage the camera zoom
    //We need a void function to control in a global way the ienumerator
    public void OnCameraZoom(float amount, float speed = .25f){
        if(zoomRoutine != null)return;
        zoomRoutine = StartCoroutine(Zoom(amount, speed));
    }
    private IEnumerator Zoom(float amount, float speed = .25f){
        float size = mainCamera.orthographicSize - amount;
        while(mainCamera.orthographicSize > size){
            mainCamera.orthographicSize = Mathf.MoveTowards(mainCamera.orthographicSize, size, speed * Time.deltaTime);
            yield return null;
        }
        OnEndZoom();
        zoomRoutine = null;
    }
}
