using UnityEngine;
using System;
using System.IO;
using System.Text;
using PupilLabs;

public class PositionLogger : MonoBehaviour
{
    public string FolderName = "C:\\Users\\kem3481\\MEGAMOVE";
    public string FileName = "Positions";
    private string OutputDir;
    public Transform Camera;
    private Transform eye0InWorld;
    private Transform eye1InWorld;
    private Transform cyclopianEyeinWorld;
    private Transform cyclopianGazeinWorld;
    private Transform fixationWorld;
    public GameObject fixationPosition;
    private Transform particiapntFixating;
    private Transform participantGaze;
    private Transform gazeWorld0;
    private Transform gazeWorld1;
    public float angularDifference;

    //Things you want to write out, set them in the inspector
    public GameObject controller;
    ////public GameObject gaze;

    //Gives user control over when to start and stop recording, trigger this with spacebar;
    public bool startWriting;
    public SubscriptionsController PupilConnection;
    //Initialize some containers
    FileStream streams;
    FileStream trialStreams;
    StringBuilder stringBuilder = new StringBuilder();
    String writeString;
    Byte[] writebytes;

    private ControlLevel_Trials controlLevel;
    public GameObject manager;

    GazeListener gazeListener = null;

    Vector3 pl_gazedirection_wrtHead_xyz;
    Vector3 pl_E0_Norm_wrtHead_xyz;
    Vector3 pl_E1_Norm_wrtHead_xyz;

    float plConfidence;
    string plTimeStamp;
    float pl_gazedistance;

    int mode;

    void Start()
    {
        controlLevel = manager.GetComponent<ControlLevel_Trials>();

        // create a folder 
        string OutputDir = Path.Combine(FolderName, string.Concat(DateTime.Now.ToString("MM-dd-yyyy"), FileName));
        Directory.CreateDirectory(OutputDir);

        // create a file to record data
        String trialOutput = Path.Combine(OutputDir, DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + "_Results.txt");
        trialStreams = new FileStream(trialOutput, FileMode.Create, FileAccess.Write);

        gazeListener = new GazeListener(PupilConnection);
        Debug.Log(gazeListener);
        gazeListener.OnReceive3dGaze += ReceiveGaze;
        //Call the function below to write the column names
        WriteHeader();
    }


    void WriteHeader()
    {

        //output file-- order of column names here should match the order you use when writing out each value 
        stringBuilder.Length = 0;
        //add header info
        stringBuilder.Append(
        DateTime.Now.ToString() + "\t" +
        "The file contains frame by frame data of location for 2 tracked objects " + Environment.NewLine +
        "The coordinate system is in Unity world coordinates." + Environment.NewLine
        );
        stringBuilder.Append("-------------------------------------------------" +
            Environment.NewLine
            );
        //add column names
        stringBuilder.Append(
            "FrameNumber\t" + 
            "StartTime\t" + 
            "pl_Confidence\t" + 
            "pl_gazedirection_wrtHead_X\t" +
            "pl_gazedirection_wrtHead_Y\t" +
            "pl_gazedirection_wrtHead_Z\t" +
            "pl_gazedistance_wrtHead\t" +
            "HandX\t" + 
            "HandY\t" + 
            "HandZ\t" + 
            Environment.NewLine);


        writeString = stringBuilder.ToString();
        writebytes = Encoding.ASCII.GetBytes(writeString);
        trialStreams.Write(writebytes, 0, writebytes.Length);

    }

