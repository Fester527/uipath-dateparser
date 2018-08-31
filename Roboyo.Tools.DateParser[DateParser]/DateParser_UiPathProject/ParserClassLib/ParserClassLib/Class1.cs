using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using System.ComponentModel;
using Chronic;

namespace Roboyo.Tools.Activities.DateParser
{
    [Category("DateParser")]
    [DisplayName("Date Parser")]
    [Description("Parse DateTime values from natural language.")]
    [Designer(typeof(ActivityDesigner1))]
    public sealed class Parser : CodeActivity
    {

        [Category("Input")]
        [Description("String input from which DateTime values will be parsed.")]
        [RequiredArgument]
        public InArgument<string> inString { get; set; }

        [Category("Output")]
        [Description("Nullable list of type DateTime.")]
        public OutArgument<List<DateTime?>> ParsedDates { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            string[] seperatingChars = { " to ", ". ", " from ", "? ", "! " };
            string[] stringToParse = inString.Get(context).Split(seperatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            List<DateTime?> listDates = new List<DateTime?>();

            foreach (string word in stringToParse)
            {
                if (string.IsNullOrEmpty(word)) Console.WriteLine("String is null or empty.");
                else
                {
                    try
                    {
                        var parser = new Chronic.Parser();
                        Span parsedObj = parser.Parse(word);

                        if (parsedObj == null)
                        {
                            listDates.Add(null);
                        }
                        else
                        {
                            DateTime? parsedDateTime = parsedObj.Start;
                            Console.WriteLine(parsedDateTime);
                            listDates.Add(parsedDateTime);
                        }

                    }
                    catch (Exception e)
                    {
                        listDates.Add(null);
                        Console.WriteLine("\"" + word + "\" was not recognized");
                    }
                    
                }

            }

            ParsedDates.Set(context, listDates);
        }
    }
}
