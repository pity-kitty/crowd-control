using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefab;
    private GameObject[] instanceArray = new GameObject[100];//AnimationInstancing[100];
    void Start()
    {
        // for(int i=0;i<10000;i++){
        //      Instantiate(prefab,new Vector3(i%100,0,Mathf.Floor(i/100)),Quaternion.identity);
        // }

        for (int i = 0; i < 1; i++)
        {
            instanceArray[i] = Instantiate(prefab, new Vector3(i % 1, 0, Mathf.Floor(i / 1)), Quaternion.identity);
            instanceArray[i].GetComponent<AnimationInstancing>().PlayAnimation(2);
            instanceArray[i].name = "Agents_1";
        }
    }

    private int index = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            index = 1 - index;
            for (int i = 0; i < 1; i++)
            {
                if (index == 0)
                {
                    instanceArray[i].GetComponent<AnimationInstancing>().CrossFade("Run", 0.2f);
                }
                else
                {
                    instanceArray[i].GetComponent<AnimationInstancing>().CrossFade("Ide", 0.2f);
                }
            }
        }
    }
}