using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace peoplePicture
{
    public class Location
    {
        public string left { get; set; }
        public string top { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string rotation { get; set; }
    }

    public class Angle
    {
        public string yaw { get; set; }
        public string pitch { get; set; }
        public string roll { get; set; }
    }

    public class Expression
    {
        public string type { get; set; }
        public string probability { get; set; }
    }

    public class Face_shape
    {
        public string type { get; set; }
        public string probability { get; set; }
    }

    public class Gender
    {
        public string type { get; set; }
        public string probability { get; set; }
    }

    public class Glasses
    {
        public string type { get; set; }
        public string probability { get; set; }
    }

    public class Race
    {
        public string type { get; set; }
        public string probability { get; set; }
    }

    public class Occlusion
    {
        public string left_eye { get; set; }
        public string right_eye { get; set; }
        public string nose { get; set; }
        public string mouth { get; set; }
        public string left_cheek { get; set; }
        public string right_cheek { get; set; }
        public string chin_contour { get; set; }
    }

    public class Quality
    {
        public Occlusion occlusion { get; set; }
        public string blur { get; set; }
        public string illumination { get; set; }
        public string completeness { get; set; }
    }

    public class Face_type
    {
        public string type { get; set; }
        public string probability { get; set; }
    }

    public class Face_list
    {
        public string face_token { get; set; }
        public Location location { get; set; }
        public string face_probability { get; set; }
        public Angle angle { get; set; }
        public string age { get; set; }
        public string beauty { get; set; }
        public Expression expression { get; set; }
        public Face_shape face_shape { get; set; }
        public Gender gender { get; set; }
        public Glasses glasses { get; set; }
        public Race race { get; set; }
        public Quality quality { get; set; }
        public Face_type face_type { get; set; }
    }

    public class Result
    {
        public string face_num { get; set; }
        public List<Face_list> face_list { get; set; }
    }

    public class RootObject
    {
        public string error_code { get; set; }
        public string error_msg { get; set; }
        public string log_id { get; set; }
        public string timestamp { get; set; }
        public string cached { get; set; }
        public Result result { get; set; }
    }
}
