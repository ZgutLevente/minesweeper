namespace minesweeper
{
    public partial class Form2 : Form
    {
        private Communicator communicator;
        public Form2(double totalsec, string message, byte difficulty)
        {
            InitializeComponent();
            this.totalsec = totalsec.ToString().Substring(0, totalsec.ToString().IndexOf(',') + 6).Replace(",", ".");
            this.difficulty = difficulty;
            this.message = message;
            label1.Text = $"{message} Time: {this.totalsec} seconds elapsed.";
            communicator = new Communicator();
            label2.Text = "Connecting...";
            Thread com = new Thread(ListRequester);
            com.IsBackground = true;
            com.Start();
        }
        private string totalsec, message;
        private byte difficulty;
        private void ListRequester()
        {
            string at = communicator.Request($"listß{difficulty}ß{totalsec}ß{message}");
            if (at != "ßßerrorßß" && at != string.Empty)
            {
                string[] splitter = at.Split("ß", StringSplitOptions.RemoveEmptyEntries);
                if (splitter[0] == "Successlist")
                {
                    try
                    {
                        Invoke(() =>
                        {
                            if (message == "You won!")
                            {
                                textBox1.Enabled = textBox1.Visible = label3.Visible = label4.Visible = listView1.Enabled = listView1.Visible = true;
                                label2.Text = $"You: {splitter[1]} {ResultText(splitter[1])}.";
                            }
                            else if (splitter.Length > 2)
                            {
                                listView1.Enabled = listView1.Visible = true;
                                label2.Text = string.Empty;
                            }
                            else label2.Text = "There are no results yet for the given difficulty level.";
                            for (int i = 0; i < (splitter.Length - 2) / 3; i++)
                            {
                                listView1.Items.Add(new ListViewItem(new string[] { splitter[2 + i * 3], splitter[3 + i * 3], splitter[4 + i * 3] }));
                            }
                        });
                    }
                    catch (Exception) { }
                }
            }
            else
            {
                try { Invoke(() => { label2.Text = "Connection Error."; }); }
                catch (Exception) { }
            }
        }
        private string ResultText(string place)
        {
            switch (place[place.Length - 1])
            {
                case '1': return "st";
                case '2': return "nd";
                case '3': return "rd";
                default: return "th";
            }
        }
        private void PublishRequester()
        {
            string at = communicator.Request($"publishß{difficulty}ß{totalsec}ß{textBox1.Text}");
            if (at != "ßßerrorßß" && at != string.Empty)
            {
                string[] splitter = at.Split("ß", StringSplitOptions.RemoveEmptyEntries);
                if (splitter[0] == "Successpublish")
                {
                    try
                    {
                        Invoke(() =>
                        {
                            label3.Text = "You have successfully published your time of completion!";
                            label2.Text = $"{textBox1.Text}: {splitter[1]} {ResultText(splitter[1])}.";
                            label5.Text = $"Your best time is {splitter[2]} seconds";
                            label4.Visible = false;
                            listView1.Items.Clear();
                            for (int i = 0; i < (splitter.Length - 3) / 3; i++)
                            {
                                listView1.Items.Add(new ListViewItem(new string[] { splitter[3 + i * 3], splitter[4 + i * 3], splitter[5 + i * 3] }));
                            }
                        });
                    }
                    catch (Exception) { }
                }
            }
            else
            {
                try
                {
                    Invoke(() =>
                    {
                        label3.Text = "Connection Error.";
                        textBox1.Enabled = true;
                    });
                }
                catch (Exception) { }
            }
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && textBox1.Text.Replace(" ", "").Length > 0)
            {
                if (textBox1.Text != "You")
                {
                    textBox1.Enabled = false;
                    label3.Text = "Connecting...";
                    Thread com = new Thread(PublishRequester);
                    com.IsBackground = true;
                    com.Start();
                }
                else MessageBox.Show("This name is not allowed!");
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string le1 = textBox1.Text;
            if (le1.Contains('ß'))
            {
                int le2 = le1.IndexOf('ß');
                textBox1.Text = le1.Replace("ß", "");
                textBox1.Select(le2, 0);
            }
        }
        private void button1_Click(object sender, EventArgs e) => Close();
    }
}
