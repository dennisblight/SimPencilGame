using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Deanor.Scene
{
    public interface IScene
    {
        UserControl Element { get; }
        void ShowScene();
        void CloseScene();
    }
}
