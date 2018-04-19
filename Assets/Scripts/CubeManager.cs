using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CubeManager : MonoBehaviour
{

    bool availableToRotate;

    // Use this for initialization
    void Start()
    {
        availableToRotate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (availableToRotate)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                StartCoroutine(RotateForward());
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                StartCoroutine(RotateForward());
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                StartCoroutine(RotateBack());
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                StartCoroutine(RotateBack());
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                StartCoroutine(RotateLeft());
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(RotateLeft());
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                StartCoroutine(RotateRight());
            }

            if (Input.GetKeyDown(KeyCode.D))
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
        float VectorX;
        float VectorZ;

        if (Mathf.Round(currentAngle) == 90 || Mathf.Round(currentAngle) == 270)
        {
            VectorX = transform.localPosition[0] + (transform.localScale[0]);
        }

        else
        {
            VectorX = transform.localPosition[0] + (transform.localScale[0] / 2);
        }


        VectorZ = transform.localPosition[2];

        Vector3 VectorToUse = new Vector3(VectorX, 0, VectorZ);

        if ((int)transform.localEulerAngles[0] == 90 || (int)transform.localEulerAngles[0] == 270)
        {
            transform.localEulerAngles = new Vector3(0, 90, -90);

            int startEulerY = (int)transform.localEulerAngles[1];
            int startEulerZ = (int)transform.localEulerAngles[2];

            angleToReach = 90;

            currentAngle = 0;

            while (currentAngle < angleToReach)
            {

                transform.RotateAround(VectorToUse, new Vector3(0, 0, -1), 100 * Time.deltaTime);
                currentAngle = (int)transform.localEulerAngles[0];

                if (startEulerY != (int)transform.localEulerAngles[1] || startEulerZ != (int)transform.localEulerAngles[2])
                {
                    transform.localEulerAngles = new Vector3(Mathf.Round(angleToReach), 90, 90);
                    break;
                }

                yield return null;
            }
        }


        else
        {
            while (currentAngle > angleToReach)
            {
                transform.RotateAround(VectorToUse, new Vector3(0, 0, -1), 100 * Time.deltaTime);
                currentAngle = (int)transform.localEulerAngles[2];

                yield return null;
            }

            transform.localEulerAngles = new Vector3(transform.localEulerAngles[0], transform.localEulerAngles[1], Mathf.Round(angleToReach));
        }

        transform.localPosition = new Vector3(Mathf.Round(transform.localPosition[0]), Mathf.Round(transform.localPosition[1]), Mathf.Round(transform.localPosition[2]));

        availableToRotate = true;
    }

    IEnumerator RotateLeft()
    {
        availableToRotate = false;

        int currentAngle = (int)transform.localEulerAngles[2];

        float VectorX;
        float VectorZ;

        print(currentAngle);

        if (Mathf.Round(currentAngle) == 90 || Mathf.Round(currentAngle) == 270)
        {
            VectorX = transform.localPosition[0] - (transform.localScale[0]);
        }

        else
        {
            VectorX = transform.localPosition[0] - (transform.localScale[0] / 2);
        }



        VectorZ = transform.localPosition[2];

        Vector3 VectorToUse = new Vector3(VectorX, 0, VectorZ);

        if ((int)transform.localEulerAngles[0] == 90 || (int)transform.localEulerAngles[0] == 270)
        {
            if (currentAngle == 0)
            {
                currentAngle = 360;
            }

            transform.localEulerAngles = new Vector3(0, 90, -90);

            int startEulerY = (int)transform.localEulerAngles[1];
            int startEulerZ = (int)transform.localEulerAngles[2];
            
            int angleToReach = 90;

            while (currentAngle > angleToReach)
            {
                transform.RotateAround(VectorToUse, new Vector3(0, 0, 1), 100 * Time.deltaTime);

                currentAngle = (int)transform.localEulerAngles[0];

                if (startEulerY != (int)transform.localEulerAngles[1] || startEulerZ != (int)transform.localEulerAngles[2])
                {
                    transform.localEulerAngles = new Vector3(Mathf.Round(angleToReach), 90, 90);
                    break;
                }
                
                yield return null;
            }
        }


        else
        {

            int angleToReach = currentAngle + 90;
            
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

            transform.localEulerAngles = new Vector3(transform.localEulerAngles[0], transform.localEulerAngles[1], Mathf.Round(angleToReach));
        }

        transform.localPosition = new Vector3(Mathf.Round(transform.localPosition[0]), Mathf.Round(transform.localPosition[1]), Mathf.Round(transform.localPosition[2]));

        availableToRotate = true;
    }

    IEnumerator RotateBack()
    {

        availableToRotate = false;

        int currentAngle = (int)transform.localEulerAngles[0];


        int angleToReach = currentAngle - 90;

        float VectorX = transform.localPosition[0];

        float VectorZ;

        int startEuler = (int)transform.localEulerAngles[2];

        if (Mathf.Round(currentAngle) == 90 || Mathf.Round(currentAngle) == 270)
        {
            VectorZ = transform.localPosition[2] - (transform.localScale[2]);
        }

        else
        {
            VectorZ = transform.localPosition[2] - (transform.localScale[2] / 2);
        }

        Vector3 VectorToUse = new Vector3(VectorX, 0, VectorZ);


        int startEulerX = (int)transform.localEulerAngles[0];

        if ((int)transform.localEulerAngles[2] == 90 || (int)transform.localEulerAngles[2] == 270)
        {
            transform.localEulerAngles = new Vector3(90, -90, 0);

            int startEulerY = (int)transform.localEulerAngles[1];
            int startEulerZ = (int)transform.localEulerAngles[2];


            angleToReach = 90;

            currentAngle = 0;

            while (currentAngle < angleToReach)
            {
                transform.RotateAround(VectorToUse, new Vector3(-1, 0, 0), 100 * Time.deltaTime);

                currentAngle = (int)transform.localEulerAngles[0];

                yield return null;
            }

            transform.localEulerAngles = new Vector3(0, 0, Mathf.Round(angleToReach));
        }

        else
        {
            Debug.Log("Potato");
            while (currentAngle > angleToReach)
            {
                transform.RotateAround(VectorToUse, new Vector3(-1, 0, 0), 100 * Time.deltaTime);

                currentAngle = (int)transform.localEulerAngles[0];

                if (startEuler != (int)transform.localEulerAngles[2] && startEulerX != 90 && startEulerX != 180 && startEulerX != 270)
                {
                    Debug.Log("NANI");
                    break;
                }

                yield return null;
            }

            transform.localEulerAngles = new Vector3(Mathf.Round(angleToReach), transform.localEulerAngles[1], transform.localEulerAngles[2]);
        }
        
        transform.localPosition = new Vector3(Mathf.Round(transform.localPosition[0]), Mathf.Round(transform.localPosition[1]), Mathf.Round(transform.localPosition[2]));

        availableToRotate = true;
    }

    IEnumerator RotateForward()
    {
        availableToRotate = false;

        int currentAngle = (int)transform.localEulerAngles[0];

        int angleToReach = currentAngle + 90;

        float VectorX = transform.localPosition[0];

        float VectorZ;

        int startEuler = (int)transform.localEulerAngles[2];

        if (Mathf.Round(currentAngle) == 90 || Mathf.Round(currentAngle) == 270)
        {
            VectorZ = transform.localPosition[2] + (transform.localScale[2]);

        }

        else
        {
            VectorZ = transform.localPosition[2] + (transform.localScale[2] / 2);
        }

        Vector3 VectorToUse = new Vector3(VectorX, 0, VectorZ);


        int startEulerX = (int)transform.localEulerAngles[0];

        if ((int)transform.localEulerAngles[2] == 90 || (int)transform.localEulerAngles[2] == 270)
        {

            transform.localEulerAngles = new Vector3(90, -90, 0);

            int startEulerY = (int)transform.localEulerAngles[1];
            int startEulerZ = (int)transform.localEulerAngles[2];

            angleToReach = 90;

            currentAngle = 0;

            while (currentAngle < angleToReach)
            {
                transform.RotateAround(VectorToUse, new Vector3(1, 0, 0), 100 * Time.deltaTime);

                currentAngle = (int)transform.localEulerAngles[0];

                yield return null;
            }

            transform.localEulerAngles = new Vector3(0, 0, Mathf.Round(angleToReach));
        }

        else
        {
            while (currentAngle < angleToReach)
            {
                transform.RotateAround(VectorToUse, new Vector3(1, 0, 0), 100 * Time.deltaTime);

                currentAngle = (int)transform.localEulerAngles[0];

                if (startEuler != (int)transform.localEulerAngles[2] && startEulerX != 90 && startEulerX != 180 && startEulerX != 270)
                {
                    break;
                }

                yield return null;
            }

            transform.localEulerAngles = new Vector3(Mathf.Round(angleToReach), transform.localEulerAngles[1], transform.localEulerAngles[2]);
        }
        
        transform.localPosition = new Vector3(Mathf.Round(transform.localPosition[0]), Mathf.Round(transform.localPosition[1]), Mathf.Round(transform.localPosition[2]));

        availableToRotate = true;
    }

}
