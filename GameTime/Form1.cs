using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace GameTime
{
    public partial class Form1 : Form
    {
        private Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;
        private TimeSpan _gameTime;

        private bool _counting = false;

        private List<bool> _notified = new List<bool>();
        private List<TimeSpan> _alarmsTime = new List<TimeSpan>();
        private List<string> _alarmMsg = new List<string>();

        public Form1()
        {
            InitializeComponent();

            FileVersionInfo ver = 
                FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Text = "GameTime / Rev." + ver.FileVersion;

            StopButton.Enabled = false;

            // アラームを出す時間
            //_alarmsTime.Add(new TimeSpan(0, 0, 2));
            //_alarmMsg.Add("テスト");
            _alarmsTime.Add(new TimeSpan(0, 30, 0));
            _alarmMsg.Add("30分経ったよ");
            _alarmsTime.Add(new TimeSpan(1, 0, 0));
            _alarmMsg.Add("もう1時間ゲームやってるよ！");
            _alarmsTime.Add(new TimeSpan(1, 30, 0));
            _alarmMsg.Add("1時間半経ったよ！ つまり90分！");
            _alarmsTime.Add(new TimeSpan(2, 0, 0));
            _alarmMsg.Add("2時間たったぞ！ 今日はこれで終わりにしようね");

            for (int i = 0; i < _alarmsTime.Count; i++)
            {
                _notified.Add(false);
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            StopButton.Enabled = true;
            StartButton.Enabled = false;

            _counting = true;
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            if (StopButton.Text == "再開")
            {
                StopButton.Text = "停止";
                _counting = true;
                
            }
            else
            {
                StopButton.Text = "再開";
                _counting = false;
            }

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    DateTime start = DateTime.Now;

                    System.Threading.Thread.Sleep(1000);
                    if (_counting == false) 
                    { 
                        continue;
                    }

                    // ゲーム時間を積算する
                    _gameTime += DateTime.Now - start;

                    // メッセージの表示
                    for (int i = 0; i < _notified.Count; i++)
                    {
                        if (_notified[i]) { continue; }
                        if (_gameTime > _alarmsTime[i])
                        {
                            using (AlarmView f = new AlarmView(_alarmMsg[i]))
                            {
                                f.ShowDialog();
                            }

                            //MessageBox.Show(_gameTime.Seconds.ToString() + "秒間ゲームをしました");
                            _notified[i] = true;
                        }
                    }

                    // ゲーム時間の表示を更新する
                    _dispatcher.Invoke(delegate
                    {
                        AccumLabel.Text = _gameTime.ToString(@"hh\:mm\:ss");
                    });
                }
            });
        }
    }
}
