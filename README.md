В репозитории находится сайт в папке "site" и solition (остальные файлы)
Для работы приложения требуется база данных PostgreSQL. В базе должны содержаться таблицы users и plants.
//запрос на создание users
create table users(id serial Primary key not null, login varchar(50) not null, password varchar(50) not null, counter_achiv integer);
//Запрос на создание plants
create table plants(id serial Primary key not null, name varchar(50) not null, type varchar(50) not null, userid integer, color varchar(50), data_sled_poliva varchar(50), chastota_poliva varchar(50), data_sled_udob varchar(50), chastota_udob varchar(50));
Для подключения к серверу баз данных у на используется файл base.txt, в нём хранится данные для подключения к базе.
