using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YSA
{
    public partial class Form1 : Form
    {
        class Data
        {
            public double[] input { get; set; }
            public int output { get; set; }

            public Data(double[] input, int output)
            {
                this.input = input;
                this.output = output;
            }
        }

        class Neuron
        {
            public double bias { get; set; }
            public double[] w { get; set; }
            public Function function { get; set; }

            public Neuron(int dimension, double bias, Function function)
            {
                this.bias = bias;
                this.function = function;
                w = new double[dimension];
                for (int i = 0; i < dimension; i++)
                {
                    w[i] = new Random().NextDouble();
                }
            }
            public string getW()
            {
                string value = "";
                for (int i = 0; i < w.Length; i++)
                {
                    value += " " + w[i].ToString();
                }
                return value;
            }
        }

        abstract class Function
        {
            public double net(double[] input, double[] w, double bias)
            {
                double sum = 0;
                for (int i = 0; i < input.Length; i++)
                {
                    sum += input[i] * w[i];
                }
                return sum + w[w.Length - 1] * bias;
            }

            public abstract double calculate(double net);
        }

        class BinaryFunction : Function
        {
            public override double calculate(double net)
            {
                if (net > 0)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }

        class ContinousFunction : Function
        {
            public override double calculate(double net)
            {
                return 1 / (1 + Math.Pow(Math.E, -net));
            }
        }
        List<Data> dataList;

        public Form1()
        {
            InitializeComponent();
            dataList = new List<Data>();

            double[] d1 = { 1, 2 , 4 , -1 };
            double[] d2 = { 2, 3 , -1 , 7 };
            double[] d3 = { -1, 0 , 2 , -3};
            double[] d4 = { -1, -2 , 4 , 2 };
            dataList.Add(new Data(d1, 1));
            dataList.Add(new Data(d2, 1));
            dataList.Add(new Data(d3, -1));
            dataList.Add(new Data(d4, -1));
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            singleLayerSingleNeuron(1.0000,4,1.0000);
        }

        public void singleLayerSingleNeuron(double c, int dimension, double bias)
        {
            Neuron neuron = new Neuron(dimension, bias, new BinaryFunction());

            while (true)
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    double error = 0;
                    double net = neuron.function.net(dataList[i].input, neuron.w, neuron.bias);
                    double fnet = neuron.function.calculate(net);
                    for (int j = 0; j < (dimension - 1); j++)
                    {
                        neuron.w[j] += c * (dataList[i].output - fnet) * dataList[i].input[j];
                    }
                    neuron.w[dimension - 1] += c * (dataList[i].output - fnet) * neuron.bias;
                    error += Math.Pow((dataList[i].output - fnet), 2) / 2;
                    if (error < 0.1)
                    {
                        break;
                    }
                }
                MessageBox.Show(neuron.getW());
            }
        }
    }
}
