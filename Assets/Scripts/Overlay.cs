using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Overlay : MonoBehaviour
{
    public Slider slider;
    public float slideFactor;
    void Start()
    {
        slider.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = ThrowBall.slideValue*slideFactor;
    }
    public IEnumerator DestroyObj(GameObject obj, float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(obj);
    }
}
