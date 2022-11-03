using System.Text;
using System.IO;

namespace WW
{
#if WWLIB
	public
#endif
	static partial class StringUtility
	{
		static readonly char[] _WordBreakChars = new char[] { ' ', '_', '\t', '.', '+', '-', '(', ')', '[', ']', '\"', /*'\'',*/ '{', '}', '!', '<', '>', '~', '`', '*', '$', '#', '@', '!', '\\', '/', ':', ';', ',', '?', '^', '%', '&', '|', '\n', '\r', '\v', '\f', '\0' };
		public static string WordWrap(this string text, int width,params char[] wordBreakChars)
		{
			if (string.IsNullOrEmpty(text) || 0 == width || width>=text.Length)
				return text;
			if (null == wordBreakChars || 0 == wordBreakChars.Length)
				wordBreakChars = _WordBreakChars;
			var sb = new StringBuilder();
			var sr = new StringReader(text);
			string line;
			var first = true;
			while(null!=(line=sr.ReadLine())) 
			{
				var col = 0;
				if (!first)
				{
					sb.AppendLine();
					col = 0;
				}
				else
					first = false;
				var words = line.Split(wordBreakChars);

				for(var i = 0;i<words.Length;i++)
				{
					var word = words[i];
					if (0 != i)
					{
						sb.Append(" ");
						++col;
					}
					if (col+word.Length>width)
					{
						sb.AppendLine();
						col = 0;
					}
					sb.Append(word);
					col += word.Length;
				}
			}
			return sb.ToString();
		}
	}
}
