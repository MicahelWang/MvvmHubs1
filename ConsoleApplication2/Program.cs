using System;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            Calc();
        }

        private static void Calc()
        {
            do
            {
                System.Console.WriteLine("请输入距离(是否停止？[按键N停止])：");

                var inputVal = System.Console.ReadLine();
                if (inputVal != null && inputVal.ToCharArray().Length == 1)
                {
                    var key = inputVal.ToCharArray()[0];
                    if (key == 'N' || key == 'n')
                    {
                        return;
                    }
                }
                try
                {
                    var value = Convert.ToDouble(inputVal);
                    var level = BaseHelper.GetZoomLevel(value);
                    Console.WriteLine("ZoommLevel  =  {0}", level);
                    Console.WriteLine("是否停止？[按键N停止]");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("必须是个浮点数");
                }
            } while (true);
        }
    }
}
