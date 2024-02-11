using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ANNDrive : MonoBehaviour {

    RacingANN ann;
    public float visibleDistance = 50.0f;
    public int epoches=1000;
    public float speed = 50.0f;
    public float rotationSpeed=10.0f;

    bool trainingDone=false;
    float trainingProgress=0;
    double sumSquaredError=0;
    double lastSumSquaredError=1;

    public float translation;
    public float rotation;

    public bool loadFromFile = true;

    private string bestWeight="-6.24313780800071,-12.5178847096692,-2.14131437792812,-14.8625612433862,-0.697053046124663,15.1876495502695,-0.319054173551023,0.744490938072641,-0.30970954971461,0.577234924257763,-0.12560735518703,-0.771454433703029,0.219173911084839,-0.201721138286696,0.397242888585972,-0.0682465213834439,0.334190284200401,0.0639670156842284,0.239913694577872,-0.0196640223315769,0.366043361102698,-0.0321564605595373,-0.4534851979496,-0.0173136254466319,-7.58214035474307,-15.3599875292867,-2.38753760434934,-17.1957115981586,-0.418835641480471,18.4129203831579,0.0759310698281368,0.0927237326073072,-0.207605782572878,-0.379591778269384,0.463050315190767,-0.431110083340323,0.0592971483266097,-0.178453481529744,-0.0849338157263355,-0.296766745432939,0.398607270642341,-0.303269009346878,-0.359457374841737,-0.123547715090066,0.154458606897555,0.496571817368548,0.0513340490968987,0.253249294431159,0.0595162459691616,0.0271407086151819,0.1982161359891,0.250806864129316,-0.362562390760416,0.118207183702307,0.335127127284963,0.233787021449689,0.402901281729451,0.286802801780957,0.232245528998649,0.35825248228644,-0.386246293243052,0.108775114262424,0.0286138388325419,0.126154577607403,-0.366159782239603,0.034319004640165,0.0680698480508513,0.0589098757424319,-0.0200132481261544,-0.0607727485299884,-0.134066568854771,-2.84036692147,-0.260066428730641,0.23183899331647,0.131071729397965,-3.05960978171416,-0.159494637856132,0.938793703306606,0.143433942498058,0.260691246050094,0.239375622267262,5.55038308302571,";

    private void OnGUI() {
        GUI.Label(new Rect(25,25,250,30),"SSE: "+lastSumSquaredError);
        GUI.Label(new Rect(25,40,250,30),"Alpha: "+ann.alpha);
        GUI.Label(new Rect(25,55,250,30),"Trained: "+trainingProgress+"%");
    }

    private double ConvertToDouble(string x){
        return System.Convert.ToDouble(x);
    }


    private void Start() {
        ann = new RacingANN(5,2,1,10,0.5);
        if(loadFromFile){
            LoadWeightsFromFile();
            trainingDone=true;
        }
        else StartCoroutine(LoadTrainingSet());
    }

    IEnumerator LoadTrainingSet() {
        string path = Application.dataPath + "/_Assets/AIDriveAssets/trainingData.txt";
        string line;
        
        if(File.Exists(path)){
            int trainingDataCount = File.ReadAllLines(path).Length;
            StreamReader trainingDataFile= File.OpenText(path);
            List<double> calcOutputs = new List<double>();
            List<double> inputs = new List<double>();
            List<double> outputs = new List<double>();

            for(int i=0;i<epoches;i++){
                sumSquaredError=0;
                trainingDataFile.BaseStream.Position=0;
                string currentWeights=ann.PrintWeights();
                while((line=trainingDataFile.ReadLine())!=null){
                    string[] data = line.Split(',');
                    float thisError=0;
                    if(ConvertToDouble(data[5])!=0 && ConvertToDouble(data[6])!=0){
                        inputs.Clear();
                        outputs.Clear();
                        inputs.Add(ConvertToDouble(data[0]));
                        inputs.Add(ConvertToDouble(data[1]));
                        inputs.Add(ConvertToDouble(data[2]));
                        inputs.Add(ConvertToDouble(data[3]));
                        inputs.Add(ConvertToDouble(data[4]));

                        double o1 = Map(0,1,-1,1,System.Convert.ToSingle(data[5]));
                        double o2 = Map(0,1,-1,1,System.Convert.ToSingle(data[6]));
                        outputs.Add(o1);
                        outputs.Add(o2);

                        calcOutputs = ann.Train(inputs,outputs);
                        thisError= (
                                    Mathf.Pow((float)(outputs[0] - calcOutputs[0]),2) + 
                                    Mathf.Pow((float)(outputs[1]-calcOutputs[1]),2)
                                    )/2.0f;

                    }
                    sumSquaredError+=thisError;
                }
                trainingProgress = (float)i/ epoches;
                sumSquaredError/=trainingDataCount;
                if(lastSumSquaredError< sumSquaredError){
                    //sse is not better
                    ann.LoadWeights(currentWeights);
                    ann.alpha = Mathf.Clamp((float)ann.alpha - 0.001f,0.01f,0.9f);
                } else{
                    ann.alpha = Mathf.Clamp((float)ann.alpha + 0.001f,0.01f,0.9f);
                    lastSumSquaredError = sumSquaredError;
                }

                yield return null;
            }
        }
        trainingDone = true;
        SaveWightsToFile();
    }

    void SaveWightsToFile(){
        string path = Application.dataPath + "/_Assets/AIDriveAssets/weights.txt";
        StreamWriter wf = File.CreateText(path);
        wf.WriteLine(ann.PrintWeights());
        wf.Close();
    }

    void LoadWeightsFromFile() {
        // string path = Application.dataPath + "/_Assets/AIDriveAssets/weights.txt";
        // StreamReader wf = File.OpenText(path);
        // if(File.Exists(path)){
        //     string line = wf.ReadLine();
        //     ann.LoadWeights(line);
        // }
        ann.LoadWeights(bestWeight);
    }

    float Map(float newfrom,float newto,float origfrom,float origto,float value){
        if(value<=origfrom) return newfrom;
        else if(value>=origto) return newto;
        return (newto-newfrom)*((value-origfrom)/(origto-origfrom)) + newfrom;
    }

    float Round(float x){
        return (float) System.Math.Round(x,System.MidpointRounding.AwayFromZero)/2.0f;
    }

    private void Update() {

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Loader.Load(Loader.Scene.MainMenuScene);
		}

        if(!trainingDone) return;

        List<double> calcOutputs = new List<double>();
        List<double> inputs = new List<double>();
        List<double> outputs = new List<double>();

        RaycastHit hit;
        float frontDist=0,
                rightDist=0,
                leftDist=0,
                right45Dist=0,
                left45Dist=0;

        Debug.DrawRay(transform.position,this.transform.forward*(visibleDistance),Color.red);
        Debug.DrawRay(transform.position,this.transform.right*visibleDistance,Color.red);
        Debug.DrawRay(transform.position,-this.transform.right*visibleDistance,Color.red);
        Debug.DrawRay(transform.position,Quaternion.AngleAxis(-45,Vector3.up)*this.transform.right*visibleDistance,Color.red);
        Debug.DrawRay(transform.position,Quaternion.AngleAxis(45,Vector3.up)*-this.transform.right*visibleDistance,Color.red);

        if(Physics.Raycast(transform.position,this.transform.forward,out hit,visibleDistance)){
            frontDist = 1- Round(hit.distance/visibleDistance);
        }
        if(Physics.Raycast(transform.position,this.transform.right,out hit,visibleDistance)){
            rightDist = 1 - Round(hit.distance/visibleDistance);
        }
        if(Physics.Raycast(transform.position,-this.transform.right,out hit,visibleDistance)){
            leftDist = 1 - Round(hit.distance/visibleDistance);
        }
        if(Physics.Raycast(transform.position,Quaternion.AngleAxis(-45,Vector3.up)*this.transform.right,out hit,visibleDistance)){
            right45Dist = 1 - Round(hit.distance/visibleDistance);
        }
        if(Physics.Raycast(transform.position,Quaternion.AngleAxis(45,Vector3.up)*-this.transform.right,out hit,visibleDistance)){
            left45Dist = 1 - Round(hit.distance/visibleDistance);
        }

        inputs.Add(frontDist);
        inputs.Add(rightDist);
        inputs.Add(leftDist);
        inputs.Add(right45Dist);
        inputs.Add(left45Dist);

        outputs.Add(0);
        outputs.Add(0);
        calcOutputs = ann.CalcOutput(inputs,outputs);
        float translationInput = Map(-1,1,0,1,(float)calcOutputs[0]);
        float rotationInput = Map(-1,1,0,1,(float)calcOutputs[1]);
        translation = translationInput * speed * Time.deltaTime;
        rotation = rotationInput * rotationSpeed * Time.deltaTime;
        this.transform.Translate(0,0,translation);
        this.transform.Rotate(0,rotation,0);
    }
}
