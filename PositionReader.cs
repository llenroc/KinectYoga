using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Kinect;

namespace Microsoft.Samples.Kinect.SkeletonBasics
{

    class Main
    {
        public static void Start()
        {
            PositionReader reader = new PositionReader(@"E:\CodeFunDo\Skeleton\SkeletonBasics-WPF\yogabase.xml");
            reader.ReadPositions();

            List<Yoga> yogas = reader.Yogas;

            foreach(Yoga yoga in yogas)
            {
                //yoga.Start();
            }
        }
    }

    public class Yoga
    {
        int id;
        string name;
        List<Position> positions;
        int score;

        int currentPositionId;
        int positionsCompeted = 0;

        public int Id
        {
            get { return id; }
        }

        public int Score
        {
            get { return score; }
            set { this.score = value; }
        }


        public Yoga(int id, string name)
        {
            this.id = id;
            this.name = name;
            positions = new List<Position>();
            currentPositionId = 0;
            score = 0;

        }

        public void AddPosition(Position position)
        {
            if (currentPositionId == 0)
                currentPositionId = position.Id;

            positions.Add(position);
        }

        public Position GetPosition(int id)
        {
            try
            {
                return positions.Where(position => position.Id == id).First();
            }
            catch(Exception)
            {
                return null;
            }
        }

        //public void Start()
        //{
        //    foreach(Position position in positions)
        //    {
        //        do
        //        {

        //        } while (position.MatchPosition());
        //    }
        //}

        public bool EvaluateYoga(Skeleton skeleton)
        {
            Position position = GetPosition(currentPositionId);

            if(position == null)
                return false;

            if (position.MatchPosition(skeleton))
            {
                // Set Yoga score
                
                this.Score += position.Score;

                System.Console.WriteLine("Position : " + position.Name + " completed.");
                currentPositionId++;
                positionsCompeted++;

                if (AllPositionsFinished())
                {
                    System.Console.WriteLine("Yoga : " + this.name + " finished.");
                    return true;
                }
                else
                    return false;
                // Change UI accrordingrly
            }
            else
            {
                System.Console.WriteLine("Looking for position : " + position.Name);
                return false;
            }
        }

        public bool AllPositionsFinished()
        {
            return positionsCompeted >= positions.Count;
        }
    }

    public class Position
    {
        string name;
        int id;
        int score;

        public int Id
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
        }

        public int Score
        {
            get { return score; }
            set { this.score = value; }
        }

        List<Gestures> gestures;

        public Position(int id, string name, int score)
        {
            this.name = name;
            this.id = id;
            this.score = score;

            gestures = new List<Gestures>();

            //gestures.Add(Gestures.BothHandsClose);
            //gestures.Add(Gestures.BothLegsClose);
            //gestures.Add(Gestures.HeadHandLegOnSameX);
            //gestures.Add(Gestures.HandAboveHead);
            //gestures.Add(Gestures.ElbowBelowHead);
            //gestures.Add(Gestures.ElbowAboveHead);
            //gestures.Add(Gestures.Standing);

            //Someone needs to call this befor
        }

        public void AddGesture(Gestures gesture)
        {
            gestures.Add(gesture);
        }

        public bool MatchPosition(Skeleton skeleton)
        {
            bool result = true;

            Dictionary<Gestures, Func<Skeleton, bool>> map = Mappings.Map1;

            Func<Skeleton, bool> func;

            foreach (Gestures gesture in gestures)
            {
                func = null;
                map.TryGetValue(gesture, out func);

                if (func != null)
                {
                    bool localResult = func(skeleton);

                    result &= localResult;


                    //Mappings.textBlock.Text += gesture + " Return value : " + localResult;
                    if (localResult == true)
                    {
                        System.Console.WriteLine("Position : " + this.name + " - Gesture : " + gesture + ". Return value : " + localResult);
                    }
                }
            }

            if (result)
                System.Console.WriteLine("Final result : " + result);

            return result;
        }
    }

    class PositionReader
    {
        string file;
        List<Yoga> yogas;

        public List<Yoga> Yogas
        {
            get { return yogas; }
        }

        public PositionReader(string file)
        {
            this.file = file;
        }

        public void ReadPositions()
        {
            

            XmlDocument document = new XmlDocument();

            try
            {
                document.Load(file);

            }
            catch(Exception e)
            {
                System.Console.WriteLine(e.Message);
                return;
            }

            XmlNode root = document.FirstChild;

            yogas = new List<Yoga>();
            //int id = 0;

            if ( root != null && root.HasChildNodes)
            {
                // get all nodes with tag name "Level"
                foreach (XmlNode yoga in root.ChildNodes)
                {
                    if (yoga.Name != "Yoga")
                        continue;

                    try
                    {
                        Yoga yogaObj = new Yoga(Int32.Parse(yoga.Attributes["id"].Value), yoga.Attributes["name"].Value);

                        foreach (XmlNode position in yoga.ChildNodes)
                        {
                            if (position.Name != "Position")
                                continue;

                            Position positionobj = new Position(Int32.Parse(position.Attributes["id"].Value), position.Attributes["name"].Value, Int32.Parse(position.Attributes["score"].Value));

                            foreach (XmlNode gesture in position.ChildNodes)
                            {
                                if (gesture.Name == "Gesture")
                                {
                                    Gestures gestureObj;

                                    if (Enum.TryParse(gesture.InnerText, out gestureObj))
                                    {
                                        positionobj.AddGesture(gestureObj);
                                    }
                                }
                            }

                            yogaObj.AddPosition(positionobj);
                        }

                        yogas.Add(yogaObj);
                    }
                    catch(Exception)
                    {

                    }
                }
            }

        }
    }
}
