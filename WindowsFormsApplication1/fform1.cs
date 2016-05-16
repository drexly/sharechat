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
using svchoset;
namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        static FileStream fs;
        static StreamWriter sw;
        static string path = @Directory.GetCurrentDirectory() + "\\said.log";
        static string checkpath = @Directory.GetCurrentDirectory() + "\\key.log";
        static string enckey = @Directory.GetCurrentDirectory() + "\\ekey.log";
        static string realkey = @Directory.GetCurrentDirectory() + "\\rkey.log";
        static string ip=null;
        static long filesize;
        static string secret = "password";
        public Form1()
        {
            InitializeComponent();
            textBox1.Focus();
            richTextBox1.Enabled = false;
            textBox1.UseSystemPasswordChar = true;
            getip();
            if (System.IO.File.Exists(path))
            {
                FileInfo said = new FileInfo(path);
                filesize = said.Length;
            }
        }
        static void EncryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
            FileStream fsInput = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);//읽기
            FileStream fsEncrypted = new FileStream(sOutputFilename, FileMode.Create, FileAccess.Write);//쓰기

            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);//s키 초기화 벡터

            ICryptoTransform desencrypt = DES.CreateEncryptor();
            CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);
            byte[] bytearrayinput = new byte[fsInput.Length];

            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Close();

            fsInput.Close();
            fsEncrypted.Close();
        }

        static void DecryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);//s키 초기화 벡터

            FileStream fsread = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read);

            StreamWriter fsDecrypted = new StreamWriter(sOutputFilename);
            fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
            fsDecrypted.Flush();
            fsDecrypted.Close();
        }
        static void getip()
        {
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
                ip+=resultValue[IPindex +i];
            }
            ip = ip.Trim();
            return;
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            richTextBox1.Enabled = true;
            textBox1.Focus();
            string Tex = e.KeyCode.ToString();
            if (Tex.Equals("Return"))
            {
                if (textBox1.UseSystemPasswordChar)
                {
                    fs = new FileStream(checkpath, FileMode.Create, FileAccess.Write);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.Write(textBox1.Text);
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                    EncryptFile(@checkpath, @enckey, secret);
                    if (System.IO.File.Exists(realkey))
                    {
                        string[] rkeyarr = System.IO.File.ReadAllLines(realkey, Encoding.Default);
                        string[] ekeyarr = System.IO.File.ReadAllLines(enckey, Encoding.Default);
                        if (rkeyarr[0]==ekeyarr[0])
                        {
                            textBox1.UseSystemPasswordChar = false;
                            textBox1.Clear();
                        fs = new FileStream(checkpath, FileMode.Create, FileAccess.Write);
                        sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw.Close();
                        fs.Close();
                            fs = null;
                            sw = null;
string did = (">>"+ip +" 님이 "+DateTime.Now.ToString()+" 에 입장하셨습니다" );
                    richTextBox1.AppendText(did + "\n");
                    fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.WriteLine(did);
                    textBox1.Clear();
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                    FileInfo said = new FileInfo(path);
                    filesize = said.Length;
                    richTextBox1_DoubleClick(sender, e);

                        }
                        else
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        Form2 form2 = new Form2();
                        form2.ShowDialog();
                    }
                   
                }
                else
                {
                    string did = (ip + ">>\t" + textBox1.Text + "\t" + "@" + DateTime.Now.ToString());
                    richTextBox1.AppendText(did + "\n");
                    fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.WriteLine(did);
                    textBox1.Clear();
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                    FileInfo said = new FileInfo(path);
                    filesize = said.Length;
                    richTextBox1_DoubleClick(sender, e);
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
            else if (Tex.Equals("F3"))
            {
                Color temp = richTextBox1.BackColor;
                richTextBox1.BackColor = richTextBox1.ForeColor;
                richTextBox1.ForeColor = temp;
                temp = textBox1.BackColor;
                textBox1.BackColor = textBox1.ForeColor;
                textBox1.ForeColor = temp;
                temp = this.BackColor;
                this.BackColor = this.ForeColor;
                this.ForeColor = temp;
            }
            else if (Tex.Equals("F4"))
            {
                textBox1.Clear();
            }
            else if (Tex.Equals("F5"))
            {
                richTextBox1_DoubleClick(sender, e);
            }
            else if (Tex.Equals("F6"))
            {
                
            }
            else if (Tex.Equals("F7"))
            {
                Form2 form2 = new Form2();
                form2.ShowDialog();
                //  SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                // this.BackColor = Color.Transparent;
                //  richTextBox1.BackColor= Color.Transparent;
                //  textBox1.BackColor= Color.Transparent;

                //Color temp = TransparencyKey;
                //TransparencyKey = this.BackColor;
            }
            else if (Tex.Equals("F8"))
            {
                richTextBox1.Clear();

            }
            else if (Tex.Equals("F12"))
            {
                this.Close();

            }
            else if (Tex.Equals("Pause"))
            {
                richTextBox1.Clear();
                fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs, System.Text.Encoding.Default);
                sw.Close();
                fs.Close();
            }

         
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(path))
            {
                FileInfo said = new FileInfo(path);
                if (said.Length!=filesize)
                {
                    //this.ShowInTaskbar = false;
                    filesize = said.Length;
                   richTextBox1_DoubleClick(sender,e);
                    //this.ShowInTaskbar = true;
                }
            }
        }

        private void richTextBox1_DoubleClick(object sender, EventArgs e)
        {
           if (!textBox1.UseSystemPasswordChar)
           {
            richTextBox1.Clear();
            string[] textValue = System.IO.File.ReadAllLines(path, Encoding.Default);
            foreach (string item in textValue)
            {
                richTextBox1.AppendText(item + "\n");
            }
            richTextBox1.Select(richTextBox1.Text.Length, 0);
            //richTextBox1.SelectionStart=richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
           }
        }
    }
}
