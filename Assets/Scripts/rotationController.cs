using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class rotationController : MonoBehaviour
{
    public GameObject[] xPos;
    public GameObject[] yPos;
    public GameObject[] zPos;
    public GameObject[] xNeg;
    public GameObject[] yNeg;
    public GameObject[] zNeg;
    public GameObject[] x;
    public GameObject[] y;
    public GameObject[] z;


    public float rotationSpeed = 0.5f;
    float timeCount;


    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> xPosL = xPos.ToList();
        List<GameObject> yPosL = yPos.ToList();
        List<GameObject> zPosL = zPos.ToList();
        List<GameObject> xNegL = xNeg.ToList();
        List<GameObject> yNegL = yNeg.ToList();
        List<GameObject> zNegL = zNeg.ToList();
        List<GameObject> xL = x.ToList();
        List<GameObject> yL = y.ToList();
        List<GameObject> zL = z.ToList();



        foreach (Transform item in this.GetComponentsInChildren<Transform>())
        {
            if (item.gameObject.CompareTag("movable"))
            {
                if (item.gameObject.name.Contains("+x"))
                {
                    xPosL.Add(item.gameObject);
                    xPos = xPosL.ToArray();
                }
                if (item.gameObject.name.Contains("+y"))
                {
                    yPosL.Add(item.gameObject);
                    yPos = yPosL.ToArray();
                }
                if (item.gameObject.name.Contains("+z"))
                {
                    zPosL.Add(item.gameObject);
                    zPos = zPosL.ToArray();
                }
                if (item.gameObject.name.Contains("-x"))
                {
                    xNegL.Add(item.gameObject);
                    xNeg = xNegL.ToArray();
                }
                if (item.gameObject.name.Contains("-y"))
                {
                    yNegL.Add(item.gameObject);
                    yNeg = yNegL.ToArray();
                }
                if (item.gameObject.name.Contains("-z"))
                {
                    zNegL.Add(item.gameObject);
                    zNeg = zNegL.ToArray();
                }
                if (!item.gameObject.name.Contains("x"))
                {
                    xL.Add(item.gameObject);
                    x = xL.ToArray();
                }
                if (!item.gameObject.name.Contains("y"))
                {
                    yL.Add(item.gameObject);
                    y = yL.ToArray();
                }
                if (!item.gameObject.name.Contains("z"))
                {
                    zL.Add(item.gameObject);
                    z = zL.ToArray();
                }
            }
        }
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow)) StartCoroutine(rotateUp());
        if (Input.GetKeyDown(KeyCode.DownArrow)) StartCoroutine(rotateDown());
        if (Input.GetKeyDown(KeyCode.LeftArrow)) StartCoroutine(rotateLeft());
        if (Input.GetKeyDown(KeyCode.RightArrow)) StartCoroutine(rotateRight());

    }

    IEnumerator rotateUp() {
        Debug.Log("RotateUp();");
        float x = transform.eulerAngles.x;
        for (int i = 1; i < 91; i++)
        {
            transform.eulerAngles = new Vector3( x + i, transform.eulerAngles.y, transform.eulerAngles.z);
            Debug.Log(transform.eulerAngles.x);

            yield return new WaitForSeconds(0.001f);
        }

        yield return null;
    }

    IEnumerator rotateDown()
    {
        Debug.Log("RotateDown();");
        for (int i = 1; i < 91; i++)
        {
            transform.eulerAngles += new Vector3(-1, 0, 0);


            yield return new WaitForSeconds(0.001f);
        }

    }

    IEnumerator rotateLeft()
    {
        Debug.Log("RotateLeft();");
        for (int i = 1; i < 91; i++)
        {
            transform.eulerAngles += new Vector3(0, 1, 0);

            yield return new WaitForSeconds(0.001f);
        }
    }

    IEnumerator rotateRight()
    {
        Debug.Log("RotateRight();");
        for (int i = 1; i < 91; i++)
        {
            transform.eulerAngles += new Vector3(0, -1, 0);


            yield return new WaitForSeconds(0.001f);
        }
    }



}

