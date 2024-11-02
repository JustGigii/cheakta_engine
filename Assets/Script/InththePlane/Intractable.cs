using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intractable : MonoBehaviour
{
    [SerializeField][Range(0.0f, 10f)] float distance = 3f;
    public LayerMask layerMask;
    public GameObject highlight;

    private GameObject oldhighlight;
    private loadjson jsondetails;

    // Start is called before the first frame update
    void Start()
    {
        jsondetails =  GameObject.Find("json").GetComponent<loadjson>();
        this.oldhighlight = this.gameObject;
 
    }

    // Update is called once per frame
    void Update()
    {
        if (highlight != null)
        {

            highlight.gameObject.GetComponent<Outline>().enabled = false;
            highlight = null;
        }
      //  PlayerInteract();
    }
    public void InteractSate(bool isInteract)
    {
        if(isInteract)
        {
            PlayerInteract();
            return;
        }
        oldhighlight = this.gameObject;
        highlight = null;
    }
    public void PlayerInteract()
    {
        Ray ray = new Ray(this.transform.position, this.GetComponent<Camera>().transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, layerMask))
        {
            if (jsondetails.plan.Study.ContainsKey(hitInfo.transform.name))
                  OutLineObject(hitInfo);
        }
        else
        {

            highlight = null;
            oldhighlight = gameObject;
        }
    }

    private void OutLineObject(RaycastHit raycastHit)
    {
        highlight = raycastHit.transform.gameObject;
        if (highlight.gameObject.GetComponent<Outline>() != null)
        {
            highlight.gameObject.GetComponent<Outline>().enabled = true;
        }
        else
        {
            Outline outline = highlight.gameObject.AddComponent<Outline>();
            outline.enabled = true;

        }
        if (!oldhighlight.Equals(highlight))
        {
            oldhighlight = highlight;
            gameObject.GetComponent<OutlineAnimation>().playScanAnimtion = true;
        }

    }
}
