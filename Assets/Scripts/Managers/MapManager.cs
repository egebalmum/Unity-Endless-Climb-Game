using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;
using TMPro;
using MPUIKIT;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
public class MapManager : MonoBehaviour
{
    [SerializeField] public Volume _volume;
    public bool controlforAbra;
    public List<Transform> splats;
    [SerializeField] GameObject scoreCanvas;
    [SerializeField] GameObject deathCanvas;
    [SerializeField] TextMeshProUGUI scoreOnDeath;
    [SerializeField] TextMeshProUGUI highscoreOnDeath;
    [SerializeField] GameObject killerObj;
    public MPImage _image;
    public MPImage _image2;

    static public MapManager _mapManager;
    GameObject[] platforms = new GameObject[6];
    float[] platformXs = new float[6];
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] public GameObject level1Block;
    [SerializeField] public GameObject level2Block;
    [SerializeField] public GameObject level3Block;
    [SerializeField] public GameObject level4Block;
    [SerializeField] public GameObject leftWall;
    [SerializeField] public GameObject rightWall;

    [SerializeField] public CinemachineVirtualCamera virtualCamera1;
    [SerializeField] public CinemachineVirtualCamera virtualCamera2;
    public float platformOffset = 3;
    public float wallOffset = 9;
    float deathY;
    [SerializeField] GameObject _characterPrefab;
    Character _character;
    float minX=-2;
    float maxX = +2;
    float positionX;
    float characterBaseY;
    float criticalLevel;
    public List<Node> nodes = new List<Node>();
    bool switchCameras;
    int abraCadabra;
    bool loopSetupFinished;

    public struct Node{
        public int id;
        public static List<Platform> TotalPlatforms;
        public List<GameObject> leftWalls; 
        public List<GameObject> rightWalls; 

        public Node(int id, GameObject leftwall1, GameObject leftwall2,GameObject rightwall1,GameObject rightwall2,GameObject platform1,GameObject platform2, GameObject platform3,GameObject platform4,GameObject platform5,GameObject platform6)
        {
            if (id==1)
            {
                TotalPlatforms = new List<Platform>();
            }
            leftWalls = new List<GameObject>();
            rightWalls = new List<GameObject>();
            this.id = id;
            leftWalls.Add(leftwall1);
            leftWalls.Add(leftwall2);
            rightWalls.Add(rightwall1);
            rightWalls.Add(rightwall2);
            platform1.gameObject.tag="WAITING";
            platform2.gameObject.tag="WAITING";
            platform3.gameObject.tag="WAITING";
            platform4.gameObject.tag="WAITING";
            platform5.gameObject.tag="WAITING";
            platform6.gameObject.tag="WAITING";
            TotalPlatforms.Add(platform1.GetComponent<Platform>());
            TotalPlatforms.Add(platform2.GetComponent<Platform>());
            TotalPlatforms.Add(platform3.GetComponent<Platform>());
            TotalPlatforms.Add(platform4.GetComponent<Platform>());
            TotalPlatforms.Add(platform5.GetComponent<Platform>());
            TotalPlatforms.Add(platform6.GetComponent<Platform>());
        }
    }

    void Awake()
    {
        _mapManager = this;
        splats = new List<Transform>();
    }
    void Start()
    {
        
        var _characterObj = Instantiate(_characterPrefab,new Vector3(0,-2.4f,0),Quaternion.identity);
        _character = _characterObj.GetComponent<Character>();
        characterBaseY = _character.transform.position.y;
        virtualCamera1.Follow = _character.transform;
        virtualCamera2.Follow = _character.transform;
        virtualCamera1.enabled=true;
        GenerateFirstNode();
        deathY = Node.TotalPlatforms[0].transform.position.y-1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        EndCameraFollow();
        CameraFollow();
        GenerateSecondNode();
        AbracadabraFunction();
    }

    
    void CameraFollow()
    {
        if (!switchCameras)
        {
            virtualCamera2.transform.position = virtualCamera1.transform.position- Vector3.up*18;
        }
        else
        {
            virtualCamera1.transform.position = virtualCamera2.transform.position- Vector3.up*18;
        }
    }

    public void UpdateScore(int value)
    {
        _scoreText.text = value.ToString();
    }
    void GenerateSecondNode()
    {
        if (nodes.Last().id<=2)
        {
            if(Node.TotalPlatforms[Node.TotalPlatforms.Count-6].transform.position.y<=_character.transform.position.y)
            {
                GenerateNewNode();
            }
        }
    }

    void AbracadabraFunction()
    {
        if (loopSetupFinished)
        {
            if ((!_character._controller.boostBool && _character._controller.boostControl) && !controlforAbra)
            {
                if ( _character.transform.position.y >= (Node.TotalPlatforms[Node.TotalPlatforms.Count-1].transform.position.y-15) && nodes[nodes.Count-1].id>=3) //player in last node currently
                {
                    print("due to boost");
                    DeleteAndReplace();
                    abraCadabra=0;
                }
                controlforAbra=true;
            }
            else
            {
                if ( abraCadabra!=0 &_character.transform.position.y <criticalLevel)
                {
                    abraCadabra=0;
                }
                if ( abraCadabra==0  & _character.transform.position.y >= criticalLevel)
                {
                    abraCadabra+=1;
                }
                if (abraCadabra==1 && _character._movementState == Character.MovementState.ready)
                {
                    abraCadabra+=1;
                }
                if (abraCadabra==2 && _character._movementState== Character.MovementState.midAir)
                {
                    print("due to limit");
                    DeleteAndReplace();
                    abraCadabra=0;
                }
            }
        }
    }
  

    public void FallingDeath()
    {
        if (_character._conditionalState == Character.ConditionalState.dead)
        {
            return;
        }
        PreDeath();
        Invoke("Death",0.3f);
    }

    public void AlienDeath()
    {
         if (_character._conditionalState == Character.ConditionalState.dead)
        {
            return;
        }
        PreDeath();
        Death();
    }

    void PreDeath()
    {
        if (_character._conditionalState == Character.ConditionalState.dead)
        {
            return;
        }
        if (virtualCamera1.enabled)
            {
                virtualCamera1.Follow = null;
            }
            else
            {
                virtualCamera2.Follow=null;
            }
    }
    void Death()
    {
        var score = int.Parse(_scoreText.text);
        if (score > PlayerPrefs.GetInt("HighScore",0))
        {
            PlayerPrefs.SetInt("HighScore",score);
        }
        scoreOnDeath.text= score.ToString();
        highscoreOnDeath.text = PlayerPrefs.GetInt("HighScore",0).ToString();
        _character.ExplosionEffect();
        Invoke("PostDeath",0.5f);
    }
    void PostDeath()
    {
        scoreCanvas.SetActive(false);
        deathCanvas.SetActive(true);
    }
    void EndCameraFollow()
    {
        if (_character.transform.position.y < deathY)
        {   
            FallingDeath();
        }
        
    }

    float HandleImpossibleCases(float value)
    {   
        bool ctrl;
        float left = value - minX;
        float right = maxX - value;
        float y;
        if (left >=right)
        {
            ctrl = false;
            y = left;
        }
        else
        {
            ctrl = true;
            y = right;
        }
        float maxDifference = 6.5f;
        y = maxDifference-y;
        float answer;
        if (ctrl)
        {
            var x = maxX-y;
            if (x<minX)
            {
                x = minX;
            }
            answer = Random.Range(x,maxX);
        }
        else
        {
            var x = minX+y;
            if (x>maxX)
            {
                x = maxX;
            }
            answer = Random.Range(minX,x);
        }
        return answer;
    }
    void GenerateFirstNode()
    {
        for (int i=0;i<6;i++)
        {
            if (i==0)
            {
                platformXs[i]=0;
            }
            else if(i==1)
            {
                platformXs[i] = HandleImpossibleCases(0f);
            }
            else
            {
                platformXs[i] = HandleImpossibleCases(platformXs[i-1]);
            }
        }
        nodes.Add(new Node(
            1,
            Instantiate(leftWall,new Vector3(-3.1f,1.5f,0f),Quaternion.identity),
            Instantiate(leftWall,new Vector3(-3.1f,10.5f,0f),Quaternion.identity),
            Instantiate(rightWall,new Vector3(3.1f,1.5f,0f),Quaternion.identity),
            Instantiate(rightWall,new Vector3(3.1f,10.5f,0f),Quaternion.identity),
            Instantiate(level1Block,new Vector3(platformXs[0],-3,0),Quaternion.identity),
            Instantiate(level1Block,new Vector3(platformXs[1],0,0),Quaternion.identity),
            Instantiate(level1Block,new Vector3(platformXs[2],3,0),Quaternion.identity),
            Instantiate(level1Block,new Vector3(platformXs[3],6,0),Quaternion.identity),
            Instantiate(level1Block,new Vector3(platformXs[4],9,0),Quaternion.identity),
            Instantiate(level1Block,new Vector3(platformXs[5],12,0),Quaternion.identity)
        ));
    }


    GameObject ExponentialLevelTest() // going to delete
    {
        int nextLevel = nodes.Last().id +1;
       
        
            var rng = Random.Range(0,3);
            if (rng==0)
            {
                return level1Block;
            }
            else if (rng==1)
            {
                return level2Block;
            }
            else
            {
                return level3Block;
            }
            
        
    }




    GameObject ExponentialLevelGenerator()
    {
        int nextLevel = nodes.Last().id +1;
        if (nextLevel <=2)
        {
            return level1Block;
        }
        else if (nextLevel<=3)
        {
            var rng = Random.Range(0,2);
            if (rng==0)
            {
                return level1Block;
            }
            else
            {
                return level2Block;
            }
        }
        else if (nextLevel<=4)
        {
            var rng = Random.Range(0,3);
            if (rng==0)
            {
                return level1Block;
            }
            else if (rng==1)
            {
                return level2Block;
            }
            else
            {
                return level3Block;
            }
        }
        else
        {
            var rng = Random.Range(0,4);
            if (rng==0)
            {
                return level1Block;
            }
            else if (rng==1)
            {
                return level2Block;
            }
            else if (rng==2)
            {
                return level3Block;
            }
            else
            {
                return level4Block;
            }
        }
    }
    void GenerateNewNode()
    {
        
        for (int i=0;i<6;i++)
        {
            platforms[i] = ExponentialLevelGenerator();
            if (i==0)
            {
                platformXs[i] = HandleImpossibleCases(Node.TotalPlatforms[Node.TotalPlatforms.Count-1].transform.position.x);
            }
            else
            {
                platformXs[i] = HandleImpossibleCases(platformXs[i-1]);
            }
        }
        var highestPlatformY = Node.TotalPlatforms[Node.TotalPlatforms.Count-1].transform.position.y;
        var highestWallY = nodes.Last().leftWalls.Last().transform.position.y;
        nodes.Add(new Node(
            nodes.Last().id+1,
            Instantiate(leftWall,new Vector3(-3.1f,highestWallY+9,0f),Quaternion.identity),
            Instantiate(leftWall,new Vector3(-3.1f,highestWallY+18,0f),Quaternion.identity),
            Instantiate(rightWall,new Vector3(3.1f,highestWallY+9,0f),Quaternion.identity),
            Instantiate(rightWall,new Vector3(3.1f,highestWallY+18,0f),Quaternion.identity),
            Instantiate(platforms[0],new Vector3(platformXs[0],highestPlatformY+3,0),Quaternion.identity),
            Instantiate(platforms[1],new Vector3(platformXs[1],highestPlatformY+6,0),Quaternion.identity),
            Instantiate(platforms[2],new Vector3(platformXs[2],highestPlatformY+9,0),Quaternion.identity),
            Instantiate(platforms[3],new Vector3(platformXs[3],highestPlatformY+12,0),Quaternion.identity),
            Instantiate(platforms[4],new Vector3(platformXs[4],highestPlatformY+15,0),Quaternion.identity),
            Instantiate(platforms[5],new Vector3(platformXs[5],highestPlatformY+18,0),Quaternion.identity)
        ));
        if (nodes.Last().id>=3)
        {
            if (!loopSetupFinished)
            {
                loopSetupFinished=true;
            }
            criticalLevel=Node.TotalPlatforms[Node.TotalPlatforms.Count-3].transform.position.y;
        }    
    }

