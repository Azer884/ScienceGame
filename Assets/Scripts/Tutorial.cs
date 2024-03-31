using System.Collections;
using System.Collections.Generic;
using LiquidVolumeFX;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public Outline[] outlines;
    public GameManager gameManager;
    private int UsesCount = 0;
    // Start is called before the first frame update
    void Awake()
    {
        outlines[0].enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.Object != null) 
        {   //Note
            if (outlines[0].transform.parent == gameManager.Object.transform && outlines[0].enabled)
            {
                outlines[0].enabled = false;

                outlines[1].enabled = true;
            }
            //Electrolysis to the Sink
            else if (outlines[1].transform.parent == gameManager.Object.transform && outlines[1].enabled && UsesCount == 0)
            {
                outlines[1].enabled = false;
                outlines[2].enabled = true;

                UsesCount = 1;
            }
            //Electrolysis to Pos
            else if (outlines[1].transform.parent == gameManager.Object.transform && outlines[1].enabled && outlines[1].transform.parent.GetChild(1).GetComponent<LiquidVolume>().liquidLayers[0].amount > .5f &&UsesCount == 1)
            {
                outlines[1].enabled = false;
                UsesCount = 2;

                outlines[3].enabled = true;
            }
            //Salt Container
            else if (outlines[4].transform.parent == gameManager.Object.transform && outlines[4].enabled)
            {
                outlines[4].enabled = false;

                outlines[1].enabled = true;
            }
            //Put Salt Into Electrolysis
            else if (outlines[1].transform.parent.GetChild(1).GetComponent<LiquidVolume>().liquidLayers[3].amount > 0f && outlines[1].enabled && UsesCount == 2)
            {
                outlines[1].enabled = false;
                UsesCount = 3;

                outlines[5].enabled = true;
            }
            //Cable
            else if (outlines[5].transform.parent == gameManager.Object.transform && outlines[5].enabled )
            {
                outlines[5].enabled = false;

                outlines[7].enabled = true;
            }
            //Clip
            else if (outlines[6].transform.parent == gameManager.Object.transform && outlines[6].enabled && UsesCount == 3)
            {
                outlines[6].enabled = false;
                UsesCount++;
                
                outlines[1].enabled = true;
            }
        }
        //Sink
        else if (FindAnyObjectByType<Sink>().IsOpened && outlines[2].enabled)
        {
            outlines[2].enabled = false;

            outlines[1].enabled = true;
        }
        //Pos
        else if (transform.GetChild(0).GetComponent<TutoTrigger>().Entered && outlines[3].enabled)
        {
            outlines[3].enabled = false;

            outlines[4].enabled = true;
        }
        //Connector
        else if (outlines[5].transform.parent.parent.GetComponent<ElectricityCheck>().start && outlines[7].enabled )
        {
            outlines[7].enabled = false;

            outlines[6].enabled = true;
        }
        //End
        else if (UsesCount == 4)
        {
            outlines[1].enabled = false;
        }
    }
}