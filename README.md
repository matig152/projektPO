# Projekt - System Obsługi Hotelu

# BACKEND
Zaczytujemy wykaz pokojów z pliku listaPokoi.xml (jest ich 60, po 5 na każdym z trzech pięter w 4 budynkach). Liczba pokoi jest zablokowana na 60 - jeżeli spróbujemy utworzyć nowy powinno nam wyrzucić wyjątek. Zaczytujemy również rejestr gości oraz kadrę z plików XML. 

Aby założyć jakąś rezerwację, uprzednio trzeba utworzyć gościa oraz pracownika, który taką rezerwację założy. Zakładanie rezerwacji jest metodą w klasie Gosc. 
Do zrobienia tutaj jeszcze: 
 - zaimplementować automatyczne i ręczne wybieranie pokoju do rezerwacji (tutaj będzie trzeba wziąć pod uwagę żeby pobyty w danych pokojach się czasowo nie nachodziły)
 - wyszukiwanie pobytow po gosciach, pokojach i datach
 - inne metody?

Poza tym zostaje całe GUI.

Diagram UML: ([link do edycji](https://lucid.app/lucidchart/aff13f31-4b23-4d71-bead-9306d98ce140/edit?viewport_loc=-140%2C-481%2C3111%2C1460%2CHWEp-vi-RSFO&invitationId=inv_88ea429b-d3f0-4630-8f70-d7bfa86d66b6)
![Diagram](./umlDiagram.jpg)

 
