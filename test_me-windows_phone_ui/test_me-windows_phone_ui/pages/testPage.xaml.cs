using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using ServerAPI;
using Newtonsoft.Json;
using System.Windows.Threading;

namespace test_me_windows_phone_ui.pages
{
    public partial class testPage : PhoneApplicationPage
    {
        private string single_answer_selected;
        private List<string> multiple_answers_selected;
        private string free_answer;

        private DispatcherTimer timer;
        private bool time_left;
        private int test_hours_left;
        private int test_minutes_left;
        private int test_seconds_left;

        public testPage()
        { 
            InitializeComponent();
            this.timer = new DispatcherTimer();
            this.timer.Tick += new EventHandler(timer_Tick);
            this.timer.Interval = new TimeSpan(0, 0, 1);
            this.timer.Start();

            getQuestion(true);
            this.multiple_answers_selected = new List<string>();
            this.free_answer = "";
            this.time_left = true;
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            this.timer.Stop();
        }
        private void getQuestion(bool is_first)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            
            RequestHandler rq = new RequestHandler();
            rq.ResponseArrived += new EventHandler<ResponseArrivedEventArgs>(rq_GetQuestionArived);
           
            Dictionary<string, string> reqData = new Dictionary<string, string>();
            reqData.Add("action", "get_question");
            reqData.Add("account_id", settings["account_id"].ToString());
            reqData.Add("session_token", settings["session_token"].ToString());
            reqData.Add("test_session_token", settings["test_session_token"].ToString());

            if (!is_first)
            {
                reqData.Add("question_number", settings["question_number"].ToString());
            }

            this.test_hours_left = int.Parse(settings["test_hours_left"].ToString());
            this.test_minutes_left = int.Parse(settings["test_minutes_left"].ToString());
            this.test_seconds_left = int.Parse(settings["test_seconds_left"].ToString());

            rq.SendRequest(reqData);
        }

