using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenAI.Chat;
using Azure.Core;
using Azure.AI.OpenAI;
using Azure;
using System.IO;
using System.Data.SqlClient;
using System.Xml;
using Azure.AI.OpenAI.Chat;

namespace SQLAIRefactor
{
    public partial class Form1 : Form
    {   
        public static string DataTypesStringF1;
        public static string SQLServerNameF1;
        public static string DatabaseNameF1;

        public Form1()
        {
            InitializeComponent();

            this.Load += Form1_Load;

            //-----Form positioning
            this.Size = new Size(1740, 1100);  
            this.StartPosition = FormStartPosition.CenterScreen;  

            // Set RichTextBox1 (on the left)
            richTextBox1.Location = new Point(10, 70);
            richTextBox1.Size = new Size(880, 980);
            richTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            // Set webView21 (on the right)
            webView21.Location = new Point(895, 70);
            webView21.Size = new Size(820, 900);
            webView21.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            button1.Location = new Point(10, 8);
            button1.Size = new Size(120, 50);
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button1.BringToFront();

            button2.Location = new Point(900, 980);
            button2.Size = new Size(80, 40);
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button2.ForeColor = Color.DarkGreen;

            this.MinimizeBox = true;        
            this.MaximizeBox = true;       

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.SizeGripStyle = SizeGripStyle.Hide;
        }



        //===============================Function to send message to OpenAI resource
        private async Task<string> GetChatResponseSavelor(string chatContext,
                                                            string SQLQueryToOptimize,
                                                            string Training1,
                                                            string Training2,
                                                            string Training3)
        {
            var endpoint = new Uri("https://resource.openai.azure.com/");
            var apiKey = "***************************";
            var deploymentName = "";
            ChatCompletionOptions requestOptions = null;

            //----------inizialization [model 4o]
            //deploymentName = "gpt-4o";
            //requestOptions = new ChatCompletionOptions()
            //{
            //    MaxOutputTokenCount = 8192,
            //    Temperature = 0.4f,
            //    TopP = 1.0f
            //};

            //----------inizialization [model 4.1]
            deploymentName = "gpt-4.1";
            requestOptions = new ChatCompletionOptions()
            {
                MaxOutputTokenCount = 800,
                Temperature = 1.0f,
                TopP = 1.0f,
                FrequencyPenalty = 0.0f,
                PresencePenalty = 0.0f,
            };

            //----------inizialization model [o3-mini]: needs dotnet add package Azure.AI.OpenAI --version 2.2.0-beta.4!
            //deploymentName = "o3-mini";
            //requestOptions = new ChatCompletionOptions()
            //{
            //    MaxOutputTokenCount = 10000
            //};
            //requestOptions.SetNewMaxCompletionTokensPropertyEnabled(true);


            //valid for all models here
            label6.Text = deploymentName;
            var azureClient = new AzureOpenAIClient(endpoint, new AzureKeyCredential(apiKey));
            var chatClient = azureClient.GetChatClient(deploymentName);

            // Define chat messages
            List<ChatMessage> messages = new List<ChatMessage>()
            {
                new SystemChatMessage(chatContext),
                new UserChatMessage(SQLQueryToOptimize),
                new UserChatMessage(Training1),
                new UserChatMessage(Training2),
                new UserChatMessage(Training3)
            };

            // Get the response
            var response = await chatClient.CompleteChatAsync(messages, requestOptions);
            string answerC = response.Value.Content[0].Text;
            answerC = answerC.Replace("\n", "\r\n");
            return answerC;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            Color inactiveBorder = SystemColors.InactiveBorder;
            string coloreHtml = $"rgb({inactiveBorder.R}, {inactiveBorder.G}, {inactiveBorder.B})";

            await webView21.EnsureCoreWebView2Async(null);
            string html = $@"
                    <html>
                    <head>
                        <style>
                            body {{
                                font-family: Arial;
                                margin: 0;
                                padding: 10px;
                                height: 100%;
                                overflow-y: scroll;
                                font-size: 16px;
                                background-color: {coloreHtml};
                            }}
                            #content {{
                                display: flex;
                                flex-direction: column;
                            }}
                        </style>
                    </head>
                    <body>
                        <div id='content' style='margin-top:850px; font-size: 12px;'>
                            <div style='font-size:24px'>Welcome to SQL Refactoring tool!</div>
                        </div>
                        <script>
                            document.body.style.zoom = '70%';
                            document.body.style.backgroundColor = '{coloreHtml}';
                        </script>
                    </body>
                    </html>";

