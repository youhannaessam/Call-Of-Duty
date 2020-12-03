using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class Player : MonoBehaviour
{
    public int Noperation;
    public int Moperation;
    public int Xoperation;
    public Hashtable hasHtable = new Hashtable();
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;

    [SerializeField]
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldsActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;
    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _laserSoundClip;
    
    private AudioSource _audioSource;
    public enum d_mode
    {
        preorder, postorder, inorder
    }



    public enum operation
    {

        insert, delete
    }


    public class node
    {

        public node left, right;
        public int val;
        public node(int val)
        {
            this.left = null;
            this.right = null;
            this.val = val;
        }
    }
    public class BST
    {




        public node root { get; internal set; }
        int size;
        int Copy;

        public BST()
        {
            this.root = null;
            this.size = 0;
        }


        public node find(int val)
        {
            node tmp2 = this.root;

            while (tmp2 != null)
            {
                if (val > tmp2.val)
                    tmp2 = tmp2.right;
                else if (val < tmp2.val)
                    tmp2 = tmp2.left;
                else
                    return tmp2;
            }
            return null;
        }










        public bool insert(int val)
        {
            var tmp = new node(val);
            var tmp2 = this.root;
            node tmp3 = null;
            if (this.root == null)
            {

                this.root = tmp;

            }
            else
            {
                while (tmp2 != null)
                {
                    tmp3 = tmp2;
                    if (val > tmp2.val)
                        tmp2 = tmp2.right;
                    else if (val < tmp2.val)
                        tmp2 = tmp2.left;
                    else
                    {
                        return false;
                    }
                }

                if (val > tmp3.val)
                {

                    tmp3.right = tmp;

                }
                else
                {
                    tmp3.left = tmp;
                }
            }
            this.size++;

            return true;
        }
        
    }
    
   
    // Start is called before the first frame update
     public BST bST = new BST();
    void Start()
    {
        bST.insert(1);
        bST.insert(2);
        bST.insert(3);
        bST.insert(4);
        bST.insert(5);
        bST.insert(6);
        bST.insert(7);
        bST.insert(8);
        bST.insert(9);
        bST.insert(10);
        transform.position = new Vector3(0, 0, 0);

        // Find the object & get the component
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }
        if(_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the player is NULL.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);


        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x,Mathf.Clamp(transform.position.y,-3.8f, 0), 0);

        if (transform.position.x > 11.3f)
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        else if (transform.position.x < -11.3f)
            transform.position = new Vector3(11.3f, transform.position.y, 0);
    }
    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }

        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        _audioSource.Play();

    }
    public int zOperation;
    static readonly string textFile = @"E:\h.txt";
    public void Damage()
    {
        if(_isShieldsActive == true)
        {
            _isShieldsActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;

        if(_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);
        if(_lives<1)
        {
            //Comunicate with the Spawn Manager 
            // Let them know to stop spwaning
            _spawnManager.OnPlayerDeath();
          //  _uiManager.CheckBestScore();
            Destroy(this.gameObject);
            int x= _uiManager.score;
            Noperation = x % 100;
            Moperation = x - Noperation;
            Xoperation = Moperation / 100;
            //bST.find(Xoperation);
            zOperation = Xoperation;
            hasHtable.Add(zOperation, "ADDIT::");
            string[] lines;
            var list = new List<string>();
            var fileStream = new FileStream(@"E:\h.txt" , FileMode.Open , FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line= streamReader.ReadLine())!= null)
                {
                    list.Add(line);
                }
            }
            lines = list.ToArray();
        }
    }
    class ReadScoresFromFile
    {
        void Main(string[] args)
        {
            
            Hashtable hashtable = new Hashtable();
            StreamReader sr = new StreamReader("E://h.txt");
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            string str;
            int i = 0;
            string sstr = "0";
            int nn = 0;
            while (true)
            {
                str = sr.ReadLine();
                if (str == null)
                {
                    break;
                }
                if (i % 2 != 0)
                {
                    int n = int.Parse(str);
                    nn = n;
                    hashtable.Add(nn, sstr);
                    nn = 0;
                    sstr = "0";
                    i++;
                }
                else if (i % 2 == 0)
                {
                    sstr = str;
                    i++;
                }
            }
            sr.Close();
            ICollection key = hashtable.Keys;
        }
    }
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostDownRoutine());
    }

    IEnumerator SpeedBoostDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }
    public void ShieldsActive()
    {
        _isShieldsActive = true;
        _shieldVisualizer.SetActive(true);
    }
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