//when node3.platform4



    public void DeletePlatform()
    {
        Platform tmp = Node.TotalPlatforms[0];
        Node.TotalPlatforms.Remove(tmp);
        Destroy(tmp.gameObject);
        deathY = Node.TotalPlatforms[0].transform.position.y-1.5f;
    }
    public void DeleteAndReplace()
    {
        var controlTest = virtualCamera1.enabled;
        //deadZone.transform.Translate(Vector3.down*18);
        Destroy(nodes[1].leftWalls[0].gameObject);
        Destroy(nodes[1].leftWalls[1].gameObject);
        Destroy(nodes[1].rightWalls[0].gameObject);
        Destroy(nodes[1].rightWalls[1].gameObject);
        nodes.RemoveAt(1);
        nodes[1].leftWalls[0].transform.Translate(Vector3.down*18);
        nodes[1].leftWalls[1].transform.Translate(Vector3.down*18);
        nodes[1].rightWalls[0].transform.Translate(Vector3.down*18);
        nodes[1].rightWalls[1].transform.Translate(Vector3.down*18);
        //nodes[1].platforms[0].transform.Translate(Vector3.down*18);
        foreach (Platform x in Node.TotalPlatforms)
        {
            x.transform.Translate(Vector3.down*18);
        }
        int lengthOfSplats = splats.Count;
        for (int i = lengthOfSplats-1; i>=0;i--)
        {
            splats[i].Translate(Vector3.down*18,Space.World);
            if (splats[i].position.y <= 0)
            {
                Transform tmp = splats[i];
                splats.Remove(tmp);
                Destroy(tmp.gameObject);
            }
        }
        virtualCamera1.enabled=false;
        virtualCamera2.enabled=false;
        switchCameras=!switchCameras;
        deathY = Node.TotalPlatforms[0].transform.position.y-1.5f;
        killerObj.transform.Translate(Vector3.down*18);
        _character.transform.Translate(Vector3.down*18);
        if (controlTest)
        {
            virtualCamera2.enabled=true;
        }
        else
        {
            virtualCamera1.enabled=true;
        }
        GenerateNewNode();

    }
}
