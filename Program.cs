using System;
using System.IO.Ports;
using System.IO;
using System.Threading;
using MyNamespace;  // Dies gewährleistet, dass Sie den richtigen Namespace für LoggingSession und Logger verwenden

class Program
{
    static void Main(string[] args){
         //MyNamespace.Logger.logging1000Val();
        // Initialisieren und Starten der Logging-Sessions für zwei unterschiedliche Ports
        MyNamespace.LoggingSession session1 = MyNamespace.Logger.InitLoggingSession("COM5");
        MyNamespace.LoggingSession session2 = MyNamespace.Logger.InitLoggingSession("COM8");

        // Starten des parallelen Loggings für beide Sessions in separaten Threads
        Thread thread1 = new Thread(() => MyNamespace.Logger.StartLogging(session1,1005));
        Thread thread2 = new Thread(() => MyNamespace.Logger.StartLogging(session2,100));

        thread1.Start();
        thread2.Start();

        thread1.Join(); // Warten, bis thread1 beendet ist
        thread2.Join(); // Warten, bis thread2 beendet ist
    }
}


/*         Console.WriteLine("Bitte geben Sie den COM-Port an (nur COM5 oder COM8 erlaubt):");
        string portName = "COM5";

        if (portName != "COM5" && portName != "COM8")
        {
            Console.WriteLine("Ungültiger COM-Port. Bitte starten Sie das Programm neu und geben Sie COM5 oder COM8 ein.");
            return;
        }

        string fileName = $"data/{DateTime.Now:yyyyMMddHHmmss}_Messdaten.txt";
        using (SerialPort port = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One))
        {
            try
            {
                port.Open();
                Console.WriteLine($"Port {portName} geöffnet. Empfangene Daten werden in die Datei {fileName} geschrieben.");
                using (StreamWriter writer = new StreamWriter("../../../" + fileName, true))
                {
                    writer.WriteLine($"COM-Port: {portName}");
                    int count = 0;
                    string dataToWrite = "";
                    

                    while (true)
                    {
                        string line = port.ReadLine();
/*                         string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        string logEntry = $"{timestamp}, {line}";
                        dataToWrite += logEntry + "\n";

                        count++;
                        if (count >= 100)
                        {
                            writer.Write(dataToWrite);
                            writer.Flush(); // Stellen Sie sicher, dass alle Daten sofort in die Datei geschrieben werden
                            dataToWrite = ""; // Reset the data buffer
                            count = 0; // Reset the counter
                        }

                        Console.WriteLine($"Geschrieben: {logEntry}"); */
/*                     }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
/*             } */

