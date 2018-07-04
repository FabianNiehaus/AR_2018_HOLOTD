using Core.Input;

namespace HoloTD.Input
{
	/// <summary>
	/// Info for mouse
	/// </summary>
	public class GestureInfo : PointerActionInfo
	{
		/// <summary>
		/// Is this gesture recognized
		/// </summary>
		public bool isRecognized;

		/// <summary>
		/// Our gesture id
		/// </summary>
		public int gestureId;
	}
}