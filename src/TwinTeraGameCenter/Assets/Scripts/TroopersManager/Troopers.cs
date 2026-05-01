using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Troopers : MonoBehaviour
{
    public enum tropperSet
    {
        first = 1,
        second = 2,
        third = 3
    }
    [SerializeField] private RawImage Octagon;
    [SerializeField] private RawImage OctagonTransparent;
    [SerializeField] private RawImage Rectangle;
    [SerializeField] private RawImage RectangleTransparent;

    [SerializeField] private RawImage Triangular8;
    [SerializeField] private RawImage TriangularTransparent8;
    [SerializeField] private RawImage Triangular4;
    [SerializeField] private RawImage TriangularTransparent4;

    [SerializeField] private RawImage Hart;
    [SerializeField] private RawImage HartTransparent;
    [SerializeField] private RawImage Skull;
    [SerializeField] private RawImage SkullTransparent;


    [SerializeField] private RawImage RoundTriangular4;
    [SerializeField] private RawImage RoundTriangularTransparent4;
    [SerializeField] private RawImage Trapez2;
    [SerializeField] private RawImage TrapezTransparent2;


    [SerializeField] private RawImage Circle;
    [SerializeField] private RawImage CircleTransparent;
    [SerializeField] private RawImage HalfCircle3;
    [SerializeField] private RawImage HalfCircleTransparent3;


    [SerializeField] private RawImage Triangular;
    [SerializeField] private RawImage TriangularTransparent;
    [SerializeField] private RawImage Hexagon;
    [SerializeField] private RawImage HexagonTransparent;


    [SerializeField] private RawImage Cross;
    [SerializeField] private RawImage CrossTransparent;
    [SerializeField] private RawImage Cross8;
    [SerializeField] private RawImage CrossTransparent8;


    [SerializeField] private RawImage CircleCross;
    [SerializeField] private RawImage CircleCrossTransparent;
    [SerializeField] private RawImage HalfCircle8;
    [SerializeField] private RawImage HalfCircleTransparent8;


    [SerializeField] private RawImage HalfCircle2;
    [SerializeField] private RawImage HalfCircleTransparent2;
    [SerializeField] private RawImage HalfCircle4;
    [SerializeField] private RawImage HalfCircleTransparent4;

    public Dictionary<int, object> tropperSets;
    public List<Trooper> alltroopers { get; set; }

    public static Troopers instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created  
    void Start()
    {
        tropperSets = new Dictionary<int, object>();
        alltroopers = new List<Trooper>()
        {
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(Octagon, OctagonTransparent, new RFIDTag("254028066239")),
                    new TwinteraGameObject(Rectangle, RectangleTransparent, new RFIDTag("014053076239"))
                }
            ),
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(CircleCross, CircleCrossTransparent, new RFIDTag("014187080239")),
                    new TwinteraGameObject(HalfCircle8, HalfCircleTransparent8, new RFIDTag("174014084239"))
                }
            ),
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(Triangular8, TriangularTransparent8, new RFIDTag("190232082239")),
                    new TwinteraGameObject(Triangular4, TriangularTransparent4, new RFIDTag("078132056239"))
                }
            ),
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(Hart, HartTransparent, new RFIDTag("014058073239")),
                    new TwinteraGameObject(Skull, SkullTransparent, new RFIDTag("110086084239"))
                }
            ),
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(RoundTriangular4, RoundTriangularTransparent4, new RFIDTag("062026077239")),
                    new TwinteraGameObject(Trapez2, TrapezTransparent2, new RFIDTag("238060085239"))
                }
            ),
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(Circle, CircleTransparent, new RFIDTag("046027085239")),
                    new TwinteraGameObject(HalfCircle3, HalfCircleTransparent3, new RFIDTag("190038081239"))
                }
            ),
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(Triangular, TriangularTransparent, new RFIDTag("222188070239")),
                    new TwinteraGameObject(Hexagon, HexagonTransparent, new RFIDTag("206211072239"))
                }
            ),
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(Cross, CrossTransparent, new RFIDTag("046174085239")),
                    new TwinteraGameObject(Cross8, CrossTransparent8, new RFIDTag("126081070239"))
                }
            ),
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(HalfCircle2, HalfCircleTransparent2, new RFIDTag("190053076239")),
                    new TwinteraGameObject(HalfCircle4, HalfCircleTransparent4, new RFIDTag("254135077239"))
                }
            )
        };
        tropperSets.Add((int)tropperSet.first, alltroopers);

        // new
        alltroopers = new List<Trooper>()
        {
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(Octagon, OctagonTransparent, new RFIDTag("238009069239")),
                    new TwinteraGameObject(Rectangle, RectangleTransparent, new RFIDTag("046149056239"))
                }
            ),
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(CircleCross, CircleCrossTransparent, new RFIDTag("046073084239")),
                    new TwinteraGameObject(HalfCircle8, HalfCircleTransparent8, new RFIDTag("126226082239"))
                }
            ),
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(Triangular8, TriangularTransparent8, new RFIDTag("190063083239")),
                    new TwinteraGameObject(Triangular4, TriangularTransparent4, new RFIDTag("238232066239"))
                }
            ),
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(Hart, HartTransparent, new RFIDTag("014013084239")),
                    new TwinteraGameObject(Skull, SkullTransparent, new RFIDTag("094045081239"))
                }
            ),
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(RoundTriangular4, RoundTriangularTransparent4, new RFIDTag("238249049239")),
                    new TwinteraGameObject(Trapez2, TrapezTransparent2, new RFIDTag("110033066239"))
                }
            ),
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(Circle, CircleTransparent, new RFIDTag("190255083239")),
                    new TwinteraGameObject(HalfCircle3, HalfCircleTransparent3, new RFIDTag("046197068239"))
                }
            ),
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(Triangular, TriangularTransparent, new RFIDTag("142033056239")),
                    new TwinteraGameObject(Hexagon, HexagonTransparent, new RFIDTag("094092084239"))
                }
            ),
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(Cross, CrossTransparent, new RFIDTag("254096073239")),
                    new TwinteraGameObject(Cross8, CrossTransparent8, new RFIDTag("142060085239"))
                }
            ),
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(HalfCircle2, HalfCircleTransparent2, new RFIDTag("238227074239")),
                    new TwinteraGameObject(HalfCircle4, HalfCircleTransparent4, new RFIDTag("078179074239"))
                }
            )
        };
        tropperSets.Add((int)tropperSet.second, alltroopers);

        // new 2
        alltroopers = new List<Trooper>()
        {
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(Octagon, OctagonTransparent, new RFIDTag("173129251178")),
                    new TwinteraGameObject(Rectangle, RectangleTransparent, new RFIDTag("029204250178"))
                }
            ),
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(CircleCross, CircleCrossTransparent, new RFIDTag("254203084239")),
                    new TwinteraGameObject(HalfCircle8, HalfCircleTransparent8, new RFIDTag("078116079239"))
                }
            ),
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(Hart, HartTransparent, new RFIDTag("157187250178")),
                    new TwinteraGameObject(Skull, SkullTransparent, new RFIDTag("189203250178"))
                }
            ),
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(RoundTriangular4, RoundTriangularTransparent4, new RFIDTag("253202250178")),
                    new TwinteraGameObject(Trapez2, TrapezTransparent2, new RFIDTag("221186250178"))
                }
            ),
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(Circle, CircleTransparent, new RFIDTag("109019251178")),
                    new TwinteraGameObject(HalfCircle3, HalfCircleTransparent3, new RFIDTag("125186250178"))
                }
            ),
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(Triangular, TriangularTransparent, new RFIDTag("029116251178")),
                    new TwinteraGameObject(Hexagon, HexagonTransparent, new RFIDTag("013178250178"))
                }
            ),
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(Cross, CrossTransparent, new RFIDTag("109178250178")),
                    new TwinteraGameObject(Cross8, CrossTransparent8, new RFIDTag("061187250178"))
                }
            ),
            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(HalfCircle2, HalfCircleTransparent2, new RFIDTag("173125251178")),
                    new TwinteraGameObject(HalfCircle4, HalfCircleTransparent4, new RFIDTag("077119251178"))
                }
            ),



            new Trooper(new List<TwinteraGameObject>
                {
                    new TwinteraGameObject(Triangular8, TriangularTransparent8, new RFIDTag("174120083239")), // THIS TAG IS NOT RECOGNIZED
                    new TwinteraGameObject(Triangular4, TriangularTransparent4, new RFIDTag("110069076239"))
                }
            )
        };
        tropperSets.Add((int)tropperSet.third, alltroopers);
    }

    public List<Trooper> getNextTroppers(int numberoftroopers, bool random)
    {
        List<Trooper> troopers = new List<Trooper>();
        List<Trooper> copyalltroopers = new List<Trooper>(alltroopers);
        if (random == true)
        {
            for(int x = 0; x < 100; x++)
            {
                int firstplace = Random.Range(1, 10) - 1;
                int secondplace = Random.Range(1, 10) - 1;
                Trooper sourcetrooper = copyalltroopers[firstplace];
                copyalltroopers[firstplace] = copyalltroopers[secondplace];
                copyalltroopers[secondplace] = sourcetrooper;
            }
        }
        for(int x = 0; x < numberoftroopers; x++)
        {
            troopers.Add(copyalltroopers[x]);
        }

        return troopers;
    }
    public Trooper getTrooperByTagID(string tagid)
    {
        Trooper thetrooper = null;
        foreach(Trooper trooper in alltroopers)
        {
            foreach(TwinteraGameObject twinteraGameObject in trooper.GameObjects)
            {
                if(twinteraGameObject.Tag.SerialNumber.CompareTo(tagid) == 0)
                {
                    thetrooper = trooper;
                    break;
                }
            }
            if(thetrooper != null)
            {
                break;
            }
        }

        return thetrooper;
    }
    public int getTrooperSetByTagID(string tagid)
    {
        Trooper thetrooper = null;
        int tropperset = 0;
        foreach (int tropset in tropperSets.Keys)
        {
            tropperset = tropset;
            alltroopers = (List<Trooper>)tropperSets[tropset];
            foreach (Trooper trooper in alltroopers)
            {
                foreach (TwinteraGameObject twinteraGameObject in trooper.GameObjects)
                {
                    if (twinteraGameObject.Tag.SerialNumber.CompareTo(tagid) == 0)
                    {
                        thetrooper = trooper;
                        break;
                    }
                }
                if (thetrooper != null)
                {
                    break;
                }
            }
            if (thetrooper != null)
            {
                break;
            }
        }
        return tropperset;
    }
    public void setTropperSet(int tropperset)
    {
        alltroopers = (List<Trooper>)tropperSets[tropperset];
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
