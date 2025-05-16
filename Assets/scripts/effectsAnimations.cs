using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


//animation controller for the break effect
public class effectsAnimations : MonoBehaviour
{
 [SerializeField]  public  TileAnimations[] _allAnimations = new TileAnimations[1]; //it is just public so that the Editor script can see it
    public static TileAnimations[] AllAnimations ;


    public static TileBase[] BreakingAnimationFrames;
    [SerializeField] TileBase[] _BreakingAnimationFrames;
    private void Awake()
    {
        BreakingAnimationFrames = _BreakingAnimationFrames;
        AllAnimations = _allAnimations;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

[Serializable]
public  class TileAnimations
{
    public  String Name; //name of the animation
[SerializeField]    public TileBase[] AnimationFrames; //the frames of this animation
    
}
