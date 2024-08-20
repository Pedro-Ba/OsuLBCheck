using OsuMemoryDataProvider;
using OsuMemoryDataProvider.OsuMemoryModels;
using OsuMemoryDataProvider.OsuMemoryModels.Abstract;
using OsuMemoryDataProvider.OsuMemoryModels.Direct;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Xml;
using System.Net;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Text;
using System.Collections.Specialized;
using static System.Formats.Asn1.AsnWriter;

namespace OsuLBCheck
{
    public partial class Form1 : Form
    {
        private readonly string _osuWindowTitleHint;
        private int _readDelay = 1000; //1 a second feels fine otherwise it might feel too abusable?)
        private readonly object _minMaxLock = new object();
        private double _memoryReadTimeMin = double.PositiveInfinity;
        private double _memoryReadTimeMax = double.NegativeInfinity;
        private string accessToken = "";
        private HttpClient client = new HttpClient();
        private readonly StructuredOsuMemoryReader _sreader;
        private CancellationTokenSource cts = new CancellationTokenSource();

        public Form1(string osuWindowTitleHint)
        {
            InitializeComponent();
            _sreader = StructuredOsuMemoryReader.Instance.GetInstanceForWindowTitleHint(osuWindowTitleHint);
            Shown += OnShown;
            Closing += OnClosing;
        }

