using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REST
{
    public class Injection_C
    {
        public string injection_string(string Text)
        {
            string[] ban_symbol = { "@", "!", "#", "&", "%", "$", "'", @"""", "OR", "AND", "\b", "\0", "\'", "\"", "\n", "\r", "\t", @"\z", "\\", @"\%", @"\_" };
            foreach (var item in ban_symbol)
            {
                if (!String.IsNullOrEmpty(Text))
                    Text = Text.Replace(item, "");
            }
            return Text;
        }
    }
}