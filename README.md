# Soulbringer game

map2ply.cs - текструры можно рипнуть NinjaRipper . Разработчик dgVoodoo знает как они "зашифрованы" . 
ob2obj(with_duplication_of_vertices).cs - дублируются вершины и ув .
ob2obj(without_duplicating_vertices).cs - убраны дубли вершин , с ув сложнее . 
ob2ply.cs	- формат ply хранит вершины без дублирования ув и координат (?)
ob2ply(search_in_subfolders).cs - (удобно) конвертит модели прямо в папках своего уровня . 

Импортируйте модели в Юнити с scale = 0,005 . Карты идут в масштабе 1:1
В программах присутствуют ошибки округления текстурных координат , поэтому текстуры накладываются чуть-чуть неправильно .
