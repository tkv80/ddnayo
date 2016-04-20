using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using ddnayo.Manager;

namespace ddnayo.Worker

{
    class Worker : BackgroundWorker
    {
        public Worker()
        {
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
        }

        public int Interval { private get; set; }
        
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            while (!CancellationPending)
            {
                
                foreach (var reservation in Reservations)
                {
                    string message = null;

                    var canReservation = HttpManager.IsPossible(1707, reservation.Item2, reservation.Item1);

                    if (canReservation)
                    {
                        new GcmManager().SendNotification("날짜찾기 성공", reservation.Item1);

                        message = string.Format("{0} {1} {2}", "성공", reservation.Item1, reservation.Item2.ToString("MM-dd"));


                        ReportProgress(0, message);

                        CancelAsync();
                        break;

                    }
                    else
                    {
                        message = string.Format("{0} {1} {2}", "자리없음", reservation.Item1, reservation.Item2.ToString("yyyy-MM-dd"));
                    }



                    ReportProgress(0, message);
                    Thread.Sleep(1000);
                }
            }
        }

        public List<System.Tuple<string, System.DateTime>> Reservations { get; set; }
    }
}
