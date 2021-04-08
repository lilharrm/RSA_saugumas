using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Numerics;

namespace RSA_saugumas
{
    public partial class Form1 : Form
    {
        char[] characters = new char[] { '#', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                                                        'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S',
                                                        '.', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', ' ', '1', '2', '3', '4', '5', '6', '7',
                                                        '8', '9', '0' };


        public Form1()
        {
            InitializeComponent();
        }

        //UZSIFRUOTI BUTTON
        private void buttonEncrypt_Click(object sender, EventArgs e)
        {
            if ((textBox_p.Text.Length > 0) && (textBox_q.Text.Length > 0))
            {
                long p = Convert.ToInt64(textBox_p.Text);
                long q = Convert.ToInt64(textBox_q.Text);

                if (IsTheNumberSimple(p) && IsTheNumberSimple(q))
                {
                    string s = "";

                    s = textBox1.Text;

                    long n = p * q;
                    long FnuoN = (p - 1) * (q - 1); // ф(n)
                    long d = Calculate_d(FnuoN);
                    long e_ = Calculate_e(d, FnuoN);

                    List<string> result = RSA_Endoce(s, e_, n);

                    StreamWriter sw = new StreamWriter("UZSIFRAVIMO_REZULTATAS.txt");
                    foreach (string item in result)
                        sw.WriteLine(item);
                    sw.Close();

                    textBox_d.Text = d.ToString();
                    textBox_n.Text = n.ToString();

                    Process.Start("UZSIFRAVIMO_REZULTATAS.txt");
                }
                else
                    MessageBox.Show("p arba q - nera pirminis skaicius!");
            }
            else
                MessageBox.Show("iveskite p ir q!");
        }


        //DESIFRUOTI BUTTON
        private void buttonDecipher_Click_1(object sender, EventArgs e)
        {
            if ((textBox_d.Text.Length > 0) && (textBox_n.Text.Length > 0))
            {
                long d = Convert.ToInt64(textBox_d.Text);
                long n = Convert.ToInt64(textBox_n.Text);

                List<string> input = new List<string>();

                StreamReader sr = new StreamReader("UZSIFRAVIMO_REZULTATAS.txt");

                while (!sr.EndOfStream)
                {
                    input.Add(sr.ReadLine());
                }

                sr.Close();

                string result = RSA_Dedoce(input, d, n);

                textBox2.Text = result;
            }
            else
                MessageBox.Show("Iveskite raktus!");
        }

        //PATIKRINA AR SKAICIAI YRA PIRMINIAI
        private bool IsTheNumberSimple(long n)
        {
            if (n < 2)
                return false;

            if (n == 2)
                return true;

            for (long i = 2; i < n; i++)
                if (n % i == 0)
                    return false;

            return true;
        }

        //PATS SIFRAVIMAS - VIESASIS RAKTAS
        private List<string> RSA_Endoce(string s, long e, long n)
        {
            List<string> result = new List<string>();

            BigInteger bi;

            for (int i = 0; i < s.Length; i++)
            {
                int index = Array.IndexOf(characters, s[i]);

                bi = new BigInteger(index);
                bi = BigInteger.Pow(bi, (int)e);

                BigInteger n_ = new BigInteger((int)n);

                bi = bi % n_;

                result.Add(bi.ToString());
            }

            return result;
        }

        //PATS DESIFRAVIMAS - PRIVATUSIS RAKTAS
        private string RSA_Dedoce(List<string> input, long d, long n)
        {
            string result = "";

            BigInteger bi;

            foreach (string item in input)
            {
                bi = new BigInteger(Convert.ToDouble(item));
                bi = BigInteger.Pow(bi, (int)d);

                BigInteger n_ = new BigInteger((int)n);

                bi = bi % n_;

                int index = Convert.ToInt32(bi.ToString());

                result += characters[index].ToString();
            }

            return result;
        }

        //PRIVATUS RAKTAS D. D TURI BUTI ABIPUSIAI PAPRASTAS SU Ф(n)
        private long Calculate_d(long FnuoN)
        {
            long d = FnuoN - 1;

            for (long i = 2; i <= FnuoN; i++)
                if ((FnuoN % i == 0) && (d % i == 0)) //jeigu turi bendrus daliklius
                {
                    d--;
                    i = 1;
                }

            return d;
        }

        //VIESOSIOS EKSPONENTES APSKAICIAVIMAS
        private long Calculate_e(long d, long FnuoN)
        {
            long e = 10;

            while (true)
            {
                if ((e * d) % FnuoN == 1)
                    break;
                else
                    e++;
            }

            return e;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
