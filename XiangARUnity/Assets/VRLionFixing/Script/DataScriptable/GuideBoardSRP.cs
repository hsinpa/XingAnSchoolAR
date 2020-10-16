using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GuideBoard", order = 1)]
public class GuideBoardSRP : ScriptableObject
{
    public string title;
    public string content_id;
    public Sprite sprite;

    public AudioClip chtAudioGuide;
    public AudioClip enAudioGuide;
}
