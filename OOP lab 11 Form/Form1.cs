using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;

namespace OOP_lab_11_Form
{
    public partial class Form1 : Form
    {
        private WolfCollection wolfCollection = new WolfCollection();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == false && radioButton2.Checked == false)
            {
                MessageBox.Show("Спочатку оберіть з чим працювати (Hashtable або List<Wolf>)", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if(wolfCollection.Count >= 2)
                {
                    MessageBox.Show("Надалі клонуйте!", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                wolfCollection.Add(new Wolf(50, 3, 200, "Альфа", "Ліс"));
                wolfCollection.Add(new Wolf(45, 4, 180, "Бета", "Тайга"));
                DisplayWolves();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час створення: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayWolves()
        {
            label1.Text = wolfCollection.GetAllInfo();
            label1.Refresh();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (wolfCollection.Count >= 16)
                {
                    MessageBox.Show("Клонування надалі неможливе, занадто багато елементів (повинно бути не більше 16)!", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (wolfCollection.Count < 2)
                {
                    MessageBox.Show("Спочатку створіть об'єкти (Кнопка 1).", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                wolfCollection.Add((Wolf)wolfCollection[0].Clone());
                wolfCollection.Add((Wolf)wolfCollection[1].Clone());
                DisplayWolves();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час клонування: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (wolfCollection.Count < 4)
                {
                    MessageBox.Show("Створіть і клонуйте всі об'єкти (Кнопки 1 і 2).", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                wolfCollection.Sort();
                DisplayWolves();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час сортування: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (wolfCollection.Count < 2)
            {
                MessageBox.Show("Спочатку створіть об'єкти (Кнопка 1).", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int index;
            if (!int.TryParse(textBox1.Text, out index))
            {
                MessageBox.Show("Введіть правильний індекс.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool useHashtable = radioButton1.Checked;
            label1.Text = wolfCollection.GetInfoByIndex(index, useHashtable);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            wolfCollection = new WolfCollection();
            label1.Text = " ";
        }
    }

    public class WolfCollection : IEnumerable<Wolf>
    {
        private Hashtable wolvesTable = new Hashtable();
        private List<Wolf> wolvesList = new List<Wolf>();
        private int id = 1;

        public int Count => wolvesList.Count;

        public Wolf this[int index]
        {
            get => wolvesList[index];
            set
            {
                wolvesList[index] = value;
                wolvesTable[index] = value;
            }
        }

        public void Add(Wolf wolf)
        {
            wolvesList.Add(wolf);
            wolvesTable.Add(id++, wolf);
        }

        public void Sort()
        {
            wolvesList.Sort();
            wolvesTable.Clear();
            for (int i = 0; i < wolvesList.Count; i++)
            {
                wolvesTable.Add(i, wolvesList[i]);
            }
        }

        public string GetAllInfo()
        {
            return string.Join("\n\n", wolvesList.Select(w => w.GetInfo()));
        }

        public string GetInfoByIndex(int index, bool useHashtable)
        {
            if (useHashtable)
            {
                if (wolvesTable.ContainsKey(index))
                    return ((Wolf)wolvesTable[index]).GetInfo();
                else
                    return "Елемент не знайдено в Hashtable.";
            }
            else
            {
                if (index >= 0 && index < wolvesList.Count)
                    return wolvesList[index].GetInfo();
                else
                    return "Елемент не знайдено в List.";
            }
        }

        public IEnumerator<Wolf> GetEnumerator()
        {
            return wolvesList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public interface IAnimalInfo
    {
        string GetInfo();
    }

    public class Animal : IAnimalInfo, IComparable<Animal>, ICloneable
    {
        public Animal(double weight, int age, double costPerDay)
        {
            if (weight < 0 || age < 0 || costPerDay < 0)
                throw new ArgumentException("Параметри тварини не можуть бути від'ємними.");

            Weight = weight;
            Age = age;
            CostPerDay = costPerDay;
        }

        public double Weight { get; protected set; }
        public int Age { get; protected set; }
        public double CostPerDay { get; protected set; }

        public virtual string GetInfo() =>
            $"Тварина: Вага = {Weight} кг, Вік = {Age} років, Вартість утримання = {CostPerDay} грн/день";

        public virtual object Clone()
        {
            return new Animal(Weight, Age, CostPerDay);
        }

        public int CompareTo(Animal other)
        {
            if (other == null) return 1;
            return CostPerDay.CompareTo(other.CostPerDay);
        }
    }

    public class Wolf : Animal, IComparable<Wolf>, ICloneable
    {
        private string _breed;
        private string _habitat;

        public Wolf(double weight, int age, double costPerDay, string breed, string habitat)
            : base(weight, age, costPerDay)
        {
            _breed = breed ?? throw new ArgumentNullException(nameof(breed));
            _habitat = habitat ?? throw new ArgumentNullException(nameof(habitat));
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $"\nПорода вовка: {_breed}, Природна локація: {_habitat}";
        }

        public override object Clone()
        {
            return new Wolf(Weight, Age, CostPerDay, _breed, _habitat);
        }

        public int CompareTo(Wolf other)
        {
            return base.CompareTo(other);
        }
    }
}