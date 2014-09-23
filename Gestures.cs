using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

using System.Windows.Controls;

namespace Microsoft.Samples.Kinect.SkeletonBasics
{

    //partial class Constants
    //{
        
    //}

    public enum Gestures
    {
        // Tadasan
        BothHandsClose,
        BothLegsClose,
        HeadHandLegOnSameX,
        HandAboveHead,
        ElbowBelowHead,
        ElbowAboveHead,

        // N90

        ElbowWristBelowHead,
        ElbowWristOnSameY,
        RightKneeRightToRightHip,
        RightKneeRightFootSameX,
        LeftKneeLeftToLeftHip,
        LeftFootLeftToLeftKnee,
        LeftKneeLeftFootSameX,
        RightFootRightToRightKnee,

        // OneLegAheadOneLegBothHandsBehind
        RightSideToKinnect,
        LeftLegOnLeftOfHipCenter,
        RightLegOnRightOfHipCenter,
        BothHandsOnRightOfHead,
        RightLegOnLeftOfHipCenter,
        LeftLegOnRightOfHipCenter,
        BothHandsOnLeftOfHead,
        LeftSideToKinnect,
        
        // WarmUp
        HandsInFront,
        ElbowWristOnSameX,

        Standing,
        StandingSidewise
    }

    class Mappings
    {
        static public TextBlock textBlock;
        static Dictionary<Gestures, Func<Skeleton, bool>> map = new Dictionary<Gestures,Func<Skeleton,bool>>();

        static List<JointType> jointsToCheck = new List<JointType>();

