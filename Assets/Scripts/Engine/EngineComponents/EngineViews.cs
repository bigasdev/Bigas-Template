using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class EngineViews : EngineComponent
{
    public const string viewsPath = "Prefabs/UI/View";
    public List<View> views;
    public override void Initialize()
    {
        var v = Resources.LoadAll<View>(viewsPath);
        views = new List<View>(v);
        ReturnAllViewsInFolder();
    }
    public void ReturnAllViewsInFolder(){
        foreach(var v in views){
            Debug.Log($"FOUND A VIEW : {v.name}");
        }
    }
    public T GetView<T>() where T:View{
        var view = views.Find(x => x is T) as T;
        if(view.closeLastOneWhenOpen){
            CloseLastOpen();
        }
        return views.Find(x => x is T) as T;
    }
    public bool CloseLastOpen(){
        var lastView = FindLastOpenView();
        if(lastView != null){
            lastView.Close();
            return true;
        }
        return false;
    }
    View FindLastOpenView(){
        List<View> views = new List<View>(FindObjectsOfType<View>());
        if(views.Count <= 0) return null;
        Debug.Log($"Starting loop through {views.Count} elements");
        for(int i = views.Count - 1; i >= 0; i--){
            if(!views[i].shouldBeClosed){
                Debug.Log($"View {views[i]} cant be closed.... continuing loop");
            }else{
                Debug.Log($"{views[i]} can be closed!");
                return views[i];
            }
        }
        Debug.Log("Couldnt find anything you are looking for, check it.");
        return null;
    }
}
