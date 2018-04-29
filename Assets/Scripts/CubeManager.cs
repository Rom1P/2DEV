using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CubeManager : MonoBehaviour
{

    bool availableToRotate;

    public GameObject GameManager;

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

        int currentAngle;
        int angleToReach;

        if ((int)transform.localEulerAngles[2] == 90 || (int)transform.localEulerAngles[2] == 270)
        {

            /* When Cube is already rotated horizontally (Axis X) */

            transform.localEulerAngles = new Vector3(0, 0, 90);

            currentAngle = 90;
            angleToReach = 0;

            int VectorX;
            int VectorZ;

            VectorX = (int)transform.localPosition[0] + (int)(transform.localScale[0]);
            VectorZ = (int)transform.localPosition[2];

            Vector3 VectorToUse = new Vector3(VectorX, 0, VectorZ);

            while (currentAngle > angleToReach)
            {
                transform.RotateAround(VectorToUse, new Vector3(0, 0, -1), 100 * Time.deltaTime);
                currentAngle = (int)transform.localEulerAngles[2];

                if (currentAngle > angleToReach + 90 || currentAngle < angleToReach)
                {
                    break;
                }

                yield return null;
            }


            transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        else if ((int)transform.localEulerAngles[0] == 90 || (int)transform.localEulerAngles[0] == 270)
        {

            /* When Cube is already rotated vertically (Axis Z) */

            transform.localEulerAngles = new Vector3(0, 90, 90);

            currentAngle = 0;
            angleToReach = 90;

            int VectorX;
            int VectorZ;

            VectorX = (int)transform.localPosition[0] + (int)(transform.localScale[0] / 2);
            VectorZ = (int)transform.localPosition[2];

            Vector3 VectorToUse = new Vector3(VectorX, 0, VectorZ);

            while (currentAngle < angleToReach)
            {
                transform.RotateAround(VectorToUse, new Vector3(0, 0, -1), 100 * Time.deltaTime);
                currentAngle = (int)transform.localEulerAngles[0];

                if (currentAngle < angleToReach - 90 || currentAngle > angleToReach)
                {
                    break;
                }

                if (transform.localEulerAngles[1] == 270)
                {
                    break;
                }

                yield return null;
            }

            transform.localEulerAngles = new Vector3(90, 0, 0);

        }

        else
        {

            /* When Cube is standing up */

            transform.localEulerAngles = new Vector3(0, 0, 0);

            currentAngle = 360;
            angleToReach = 270;

            int VectorX;
            int VectorZ;

            VectorX = (int)transform.localPosition[0] + (int)(transform.localScale[0] / 2);
            VectorZ = (int)transform.localPosition[2];

            Vector3 VectorToUse = new Vector3(VectorX, 0, VectorZ);

            while (currentAngle > angleToReach)
            {
                transform.RotateAround(VectorToUse, new Vector3(0, 0, -1), 100 * Time.deltaTime);
                currentAngle = (int)transform.localEulerAngles[2];

                if (currentAngle > angleToReach + 90 || currentAngle < angleToReach)
                {
                    break;
                }

                yield return null;
            }

            transform.localEulerAngles = new Vector3(0, 0, 270);

        }

        transform.localPosition = new Vector3(Mathf.Round(transform.localPosition[0]), Mathf.Round(transform.localPosition[1]), Mathf.Round(transform.localPosition[2]));

        availableToRotate = true;

        GameManager.SendMessage("ReceiveAfterRotate");
    }

    IEnumerator RotateLeft()
    {
        availableToRotate = false;

        int currentAngle;
        int angleToReach;

        if ((int)transform.localEulerAngles[2] == 90 || (int)transform.localEulerAngles[2] == 270)
        {

            /* When Cube is already rotated horizontally (Axis X) */

            transform.localEulerAngles = new Vector3(0, 0, -90);

            currentAngle = 270;
            angleToReach = 360;

            int VectorX;
            int VectorZ;

            VectorX = (int)transform.localPosition[0] - (int)(transform.localScale[0]);
            VectorZ = (int)transform.localPosition[2];

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


            transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        else if ((int)transform.localEulerAngles[0] == 90 || (int)transform.localEulerAngles[0] == 270)
        {

            /* When Cube is already rotated vertically (Axis Z) */

            transform.localEulerAngles = new Vector3(0, 90, 90);

            currentAngle = 360;
            angleToReach = 270;

            int VectorX;
            int VectorZ;

            VectorX = (int)transform.localPosition[0] - (int)(transform.localScale[0] / 2);
            VectorZ = (int)transform.localPosition[2];

            Vector3 VectorToUse = new Vector3(VectorX, 0, VectorZ);

            while (currentAngle > angleToReach)
            {
                transform.RotateAround(VectorToUse, new Vector3(0, 0, 1), 100 * Time.deltaTime);
                currentAngle = (int)transform.localEulerAngles[0];

                if (currentAngle > angleToReach + 90 || currentAngle < angleToReach)
                {
                    break;
                }

                if (transform.localEulerAngles[1] == 270)
                {
                    break;
                }

                yield return null;
            }

            transform.localEulerAngles = new Vector3(270, 0, 0);

        }

        else
        {

            /* When Cube is standing up */

            transform.localEulerAngles = new Vector3(0, 0, 0);

            currentAngle = 0;
            angleToReach = 90;

            int VectorX;
            int VectorZ;

            VectorX = (int)transform.localPosition[0] - (int)(transform.localScale[0] / 2);
            VectorZ = (int)transform.localPosition[2];

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

            transform.localEulerAngles = new Vector3(0, 0, 90);

        }

        transform.localPosition = new Vector3(Mathf.Round(transform.localPosition[0]), Mathf.Round(transform.localPosition[1]), Mathf.Round(transform.localPosition[2]));

        availableToRotate = true;

        GameManager.SendMessage("ReceiveAfterRotate");
    }

    IEnumerator RotateBack()
    {

        availableToRotate = false;

        int currentAngle;
        int angleToReach;

        if ((int)transform.localEulerAngles[2] == 90 || (int)transform.localEulerAngles[2] == 270)
        {

            /* When Cube is already rotated horizontally (Axis X) */

            transform.localEulerAngles = new Vector3(0, 0, 90);

            currentAngle = 360;
            angleToReach = 270;

            int VectorX;
            int VectorZ;

            VectorX = (int)transform.localPosition[0];
            VectorZ = (int)transform.localPosition[2] - (int)(transform.localScale[2] / 2);

            Vector3 VectorToUse = new Vector3(VectorX, 0, VectorZ);

            while (currentAngle > angleToReach)
            {
                transform.RotateAround(VectorToUse, new Vector3(-1, 0, 0), 100 * Time.deltaTime);
                currentAngle = (int)transform.localEulerAngles[0];

                if (currentAngle > angleToReach + 90 || currentAngle < angleToReach)
                {
                    break;
                }

                yield return null;
            }


            transform.localEulerAngles = new Vector3(0, 0, 90);
        }

        else if ((int)transform.localEulerAngles[0] == 90 || (int)transform.localEulerAngles[0] == 270)
        {

            /* When Cube is already rotated vertically (Axis Z) */

            transform.localEulerAngles = new Vector3(0, 90, 90);

            currentAngle = 0;
            angleToReach = 90;

            int VectorX;
            int VectorZ;

            VectorX = (int)transform.localPosition[0];
            VectorZ = (int)transform.localPosition[2] - (int)(transform.localScale[2]);

            Vector3 VectorToUse = new Vector3(VectorX, 0, VectorZ);

            while (currentAngle < angleToReach)
            {
                transform.RotateAround(VectorToUse, new Vector3(-1, 0, 0), 100 * Time.deltaTime);
                currentAngle = (int)transform.localEulerAngles[2];

                if (currentAngle < angleToReach - 90 || currentAngle > angleToReach)
                {
                    break;
                }

                if (transform.localEulerAngles[1] == 270)
                {
                    break;
                }

                yield return null;
            }

            transform.localEulerAngles = new Vector3(0, 0, 0);

        }

        else
        {

            /* When Cube is standing up */

            transform.localEulerAngles = new Vector3(0, 0, 0);

            currentAngle = 360;
            angleToReach = 270;

            int VectorX;
            int VectorZ;

            VectorX = (int)transform.localPosition[0];
            VectorZ = (int)transform.localPosition[2] - (int)(transform.localScale[2] / 2);

            Vector3 VectorToUse = new Vector3(VectorX, 0, VectorZ);

            while (currentAngle > angleToReach)
            {
                transform.RotateAround(VectorToUse, new Vector3(-1, 0, 0), 100 * Time.deltaTime);
                currentAngle = (int)transform.localEulerAngles[0];

                if (currentAngle > angleToReach + 90 || currentAngle < angleToReach)
                {
                    break;
                }

                yield return null;
            }

            transform.localEulerAngles = new Vector3(270, 0, 0);

        }

        transform.localPosition = new Vector3(Mathf.Round(transform.localPosition[0]), Mathf.Round(transform.localPosition[1]), Mathf.Round(transform.localPosition[2]));

        availableToRotate = true;

        GameManager.SendMessage("ReceiveAfterRotate");

    }

    IEnumerator RotateForward()
    {
        availableToRotate = false;

        int currentAngle;
        int angleToReach;

        if ((int)transform.localEulerAngles[2] == 90 || (int)transform.localEulerAngles[2] == 270)
        {

            /* When Cube is already rotated horizontally (Axis X) */

            transform.localEulerAngles = new Vector3(0, 0, 90);

            currentAngle = 0;
            angleToReach = 90;

            int VectorX;
            int VectorZ;

            VectorX = (int)transform.localPosition[0];
            VectorZ = (int)transform.localPosition[2] + (int)(transform.localScale[2] / 2);

            Vector3 VectorToUse = new Vector3(VectorX, 0, VectorZ);

            while (currentAngle < angleToReach)
            {
                transform.RotateAround(VectorToUse, new Vector3(1, 0, 0), 100 * Time.deltaTime);
                currentAngle = (int)transform.localEulerAngles[0];

                if (currentAngle < angleToReach - 90 || currentAngle > angleToReach)
                {
                    break;
                }

                if (transform.localEulerAngles[1] == 270 || transform.localEulerAngles[2] != 90)
                {
                    break;
                }

                yield return null;
            }


            transform.localEulerAngles = new Vector3(0, 0, 90);
        }

        else if ((int)transform.localEulerAngles[0] == 90 || (int)transform.localEulerAngles[0] == 270)
        {

            /* When Cube is already rotated vertically (Axis Z) */

            transform.localEulerAngles = new Vector3(0, 90, 90);

            currentAngle = 90;
            angleToReach = 180;

            int VectorX;
            int VectorZ;

            VectorX = (int)transform.localPosition[0];
            VectorZ = (int)transform.localPosition[2] + (int)(transform.localScale[2]);

            Vector3 VectorToUse = new Vector3(VectorX, 0, VectorZ);

            while (currentAngle < angleToReach)
            {
                transform.RotateAround(VectorToUse, new Vector3(1, 0, 0), 100 * Time.deltaTime);
                currentAngle = (int)transform.localEulerAngles[2];
                
                if (currentAngle < angleToReach - 90 || currentAngle > angleToReach)
                {
                    break;
                }

                if (transform.localEulerAngles[1] == 270)
                {
                    break;
                }

                yield return null;
            }

            transform.localEulerAngles = new Vector3(0, 0, 0);

        }

        else
        {

            /* When Cube is standing up */

            transform.localEulerAngles = new Vector3(0, 0, 0);

            currentAngle = 0;
            angleToReach = 90;

            int VectorX;
            int VectorZ;

            VectorX = (int)transform.localPosition[0];
            VectorZ = (int)transform.localPosition[2] + (int)(transform.localScale[2] / 2);

            Vector3 VectorToUse = new Vector3(VectorX, 0, VectorZ);

            while (currentAngle < angleToReach)
            {
                transform.RotateAround(VectorToUse, new Vector3(1, 0, 0), 100 * Time.deltaTime);
                currentAngle = (int)transform.localEulerAngles[0];

                if (currentAngle < angleToReach - 90 || currentAngle > angleToReach)
                {
                    break;
                }


                if (transform.localEulerAngles[1] != 0)
                {
                    break;
                }

                yield return null;
            }

            transform.localEulerAngles = new Vector3(270, 0, 0);

        }

        transform.localPosition = new Vector3(Mathf.Round(transform.localPosition[0]), Mathf.Round(transform.localPosition[1]), Mathf.Round(transform.localPosition[2]));

        availableToRotate = true;

        GameManager.SendMessage("ReceiveAfterRotate");

    }

    void Loose()
    {
        availableToRotate = false;
    }

    void Restart()
    {
        availableToRotate = true;
    }

}
