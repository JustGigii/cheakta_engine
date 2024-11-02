using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

namespace cakeslice
{
    public class OutlineAnimation : MonoBehaviour
    {
        bool pingPong = false;
        public bool playScanAnimtion = false;
        private bool isFirstTime;
        OutlineEffect outlineMannger;
        // Use this for initialization
        void Start()
        {
            outlineMannger = GetComponent<OutlineEffect>();

            isFirstTime= true;
        }

        // Update is called once per frame
        void Update()
        {
            if(playScanAnimtion)
            {
                scananimtaion();
            }
            else
            {
                isFirstTime = true;
                selected();
            }
        }

        void scananimtaion()
        {
            if(isFirstTime)
            {
                outlineMannger.fillAmount = 0 ;
                GetComponent<OutlineEffect>().lineColor0= Color.yellow;
                isFirstTime = false;
            }
            outlineMannger.fillAmount += Time.deltaTime/4.5f;
            if (outlineMannger.fillAmount >= 0.2f)
            {
                outlineMannger.fillAmount = 0.2f;
                GetComponent<OutlineEffect>().lineColor0 = Color.blue;
                    isFirstTime = true;
                    playScanAnimtion = false;
                
            }
            GetComponent<OutlineEffect>().UpdateMaterialsPublicProperties();


        }
        private void selected()
        {
            Color c = GetComponent<OutlineEffect>().lineColor0;

            if (pingPong)
            {
                c.a += Time.deltaTime;

                if (c.a >= 1)
                    pingPong = false;
            }
            else
            {
                c.a -= Time.deltaTime;

                if (c.a <= 0.5f)
                    pingPong = true;
            }

            c.a = Mathf.Clamp01(c.a);
            GetComponent<OutlineEffect>().lineColor0 = c;
            GetComponent<OutlineEffect>().UpdateMaterialsPublicProperties();
        }
    }
}