using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ddnayo.Manager;

namespace ddnayo
{
    public partial class Form1 : Form
    {
        private readonly Worker.Worker _worker = new Worker.Worker();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var sites = HttpManager.GetSite(1707, DateTime.Now);
            checkedListBox1.DataSource = sites.ToList();
            checkedListBox1.DisplayMember = "Item1";
            _worker.ProgressChanged += worker_ProgressChanged;
            _worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        }

        private List<Tuple<string, DateTime>> reservations;

        private void btnInsert_Click(object sender, EventArgs e)
        {
            reservations = new List<Tuple<string, DateTime>>();

            foreach (Tuple<string, string> item in checkedListBox1.CheckedItems)
            {
                reservations.Add(new Tuple<string, DateTime>(item.Item1, dateTimePicker1.Value));
                lbReservation.Items.Add(dateTimePicker1.Value + " " + item.Item1);

            }
            
        }

        #region worker

        private void worker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (txtLog.Lines.Length <= 30)
            {
                Util.Logging(txtLog, e.UserState.ToString());
            }
            else
            {
                txtLog.Clear();
                Util.Logging(txtLog, e.UserState.ToString());
            }
        }

        private void worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Util.Logging(txtLog, "완료~!!");
            btnStartReservation.Enabled = true;
            btnStartReservation.Text = @"예약시작";
        }

        #endregion

        private void btnStartReservation_Click(object sender, EventArgs e)
        {
            if (btnStartReservation.Text == "예약시작")
            {
                _worker.Interval = 1;
                _worker.Reservations = reservations;
                _worker.RunWorkerAsync();

                btnStartReservation.Text = "예약중지";
            }
            else
            {
                _worker.CancelAsync();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.lbReservation.Items.RemoveAt(lbReservation.SelectedIndex);
        }
    }
}
