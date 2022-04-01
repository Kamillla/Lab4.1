using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _4._1
{
    public partial class Form1 : Form
    {
        MyStorage storage;
        Bitmap bmp = new Bitmap(1000, 1000); //инициализация нового экземпляра класса Bitmap с размером 1000х1000 пикселей

        public Form1()
        {
            InitializeComponent();
            storage = new MyStorage();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)//объявление классов для рисования объектов
        {
            Graphics g = Graphics.FromImage(bmp);
            storage.DrawAll(pictureBox1, g, bmp);
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (storage.CheckedStorage(e) == false)//если нажали на пустое место
            {
                storage.AllNotChecked();
                storage.AddObject(new CCircle(e.X, e.Y, 2 * 40));//добавление нового круга с координатами в хранилище
            }
            else if (Control.ModifierKeys == Keys.Control)//если нажат ctrl, можно выделить несколько объектов
                storage.ObjBlue(e);
            else//если не нажат, выделяется только один круг
            {
                storage.AllNotChecked();
                storage.ObjBlue(e);
            }
            this.Refresh();//перерисовка элементов
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)//удаление выделенных объектов
            {
                Graphics g = Graphics.FromImage(bmp);
                storage.Del_ObjBlue();
                g.Clear(Color.White);
            }
        }
    }

    class CCircle
    {
        private int rad;
        private int x;
        private int y;
        private bool clck;
        public CCircle(int x, int y, int rad)//конструктор с параметрами
        {
            this.x = x;
            this.y = y;
            this.rad = rad;
            clck = true;
        }
        public void Draw(PictureBox pctr, Graphics g, Bitmap bmp)//метод, рисующий круг 
        {
            Pen pen;
            if (clck == true)
                pen = new Pen(Color.Blue);
            else
                pen = new Pen(Color.Black);
            g.DrawEllipse(pen, (x - (rad / 2)), (y - (rad / 2)), rad, rad);//круг с начальными координатами в центре круга
            pctr.Image = bmp;
        }
        public bool Click_hit(MouseEventArgs e)//нажатие мыши в области/за пределами круга
        {
            if (((e.X - x) * (e.X - x) + (e.Y - y) * (e.Y - y)) <= rad * rad)//за пределами существующего круга
                return true;
            else
                return false;
        }
        public void Click_true()
        {
            clck = true;
        }
        public void Click_false()
        {
            clck = false;
        }
        public bool Clck()
        {
            return clck;
        }
    }

    class MyStorage//хранилище
    {
        int size;
        CCircle[] storage;
        public MyStorage()
        {
            size = 0;
        }
        public void SetObject(int i, CCircle obj)
        {
            storage[i] = obj;
        }
        public void AddObject(CCircle obj)
        {
            Array.Resize(ref storage, size + 1);
            storage[size] = obj;
            size = size + 1;
        }
        public bool CheckedStorage(MouseEventArgs e)//проверка, нажато ли на какой-либо круг
        {
            for (int i = 0; i < size; i++)
                if (storage[i].Click_hit(e) == true)
                    return true;
            return false;
        }
        public void ObjBlue(MouseEventArgs e)//выделение объекта при нажатии на него
        {
            for (int i = 0; i < size; i++)
            {
                if (storage[i].Click_hit(e) == true)
                {
                    storage[i].Click_true();
                    i = size;
                }
            }
        }
        public void DelObj(int i)//удаление объекта
        {
            if (size > 1 && i < size)
            {
                CCircle[] storage2 = new CCircle[size - 1];
                for (int j = 0; j < i; j++)
                    storage2[j] = storage[j];
                storage[i] = null;
                for (int j = i; j < size - 1; j++)
                    storage2[j] = storage[j + 1];
                size = size - 1;
                storage = storage2;
            }
            else
            {
                size = 0;
                storage[size] = null;
            }
        }
        public void Del_ObjBlue()//удаление выделенных объектов
        {
            for (int i = 0; i < size; i++)
            {
                if (storage[i].Clck() == true)
                {
                    DelObj(i);
                    i = i - 1;
                }
            }
        }
        public void AllNotChecked()
        {
            for (int i = 0; i < size; i++)
                storage[i].Click_false();
        }
        public void DrawAll(PictureBox pb, Graphics g, Bitmap bmp)
        {
            for (int i = 0; i < size; i++)
                storage[i].Draw(pb, g, bmp);
        }
        public int getSize()
        {
            return size;
        }
    }
}
