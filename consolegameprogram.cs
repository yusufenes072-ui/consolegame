using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace SkorToplamaOyunu
{
    class Program
    {
        // --- OYUN AYARLARI ---
        static int genislik = 40;
        static int yukseklik = 20;
        static int oyuncuX = 20;
        static int skor = 0;
        static bool oyunDevamEdiyor = true;
        static string logDosyasi = "game_log.txt";

        struct Obje { public int x, y; }
        static List<Obje> objeler = new List<Obje>();
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            Console.Title = "Skor Toplama Oyunu";
            Console.CursorVisible = false;

            // İlk Log: Oyun Başlatıldı
            LogYaz("START", $"Oyun baslatildi. Genislik={genislik}, Yukseklik={yukseklik}");

            while (oyunDevamEdiyor)
            {
                // 1. INPUT (Girdi Kontrolü)
                if (Console.KeyAvailable)
                {
                    var tus = Console.ReadKey(true).Key;
                    GirdiYonet(tus);
                }

                // 2. UPDATE (Güncelleme)
                Guncelle();

                // 3. DRAW (Çizim)
                Ciz();

                // 4. BITIS KONTROLU (Örn: 15 Puan)
                if (skor >= 15)
                {
                    LogYaz("GAMEOVER", $"Hedef skora ulasildi! Final Skor={skor}");
                    oyunDevamEdiyor = false;
                }

                Thread.Sleep(80); // Oyun hızı (ms)
            }

            OyunBitisEkrani();
        }

        static void GirdiYonet(ConsoleKey tus)
        {
            int eskiX = oyuncuX;
            if (tus == ConsoleKey.LeftArrow && oyuncuX > 0) oyuncuX--;
            if (tus == ConsoleKey.RightArrow && oyuncuX < genislik - 1) oyuncuX++;

            if (eskiX != oyuncuX)
            {
                LogYaz("INPUT", $"key={tus} playerX={oyuncuX} playerY={yukseklik - 1}");
            }
        }

        static void Guncelle()
        {
            // Yeni obje düşürme şansı
            if (rnd.Next(0, 100) < 15)
            {
                int yeniX = rnd.Next(0, genislik);
                objeler.Add(new Obje { x = yeniX, y = 0 });
                LogYaz("UPDATE", $"itemSpawned x={yeniX} y=0");
            }

            for (int i = 0; i < objeler.Count; i++)
            {
                Obje obj = objeler[i];
                obj.y++; // Aşağı hareket
                objeler[i] = obj;

                LogYaz("MOVEMENT", $"itemMoved x={obj.x} newY={obj.y}");

                // Çarpışma Kontrolü
                if (obj.y == yukseklik - 1 && obj.x == oyuncuX)
                {
                    skor++;
                    LogYaz("COLLISION", $"score={skor} at x={obj.x}");
                    objeler.RemoveAt(i);
                    i--;
                }
                else if (obj.y >= yukseklik)
                {
                    objeler.RemoveAt(i);
                    i--;
                }
            }
        }

        static void Ciz()
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" SKOR: {skor} / 15 | Hareket: OK TUSLARI ");
            Console.ResetColor();

            char[,] ekran = new char[yukseklik, genislik];
            for (int y = 0; y < yukseklik; y++)
                for (int x = 0; x < genislik; x++) ekran[y, x] = ' ';

            // Oyuncu Çizimi (@)
            ekran[yukseklik - 1, oyuncuX] = '@';

            // Objelerin Çizimi (*)
            foreach (var obj in objeler)
                if (obj.y < yukseklik) ekran[obj.y, obj.x] = '*';

            // Ekrana tek seferde yazdır (Titremeyi önlemek için)
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int y = 0; y < yukseklik; y++)
            {
                for (int x = 0; x < genislik; x++) sb.Append(ekran[y, x]);
                sb.AppendLine();
            }
            Console.Write(sb.ToString());
        }

        static void LogYaz(string etiket, string mesaj)
        {
            string logSatiri = $"{DateTime.Now:HH:mm:ss} | {etiket} -> {mesaj}";
            try
            {
                File.AppendAllText(logDosyasi, logSatiri + Environment.NewLine);
            }
            catch { /* Dosya o an kullanımdaysa hata verme */ }
        }

        static void OyunBitisEkrani()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("***************************");
            Console.WriteLine("    TEBRIKLER! OYUN BITTI  ");
            Console.WriteLine($"    TOPLAM SKORUNUZ: {skor}");
            Console.WriteLine("    Loglar kaydedildi.     ");
            Console.WriteLine("***************************");
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
