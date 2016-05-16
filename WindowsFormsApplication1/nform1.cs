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
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        static FileStream fs;
        static StreamWriter sw;
        //static string path = @"2artbde.army.mil/upload/board/3993/said.log";// + "said.log";
        //static string realkey = @"2artbde.army.mil/upload/board/3993/said.log";// + "rkey.cer";
        static string path = @Directory.GetCurrentDirectory() + "\\said.log";
        //static string errpath = @Directory.GetCurrentDirectory() + "\\bugreport.log";
        static string realkey = @Directory.GetCurrentDirectory() + "\\rkey.cer";
        static string ip = null;
        static long filesize;
        static string secret = "password";
        static int active = 0;
        static string current = null;
        static bool onmain = true;
        static bool security = false;
        static string[] rkeyarr;
        static string decrikey;
        static bool realexit = false;
        static string realname=null;
        static bool hasbeen=false;
        static bool pop = false;
        static bool poponce = true;
        static bool javis = false;
        static string javistemp = null;
        static RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
        static string info = "\n\n\n\t\t   SECURE MODE LOCK ACTIVATED: ENTER CORRECT PASSWORD OR LEAVE\n\n\n\n\t\tF1: Opacity+\t F2: Opacity-\t F3: BLACK/WHITE\t F11: BALLOON/POP\n\n\n\t\tF4: TYPING CLEAR\t F5: REFRESH DIALOGUE MANUALLY\n\n\n\t\tF6: SECURE MODE LOCK ON/OFF(like PC KaTalk)\t F7: RESET PW\n\n\n\t\tF8: DIALOGUE CLEAR\tF10: START PROGRAM ON/OFF\tF12: EXIT TO TRAY";
        [DllImport("user32.dll")]
        static extern Int32 FlashWindowEx(ref FLASHWINFO pwfi);
        [StructLayout(LayoutKind.Sequential)]

        public struct FLASHWINFO
        {
            public UInt32 cbSize;
            public IntPtr hwnd;
            public UInt32 dwFlags;
            public UInt32 uCount;
            public UInt32 dwTimeout;
        }

        public const UInt32 FLASHW_STOP = 0;//stop flashin system restores the window to its original state
        public const UInt32 FLASHW_CAPTION = 1;//flashin the window caption
        public const UInt32 FLASHW_TRAY = 2;//flashin the taskbar button
        public const UInt32 FLASHW_ALL = 3;//flashin the taskbar button&caption
        public const UInt32 FLASHW_TIMER = 4;//flashin continuously till flashw_stop flag is set
        public const UInt32 FLASHW_TIMERNOFG = 12;//flashin continuously till window comes to the foreground

      
        public Form1()
        {
            InitializeComponent();
            textBox1.Focus();
            richTextBox1.Text = info;
            richTextBox1.Enabled = false;
            textBox1.UseSystemPasswordChar = true;
            getname();
            getip();
            if (System.IO.File.Exists(path))
            {
                connecting();
                FileInfo said = new FileInfo(path);
                filesize = said.Length;
                if (security)
                {
                    Text = string.Format("RAWS *SECURED* {0}::{1}", active, current);
                }
                else
                {
                    Text = string.Format("RAWS {0}::{1}", active, current);
                }
            }
            Text = "★잠금모드 ON(Off/On F6), 시작프로그램 등록 OFF(등록하려면 F10), 이제 비번입력 및 ENTER";
        }

        static void connecting()
        {
            refreshkey();
            string[] inandout = System.IO.File.ReadAllLines(path, Encoding.Default);
            int s = 0;
            foreach (string item in inandout)
            {
                if (item.Contains("비번"))
                {
                    s += 1;
                    continue;
                }
                inandout[s++] = AESDecrypt256(item, decrikey);
            }
            current = null;
            active = 0;
            for (int j = 0; j < inandout.Length; j++)
            {
                if (inandout[j][0] == '→')
                {
                    string inip = null;
                    for (int i = 1; !char.IsLetter(inandout[j][i]); i++)
                    {
                        inip += inandout[j][i];
                    }
                    bool isin = true;
                    for (int k = j; k < inandout.Length; k++)
                    {
                        if (inandout[k][0] == '←' && inandout[k].Contains(inip))
                        {
                            isin = false;
                        }
                    }
                    if (isin)
                    {
                        active++;
                        current += (inip + " ");
                    }
                }
            }
            
        }
      
        static void getname()
        {
            string[] nameo = Application.StartupPath.ToString().Split('\\');
            realname=nameo[nameo.Length - 1].ToUpper();
            /*string engname=nameo[nameo.Length - 1].ToUpper();
            foreach (char item2 in engname)
            {
                switch (item2)
                {
                    default:
                        realname += item2; break;
                    case 'A':
                        realname += 'ㅁ'; break;
                    case 'B':
                        realname += 'ㅠ'; break;
                    case 'C': realname += 'ㅊ'; break;
                    case 'D': realname += 'ㅇ'; break;
                    case 'E': realname += 'ㄷ'; break;
                    case 'F': realname += 'ㄹ'; break;
                    case 'G': realname += 'ㅎ'; break;
                    case 'H': realname += 'ㅗ'; break;
                    case 'I': realname += 'ㅑ'; break;
                    case 'J': realname += 'ㅓ'; break;
                    case 'K': realname += 'ㅏ'; break;
                    case 'L': realname += 'ㅣ'; break;
                    case 'M': realname += 'ㅡ'; break;
                    case 'N': realname += 'ㅜ'; break;
                    case 'O': realname += 'ㅐ'; break;
                    case 'P': realname += 'ㅔ'; break;
                    case 'Q': realname += 'ㅂ'; break;
                    case 'R': realname += 'ㄱ'; break;
                    case 'S': realname += 'ㄴ'; break;
                    case 'T': realname += 'ㅅ'; break;
                    case 'U': realname += 'ㅕ'; break;
                    case 'V': realname += 'ㅍ'; break;
                    case 'W': realname += 'ㅈ'; break;
                    case 'X': realname += 'ㅌ'; break;
                    case 'Y': realname += 'ㅛ'; break;
                    case 'Z': realname += 'ㅋ'; break;
                    case ' ': realname += ' '; break;
                    case 'ㅁ': realname += 'A'; break;
                    case 'ㅠ': realname += 'B'; break;
                    case 'ㅊ': realname += 'C'; break;
                    case 'ㅇ': realname += 'D'; break;
                    case 'ㄷ': realname += 'E'; break;
                    case 'ㄹ': realname += 'F'; break;
                    case 'ㅎ': realname += 'G'; break;
                    case 'ㅗ': realname += 'H'; break;
                    case 'ㅑ': realname += 'I'; break;
                    case 'ㅓ': realname += 'J'; break;
                    case 'ㅏ': realname += 'K'; break;
                    case 'ㅣ': realname += 'L'; break;
                    case 'ㅡ': realname += 'M'; break;
                    case 'ㅜ': realname += 'N'; break;
                    case 'ㅐ': realname += 'O'; break;
                    case 'ㅔ': realname += 'P'; break;
                    case 'ㅂ': realname += 'Q'; break;
                    case 'ㄱ': realname += 'R'; break;
                    case 'ㄴ': realname += 'S'; break;
                    case 'ㅅ': realname += 'T'; break;
                    case 'ㅕ': realname += 'U'; break;
                    case 'ㅍ': realname += 'V'; break;
                    case 'ㅈ': realname += 'W'; break;
                    case 'ㅌ': realname += 'X'; break;
                    case 'ㅛ': realname += 'Y'; break;
                    case 'ㅋ': realname += 'Z'; break;
                }
            }*/
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
                ip += resultValue[IPindex + i];
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
                refreshkey();
                if (javis)
                {
                    string did = null;
                    if (textBox1.Text == "rlawngusslacksdidgkqslek")
                    {
                        did = (ip + ">>\t" + javistemp + "\t" + "@" + DateTime.Now.ToString());
                    }
                    else
                    {
                        did = (ip + ">>\t인공지능 자비스는 주인님 말만 듣는답니다\t" + "@" + DateTime.Now.ToString());
                    }
                    richTextBox1.AppendText(did + "\n");
                    fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.WriteLine(AESEncrypt256(did, decrikey));
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                    FileInfo said = new FileInfo(path);
                    filesize = said.Length;
                    textBox1.Clear();                    
                    textBox1.UseSystemPasswordChar = false;
                    javis = false;
                    richTextBox1_DoubleClick(sender, e);
                }
                else if (textBox1.Text.Contains("자비스,"))
                {
                    javistemp = textBox1.Text;
                    textBox1.Clear();
                    textBox1.UseSystemPasswordChar = true;
                    javis = true;
                }
                else if (textBox1.UseSystemPasswordChar)
                {
                    string ekey = AESEncrypt256(textBox1.Text,secret);
                    if (System.IO.File.Exists(realkey))
                    {
                        if (rkeyarr[0] == ekey)
                        {
                            textBox1.Clear();
                            if (!security)
                            {
                                string did = ("→" + ip + " 님이 " + DateTime.Now.ToString() + " 에 입장하셨습니다");
                                hasbeen = true;
                                richTextBox1.AppendText(did + "\n");
                                fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                                sw = new StreamWriter(fs, System.Text.Encoding.Default);
                                sw.WriteLine(AESEncrypt256(did, decrikey));
                                textBox1.Clear();
                                sw.Flush();
                                sw.Close();
                                fs.Close();
                            }
                          
                            FileInfo said = new FileInfo(path);
                            filesize = said.Length;
                            textBox1.UseSystemPasswordChar = false;
                            security = true;
                        }
                        else
                        {
                            refreshkey();
                            string did = ("[X]" + ip + " 님이 " + DateTime.Now.ToString() + "에 잘못된 비밀번호" + textBox1.Text + "를 입력하였습니다.");
                            string did1 = ("←" + ip + " 님이 " + DateTime.Now.ToString() + " 에 강퇴당하셨습니다");
                            richTextBox1.AppendText(did + "\n" + did1 + "\n");
                            fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                            sw = new StreamWriter(fs, System.Text.Encoding.Default);
                            sw.WriteLine(AESEncrypt256(did, decrikey));
                            sw.WriteLine(AESEncrypt256(did1, decrikey));
                            textBox1.Clear();
                            sw.Flush();
                            sw.Close();
                            fs.Close();
                            FileInfo said = new FileInfo(path);
                            filesize = said.Length;
                            realexit = true;
                            this.Close();
                            Application.Exit();
                        }
                        richTextBox1_DoubleClick(sender, e);                        
                    }
                    else
                    {
                        Form2 form2 = new Form2();
                        form2.ShowDialog();
                    }
                }
                else
                {
                    refreshkey();
                    string did = (ip + ">>\t" + textBox1.Text + "\t" + "@" + DateTime.Now.ToString());
                    richTextBox1.AppendText(did + "\n");
                    fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.WriteLine(AESEncrypt256(did, decrikey));
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
                if (!textBox1.UseSystemPasswordChar)
                {
                    if (security)
                    {
                        security = false;
                        notifyIcon1.BalloonTipTitle = realname;
                        notifyIcon1.BalloonTipText = "★잠금모드가 비활성화 되었습니다(잠금모드 활성화 F6)";
                        notifyIcon1.ShowBalloonTip(50);
                    }
                    else
                    {
                        security = true;
                        notifyIcon1.BalloonTipTitle = realname;
                        notifyIcon1.BalloonTipText = "★잠금모드가 활성화 되었습니다(잠금모드 해제 F6)";
                        notifyIcon1.ShowBalloonTip(50);
                    }
                   
                }
               
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
            else if (Tex.Equals("F10"))
            {
                if (rk.GetValue(Application.ExecutablePath.ToString()) == null)
                {
                    rk.SetValue(realname, Application.StartupPath.ToString() + "\\baro.lnk");
                    notifyIcon1.BalloonTipTitle = realname;
                    notifyIcon1.BalloonTipText = "★시작프로그램에 등록 되었습니다 (등록 해제 F10)";
                    notifyIcon1.ShowBalloonTip(50);
                }
                else 
                {
                    rk.DeleteValue(realname, false);
                    notifyIcon1.BalloonTipTitle = realname;
                    notifyIcon1.BalloonTipText = "★시작프로그램으로부터 해제 되었습니다 (재등록 F10)";
                    notifyIcon1.ShowBalloonTip(50);
                }
            }
            else if (Tex.Equals("F11"))
            {
                if (pop)
                {
                    pop = false;
                    notifyIcon1.BalloonTipTitle = realname;
                    notifyIcon1.BalloonTipText = "★이제 트레이 풍선으로만 알림이옵니다.\n(팝업으로 바꾸려면 F11)";
                    notifyIcon1.ShowBalloonTip(50);
                }
                else
                {
                    pop = true;
                    notifyIcon1.BalloonTipTitle = realname;
                    notifyIcon1.BalloonTipText = "★이제 팝업으로만 알림이옵니다.\n(트레이 풍선으로 바꾸려면 F11)";
                    notifyIcon1.ShowBalloonTip(50);
                }
            }
            else if (Tex.Equals("F12"))
            {
                this.Close();
            }
            else if (Tex.Equals("F9"))
            {
                int actived = active;
                string []currented = current.Split(' ');
                richTextBox1.Clear();
                fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs, System.Text.Encoding.Default);
                sw.Close();
                fs.Close();    
                foreach (string item in currented)
                {
                    if (string.IsNullOrEmpty(item))
                    {
                        continue;
                    }
                    string did = ("→" + item + " 님이 " + DateTime.Now.ToString() + " 현재 입장중입니다");
                    richTextBox1.AppendText(did + "\n");
                    fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.WriteLine(AESEncrypt256(did, decrikey));
                    sw.Close();
                    fs.Close();    
                }                
            }
        }
        public static bool FlashWindowEx(IntPtr hWnd)
        {
            FLASHWINFO newmsg = new FLASHWINFO();
            newmsg.cbSize = Convert.ToUInt32(Marshal.SizeOf(newmsg));
            newmsg.hwnd = hWnd;
            newmsg.dwFlags =onmain ? (FLASHW_CAPTION | FLASHW_TIMER):(FLASHW_ALL | FLASHW_TIMERNOFG);
            newmsg.uCount = 3;
            newmsg.dwTimeout = 500;
            return (FlashWindowEx(ref newmsg) == 0);
        }
     
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(path))
            {
                FileInfo said = new FileInfo(path);
                if (said.Length != filesize)
                {    
                    filesize = said.Length;
                    richTextBox1_DoubleClick(sender, e);
                        if (this.Visible==false)
                        {
                            if (pop)
                            {
                                this.Visible = true;
                                if (this.WindowState == FormWindowState.Minimized)
                                {
                                    this.WindowState = FormWindowState.Normal;
                                }
                                this.Activate();
                            }
                            else
                            {
                                if (!hasbeen)
                                {
                                    notifyIcon1.BalloonTipTitle = realname+" LOCKED";
                                    notifyIcon1.BalloonTipText = "Unlock to Start";
                                }
                                else if (security)
                                {
                                    notifyIcon1.BalloonTipTitle = realname+" SECURE MODE";
                                    notifyIcon1.BalloonTipText = Text;
                                }
                                else
                                {
                                    notifyIcon1.BalloonTipTitle = realname + " New MSG";
                                    string[] tv = richTextBox1.Text.Split('\n');
                                   foreach (string item in tv)
                                    {
                                        if (!string.IsNullOrEmpty(item))
                                        {
                                            notifyIcon1.BalloonTipText = item;
                                        }
                                    }
                                }
                                notifyIcon1.ShowBalloonTip(50);
                            }
                        }
                        FlashWindowEx(this.Handle);
                }
            }
            /* if (!richTextBox1.Enabled&&!textBox1.UseSystemPasswordChar)
            {
                 textBox1.Enabled = false;
                         notifyIcon1.BalloonTipTitle = "ⓒ332HQ A.C.E 2015";
                         notifyIcon1.BalloonTipText = "시스템 보안위협이 감지되어 종료합니다\n다시 시작해주세요";
                         notifyIcon1.ShowBalloonTip(3000);
                         fs = new FileStream(errpath, FileMode.Append, FileAccess.Write);
                         sw = new StreamWriter(fs, System.Text.Encoding.Default);
                         sw.WriteLine(DateTime.Now.ToString()+" "+ip);
                         sw.Flush();
                         sw.Close();
                         fs.Close();
                         Thread.Sleep(3000);
                         realexit = true;
                         this.Close();
                         Application.Exit();
            }*/
        }

        private void richTextBox1_DoubleClick(object sender, EventArgs e)
        {
            if (!textBox1.UseSystemPasswordChar)
            {
                refreshkey();
                richTextBox1.Clear();
                string[] textValue = System.IO.File.ReadAllLines(path, Encoding.Default);
                int s = 0;
                foreach (string item in textValue)
	            {
                    if (item.Contains("비번"))
                    {
                        s += 1;
                        continue;
                    }
                     textValue[s++] = AESDecrypt256(item, decrikey);
	            }
                foreach (string item in textValue)
                {
                    richTextBox1.AppendText(item + "\n");
                    if (item[0] == '→' || item[0] == '←')
                    {
                        richTextBox1.Select(richTextBox1.TextLength - item.Length, item.Length);
                        richTextBox1.SelectionColor = Color.Blue;
                    }
                    else if (item.Contains(ip+">>"))
                    {
                        richTextBox1.Select(richTextBox1.TextLength - item.Length-1, item.Length);
                        richTextBox1.SelectionColor = Color.Green;
                    }
               
                }
                richTextBox1.Select(richTextBox1.Text.Length, 0);
                //richTextBox1.SelectionStart=richTextBox1.Text.Length;
                connecting();
                if (security)
                {
                    Text = string.Format("RAWS *SECURED* {0}::{1}", active, current);
                }
                else
                {
                    Text = string.Format("RAWS {0}::{1}", active, current);
                }
                
                richTextBox1.ScrollToCaret();
                
            }
	else
	{
	string[] textValue = System.IO.File.ReadAllLines(path, Encoding.Default);
                int s = 0;
                foreach (string item in textValue)
	            {
                    if (item.Contains("비번"))
                    {
                        s += 1;
                        continue;
                    }
                     textValue[s++] = AESDecrypt256(item, decrikey);
	            }
	foreach(string item in textValue)
	{
		  s--;
                 if (s<1&&item.Contains("자비스,")&&item.Contains("강퇴"))
                 {
                     if (item.Substring(15).Contains(ip))
                     {
                         string did1 = ("←" + ip + " 님이 " + DateTime.Now.ToString() + " 에 강퇴당하였습니다");
                         fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                         sw = new StreamWriter(fs, System.Text.Encoding.Default);
                         sw.WriteLine(AESEncrypt256(did1, decrikey));
                         textBox1.Clear();
                         sw.Flush();
                         sw.Close();
                         fs.Close();
                         textBox1.Enabled = false;
                         notifyIcon1.BalloonTipTitle = "ⓒ332HQ A.C.E 2015";
                         notifyIcon1.BalloonTipText = "당신을 인공지능이 강제퇴장시켰습니다";
                         notifyIcon1.ShowBalloonTip(3000);
                         Thread.Sleep(3000);
                         realexit = true;
                         this.Close();
                         Application.Exit();
                     }
                 }
                 else if (item.Contains("자비스,")&&item.Contains("메리크리스마스"))
                 {
                     string did1 = ("←" + ip + " 님이 " + DateTime.Now.ToString() + " 에 강퇴당하였습니다");
                     fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                     sw = new StreamWriter(fs, System.Text.Encoding.Default);
                     sw.WriteLine(AESEncrypt256(did1, decrikey));
                     textBox1.Clear();
                     sw.Flush();
                     sw.Close();
                     fs.Close();
                     textBox1.Enabled = false;
                     notifyIcon1.BalloonTipTitle = "ⓒ332HQ A.C.E 2015";
                     notifyIcon1.BalloonTipText = "당신을 인공지능이 강제퇴장시켰습니다";
                     notifyIcon1.ShowBalloonTip(3000);
                     Thread.Sleep(3000);
                     realexit = true;
                     this.Close();
                     Application.Exit();
                 }
	}
	}
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
          
            if (realexit)
            {
                e.Cancel = false;
            }
            else
            {
                if (poponce)
                {
                    notifyIcon1.BalloonTipTitle = realname;
                    notifyIcon1.BalloonTipText = "여기로 최소화 되었습니다.\n이제 트레이 아이콘 통해 확인하세요";
                    notifyIcon1.ShowBalloonTip(70);
                    poponce = false;
                }
                e.Cancel = true;
                this.Visible = false;
            }
        }
            /*if (!textBox1.UseSystemPasswordChar)
            {
                refreshkey();
                string did = ("←" + ip + " 님이 " + DateTime.Now.ToString() + " 에 퇴장하셨습니다");
                richTextBox1.AppendText(did + "\n");

                fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                sw = new StreamWriter(fs, System.Text.Encoding.Default);
                sw.WriteLine(AESEncrypt256(did, decrikey));
                textBox1.Clear();
                sw.Flush();
                sw.Close();
                fs.Close();
                FileInfo said = new FileInfo(path);
                filesize = said.Length;
                richTextBox1_DoubleClick(sender, e);
                */
            /*
                if (MessageBox.Show("스텔스모드(작업표시줄 및 화면에서 완전히 숨기고 메시지 올 때만 나타남. 메시지 확인 후 최소화 누르기. 스텔스모드 활성/비활성화 하려면 Alt+Tab으로 찾아서 F11)를 쓰려면 '아니오', 그냥 종료하려면 '예'를 누르세요.", "Stealth Mode Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    //if (!textBox1.UseSystemPasswordChar)
                    {
                        refreshkey();
                        string did = ("←" + ip + " 님이 " + DateTime.Now.ToString() + " 에 퇴장하셨습니다");
                        richTextBox1.AppendText(did + "\n");

                        fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                        sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw.WriteLine(AESEncrypt256(did, decrikey));
                        textBox1.Clear();
                        sw.Flush();
                        sw.Close();
                        fs.Close();
                        FileInfo said = new FileInfo(path);
                        filesize = said.Length;
                        richTextBox1_DoubleClick(sender, e);
                    }
                }
                else
                {
                    e.Cancel = true;
                    stealth = true;
                    this.Opacity = 0;
                    this.ShowInTaskbar = false;
                    Form1_Deactivate(sender, e);
                }
                
            }
            */
            /*
            if (MessageBox.Show("종료전에 시작프로그램 등록을 유지하시겠습니까?\n(이미 시작프로그램에 등록되어있습니다. 재부팅시 잠금모드 상태로 켜집니다)", "시작프로그램 유지 여부", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {

            }
            else
            {
                 
            
            }


        }
                if (e.CloseReason != CloseReason.UserClosing)
                    e.Cancel = false;
                else
                    e.Cancel = true;*/
               
       
       /* private void timer2_Tick(object sender, EventArgs e)
        {
            tflash++;
            if (tflash==5)
            {
                timer2.Enabled = false;
                FlashWindowEx(this.Handle);
            }
        }*/
        

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            onmain = false;
            if (!textBox1.UseSystemPasswordChar)
            {
                if (security)
                {
                    textBox1.Clear();
                    textBox1.Focus();
                    richTextBox1.Text = info;
                    richTextBox1.Enabled = false;
                    textBox1.UseSystemPasswordChar = true;
                }
                else
                {
                    if (textBox1.UseSystemPasswordChar)
                    {
                        textBox1.Clear();
                    }
                    textBox1.Focus();
                    richTextBox1.Enabled = true;
                    textBox1.UseSystemPasswordChar = false;
                }
            }
            else if (javis)
            {
                javis = false;
                if (security)
                {
                    textBox1.Clear();
                    textBox1.Focus();
                    richTextBox1.Text = info;
                    richTextBox1.Enabled = false;
                    textBox1.UseSystemPasswordChar = true;
                }
                else
                {
                    if (textBox1.UseSystemPasswordChar)
                    {
                        textBox1.Clear();
                    }
                    textBox1.Focus();
                    richTextBox1.Enabled = true;
                    textBox1.UseSystemPasswordChar = false;
                }
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            onmain = true;

        }

        private void richTextBox1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Focus();
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

                rkeyarr = null;
                decrikey = null;
                rkeyarr = System.IO.File.ReadAllLines(realkey, Encoding.Default);
                decrikey = AESDecrypt256(rkeyarr[0], secret);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!textBox1.UseSystemPasswordChar||hasbeen)
            {
                string did = ("←" + ip + " 님이 " + DateTime.Now.ToString() + " 에 퇴장하셨습니다");
                //active--;
                //current.Replace(ip, "");
                //Text = string.Format("RAWS {0}::{1}", active, current);
                richTextBox1.AppendText(did + "\n");
                fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                sw = new StreamWriter(fs, System.Text.Encoding.Default);
                sw.WriteLine(AESEncrypt256(did, decrikey));
                textBox1.Clear();
                sw.Flush();
                sw.Close();
                fs.Close();
                FileInfo said = new FileInfo(path);
                filesize = said.Length;
                richTextBox1_DoubleClick(sender, e);
            }
            realexit = true;
            notifyIcon1.Visible = false;
            this.Close();
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Text = realname;
	javis=false;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            this.Activate();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipTitle = "ⓒ332HQ A.C.E 2015";
            notifyIcon1.BalloonTipText = "F1:투명도+\nF2:투명도-\nF3:흑백전환\nF4:입력하던거 지우기\nF5:대화내용수동갱신\nF6:피씨카톡처럼 잠금모드(On/Off)\nF7:비번 재설정(마스터키 필요, 내용 초기화)\nF8:대화내용 잠깐 지우기\nF9:대화내용 완전 지우기\nF10:시작프로그램에 등록(ON/OFF)\nF11:트레이있을때 알림모드 (풍선/팝업)\nF12:트레이로";
            notifyIcon1.ShowBalloonTip(2000);
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {

        }
        /*static void enc()
        {
            if (System.IO.File.Exists(realkey) && System.IO.File.Exists(path))
            {
                string[] rkeyarr = System.IO.File.ReadAllLines(realkey, Encoding.Default);
                string[] contents = System.IO.File.ReadAllLines(path, Encoding.Default);
                string decrikey = AESDecrypt256(rkeyarr[0], secret);
                fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs, System.Text.Encoding.Default);
                for (int i = 0; i < contents.Length; i++)
                {
                    if (string.IsNullOrEmpty(contents[i]))
                    {
                        break;
                    }
                    contents[i] = AESEncrypt256(contents[i], decrikey);
                    sw.WriteLine(contents[i]);
                    sw.Flush();
                }
                sw.Close();
                fs.Close();
                encrypted = true;
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
                for (int i = 0; i < contents.Length; i++)
                {
                    if (string.IsNullOrEmpty(contents[i]))
                    {
                        break;
                    }
                    contents[i] = AESDecrypt256(contents[i], decrikey);
                    sw.WriteLine(contents[i]);
                    sw.Flush();
                }
                sw.Close();
                fs.Close();
                encrypted = false;
            }
        }*/
    }
}