            // Load initial content
            webView21.CoreWebView2.NavigateToString(html);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            button2.ForeColor = Color.Black;
            string chatContext = "You are a SQL Server developer. Analyze and Rewrite the T-SQL batch that you have in input " +
                "applying the best practice rules that are given to you. Then add your observations. All the content returned must be in HTML.";

            //Load Query to Optimize
            string SQLQueryToOptimize = "This is the query to analyze: " + Environment.NewLine + richTextBox1.Text + Environment.NewLine;  //Query to optimize

            //Define the refactoring rules
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Training.txt");
            string Training1 = File.ReadAllText(filePath);

            //Load columns data types
            string Training2 = "This is the list of all tables, columns and data types in JSON format:" + Environment.NewLine + DataTypesStringF1 + Environment.NewLine;

            string Training3 = "All the content returned must be in HTML. Put the optimized code on an Azure backgroud";

            string ChatAnswer = await GetChatResponseSavelor(chatContext, SQLQueryToOptimize, Training1, Training2, Training3);

            ChatAnswer = ChatAnswer.Replace("```html", "");
            ChatAnswer = ChatAnswer.Replace("```", "");
            string nuovoHtml = "<div>" + ChatAnswer + "</div>";
            button2.ForeColor = Color.DarkGreen;
            AddHtmlContent(nuovoHtml);
        }


        private void AddHtmlContent(string nuovoHtml)
        {
            Color inactiveBorder = SystemColors.InactiveBorder;
            string coloreHtml = $"rgb({inactiveBorder.R}, {inactiveBorder.G}, {inactiveBorder.B})";

            string script = $@"
            document.body.style.backgroundColor = '{coloreHtml}';
            var contentDiv = document.getElementById('content');
            var nuovoDiv = document.createElement('div');
            nuovoDiv.innerHTML = `" + nuovoHtml.Replace("`", "\\`") + @"`;
            contentDiv.appendChild(nuovoDiv);
            document.body.style.zoom = '70%';

            function smoothScrollToBottom(duration = 3000) {
                const start = window.scrollY;
                const end = document.body.scrollHeight;
                const distance = end - start;
                const startTime = performance.now();

                function scrollStep(currentTime) {
                    const elapsed = currentTime - startTime;
                    const progress = Math.min(elapsed / duration, 1);
                    window.scrollTo(0, start + distance * easeInOutQuad(progress));
                    if (progress < 1) {
                        requestAnimationFrame(scrollStep);
                    }
                }

                function easeInOutQuad(t) {
                    return t < 0.5 ? 2 * t * t : -1 + (4 - 2 * t) * t;
                }
                requestAnimationFrame(scrollStep);
            }
            smoothScrollToBottom(3000); // ← cambia qui la durata in ms per più lentezza";

            webView21.CoreWebView2.ExecuteScriptAsync(script);
        }



        private void button1_Click(object sender, EventArgs e)
        {
            Form2 SQLConnWindows = new Form2();
            DialogResult result = SQLConnWindows.ShowDialog(); 

            if (result == DialogResult.OK)
            {
                DataTypesStringF1 = SQLConnWindows.DataTypesString;
                SQLServerNameF1 = SQLConnWindows.SQLServerName;
                DatabaseNameF1 = SQLConnWindows.DatabaseName;

                label3.Text = SQLServerNameF1;
                label4.Text = DatabaseNameF1;
                button1.ForeColor = Color.DarkGreen;
            }
        }

    }
}
