using System;
using System.Linq;
using System.Windows.Forms;

namespace OOP_lab_11_Form
{
    public partial class Form1 : Form
    {
        private Wolf[] wolves = new Wolf[4];
        bool IsNull(Wolf w) => w == null;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                wolves[0] = new Wolf(50, 3, 200, "Альфа", "Ліс");
                wolves[1] = new Wolf(45, 4, 180, "Бета", "Тайга");
                DisplayWolves();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час створення: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayWolves()
        {
            label1.Text = string.Join("\n\n", wolves.Where(w => w != null).Select(w => w.GetInfo()));
            label1.Refresh();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (wolves[0] == null || wolves[1] == null)
                {
                    MessageBox.Show("Спочатку створіть об'єкти (Кнопка 1).", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                wolves[2] = (Wolf)wolves[0].Clone();
                wolves[3] = (Wolf)wolves[1].Clone();
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
                if (wolves.Any(IsNull))
                {
                    MessageBox.Show("Створіть і клонуйте всі об'єкти (Кнопки 1 і 2).", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Array.Sort(wolves);
                DisplayWolves();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час сортування: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                throw new ArgumentException("Параметри тварини не можуть бути від’ємними.");

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