        private void rq_GetQuestionArived(object sender, ResponseArrivedEventArgs e)
        {
            ServerResponseData srd = e.response;
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            if (srd.result != null && srd.status == "OK")
            {
                QuestionData respData = JsonConvert.DeserializeObject<QuestionData>(srd.resultString);
                if (respData.question_number == -1)
                {
                    getResult();
                }
                else
                {

                    if (settings.Contains("question_number"))
                    {
                        settings.Remove("question_number");
                    }

                    settings.Add("question_number", respData.question_number);
                    settings.Save();

                    this.single_answer_selected = "";
                    this.multiple_answers_selected.Clear();
                    this.free_answer = "";

                    this.test_hours_left = respData.test_hours_left;
                    this.test_minutes_left = respData.test_minutes_left;
                    this.test_seconds_left = respData.test_seconds_left;

                    this.Dispatcher.BeginInvoke(delegate()
                    {
                        PageTitle.Text = "Question " + (respData.question_number + 1).ToString();

                        //Question Text
                        canvas1.Children.Clear();
                        canvas2.Children.Clear();
                        TextBlock questionText = new TextBlock();
                        questionText.Text = respData.text;
                        questionText.Height = 105;
                        questionText.LineHeight = 20;
                        questionText.Width = 444;
                        questionText.TextWrapping = TextWrapping.Wrap;
                        canvas1.Children.Add(questionText);

                        if (respData.type == "single_answer")
                        {
                            handleSingleAnswerQuestion(respData);
                        }
                        else if (respData.type == "multiple_answers")
                        {
                            handleMultipleAnswersQuestion(respData);
                        }
                        else if (respData.type == "free_answer")
                        {
                            handleFreeAnswerQuestion(respData);
                        }
                        else
                        {
                            MessageBox.Show("An Error occured");
                        }

                    });
                }
            }
            else
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    MessageBox.Show(srd.message);
                });
            }
        }

        private void getResult()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            RequestHandler rq = new RequestHandler();
            rq.ResponseArrived += new EventHandler<ResponseArrivedEventArgs>(rq_GetResultArived);

            Dictionary<string, string> reqData = new Dictionary<string, string>();
            reqData.Add("action", "get_result");
            reqData.Add("account_id", settings["account_id"].ToString());
            reqData.Add("session_token", settings["session_token"].ToString());
            reqData.Add("test_session_token", settings["test_session_token"].ToString());

            rq.SendRequest(reqData);
        }

        private void rq_GetResultArived(object sender, ResponseArrivedEventArgs e)
        {
            ServerResponseData srd = e.response;
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            if (srd.result != null && srd.status == "OK")
            {
                ResultData respData = JsonConvert.DeserializeObject<ResultData>(srd.resultString);
                this.Dispatcher.BeginInvoke(delegate()
                {
                    MessageBox.Show("Result: " + respData.earned.ToString() + "/" + respData.total.ToString());
                    this.NavigationService.Navigate(
                        new Uri("//pages/profilePage.xaml", UriKind.Relative));
                });
            }
            else
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    MessageBox.Show(srd.message);
                });
            }
        }


        private void handleSingleAnswerQuestion(QuestionData respData)
        {
            double margin = 0;
            int radio_name = 1;
            foreach (Answer answer in respData.answers)
            {
                RadioButton radio = new RadioButton();
                radio.Content = answer.text;
                radio.GroupName = "RadioAnswersGroup";
                radio.Tag = answer.id;
                radio.Height = 75;
                radio.Margin = new Thickness(20, margin, 0, 0);
                radio.Name = "radioBtn" + radio_name.ToString();
                radio_name++;
                margin += 80;
                radio.Checked += new RoutedEventHandler(radio_answer_Check);
                canvas2.Children.Add(radio);
            }

        }
        private void handleMultipleAnswersQuestion(QuestionData respData)
        {
            double margin = 0;
            int name = 1;
            foreach (Answer answer in respData.answers)
            {
                CheckBox checkbox = new CheckBox();
                checkbox.Content = answer.text;
                checkbox.Tag = answer.id;
                checkbox.Height = 75;
                checkbox.Margin = new Thickness(20, margin, 0, 0);
                checkbox.Name = "checkBox" + name.ToString();
                name++;
                margin += 80;
                checkbox.Checked += new RoutedEventHandler(checkbox_answer_Check);
                checkbox.Unchecked += new RoutedEventHandler(checkbox_answer_Uncheck);

                
                canvas2.Children.Add(checkbox);
            }
        }
        private void handleFreeAnswerQuestion(QuestionData respData)
        {
            TextBox textbox = new TextBox();
            textbox.Width = 420;
            textbox.Margin = new Thickness(20, 0, 0, 0);
            textbox.Name = "txtAnswer";
            textbox.TextChanged += new TextChangedEventHandler(text_answer_Changed);


            canvas2.Children.Add(textbox);

        }

        private void radio_answer_Check(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            this.single_answer_selected = rb.Tag.ToString();
        }

        private void checkbox_answer_Check(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (!multiple_answers_selected.Contains(cb.Tag.ToString()))
            {
                this.multiple_answers_selected.Add(cb.Tag.ToString());
            }
        }
        private void checkbox_answer_Uncheck(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (!multiple_answers_selected.Contains(cb.Tag.ToString()))
            {
                this.multiple_answers_selected.Remove(cb.Tag.ToString());
            }
        }

        private void text_answer_Changed(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            this.free_answer = tb.Text;
        }

        private void btnSubmitAnswer_Click(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            RequestHandler rq = new RequestHandler();
            rq.ResponseArrived += new EventHandler<ResponseArrivedEventArgs>(rq_GetQuestionArived);
            Dictionary<string, string> reqData = new Dictionary<string, string>();
            reqData.Add("action", "submit_answer");
            reqData.Add("account_id", settings["account_id"].ToString());
            reqData.Add("session_token", settings["session_token"].ToString());
            reqData.Add("test_session_token", settings["test_session_token"].ToString());
            reqData.Add("question_number", settings["question_number"].ToString());
            reqData.Add("single_answer", this.single_answer_selected);
            reqData.Add("free_answer", this.free_answer);
            if (this.multiple_answers_selected.Count == 0)
            {
                reqData.Add("multiple_answers", "");
            }
            else
            {
                reqData.Add("multiple_answers", string.Join(",", this.multiple_answers_selected.ToArray()));
            }
            

            rq.SendRequest(reqData);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            string hours_to_show = "--";
            string minutes_to_show = "--";
            string seconds_to_show = "--";

            if (settings["test_hours_left"].ToString() != "-1" && settings["test_minutes_left"].ToString() != "-1" && settings["test_seconds_left"].ToString() != "-1")
            {     
                if (this.time_left)
                {
                    if (this.test_hours_left < 10)
                    {
                        hours_to_show = "0" + this.test_hours_left.ToString();
                    }
                    else
                    {
                        hours_to_show = this.test_hours_left.ToString();
                    }
                    if (this.test_minutes_left < 10)
                    {
                        minutes_to_show = "0" + this.test_minutes_left.ToString();
                    }
                    else
                    {
                        minutes_to_show = this.test_minutes_left.ToString();
                    }
                    if (this.test_seconds_left < 10)
                    {
                        seconds_to_show = "0" + this.test_seconds_left.ToString();
                    }
                    else
                    {
                        seconds_to_show = this.test_seconds_left.ToString();
                    }

                    this.texBlTime.Text = hours_to_show + ":" + minutes_to_show + ":" + seconds_to_show;

                    this.test_seconds_left--;
                    if (this.test_seconds_left < 0)
                    {
                        this.test_seconds_left = 59;
                        this.test_minutes_left--;
                    }

                    if (this.test_minutes_left < 0)
                    {
                        this.test_minutes_left = 59;
                        this.test_hours_left--;
                    }
                    if (this.test_hours_left < 0)
                    {
                        this.time_left = false;
                    }
                }
                else
                {
                    this.texBlTime.Text = "--:--:--";
                    this.timer.Stop();
                    getResult();     
                }
            }
        }

    }
    public struct QuestionData
    {
        public int question_number;
        public string text;
        public string type;
        public List<Answer> answers;
        public int test_hours_left;
        public int test_minutes_left;
        public int test_seconds_left;
    }

    public class RadioButtonArgs : EventArgs
    {
        public RadioButton btn { get; set; }
    }

    public struct Answer
    {
        public string text;
        public int id;
    }

    public struct ResultData
    {
        public int earned;
        public int total;
    }

}