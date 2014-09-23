using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace Microsoft.Samples.Kinect.SkeletonBasics
{
    public class Constants
    {
        public static float kneeThreshold = 0.5f;
        public static float ankleThreshold = 0.5f;
        public static readonly string yogabaseFile = @"E:\CodeFunDo\Skeleton\SkeletonBasics-WPF\yogabase.xml";
        public static readonly string profileFile = @"E:\CodeFunDo\Skeleton\SkeletonBasics-WPF\profile.xml";
    }

    public class YogaEvaluator
    {
        static int currentYoga;
        static List<Yoga> yogas;
        static Profile userProfile;

        public YogaEvaluator()
        {
            PositionReader reader = new PositionReader(Constants.yogabaseFile);
            reader.ReadPositions();

            userProfile = new Profile();
            userProfile.ReadProfile();

            //this.currentYoga = 1;
            yogas = reader.Yogas;

            if (yogas.Count > 0)
            {
                currentYoga = yogas[0].Id;
            }
        }


        public static Yoga GetYoga(int id)
        {
            try
            {
                return yogas.Where(yoga => yoga.Id == id).First();
            }
            catch (Exception)
            {
                return null;
            }
        }

        //public static void SetYogaScore(int score)
        //{
        //    GetYoga(currentYoga);
        //}

        public void Evaluate(Skeleton skeleton)
        {
            Yoga yoga = GetYoga(currentYoga);

            if (yoga == null)
            {
                System.Console.WriteLine("No more yogas");
                return;
            }

            if (yoga.EvaluateYoga(skeleton))
            {
                // Update profile

                userProfile.SetYogaComplete(currentYoga);
                userProfile.SetYogaScore(currentYoga, GetYoga(currentYoga).Score);
                userProfile.SaveProfile();
                userProfile.ReadProfile();

                // reinitialise score to 0
                yoga.Score = 0;

                // Do necessary UI changes.
                currentYoga++;


            }
        }



    }
}
