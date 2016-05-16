using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
//using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace svchoset
{

    public partial class Form2 : Form
    {

        static string realkey = @Directory.GetCurrentDirectory() + "\\rkey.cer";
        static string path = @Directory.GetCurrentDirectory() + "\\said.log";
        static string secret = "password";
        static FileStream fs;
        static StreamWriter sw;
        static string[] rkeyarr;
        static string decrikey;
        public Form2()
        {
            InitializeComponent();
            getip();
            textBox1.Focus();
            textBox2.Enabled = false;
            textBox1.UseSystemPasswordChar = true;
            refreshkey();
        }
       /* static void enc()
        {
            if (System.IO.File.Exists(realkey) && System.IO.File.Exists(path))
            {
                string[] rkeyarr = System.IO.File.ReadAllLines(realkey, Encoding.Default);
                string[] contents = System.IO.File.ReadAllLines(path, Encoding.Default);
                string decrikey = AESDecrypt256(rkeyarr[0], secret);
                fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs, System.Text.Encoding.Default);
                for (int i = 0; i < contents.Length - 1; i++)
                {
                    contents[i] = AESEncrypt256(contents[i], decrikey);
                    sw.WriteLine(contents[i]);
                    sw.Flush();
                }
                sw.Close();
                fs.Close();
                
            }
        }
        static void dec()
        {
            if (System.IO.File.Exists(realkey) && System.IO.File.Exists(path))
            {
                string[] rkeyarr = System.IO.File.ReadAllLines(realkey, Encoding.Default);
                string[] contents = System.IO.File.ReadAllLines(path, Encoding.Default);
                string decrikey = AESDecrypt256(rkeyarr[0], secret);
                fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs, System.Text.Encoding.Default);
                for (int i = 0; i < contents.Length - 1; i++)
                {
                    contents[i] = AESDecrypt256(contents[i], decrikey);
                    sw.WriteLine(contents[i]);
                    sw.Flush();
                }
                sw.Close();
                fs.Close();
             
            }
        }*/
        static string getip()
        {
            string ips = null;
            System.Diagnostics.ProcessStartInfo proInfo = new System.Diagnostics.ProcessStartInfo();//Process 실행 정보를 설정할 ProcessStartInfo생성
            System.Diagnostics.Process pro = new System.Diagnostics.Process();//Process생성
            proInfo.FileName = "CMD.exe";//실행할 파일명 입력--cmd
            proInfo.WorkingDirectory = @"C:\windows\system32";
            proInfo.CreateNoWindow = true;//cmd창 띄우기
            proInfo.UseShellExecute = false;//
            proInfo.RedirectStandardOutput = true;//cmd 에서 나온 데이터 받기
            proInfo.RedirectStandardInput = true;//cmd 에 데이터 보내기
            proInfo.RedirectStandardError = true;//cmd 오류내용 받기

            pro.StartInfo = proInfo;//Process 실행정보 추가
            pro.Start();//Process 시작
            pro.StandardInput.Write("ipconfig" + Environment.NewLine);
            pro.StandardInput.Close();
            String resultValue = pro.StandardOutput.ReadToEnd();
            pro.Close();
            int IPindex = resultValue.IndexOf("IPv4");
            for (int i = 28; i < 40; i++)
            {
                ips += resultValue[IPindex + i];
            }
            ips = ips.Trim();
            return ips;

        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            string Tex = e.KeyCode.ToString();
            if (Tex.Equals("Return"))
            {
                if (textBox1.Text == "password")
                {
                    textBox1.Clear();
                    textBox1.Enabled = false;
                    textBox2.Enabled = true;
                    textBox2.UseSystemPasswordChar = true;
                    textBox2.Focus();
                }
                else
                {
                    if (System.IO.File.Exists(realkey))
                    {
                        string did = ("[X]" + getip() + " 님이 " + DateTime.Now.ToString() + "에 잘못된 비밀번호" + textBox1.Text + "로 비번 변경을 실패하였습니다.");
                        fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                        sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw.WriteLine(did);
                        textBox1.Clear();
                        sw.Flush();
                        sw.Close();
                        fs.Close();
                        this.Close();
                    }
                }
            }
            else if (Tex.Equals("F1"))
            {
                Opacity += 0.1;
            }
            else if (Tex.Equals("F2"))
            {
                Opacity -= 0.1;
            }
            
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            string Tex = e.KeyCode.ToString();
            if (Tex.Equals("Return"))
            {
                if (textBox1.Enabled==false)
                {
                    fs = new FileStream(realkey, FileMode.Create, FileAccess.Write);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.Write(AESEncrypt256(textBox2.Text,secret));
                    sw.Close();
                    fs.Close();
                    refreshkey();
                    string did = ("[O]" + getip() + " 님이 " + DateTime.Now.ToString() + "에 비밀번호 변경을 성공하였습니다. 대화 내용을 초기화합니다.");
                    fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.Close();
                    fs.Close();
                    fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.WriteLine(AESEncrypt256(did,decrikey));
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                    
                    this.Close();
                    
                }
                else
                {
                    this.Close();
                }
            }
        }
        static string AESEncrypt256(string InputText, string Password)
        {
            RijndaelManaged RijndaelCipher = new RijndaelManaged();
            byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(InputText);//입력받은문자열바이트배열로변환
            byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);//dictionary 대비
            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));//IV 16바이트 secretkey 32바이트 통해 encryptor 객체 만듬
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptostream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);//cryptostream 객체를 암호화된 데이터를 쓰기위한 용도로 선언
            cryptostream.Write(PlainText, 0, PlainText.Length);
            cryptostream.FlushFinalBlock();
            byte[] CipherBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptostream.Close();
            string EncryptedData = Convert.ToBase64String(CipherBytes);
            return EncryptedData;
        }
        static string AESDecrypt256(string InputText, string Password)
        {
            RijndaelManaged RijndaelCipher = new RijndaelManaged();
            byte[] EncryptedData = Convert.FromBase64String(InputText);
            byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);//dictionary 대비
            ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));//IV 16바이트 secretkey 32바이트 통해 encryptor 객체 만듬

            MemoryStream memoryStream = new MemoryStream(EncryptedData);

            CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);//cryptostream 객체를 암호화된 데이터를 읽기위한 용도로 선언

            byte[] PlainText = new byte[EncryptedData.Length];//입력받은문자열바이트배열로변환

            int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);

            memoryStream.Close();
            cryptoStream.Close();
            string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
            return DecryptedData;
        }
        static void refreshkey()
        {
            if (System.IO.File.Exists(realkey))
            {
                rkeyarr = System.IO.File.ReadAllLines(realkey, Encoding.Default);
                decrikey = AESDecrypt256(rkeyarr[0], secret);
            }
        }

    }
}


   