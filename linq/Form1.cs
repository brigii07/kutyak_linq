using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace linq
{
    public partial class Form1 : Form
    {
        class kutyafajtak
        {
            public int id { get; set; }
            public string nev { get; set; }
            public string eredetinev { get; set; }
        }

        class kutyak
        {
            public int id{ get; set; }
            public int fajtaid { get; set; }
            public int nevid { get; set; }
            public int eletkor { get; set; }
            public string ellenorzes{ get; set; }
        }

        class kutyanevek
        {
            public int id { get; set; }
            public string knev { get; set; }
        }

        List<kutyafajtak> kf_list = new List<kutyafajtak>();
        List<kutyak> kutyak_list = new List<kutyak>();
        List<kutyanevek> kn_list = new List<kutyanevek>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader o1 = new StreamReader("KutyaFajták.csv");
            StreamReader o2 = new StreamReader("Kutyák.csv");
            StreamReader o3 = new StreamReader("KutyaNevek.csv");

            o1.ReadLine();
            o2.ReadLine();
            o3.ReadLine();

            while (!o1.EndOfStream)
            {
                string[] egysor = o1.ReadLine().Split(';');

                kutyafajtak tmp = new kutyafajtak();
                tmp.id = Convert.ToInt32(egysor[0]);
                tmp.nev = egysor[1];
                tmp.eredetinev = egysor[2];
                kf_list.Add(tmp); //kutyafajták lista feltöltve

            }

            while (!o2.EndOfStream)
            {
                string[] egysor = o2.ReadLine().Split(';');

                kutyak tmp = new kutyak();
                tmp.id = Convert.ToInt32(egysor[0]);
                tmp.fajtaid = Convert.ToInt32(egysor[1]);
                tmp.nevid = Convert.ToInt32(egysor[2]);
                tmp.eletkor = Convert.ToInt32(egysor[3]);
                tmp.ellenorzes = egysor[4];
                kutyak_list.Add(tmp); //kutyák lista feltöltve

            }

            while (!o3.EndOfStream)
            {
                string[] egysor = o3.ReadLine().Split(';');

                kutyanevek tmp = new kutyanevek();
                tmp.id = Convert.ToInt32(egysor[0]);
                tmp.knev = egysor[1];
                kn_list.Add(tmp); //kutyanevek lista feltöltve

            }
            o1.Close(); o2.Close(); o3.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add("A kutyanevek száma: " + kn_list.Count.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //linq - kutyák átlagéletkora
            double atlag = kutyak_list.Average(x => x.eletkor);
            listBox1.Items.Add("Kutyák átlagéletkora: " + Math.Round(atlag, 2).ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //legmagasabb életkor
            int legmax = kutyak_list.Max(x => x.eletkor);

            //legmagasabb életkor id-je
            int id = kutyak_list.FindIndex(x => x.eletkor == legmax);

            int nevid = kutyak_list[id].nevid;
            int fajtaid = kutyak_list[id].fajtaid;

            listBox1.Items.Add("Legidősebb kutya neve és fajtája: " 
                + kn_list[nevid-1].knev + " " + kf_list[fajtaid - 1].nev );
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Dictionary<int, int> szotar = new Dictionary<int, int>();
            foreach (var item in kutyak_list)
            {
                if (item.ellenorzes == "2018.01.10")
                {
                    if (szotar.ContainsKey(item.fajtaid))
                    {
                        szotar[item.fajtaid]++;
                    }
                    else
                    {
                        szotar.Add(item.fajtaid, 1);
                    }
                }
                
            }
            listBox1.Items.Add("Január 10-év vizsgált:");
            foreach (KeyValuePair<int, int> fajta in szotar)
            {
                listBox1.Items.Add(kf_list[fajta.Key - 1].nev + " " + fajta.Value.ToString());
            }


        }

        private void button5_Click(object sender, EventArgs e)
        {
            SortedDictionary<string, int> datum = new SortedDictionary<string, int>();

            foreach (var item in kutyak_list)
            {
                if (datum.ContainsKey(item.ellenorzes))
                {
                    datum[item.ellenorzes]++;
                }
                else
                {
                    datum.Add(item.ellenorzes, 1);
                }
            }

            var ordered = datum.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            listBox1.Items.Add("Legjobban terhelt nap: ");
            listBox1.Items.Add(ordered.Keys.ElementAt(ordered.Count - 1) + " " + ordered.Values.ElementAt(ordered.Count - 1));

        }

        private void button6_Click(object sender, EventArgs e)
        {
            StreamWriter kiirat = new StreamWriter("Nevstatisztika.txt");
            Dictionary<int, int> szotar = new Dictionary<int, int>();
            foreach (var item in kutyak_list)
            {
                if (szotar.ContainsKey(item.nevid))
                {
                    szotar[item.nevid]++;
                }
                else
                {
                    szotar.Add(item.nevid, 1);
                }
            }
            var ordered = szotar.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            foreach (KeyValuePair<int, int> item in ordered)
            {
                kiirat.WriteLine(kn_list[item.Key - 1].knev + " " +item.Value);
            }

            kiirat.Close();
        }
    }
}
