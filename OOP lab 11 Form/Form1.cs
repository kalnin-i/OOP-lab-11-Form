using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_lab_11_Form
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Wolf wolf = new Wolf(45.5, 5, 150.0, "Сірий вовк", "Тайга");
            label1.Text = wolf.GetInfo();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label2.Visible = true;
        }

        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            button2.Visible = true;
        }
    }

    public class Animal
    {
        protected double weight;
        protected int age;
        protected double costPerDay;

        public Animal(double weight, int age, double costPerDay)
        {
            this.weight = weight;
            this.age = age;
            this.costPerDay = costPerDay;
        }

        public virtual string GetInfo()
        {
            return $"Тварина: Вага = {weight} кг, Вік = {age} років, Вартість утримання = {costPerDay} грн/день";
        }
    }

    public class Wolf : Animal
    {
        private string breed;
        private string habitat;

        public Wolf(double weight, int age, double costPerDay, string breed, string habitat)
            : base(weight, age, costPerDay)
        {
            this.breed = breed;
            this.habitat = habitat;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $"\nПорода вовка: {breed}, Природна локація: {habitat}";
        }
    }
}