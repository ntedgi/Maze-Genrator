using MVP.Model;
using MVP.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MVP
{
    class Program
    {
        static void Main(string[] args)
        {
            IModel model = new MyModel();
            IView view = new MyView();
            MyPresenter presenter = new MyPresenter(model, view);
            view.Start();
        }
    }
}
