using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Lab2_Stocks
{
    

    

    class Stock
    {
        //Using a pure delegate instead of the Event type.
        

        private String name;
        private int initialValue;
        private int currentValue;
        private int maxChange;
        private int threshold;
        private int numberChanges;
        // Each stock needs a thread to allow for independent work.
        private Thread thread;
        // The Random object is used to increase (or decrease) the stock value between (1, maxChange).
        Random rand;
        // Event raised when a stock reaches its threshold.
        public event ThresholdReachedEventHandler ThresholdReached;
        
        public void doSomething(String some, int blah)
        {

        }

        public Stock(String name, int initialValue, int maxChange, int threshold)
        {
            this.name = name;
            this.initialValue = initialValue;
            this.currentValue = initialValue;
            this.maxChange = maxChange;
            this.threshold = threshold;
            numberChanges = 0;
            rand = new Random();
            // To create a thread, you need to make a new one with a given amount of work.
            // ThreadStart(Activate) is the work done in "thread".
            // Activate is a method in our code and ThreadStart translates the method into a
            // workable thread method.
            thread = new Thread(new ThreadStart(Activate));
            thread.Start();
        }

        // Work done for each thread.
        public void Activate()
        {
            // Loop made to change the stock's value.
            for(int i = 0; i < 10; i++)
            {
                // The sleep gives us time to monitor each change.
                // Without sleep, the stock would change too fast.
                Thread.Sleep(500);
                ChangeStockValue();
            }
            Console.ReadLine();
        }

        // Changing the Current Value of each stock.
        private void ChangeStockValue()
        {
            currentValue += rand.Next(1, maxChange);
            numberChanges++;
            // If we reach our threshold, then we raise an event.
            if((currentValue - initialValue) > threshold)
            {
                // Creating our argument data to give to our event handler.
                ThresholdReachedEventArgs args = new ThresholdReachedEventArgs();
                args.StockName = name;
                args.CurrentValue = currentValue;
                args.NumberChanges = numberChanges;
                OnThresholdReached(args);
            }
        }
        
        // Method to raise a threshold event. (Notice it starts with "On").
        protected virtual void OnThresholdReached(ThresholdReachedEventArgs e)
        {
            ThresholdReachedEventHandler handler = ThresholdReached;
            // C# 6.0
            handler?.Invoke(this, e);

            // C# 5.0
            /*
            if (handler != null)
            {
                handler(this, e);
            }
            */
        }
    }

    // Custom EventArgs class to hold extra data.
    public class ThresholdReachedEventArgs : EventArgs
    {
        public String StockName { get; set; }
        public int CurrentValue { get; set; }
        public int NumberChanges { get; set; }
    }

    public delegate void ThresholdReachedEventHandler(Object sender, ThresholdReachedEventArgs e);


    class StockBroker
    {
        String brokerName;
        List<Stock> stocks;
        // Locking object.
        static object syncRoot = new object();
        static string path = @"C:\Users\ScottRoberts\source\repos\CECS475_Labs\Lab2_Stocks\bin\stuff.txt";
        static StreamWriter streamWriter = new StreamWriter(path);
        static bool titleDisplayed;

        public StockBroker(String brokerName)
        {
            // Adding the brokerName and list of empty stocks to each StockBroker.
            this.brokerName = brokerName;
            stocks = new List<Stock>(); 
        }

        public void AddStock(Stock stock)
        {
            // Adding a param stock.
            stocks.Add(stock);
            // Subscribing to the Stock class event "ThresholdReached" by adding the StockBroker
            // class listening method "Stock_ThresholdReached".
            stock.ThresholdReached += Stock_ThresholdReached;
        }

        // Listening method for threshold reached.
        private void Stock_ThresholdReached(object sender, ThresholdReachedEventArgs e)
        {
            // Locking access to a single thread to avoid desyncronization.
            lock (syncRoot)
            {
                // Creating the titles for the console and text file outputs.
                if (!titleDisplayed)
                {
                    var bName = "Broker Name:";
                    var sName = "Stock Name:";
                    var cValue = "Current Value:";
                    var nChanges = "Number of Changes:";
                    Console.WriteLine(bName.PadRight(20) + sName.PadRight(20) + cValue.PadRight(20) + nChanges.PadRight(20));
                    streamWriter.WriteLine(bName.PadRight(20) + sName.PadRight(20) + cValue.PadRight(20) + nChanges.PadRight(20));
                    streamWriter.Close();
                    titleDisplayed = true;

                }
                String lineOutput = brokerName.PadRight(20) + e.StockName.PadRight(20) + e.CurrentValue.ToString().PadRight(20) + e.NumberChanges.ToString().PadRight(20);
                Console.WriteLine(lineOutput);

                
                // This text is added only once to the file.
                if (!File.Exists(path))
                {
                    // Create a file to write to with the title and first line output.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        var bName = "Broker Name:";
                        var sName = "Stock Name:";
                        var cValue = "Current Value:";
                        var nChanges = "Number of Changes:";
                        streamWriter.WriteLine(bName.PadRight(20) + sName.PadRight(20) + cValue.PadRight(20) + nChanges.PadRight(20));
                        sw.WriteLine(lineOutput);
                    }
                }

                // This text is always added, making the file longer over time
                // if it is not deleted.
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(lineOutput);
                }
            }
        }
    }

    // Provided Driver for Lab 2.
    class StockDriver
    {
        delegate void StockNotification(String stockName, int currentValue);
        static void Main(String[] args)
        {
            Stock stock1 = new Stock("Technology", 160, 5, 15);
            
            StockNotification op = stock1.doSomething;
            op("this", 1);
            Stock stock2 = new Stock("Retail", 30, 2, 6);
            Stock stock3 = new Stock("Banking", 90, 4, 10);
            Stock stock4 = new Stock("Commodity", 500, 20, 50);

            StockBroker b1 = new StockBroker("Broker 1");
            b1.AddStock(stock1);
            b1.AddStock(stock2);

            StockBroker b2 = new StockBroker("Broker 2");
            b2.AddStock(stock1);
            b2.AddStock(stock3);
            b2.AddStock(stock4);

            StockBroker b3 = new StockBroker("Broker 3");
            b3.AddStock(stock1);
            b3.AddStock(stock3);

            StockBroker b4 = new StockBroker("Broker 4");
            b4.AddStock(stock1);
            b4.AddStock(stock2);
            b4.AddStock(stock3);
            b4.AddStock(stock4);
        }
    }
}
