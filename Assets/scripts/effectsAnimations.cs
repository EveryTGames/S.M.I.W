using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


//animation controller for the break effect
public class effectsAnimations : MonoBehaviour
{
    public static TileBase[] BreakingAnimationFrames;
    [SerializeField] TileBase[] _BreakingAnimationFrames;
    private void Awake() {
        BreakingAnimationFrames = _BreakingAnimationFrames;
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
