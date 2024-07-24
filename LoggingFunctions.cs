using System;
using System.IO.Ports;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Reflection.Metadata;

namespace MyNamespace
{
    public class Logger
    {
        public static void logging1000Val()
        {
            string portName = "COM5";
            Console.WriteLine("Welches Messgerät?");
            string measurementDevice = Console.ReadLine() ?? "NaN";  // Nimmt den Messwert in mm vom Benutzer entgegen.

            Console.WriteLine("Welche Werte in mm?");
            string measurementValue = Console.ReadLine() ?? "NaN";  // Nimmt den Messwert in mm vom Benutzer entgegen.3

            string fileName = $"{measurementValue}mm_M{measurementDevice}_1V8_{DateTime.Now:ddMMyyyy_HHmmss}.txt";
            using (SerialPort port = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One))
            {
                try
                {
                    port.Open();
                    Console.WriteLine($"Port {portName} geöffnet. Empfangene Daten werden in die Datei {fileName} geschrieben.");
                    using (StreamWriter writer = new StreamWriter("../../../calibrationData/Calibration_Supply_1V8/"+ fileName, true))
                    {
                        writer.WriteLine($"COM-Port: {portName}");
                        writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
                        writer.WriteLine($"Messgerät: {measurementDevice}");
                        writer.WriteLine($"Versorgungsspannung 1.8030 V");
                        writer.WriteLine($"Referenzspannung 0.55 V");
                        writer.WriteLine($"Widerstand: 1MOhm");
                        int count = 0;
                        string dataToWrite = "";

                        while (true)
                        {
                            string line = port.ReadLine();
                            string logEntry = $"{line}";
                            dataToWrite += logEntry + "\n";

                            count++;
                            if (count >= 1000)
                            {
                                writer.Write(dataToWrite);
                                writer.Flush(); // Stellen Sie sicher, dass alle Daten sofort in die Datei geschrieben werden
                                dataToWrite = ""; // Reset the data buffer
                                count = 0; // Reset the counter
                                Console.WriteLine("Programm wird beendet...");
                                Environment.Exit(0); // 0 üblicherweise für erfolgreiches Beenden
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
                }
            }
        }

        public static void LoggingCapacitor()
        {
            string portName = "COM5";
            Console.WriteLine("Welcher Kondensator?");
            string measurementDevice = Console.ReadLine() ?? "NaN";  // Nimmt den Messwert in mm vom Benutzer entgegen.

/*             Console.WriteLine("Welche Werte in mm?");
            string measurementValue = Console.ReadLine() ?? "NaN";  // Nimmt den Messwert in mm vom Benutzer entgegen.3 */
            string measurementValue = "80";
            string fileName = $"{measurementValue}mm_T{measurementDevice}_{DateTime.Now:ddMMyyyy_HHmmss}.txt";
            using (SerialPort port = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One))
            {
                try
                {
                    port.Open();
                    Console.WriteLine($"Port {portName} geöffnet. Empfangene Daten werden in die Datei {fileName} geschrieben.");
                    using (StreamWriter writer = new StreamWriter("C:/work/dataLogging/calibrationData/CapacitorTestingEarth/"+ fileName, true))
                    {
/*                         writer.WriteLine($"COM-Port: {portName}"); */
                        writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                        writer.WriteLine($"T{measurementDevice}");
/*                         writer.WriteLine($"Versorgungsspannung 1.8030 V");
                        writer.WriteLine($"Referenzspannung 0.55 V");
                        writer.WriteLine($"Widerstand: 1MOhm"); */
                        int count = 0;
                        string dataToWrite = "";

                        while (true)
                        {
                            string line = port.ReadLine();
                            string logEntry = $"{line}";
                            dataToWrite += logEntry;

                            count++;
                            if (count >= 100)
                            {
                                writer.Write(dataToWrite);
                                writer.Flush(); // Stellen Sie sicher, dass alle Daten sofort in die Datei geschrieben werden
                                dataToWrite = ""; // Reset the data buffer
                                count = 0; // Reset the counter
                                Console.WriteLine("Programm wird beendet...");
                                Environment.Exit(0); // 0 üblicherweise für erfolgreiches Beenden
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
                }
            }
        }

        public static void fileInit(string portName)
        {
            
            Console.WriteLine($"Bitte geben Sie das Messgerät für {portName} ein:");
            string measurementDevice = Console.ReadLine() ?? "NaN";

            string filename = $"M{measurementDevice}_ERDE_{DateTime.Now:yyyyMMddHHmmss}.txt";
            Console.WriteLine("Es wurde ein File erstellt");
            try{
                string path = Path.Combine("..", "..", "..", "calibrationData", "refMeasInEarth", filename);
                using (StreamWriter writer = new StreamWriter(path, true)){
                        writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
                        writer.WriteLine($"Messgerät: {measurementDevice}");
                        writer.WriteLine($"Versorgungsspannung 2.8051 V");
                        writer.WriteLine($"Referenzspannung 0.55 V");
                }
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("Der angegebene Pfad wurde nicht gefunden: " + e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Keine Zugriffsberechtigung auf den Dateipfad: " + e.Message);
            }
            catch (IOException e)
            {
                Console.WriteLine("Fehler beim Schreiben in die Datei: " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Ein unerwarteter Fehler ist aufgetreten: " + e.Message);
            }
        }


        // neu
        public static LoggingSession InitLoggingSession(string portName)
            {
                Console.WriteLine($"Bitte geben Sie das Messgerät für {portName} ein:");
                string measurementDevice = Console.ReadLine() ?? "NaN";
                string filename = $"M{measurementDevice}_ERDE_{DateTime.Now:yyyyMMddHHmmss}.txt";
                string filePath = Path.Combine("..", "..", "..", "calibrationData", "refMeasInEarth", filename);

                try
                {
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine($"Messgerät: {measurementDevice}");
                        writer.WriteLine($"Port: {portName}");
                        writer.WriteLine($"Erstellungszeitpunkt: {DateTime.Now:yyyy-MM-dd HH:mm}");
                    }
                    return new LoggingSession(portName, filePath); // Stellen Sie sicher, dass das Rückgabeobjekt erstellt wird
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ein unerwarteter Fehler ist aufgetreten: {e.Message}");
                    return null; // Geben Sie null oder eine alternative Fehlerbehandlung zurück
                }
            }

        public static void StartLogging(LoggingSession session)
        {
            using (SerialPort port = new SerialPort(session.PortName, 9600, Parity.None, 8, StopBits.One))
            {
                try
                {
                    port.Open();
                    Console.WriteLine($"Port {session.PortName} geöffnet. Empfangene Daten werden in die Datei {session.FilePath} geschrieben.");
                    using (StreamWriter writer = new StreamWriter(session.FilePath, true))
                    {
                        int dataPoints = 100; // Anzahl der Messpunkte pro Minute
                        while (true)
                        {
                            // Beginne eine neue Zeile mit dem aktuellen Zeitstempel
                            string dataToWrite = DateTime.Now.ToString("yyyy-MM-dd") + "," + DateTime.Now.ToString("HH:mm:ss") + "," ;

                            // Sammle die Datenpunkte in derselben Zeile
                            for (int i = 0; i < dataPoints; i++)
                            {
                                string line = port.ReadLine();
                                dataToWrite += line + (i < (dataPoints - 1) ? "," : ""); // Fügt Daten hinzu und trennt sie durch Kommas, außer am Ende
                            }

                            // Schreibe die vollständige Zeile mit Zeitstempel und Daten11
                            writer.WriteLine(dataToWrite);
                            writer.Flush(); // Sicherstellen, dass die Daten sofort in die Datei geschrieben werden

                            Console.WriteLine("Daten geschrieben. Warte 1 Minute bis zum nächsten Durchgang.");
                            Thread.Sleep(60000); // Wartet eine Minute (60000 Millisekunden) bis zum nächsten Schreibvorgang
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
                }
            }
        }
    }

    public class LoggingSession
    {
        public string PortName { get; set; }
        public string FilePath { get; set; }

        public LoggingSession(string portName, string filePath)
        {
            PortName = portName;
            FilePath = filePath;
        }
    }
}


