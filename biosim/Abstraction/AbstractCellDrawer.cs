using System.Drawing;

namespace Biosim.Abstraction
{
	public interface ICellDrawer
	{
		void Draw(Graphics g, Rectangle rect);
	}
}

