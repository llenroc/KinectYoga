using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Microsoft.Samples.Kinect.SkeletonBasics
{

    class YogaProfile
    {
        public int id;
        public bool completed;
        public int score;
    }
    class Profile
    {
        List<YogaProfile> profile;

        public void ReadProfile()
        {
            profile = new List<YogaProfile>();

            try
            {
                XmlDocument profileDoc = new XmlDocument();
                profileDoc.Load(Constants.profileFile);
            

                XmlNode root = profileDoc.FirstChild;

                //int id = 0;

                if (root != null && root.HasChildNodes)
                {
                    // get all nodes with tag name "Level"
                    foreach (XmlNode yoga in root.ChildNodes)
                    {
                        if (yoga.Name != "Yoga")
                            continue;

                        YogaProfile yogaProfile = new YogaProfile();
                        yogaProfile.id = Int32.Parse(yoga.Attributes["id"].Value);
                        yogaProfile.completed = yoga.Attributes["completed"].Value.Equals("yes");
                        yogaProfile.score = Int32.Parse(yoga.Attributes["score"].Value);

                        profile.Add(yogaProfile);
                    }
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }

        public void SetYogaComplete(int id)
        {
            try
            {
                YogaProfile p = profile.Where(yogaProfile => yogaProfile.id == id).First();

                if(p != null)
                    p.completed = true;
            }
            catch(Exception)
            {

            }
        }

        public void SetYogaScore(int id, int score)
        {
            try
            {
                YogaProfile p = profile.Where(yogaProfile => yogaProfile.id == id).First();

                if (p != null)
                    p.score = score;
            }
            catch (Exception)
            {

            }
        }

        public void SaveProfile()
        {
            try
            {
                XmlDocument profileDoc = new XmlDocument();
                XmlElement root = profileDoc.CreateElement("Profile");
                profileDoc.AppendChild(root);

                foreach (YogaProfile yogaProfile in profile)
                {
                    XmlElement yoga = profileDoc.CreateElement("Yoga");
                    yoga.SetAttribute("id", ""+yogaProfile.id);
                    yoga.SetAttribute("completed", yogaProfile.completed ? "yes":"no");
                    yoga.SetAttribute("score", "" + yogaProfile.score);
                    root.AppendChild(yoga);
                }

                profileDoc.Save(Constants.profileFile);
            }
            catch(Exception)
            {

            }
        }
    }
}
