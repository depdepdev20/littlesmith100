using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusicSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MusicManager.Instance.PlayMusic("main");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
