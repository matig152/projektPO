# projektPO

# BACKEND
Zaczytujemy wykaz pokojów z pliku listaPokoi.xml (jest ich 60, po 5 na każdym z trzech pięter w 4 budynkach). Liczba pokoi jest zablokowana na 60 - jeżeli spróbujemy utworzyć nowy powinno nam wyrzucić wyjątek.
Aby założyć jakąś rezerwację, uprzednio trzeba utworzyć gościa oraz pracownika, który taką rezerwację założy. Zakładanie rezerwacji jest metodą w klasie Gosc. 
Do zrobienia tutaj jeszcze: 
 - ujednolicić pola w tych klasach, niektóre są niepotrzebne
 - zaimplementować automatyczne i ręczne wybieranie pokoju do rezerwacji (tutaj będzie trzeba wziąć pod uwagę żeby pobyty w danych pokojach się czasowo nie nachodziły)
 - stworzyć klasy agregujace Gości oraz Kadrę, tak żeby móc odczytywać i zapisywać zmiany (wygenerować wstępne dane w postaci plików XML)

Poza tym zostaje całe GUI.
