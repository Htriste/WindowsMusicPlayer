using NAudio.Vorbis;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WindowsMusicPlayer
{
    public partial class Form1 : Form
    {
        // 本地音乐列表
        List<string> localmusiclist = new List<string>();
        // 用于 OGG 文件播放
        private WaveOutEvent waveOut = new WaveOutEvent();
        private VorbisWaveReader currentVorbisReader;
        public Form1()
        {
            InitializeComponent();
        }
        private void musicplay(string filename)
        {
            string extension = Path.GetExtension(filename).ToLower();

            if (extension == ".ogg")
            {
                Console.WriteLine("这是ogg文件。");
                if (currentVorbisReader != null)
                {
                    // 确保先停止当前播放再释放资源
                    if (waveOut.PlaybackState != PlaybackState.Stopped)
                    {
                        waveOut.Stop();
                    }
                    currentVorbisReader.Dispose();
                }

                currentVorbisReader = new VorbisWaveReader(filename);
                waveOut.Init(currentVorbisReader);
                waveOut.Play();
            }
            else
            {
                axWindowsMediaPlayer1.URL = filename;
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
        }
        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 设置默认音量
            trackBar1.Value = 50;
            axWindowsMediaPlayer1.settings.volume = 50;
            label2.Text = "50%";
        }

        private void selectNormalFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "选择普通音乐文件|*.mp3;*.flac;*.wav";
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in openFileDialog1.FileNames)
                {
                    string fileName = Path.GetFileName(file);
                    if (!localmusiclist.Contains(file))
                    {
                        listBox1.Items.Add(fileName);
                        localmusiclist.Add(file);
                    }
                }
            }
        }

        private void selectOggFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "选择.ogg音乐文件|*.ogg";
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in openFileDialog1.FileNames)
                {
                    string fileName = Path.GetFileName(file);
                    if (!localmusiclist.Contains(file))
                    {
                        listBox1.Items.Add(fileName);
                        localmusiclist.Add(file);
                    }
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (localmusiclist.Count > 0 && listBox1.SelectedIndex >= 0)
            {
                string selectedFile = localmusiclist[listBox1.SelectedIndex];
                musicplay(selectedFile);
                label1.Text = Path.GetFileNameWithoutExtension(selectedFile); // 设置曲目名称
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (waveOut.PlaybackState == PlaybackState.Playing)
            {
                waveOut.Pause(); // 暂停播放 OGG
            }

            axWindowsMediaPlayer1.Ctlcontrols.stop(); // 停止其他格式的播放
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (localmusiclist.Count > 0)
            {
                int index = (listBox1.SelectedIndex + 1);

                if (index >= localmusiclist.Count())
                {
                    index = 0;
                }

                string nextFile = localmusiclist[index];
                musicplay(nextFile);
                label1.Text = Path.GetFileNameWithoutExtension(nextFile);
                listBox1.SelectedIndex = index;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            contextMenuStrip9.Show(button1, new System.Drawing.Point(0, button1.Height));
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = trackBar1.Value;
            label2.Text = trackBar1.Value + "%";
            if (waveOut != null)
            {
                waveOut.Volume = trackBar1.Value / 100f;
            }
        }
    }
}
