# Konsol Skor Toplama Oyunu

Bu proje, C# diliyle geliştirilmiş basit bir konsol tabanlı skor toplama oyunudur.

## Özellikler
- **Karakter:** `@` sembolü ile temsil edilir.
- **Kontroller:** Sol ve Sağ ok tuşları.
- **Hedef:** Yukarıdan düşen `*` karakterlerini toplayarak 15 puana ulaşmak.
- **Debug Sistemi:** Oyundaki tüm hareketler ve olaylar anlık olarak `game_log.txt` dosyasına kaydedilir.

## Nasıl Çalıştırılır?
1. Proje dosyalarını indirin.
2. Terminal/CMD üzerinden proje klasörüne gidin.
3. `dotnet run` komutunu çalıştırın.

## Log Örneği
```text
14:20:01 | INPUT -> key=LeftArrow playerX=19 playerY=19
14:20:02 | COLLISION -> score=1 at x=19