        public Mappings()
        {
            try
            {
                // Tadasan
                map.Add(Gestures.BothHandsClose, BothHandsClose);
                map.Add(Gestures.BothLegsClose, BothLegsClose);
                map.Add(Gestures.HeadHandLegOnSameX, HeadHandLegOnSameX);
                map.Add(Gestures.HandAboveHead, HandAboveHead);
                map.Add(Gestures.ElbowBelowHead, ElbowBelowHead);
                map.Add(Gestures.ElbowAboveHead, ElbowAboveHead);
                map.Add(Gestures.Standing, Standing);

                // N90
                map.Add(Gestures.ElbowWristBelowHead, ElbowWristBelowHead);
                map.Add(Gestures.ElbowWristOnSameY, ElbowWristOnSameY);
                map.Add(Gestures.RightKneeRightToRightHip, RightKneeRightToRightHip);
                map.Add(Gestures.RightKneeRightFootSameX, RightKneeRightFootSameX);
                map.Add(Gestures.LeftKneeLeftToLeftHip, LeftKneeLeftToLeftHip);
                map.Add(Gestures.LeftFootLeftToLeftKnee, LeftFootLeftToLeftKnee);
                map.Add(Gestures.LeftKneeLeftFootSameX, LeftKneeLeftFootSameX);
                map.Add(Gestures.RightFootRightToRightKnee, RightFootRightToRightKnee);

                        // OneLegAheadOneLegBothHandsBehind
                map.Add(Gestures.StandingSidewise, StandingSidewise);
                map.Add(Gestures.RightSideToKinnect, RightSideToKinnect);
                map.Add(Gestures.LeftLegOnLeftOfHipCenter, LeftLegOnLeftOfHipCenter);
                map.Add(Gestures.RightLegOnRightOfHipCenter, RightLegOnRightOfHipCenter);
                map.Add(Gestures.BothHandsOnRightOfHead, BothHandsOnRightOfHead);
                map.Add(Gestures.RightLegOnLeftOfHipCenter, RightLegOnLeftOfHipCenter);
                map.Add(Gestures.LeftLegOnRightOfHipCenter, LeftLegOnRightOfHipCenter);
                map.Add(Gestures.LeftSideToKinnect, LeftSideToKinnect);
                map.Add(Gestures.BothHandsOnLeftOfHead, BothHandsOnLeftOfHead);

                // WarmUp
                map.Add(Gestures.ElbowWristOnSameX, ElbowWristOnSameX);
                map.Add(Gestures.HandsInFront, HandsInFront);

            }
            catch(Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            
        }

        // Tadasan
        public static bool BothHandsClose(Skeleton skeleton)
        {
            // Call specific method for hands

            jointsToCheck.Clear();
            jointsToCheck.Add(JointType.HandLeft);
            jointsToCheck.Add(JointType.HandRight);


            return Conditions.CloseEnough(skeleton, jointsToCheck);


            //System.Console.WriteLine("BothHandsClose called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return true;
        }

        public static bool BothLegsClose(Skeleton skeleton)
        {
            // Call specific method for hands

            jointsToCheck.Clear();
            jointsToCheck.Add(JointType.FootLeft);
            jointsToCheck.Add(JointType.FootRight);
            
            bool legClose = Conditions.CloseEnough(skeleton, jointsToCheck);

            jointsToCheck.Clear();
            jointsToCheck.Add(JointType.KneeLeft);
            jointsToCheck.Add(JointType.KneeRight);


            return legClose && Conditions.CloseEnough(skeleton, jointsToCheck, Constants.kneeThreshold);

            //System.Console.WriteLine("BothLegsClose called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return true;
        }

        public static bool HeadHandLegOnSameX(Skeleton skeleton)
        {
            // Call specific method for hands

            jointsToCheck.Clear();
            //jointsToCheck.Add(JointType.Head);
            jointsToCheck.Add(JointType.HandLeft);
            jointsToCheck.Add(JointType.HandRight);
            jointsToCheck.Add(JointType.FootLeft);
            jointsToCheck.Add(JointType.FootRight);
            //jointsToCheck.Add(JointType.KneeLeft);
            //jointsToCheck.Add(JointType.KneeRight);

            return Conditions.CloseEnoughX(skeleton, jointsToCheck);

            //System.Console.WriteLine("HeadHandLegOnSameX called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return true;
        }

        public static bool HandAboveHead(Skeleton skeleton)
        {
            // Call specific method for hands

            return Conditions.IsUp(skeleton, JointType.HandLeft, JointType.Head) && Conditions.IsUp(skeleton, JointType.HandRight, JointType.Head);

            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return false;
        }

        public static bool ElbowBelowHead(Skeleton skeleton)
        {
            // Call specific method for hands

            return Conditions.IsDown(skeleton, JointType.ElbowLeft, JointType.Head) && Conditions.IsDown(skeleton, JointType.ElbowRight, JointType.Head);


            //System.Console.WriteLine("ElbowBelowHead called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return false;
        }

        public static bool ElbowAboveHead(Skeleton skeleton)
        {
            // Call specific method for hands

            return Conditions.IsUp(skeleton, JointType.ElbowLeft, JointType.Head) && Conditions.IsUp(skeleton, JointType.ElbowRight, JointType.Head);


            //System.Console.WriteLine("ElbowAboveHead called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return false;
        }


        // N90

        public static bool ElbowWristBelowHead(Skeleton skeleton)
        {
            // Call specific method for hands

            return 
                Conditions.IsDown(skeleton, JointType.ElbowLeft, JointType.Head) 
             && Conditions.IsDown(skeleton, JointType.ElbowRight, JointType.Head)
             && Conditions.IsDown(skeleton, JointType.WristRight, JointType.Head) 
             && Conditions.IsDown(skeleton, JointType.WristLeft, JointType.Head);


            //System.Console.WriteLine("ElbowAboveHead called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return false;
        }

        public static bool ElbowWristOnSameY(Skeleton skeleton)
        {
            // Call specific method for hands

            jointsToCheck.Clear();
            jointsToCheck.Add(JointType.ElbowLeft);
            jointsToCheck.Add(JointType.ElbowRight);
            jointsToCheck.Add(JointType.WristLeft);
            jointsToCheck.Add(JointType.WristRight);

            return Conditions.CloseEnoughY(skeleton, jointsToCheck);


            //System.Console.WriteLine("ElbowAboveHead called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return false;
        }

        public static bool RightKneeRightToRightHip(Skeleton skeleton)
        {
            // Call specific method for hands

            return Conditions.IsRight(skeleton, JointType.KneeRight, JointType.HipRight);


            //System.Console.WriteLine("ElbowAboveHead called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return false;
        }
        public static bool RightKneeRightFootSameX(Skeleton skeleton)
        {
            // Call specific method for hands

            jointsToCheck.Clear();
            jointsToCheck.Add(JointType.KneeRight);
            jointsToCheck.Add(JointType.FootRight);

            return Conditions.CloseEnoughX(skeleton, jointsToCheck);


            //System.Console.WriteLine("ElbowAboveHead called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return false;
        }
        public static bool LeftKneeLeftToLeftHip(Skeleton skeleton)
        {
            // Call specific method for hands

            return Conditions.IsLeft(skeleton, JointType.KneeLeft, JointType.HipLeft);


            //System.Console.WriteLine("ElbowAboveHead called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return false;
        }
        public static bool LeftFootLeftToLeftKnee(Skeleton skeleton)
        {
            // Call specific method for hands

            return Conditions.IsLeft(skeleton, JointType.FootLeft, JointType.KneeLeft);


            //System.Console.WriteLine("ElbowAboveHead called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return false;
        }
        public static bool LeftKneeLeftFootSameX(Skeleton skeleton)
        {
            // Call specific method for hands

            jointsToCheck.Clear();
            jointsToCheck.Add(JointType.KneeLeft);
            jointsToCheck.Add(JointType.FootLeft);

            return Conditions.CloseEnoughX(skeleton, jointsToCheck);


            //System.Console.WriteLine("ElbowAboveHead called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return false;
        }

        public static bool RightFootRightToRightKnee(Skeleton skeleton)
        {
            // Call specific method for hands

            return Conditions.IsRight(skeleton, JointType.FootRight, JointType.KneeRight);


            //System.Console.WriteLine("ElbowAboveHead called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return false;
        }


        // OneLegAheadOneLegBothHandsBehind

        public static bool RightSideToKinnect(Skeleton skeleton)
        {
            // Call specific method for hands

            return Conditions.IsBehind(skeleton, JointType.Spine, JointType.HandRight);


            //System.Console.WriteLine("ElbowAboveHead called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return false;
        }
        public static bool LeftLegOnLeftOfHipCenter(Skeleton skeleton)
        {
            // Call specific method for hands

            return Conditions.IsLeft(skeleton, JointType.FootLeft, JointType.HipCenter);


            //System.Console.WriteLine("ElbowAboveHead called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return false;
        }

        public static bool RightLegOnRightOfHipCenter(Skeleton skeleton)
        {
            // Call specific method for hands

            return 
                Conditions.IsRight(skeleton, JointType.FootRight, JointType.HipCenter);


            //System.Console.WriteLine("ElbowAboveHead called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return false;
        }

        public static bool BothHandsOnRightOfHead(Skeleton skeleton)
        {
            // Call specific method for hands

            return 
                Conditions.IsRight(skeleton, JointType.HandRight, JointType.Head) 
             && Conditions.IsRight(skeleton, JointType.HandLeft, JointType.Head);


            //System.Console.WriteLine("ElbowAboveHead called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return false;
        }

        public static bool RightLegOnLeftOfHipCenter(Skeleton skeleton)
        {
            // Call specific method for hands

            return 
                Conditions.IsLeft(skeleton, JointType.FootRight, JointType.HipCenter);


            //System.Console.WriteLine("ElbowAboveHead called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return false;
        }

        public static bool LeftLegOnRightOfHipCenter(Skeleton skeleton)
        {
            // Call specific method for hands

            return 
                Conditions.IsRight(skeleton, JointType.FootLeft, JointType.HipCenter);


            //System.Console.WriteLine("ElbowAboveHead called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return false;
        }


        public static bool LeftSideToKinnect(Skeleton skeleton)
        {
            // Call specific method for hands

            return Conditions.IsBehind(skeleton, JointType.Spine, JointType.HandLeft);


            //System.Console.WriteLine("ElbowAboveHead called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return false;
        }

        public static bool BothHandsOnLeftOfHead(Skeleton skeleton)
        {
            // Call specific method for hands


            return
                Conditions.IsLeft(skeleton, JointType.HandRight, JointType.Head)
             && Conditions.IsLeft(skeleton, JointType.HandLeft, JointType.Head);

            //System.Console.WriteLine("ElbowAboveHead called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return false;
        }


        
        public static bool ElbowWristOnSameX(Skeleton skeleton)
        {
            // Call specific method for hands

            jointsToCheck.Clear();
            jointsToCheck.Add(JointType.WristLeft);
            jointsToCheck.Add(JointType.ElbowLeft);

            bool result1 = Conditions.CloseEnoughX(skeleton, jointsToCheck);

            jointsToCheck.Clear();
            jointsToCheck.Add(JointType.ElbowRight);
            jointsToCheck.Add(JointType.WristRight);


            return result1 && Conditions.CloseEnoughX(skeleton, jointsToCheck);

            //System.Console.WriteLine("ElbowAboveHead called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return false;
        }

        
        public static bool HandsInFront(Skeleton skeleton)
        {
            // Call specific method for hands


            return
                Conditions.IsInFrontOf(skeleton, JointType.HandRight, JointType.ElbowRight)
             && Conditions.IsInFrontOf(skeleton, JointType.HandLeft, JointType.ElbowLeft)
             && Conditions.IsDown(skeleton, JointType.HandRight, JointType.Head)
             && Conditions.IsDown(skeleton, JointType.HandLeft, JointType.Head)
             && Conditions.IsUp(skeleton, JointType.HandRight, JointType.Spine)
             && Conditions.IsUp(skeleton, JointType.HandLeft, JointType.Spine);

            //System.Console.WriteLine("ElbowAboveHead called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return false;
        }

        public static bool Standing(Skeleton skeleton)
        {
            // Call specific method for hands
            return
            //Conditions.IsDown(skeleton, JointType.ElbowLeft, JointType.Spine)
            //&& Conditions.IsDown(skeleton, JointType.ElbowRight, JointType.Spine)
             Conditions.IsDown(skeleton, JointType.HandLeft, JointType.HipCenter)
            && Conditions.IsDown(skeleton, JointType.HandRight, JointType.HipCenter)
            && Conditions.IsUp(skeleton, JointType.HandLeft, JointType.KneeLeft)
            && Conditions.IsUp(skeleton, JointType.HandRight, JointType.KneeRight);


            //System.Console.WriteLine("Standing called");
            //System.Console.WriteLine(skeleton.Joints[JointType.Head].Position.X);
            //return false;
        }

        public static bool StandingSidewise(Skeleton skeleton)
        {
            jointsToCheck.Clear();

            jointsToCheck.Add(JointType.ShoulderLeft);
            jointsToCheck.Add(JointType.ShoulderRight);

            bool result1 = Conditions.CloseEnoughX(skeleton, jointsToCheck);
            
            //jointsToCheck.Add(JointType.HandLeft);
            //jointsToCheck.Add(JointType.HandRight);
            jointsToCheck.Clear();
            jointsToCheck.Add(JointType.AnkleLeft);
            jointsToCheck.Add(JointType.AnkleRight);
            //jointsToCheck.Add(JointType.Head);
            //jointsToCheck.Add(JointType.Spine);

            return result1 && Conditions.CloseEnoughX(skeleton, jointsToCheck, Constants.ankleThreshold);
        }

        public static Dictionary<Gestures, Func<Skeleton, bool>> Map1
        {
            get { return map; }
        }
    }



        

    }
