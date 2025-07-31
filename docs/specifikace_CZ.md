# Zápočtový program – Audio Visualizer

Jako téma zápočtového programu do NPRG035 a NPRG038 bych chtěl vytvořit jednoduchý **Audio Visualizer**.

## Základní funkcionalita

Program na vstup bere zvukový soubor (WAV/MP3).  
Začne přehrávat zvukovou stopu a zobrazí aktuální hodnoty pro několik frekvenčních rozsahů – ve formě sloupcového grafu.

Uživatel pak může buď pozorovat, v jakých frekvencích se zrovna pohybuje zvuková stopa, nebo přímo ovlivnit její vlastnosti – tím, že některé frekvenční rozsahy utlumí/zesílí.

### Pluginy

Aplikace bude podporovat možnost vytvoření custom pluginů - formou C# třídy implementující speciální plugin interface.
Přes reflexi se pak načtou všechny takovéto pluginy a během přehrávání se jimi definované modifikace budou aplikovat na audio data.

## Cíl projektu

Vytvořit aplikaci, která pomůže uživateli lépe si představit, jak zvuk funguje jakožto složenina frekvencí.  

Uživatel může zjistit, jak zesílení/odebrání určitých frekvencí mění vnímání původního zvuku.

---

### Externí technologie

- `NAudio`: dekódování audio souborů, spuštění přehrávání zvuku na zdroji  
- `Terminal.Gui`: pro rendering UI  
- `MathNet.Numerics`: pro FFT (rozložení zvuku na frekvence)

### Pokročilé technologie

- Vícevláknové programování – pro přehrávání zvuku a zároveň komunikaci s UI  
- Možné využití LINQ při agregování FFT pásem
- Reflexe pro načtení pluginů
