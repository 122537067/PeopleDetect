using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace peoplePicture
{
    public partial class Form1 : Form
    {
        // 人脸检测与属性分析
        public static string Detect(String PictureBase64)
        {
            string token = AccessToken.getAccessToken();
            string host = "https://aip.baidubce.com/rest/2.0/face/v3/detect?access_token=" + token;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
            String str = "{\"image\":\"" + PictureBase64 + "\",\"image_type\":\"BASE64\",\"face_field\":\"age,beauty,expression,face_shape,gender,glasses,race,quality,face_type\"}";
            byte[] buffer = encoding.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string result = reader.ReadToEnd();
            Console.WriteLine("人脸检测与属性分析:");
            Console.WriteLine(result);
            return result;
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox_token.Text = AccessToken.getAccessToken();
        }

        private string pathname = string.Empty;
        private void button_open_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = ".";
            file.Filter = "所有文件(*.*)|*.*";
            file.ShowDialog();
            if (file.FileName != string.Empty)
            {
                try
                {
                    pathname = file.FileName;   //获得文件的绝对路径
                    this.pictureBox1.Load(pathname);
                    label_path.Text = pathname;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button_detect_Click(object sender, EventArgs e)
        {
            try
            {
                //将图片转换成BASE64编码
                Bitmap bmp = new Bitmap(label_path.Text);
                this.pictureBox1.Image = bmp;
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                String strbaser64 = Convert.ToBase64String(arr);

                //显示数据
                textBox_token.Text = Detect(strbaser64);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ImgToBase64String 转换失败\nException:" + ex.Message);
            }
        }

        private void button_change_Click(object sender, EventArgs e)
        {
            //解析JSON数据
            RootObject people = JsonConvert.DeserializeObject<RootObject>(textBox_token.Text);

            //获取值
            textBox_token.Text = "转换结果：";
            textBox_token.Text += people.error_msg + "\r\n" + "\r\n";

            textBox_token.Text += "这是一张人脸的概率：";
            textBox_token.Text += people.result.face_list[0].face_probability + "\r\n" + "\r\n";

            textBox_token.Text += "年龄：";
            textBox_token.Text += people.result.face_list[0].age + "\r\n" + "\r\n";

            textBox_token.Text += "颜值：";
            textBox_token.Text += people.result.face_list[0].beauty + "\r\n" + "\r\n";

            textBox_token.Text += "表情：";
            textBox_token.Text += people.result.face_list[0].expression.probability + "概率是";
            string expression_type = people.result.face_list[0].expression.type;
            if(expression_type == "none") { expression_type = "冷漠脸"; }
            else if(expression_type == "smile") { expression_type = "微笑"; }
            else if (expression_type == "laugh") { expression_type = "大笑"; }
            else { expression_type = "都不知道你什么表情"; }
            textBox_token.Text += expression_type + "\r\n" + "\r\n";

            //square: 正方形 triangle:三角形 oval: 椭圆 heart: 心形 round: 圆形
            textBox_token.Text += "脸型：";
            textBox_token.Text += people.result.face_list[0].face_shape.probability + "概率是";
            string faceShape = people.result.face_list[0].face_shape.type;
            if (faceShape == "square") { faceShape = "正方形"; }
            else if (faceShape == "triangle") { faceShape = "三角形"; }
            else if (faceShape == "oval") { faceShape = "椭圆"; }
            else if (faceShape == "heart") { faceShape = "心形"; }
            else if (faceShape == "round") { faceShape = "圆形"; }
            else { faceShape = "都不知道你什么头"; }
            textBox_token.Text += faceShape + "\r\n" + "\r\n";

            //male:男性 female:女性
            textBox_token.Text += "性别：";
            textBox_token.Text += people.result.face_list[0].gender.probability + "概率是";
            string sex = people.result.face_list[0].gender.type;
            if (sex == "male") { sex = "男性"; }
            else if (sex == "female") { sex = "女性"; }
            else { sex = "都不知道你什么性别"; }
            textBox_token.Text += sex + "\r\n" + "\r\n";

            //none:无眼镜，common:普通眼镜，sun:墨镜
            textBox_token.Text += "眼镜类型：";
            textBox_token.Text += people.result.face_list[0].glasses.probability + "概率是";
            string glasse = people.result.face_list[0].glasses.type;
            if (glasse == "none") { glasse = "无眼镜"; }
            else if (glasse == "common") { glasse = "普通眼镜"; }
            else if (glasse == "sun") { glasse = "墨镜"; }
            else { faceShape = "眼呢？"; }
            textBox_token.Text += glasse + "\r\n" + "\r\n";

            //yellow: 黄种人 white: 白种人 black:黑种人 arabs: 阿拉伯人
            textBox_token.Text += "种族：";
            textBox_token.Text += people.result.face_list[0].race.probability + "概率是";
            string race = people.result.face_list[0].race.type;
            if (race == "yellow") { race = "黄种人"; }
            else if (race == "white") { race = "白种人"; }
            else if (race == "black") { race = "黑种人"; }
            else if (race == "arabs") { race = "阿拉伯人"; }
            else { race = "人呢？"; }
            textBox_token.Text += race + "\r\n" + "\r\n";
        }
    }

    public static class AccessToken
    {
        // 调用getAccessToken()获取的 access_token建议根据expires_in 时间 设置缓存
        // 返回token示例
        public static String TOKEN = "24.adda70c11b9786206253ddb70affdc46.2592000.1493524354.282335-1234567";

        // 百度云中开通对应服务应用的 API Key 建议开通应用的时候多选服务
        private static String clientId = "x3S0vAVP7MMNVHn4bM1m2LlS";
        // 百度云中开通对应服务应用的 Secret Key
        private static String clientSecret = "3HOiMXgkOtimM3HekXLmIugUXlgDPnmA";

        public static String getAccessToken()
        {
            String authHost = "https://aip.baidubce.com/oauth/2.0/token";
            HttpClient client = new HttpClient();
            List<KeyValuePair<String, String>> paraList = new List<KeyValuePair<string, string>>();
            paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            paraList.Add(new KeyValuePair<string, string>("client_id", clientId));
            paraList.Add(new KeyValuePair<string, string>("client_secret", clientSecret));

            HttpResponseMessage response = client.PostAsync(authHost, new FormUrlEncodedContent(paraList)).Result;
            String result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
            return result;
        }
    }
}