    void ReceiveGaze(GazeData gazeData)
    {

        if (gazeData.MappingContext != GazeData.GazeMappingContext.Binocular)
        {
            Debug.Log("pupil has lost binocular track");
            plConfidence = float.NaN;
            plTimeStamp = "NaN";
            pl_gazedirection_wrtHead_xyz = new Vector3(float.NaN, float.NaN, float.NaN);

            pl_E0_Norm_wrtHead_xyz = new Vector3(float.NaN, float.NaN, float.NaN);
            pl_E1_Norm_wrtHead_xyz = new Vector3(float.NaN, float.NaN, float.NaN);

            pl_gazedistance = float.NaN;

            mode = 0;
        }
        if (gazeData.MappingContext == GazeData.GazeMappingContext.Binocular)
        {
            Debug.Log("pupil is tracking binocularly");
            plConfidence = gazeData.Confidence;
            plTimeStamp = DateTime.UtcNow.ToString("HH:mm:ss"); ;
            pl_gazedirection_wrtHead_xyz = gazeData.GazeDirection;


            pl_E0_Norm_wrtHead_xyz = gazeData.GazeNormal0;
            pl_E1_Norm_wrtHead_xyz = gazeData.GazeNormal1;

            pl_gazedistance = gazeData.GazeDistance;

            mode = 1;
        }

        // Defining Cyclopian Eye in World Coordiante System
        eye0InWorld.position = Camera.position + new Vector3( gazeData.EyeCenter0.x, gazeData.EyeCenter0.y, gazeData.EyeCenter0.z);
        eye1InWorld.position = Camera.position + new Vector3(gazeData.EyeCenter1.x, gazeData.EyeCenter1.y, gazeData.EyeCenter1.z);

        eye0InWorld.rotation = Camera.rotation * eye0InWorld.rotation;
        eye1InWorld.rotation = Camera.rotation * eye1InWorld.rotation;

        cyclopianEyeinWorld.position = ((eye0InWorld.position + eye1InWorld.position)/2);
        cyclopianEyeinWorld.rotation = Camera.rotation * cyclopianEyeinWorld.rotation;

        // Defining Position of Fixation point in World Coordinate System
        fixationWorld.position = fixationPosition.transform.position;

        // Where the participant should be looking in World coordinates
        particiapntFixating.position = fixationWorld.position - cyclopianEyeinWorld.position;
        particiapntFixating.rotation = Camera.rotation * particiapntFixating.rotation;

        // Gaze in world coordinates
        gazeWorld0.position = gazeData.GazeNormal0 + cyclopianEyeinWorld.position;
        gazeWorld0.rotation = cyclopianEyeinWorld.rotation * gazeWorld0.rotation;

        gazeWorld1.position = gazeData.GazeNormal1 + cyclopianEyeinWorld.position;
        gazeWorld1.rotation = cyclopianEyeinWorld.rotation * gazeWorld1.rotation;

        cyclopianGazeinWorld.position = ((gazeWorld0.position + gazeWorld1.position) / 2);
        cyclopianGazeinWorld.rotation = Camera.rotation * cyclopianGazeinWorld.rotation;

        // Where the participant is looking in World coordinates
        participantGaze.position = cyclopianGazeinWorld.position - cyclopianEyeinWorld.position;
        participantGaze.rotation = Camera.rotation * participantGaze.rotation;

        // Angular Difference
        angularDifference = Mathf.Acos((Vector3.Dot(participantGaze.position, particiapntFixating.position))/(particiapntFixating.position.magnitude * participantGaze.position.magnitude));
    }

    void WriteFile()
    {
        controller = controlLevel.gamecontroller;

        stringBuilder.Length = 0;
        stringBuilder.Append(
                    Time.frameCount + "\t"
                    + Time.time * 1000 + "\t"
                    + plConfidence.ToString() + "\t"
                    + pl_gazedirection_wrtHead_xyz.x.ToString() + "\t"
                    + pl_gazedirection_wrtHead_xyz.y.ToString() + "\t"
                    + pl_gazedirection_wrtHead_xyz.z.ToString() + "\t"
                    + pl_gazedistance.ToString() + "\t" 
                    + controller.transform.position.x.ToString() + "\t"
                    + controller.transform.position.y.ToString() + "\t"
                    + controller.transform.position.z.ToString() + "\t" +
                    Environment.NewLine
                );
        writeString = stringBuilder.ToString();
        writebytes = Encoding.ASCII.GetBytes(writeString);
        trialStreams.Write(writebytes, 0, writebytes.Length);
    }

    public void Update()
    {
        //Use spacebar to initiate/stop recording values, you can change this if you want 
        if (controller != null)
        {
            WriteFile();
        }


    }

    public void OnApplicationQuit()
    {
        trialStreams.Close();

    }
}