        private async void retrieveNewAccessToken()
        {
            if(textBox_clientID.Text == "" || textBox_clientSecret.Text == "")
            {
                label_Info.Text += "Please add a clientID and client secret to continue.\r\n";
                return;
            }
            var field = typeof(System.Net.Http.Headers.HttpRequestHeaders)
            .GetField("invalidHeaders", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
          ?? typeof(System.Net.Http.Headers.HttpRequestHeaders)
            .GetField("s_invalidHeaders", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            if (field != null)
            {
                var invalidFields = (HashSet<string>)field.GetValue(null);
                invalidFields.Remove("Content-Type");
            }
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var values = new Dictionary<string, string>
            {
                { "client_id", textBox_clientID.Text },
                { "client_secret", textBox_clientSecret.Text },
                { "grant_type", "client_credentials" },
                { "scope", "public" }
            };

            var contentReq = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("https://osu.ppy.sh/oauth/token", contentReq);

            var responseString = await response.Content.ReadAsStringAsync();
            var APIresponseObj = JsonNode.Parse(responseString);
            accessToken = (APIresponseObj["access_token"].ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            //textBox_Data.Text += accessToken; Was previously here to guarantee it was only running once. Also, this piece still bugs sometimes. Look into it.
        }

        private async void retrieveScoresFromAPI(int beatId)
        {
            var urlScores = "https://osu.ppy.sh/api/v2/beatmaps/" + beatId.ToString() + "/scores?legacy_only=1&limit=100"; //Holy shit this sucks lol
            using (HttpResponseMessage res = await client.GetAsync(urlScores))
            {
                using (HttpContent content = res.Content)
                {
                    var data = await content.ReadAsStringAsync();
                    int i = 1;
                    if(data != null)
                    {
                        var dataObj = JsonNode.Parse(data);
                        foreach(var item in dataObj["scores"].AsArray())
                        {
                            if (item["user"]["country_code"].ToString() == "BR")
                            {
                                var user = item["user"]["username"];
                                var score = item["score"];
                                string mods = "";
                                int modCount = 0;
                                foreach(var mod in item["mods"].AsArray())
                                {
                                    mods += mod;
                                    modCount++;
                                }
                                if (modCount == 0) mods += "None";
                                float pp = 0;
                                if (item["pp"] != null)
                                {
                                    pp = float.Parse(item["pp"].ToString(), System.Globalization.CultureInfo.GetCultureInfo("en-US"));
                                } 
                                float acc = float.Parse(item["accuracy"].ToString(), System.Globalization.CultureInfo.GetCultureInfo("en-US"));
                                var combo = "";
                                if (item["perfect"].ToString() == "true")
                                {
                                    combo = "Their max combo was " + item["max_combo"].ToString() + ". It is a perfect combo.";
                                }
                                else
                                {
                                    combo = "Their max combo was " + item["max_combo"].ToString() + ". It is not a perfect combo.";
                                }
                                acc *= 100;
                                label_Info.Text += "Current #1 Brazil score set by " + user + ". \r\n" +
                                    "Rank: #" + i.ToString() + "\r\n" +
                                    "Total score: " + score + "\r\n" +
                                    "Mods used: " + mods + "\r\n" +
                                    "PP: " + pp.ToString("0") + "\r\n" +
                                    "Acc: " + acc.ToString("n2") + "%\r\n" +
                                    combo + "\r\n";
                                label_ScoreToBeat.Text = score.ToString();
                                pictureBox1.Load(item["user"]["avatar_url"].ToString());
                                label_CurrentScore.Text = ""; //Not the best to have it here but w/e
                                return;
                            }
                            i++;
                        }
                    }
                }
            }
            label_Info.Text += "No scores found OR no Brazil scores found.";
            pictureBox1.Image = null;
            label_ScoreToBeat.Text = "0";
            label_CurrentScore.Text = ""; //Not the best to have it here but w/e
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            cts.Cancel();
        }

        private T ReadProperty<T>(object readObj, string propName, T defaultValue = default) where T : struct
        {
            if (_sreader.TryReadProperty(readObj, propName, out var readResult))
                return (T)readResult;

            return defaultValue;
        }

        private T ReadClassProperty<T>(object readObj, string propName, T defaultValue = default) where T : class
        {
            if (_sreader.TryReadProperty(readObj, propName, out var readResult))
                return (T)readResult;

            return defaultValue;
        }

        private int ReadInt(object readObj, string propName)
            => ReadProperty<int>(readObj, propName, -5);
        private short ReadShort(object readObj, string propName)
            => ReadProperty<short>(readObj, propName, -5);

        private float ReadFloat(object readObj, string propName)
            => ReadProperty<float>(readObj, propName, -5f);

        private string ReadString(object readObj, string propName)
            => ReadClassProperty<string>(readObj, propName, "INVALID READ");

        private async void OnShown(object sender, EventArgs eventArgs)
        {
            if (!string.IsNullOrEmpty(_osuWindowTitleHint)) Text += $": {_osuWindowTitleHint}";
            Text += $" ({(Environment.Is64BitProcess ? "x64" : "x86")})";
            _sreader.InvalidRead += SreaderOnInvalidRead;
            await Task.Run(async () =>
            {
                Stopwatch stopwatch;
                double readTimeMs, readTimeMsMin, readTimeMsMax;
                _sreader.WithTimes = true;
                var readUsingProperty = false;
                var baseAddresses = new OsuBaseAddresses();
                bool beatmapChange = false;
                var oldBeatmapID = 0;
                while (true)
                {
                    if (cts.IsCancellationRequested)
                        return;

                    if (!_sreader.CanRead)
                    {
                        Invoke((MethodInvoker)(() =>
                        {
                            //textBox_Data.Text = "osu! process not found";
                        }));
                        await Task.Delay(_readDelay);
                        continue;
                    }

                    stopwatch = Stopwatch.StartNew();
                    if (readUsingProperty)
                    {
                        baseAddresses.Beatmap.Id = ReadInt(baseAddresses.Beatmap, nameof(CurrentBeatmap.Id));
                        baseAddresses.Beatmap.SetId = ReadInt(baseAddresses.Beatmap, nameof(CurrentBeatmap.SetId));
                        baseAddresses.Beatmap.MapString = ReadString(baseAddresses.Beatmap, nameof(CurrentBeatmap.MapString));
                        baseAddresses.Beatmap.FolderName = ReadString(baseAddresses.Beatmap, nameof(CurrentBeatmap.FolderName));
                        baseAddresses.Beatmap.OsuFileName = ReadString(baseAddresses.Beatmap, nameof(CurrentBeatmap.OsuFileName));
                        baseAddresses.Beatmap.Md5 = ReadString(baseAddresses.Beatmap, nameof(CurrentBeatmap.Md5));
                        baseAddresses.Beatmap.Ar = ReadFloat(baseAddresses.Beatmap, nameof(CurrentBeatmap.Ar));
                        baseAddresses.Beatmap.Cs = ReadFloat(baseAddresses.Beatmap, nameof(CurrentBeatmap.Cs));
                        baseAddresses.Beatmap.Hp = ReadFloat(baseAddresses.Beatmap, nameof(CurrentBeatmap.Hp));
                        baseAddresses.Beatmap.Od = ReadFloat(baseAddresses.Beatmap, nameof(CurrentBeatmap.Od));
                        baseAddresses.Beatmap.Status = ReadShort(baseAddresses.Beatmap, nameof(CurrentBeatmap.Status));
                        baseAddresses.Skin.Folder = ReadString(baseAddresses.Skin, nameof(Skin.Folder));
                        baseAddresses.GeneralData.RawStatus = ReadInt(baseAddresses.GeneralData, nameof(GeneralData.RawStatus));
                        baseAddresses.GeneralData.GameMode = ReadInt(baseAddresses.GeneralData, nameof(GeneralData.GameMode));
                        baseAddresses.GeneralData.Retries = ReadInt(baseAddresses.GeneralData, nameof(GeneralData.Retries));
                        baseAddresses.GeneralData.AudioTime = ReadInt(baseAddresses.GeneralData, nameof(GeneralData.AudioTime));
                        baseAddresses.GeneralData.Mods = ReadInt(baseAddresses.GeneralData, nameof(GeneralData.Mods));
                    }
                    else
                    {
                        _sreader.TryRead(baseAddresses.Beatmap);
                        _sreader.TryRead(baseAddresses.Skin);
                        _sreader.TryRead(baseAddresses.GeneralData);
                        _sreader.TryRead(baseAddresses.BanchoUser);
                    }

                    if (baseAddresses.GeneralData.OsuStatus == OsuMemoryStatus.SongSelect)
                        _sreader.TryRead(baseAddresses.SongSelectionScores);
                    else
                        baseAddresses.SongSelectionScores.Scores.Clear();

                    if (baseAddresses.GeneralData.OsuStatus == OsuMemoryStatus.ResultsScreen)
                        _sreader.TryRead(baseAddresses.ResultsScreen);

                    if (baseAddresses.GeneralData.OsuStatus == OsuMemoryStatus.Playing)
                    {
                        _sreader.TryRead(baseAddresses.Player);
                        //TODO: flag needed for single/multi player detection (should be read once per play in singleplayer)
                        _sreader.TryRead(baseAddresses.LeaderBoard);
                        _sreader.TryRead(baseAddresses.KeyOverlay);
                        if (readUsingProperty)
                        {
                            //Testing reading of reference types(other than string)
                            _sreader.TryReadProperty(baseAddresses.Player, nameof(Player.Mods), out var dummyResult);
                        }
                    }
                    else
                    {
                        baseAddresses.LeaderBoard.Players.Clear();
                    }

                    var hitErrors = baseAddresses.Player?.HitErrors;
                    if (hitErrors != null)
                    {
                        var hitErrorsCount = hitErrors.Count;
                        hitErrors.Clear();
                        hitErrors.Add(hitErrorsCount);
                    }

                    stopwatch.Stop();
                    readTimeMs = stopwatch.ElapsedTicks / (double)TimeSpan.TicksPerMillisecond;
                    lock (_minMaxLock)
                    {
                        if (readTimeMs < _memoryReadTimeMin) _memoryReadTimeMin = readTimeMs;
                        if (readTimeMs > _memoryReadTimeMax) _memoryReadTimeMax = readTimeMs;
                        // copy value since we're inside lock
                        readTimeMsMin = _memoryReadTimeMin;
                        readTimeMsMax = _memoryReadTimeMax;
                    }

                    try
                    {
                        Invoke((MethodInvoker)(() =>
                        {                           
                            if (oldBeatmapID != baseAddresses.Beatmap.Id)
                            {
                                label_Info.Text = "";
                                //there was a beatmap change; request API for new top scores.
                                if (accessToken == "")
                                {
                                    label_Info.Text += "Access token was empty. requesting new one.\r\n";
                                    retrieveNewAccessToken();
                                }
                                else
                                {
                                    //access token is valid and existing; retrieve beatmap scores from API
                                    //check if beatmap ID is valid (beatmap might not be submitted!!)
                                    if (baseAddresses.Beatmap.Id != 0) retrieveScoresFromAPI(baseAddresses.Beatmap.Id);
                                    else label_Info.Text += "This beatmap doesn't appear to be submitted.";
                                }
                                oldBeatmapID = baseAddresses.Beatmap.Id;
                                label_Info.Text += baseAddresses.Beatmap.MapString + "\r\n";
                            }

                            if(baseAddresses.GeneralData.OsuStatus == OsuMemoryStatus.Playing)
                            {
                                if (_readDelay != 33)
                                {
                                    _readDelay = 33;
                                };
                                label_CurrentScore.Text = baseAddresses.Player.Score.ToString();
                                if (int.Parse(label_CurrentScore.Text) <= int.Parse(label_ScoreToBeat.Text))
                                {
                                    label_CurrentScore.ForeColor = System.Drawing.Color.Red;
                                }
                                else
                                {
                                    label_CurrentScore.ForeColor = System.Drawing.Color.Green;
                                }
                            }
                            else
                            {
                                if(_readDelay != 1000)
                                {
                                    _readDelay = 1000;
                                }
                            }
                            
                        }));
                    }
                    catch (ObjectDisposedException)
                    {
                        return;
                    }

                    _sreader.ReadTimes.Clear();
                    await Task.Delay(_readDelay);
                }
            }, cts.Token);
        }

        private void SreaderOnInvalidRead(object sender, (object readObject, string propPath) e)
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)(() => SreaderOnInvalidRead(sender, e)));
                    return;
                }
            }
            catch (ObjectDisposedException)
            {

            }
        }

        private void button_ResetReadTimeMinMax_Click(object sender, EventArgs e)
        {
            lock (_minMaxLock)
            {
                _memoryReadTimeMin = double.PositiveInfinity;
                _memoryReadTimeMax = double.NegativeInfinity;
            }
        }

    }
}
