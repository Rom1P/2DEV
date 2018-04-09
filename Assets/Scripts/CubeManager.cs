using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CubeManager : MonoBehaviour
{

    bool availableToRotate = true;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {




        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (availableToRotate)
            {
                StartCoroutine(RotateForward());
            };
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (availableToRotate)
            {
                StartCoroutine(RotateForward());
            };
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (availableToRotate)
            {
                StartCoroutine(RotateBack());
            };
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (availableToRotate)
            {
                StartCoroutine(RotateBack());
            };
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (availableToRotate)
            {
                StartCoroutine(RotateLeft());
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (availableToRotate)
            {
                StartCoroutine(RotateLeft());
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (availableToRotate)
            {
                StartCoroutine(RotateRight());
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (availableToRotate)
            {
                StartCoroutine(RotateRight());
            }
        }
    }

    IEnumerator RotateRight()
    {
        availableToRotate = false;

        int currentAngle = (int)transform.localEulerAngles[2];

        if (currentAngle == 0)
        {
            currentAngle = 360;
        }

        int angleToReach = currentAngle - 90;

        float VectorX = transform.localPosition[0] + (transform.localScale[0] / 2);
        float VectorZ = transform.localPosition[2] + (transform.localScale[2] / 2);

        Vector3 VectorToUse = new Vector3(VectorX, 0, VectorZ);

        while (currentAngle > angleToReach)
        {
            transform.RotateAround(VectorToUse, new Vector3(0, 0, -1), 100 * Time.deltaTime);

            currentAngle = (int)transform.localEulerAngles[2];

            Debug.Log(transform.localEulerAngles[2]);

            Debug.Log(currentAngle);

            yield return null;
        }

        transform.localEulerAngles = new Vector3(transform.localEulerAngles[0], transform.localEulerAngles[1], (float)angleToReach);

        transform.localPosition = new Vector3(Mathf.Round(transform.localPosition[0]), Mathf.Round(transform.localPosition[1]), Mathf.Round(transform.localPosition[2]));

        availableToRotate = true;
    }

    IEnumerator RotateLeft()
    {

        availableToRotate = false;

        float currentAngle = transform.localEulerAngles[2];




        float angleToReach = currentAngle + 90;



        Debug.Log(currentAngle);

        Debug.Log(angleToReach);




        float VectorX = transform.localPosition[0] - (transform.localScale[0] / 2);
        float VectorZ = transform.localPosition[2] + (transform.localScale[2] / 2);

        Vector3 VectorToUse = new Vector3(VectorX, 0, VectorZ);



        while (currentAngle < angleToReach)
        {
            transform.RotateAround(VectorToUse, new Vector3(0, 0, 1), 100 * Time.deltaTime);

            currentAngle = (int)transform.localEulerAngles[2];

            if (currentAngle < angleToReach - 90 || currentAngle > angleToReach)
            {
                break;
            }

            yield return null;
        }



        transform.localEulerAngles = new Vector3(transform.localEulerAngles[0], transform.localEulerAngles[1], (float)angleToReach);

        transform.localPosition = new Vector3(Mathf.Round(transform.localPosition[0]), Mathf.Round(transform.localPosition[1]), Mathf.Round(transform.localPosition[2]));

        availableToRotate = true;
    }

    IEnumerator RotateBack()
    {

        availableToRotate = false;

        int currentAngle = (int)transform.localEulerAngles[0];

        if (currentAngle == 0)
        {
            currentAngle = 360;
        }

        Debug.Log(currentAngle);

        int angleToReach = currentAngle - 90;

        Debug.Log(angleToReach);

        float VectorX = transform.localPosition[0];
        float VectorZ = transform.localPosition[2] - (transform.localScale[2] / 2);

        Vector3 VectorToUse = new Vector3(VectorX, 0, VectorZ);

        while (currentAngle > angleToReach)
        {
            transform.RotateAround(VectorToUse, new Vector3(-1, 0, 0), 100 * Time.deltaTime);

            currentAngle = (int)transform.localEulerAngles[0];

            Debug.Log(transform.localEulerAngles[0]);

            Debug.Log(currentAngle);

            yield return null;
        }

        transform.localEulerAngles = new Vector3((float)angleToReach, transform.localEulerAngles[1], transform.localEulerAngles[2]);

        transform.localPosition = new Vector3(Mathf.Round(transform.localPosition[0]), Mathf.Round(transform.localPosition[1]), Mathf.Round(transform.localPosition[2]));

        availableToRotate = true;
    }

    IEnumerator RotateForward()
    {

        availableToRotate = false;

        int currentAngle = (int)transform.localEulerAngles[0];



        int angleToReach = currentAngle + 90;

        Debug.Log(angleToReach);

        float VectorX = transform.localPosition[0];
        float VectorZ = transform.localPosition[2] + (transform.localScale[2] / 2);

        Vector3 VectorToUse = new Vector3(5, 0, 10);

        while (currentAngle < angleToReach)
        {
            transform.RotateAround(VectorToUse, new Vector3(1, 0, 0), 70 * Time.deltaTime);

            currentAngle = (int)transform.localEulerAngles[0];

            Debug.Log(transform.localEulerAngles);

            Debug.Log(transform.localEulerAngles[0]);

            if ((int)transform.localEulerAngles[1] == 180 || (int)transform.localEulerAngles[2] == 180)
            {
                print("lkkl");
                break;
            }

            yield return null;
        }

        transform.localEulerAngles = new Vector3((float)angleToReach, transform.localEulerAngles[1], transform.localEulerAngles[2]);

        transform.localPosition = new Vector3(Mathf.Round(transform.localPosition[0]), Mathf.Round(transform.localPosition[1]), Mathf.Round(transform.localPosition[2]));

        availableToRotate = true;
    }

}
